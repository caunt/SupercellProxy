using System.Globalization;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed partial class AmbientAnimalState
{
    private void Update(GameRandom random)
    {
        InitializeSpawnerBounds();
        var effectivePosition = ResolveAbsolutePosition(GameObject);
        _effectivePositionX = effectivePosition.X;
        _effectivePositionY = effectivePosition.Y;

        if (BeginUpdate(random))
            return;

        if (Behavior is 3 && UpdateBehaviorThreeState(random))
            return;

        if (UpdateMovementState(random))
            return;

        var extraBoundary = Behavior < 2 ? _birdExtraTiles << 9 : 0;
        var maximumAnimalX = _maximumX + extraBoundary;
        var maximumAnimalY = _maximumY + extraBoundary;
        var adjustedMaximumY = Behavior is 2 ? maximumAnimalY - 500 : maximumAnimalY;

        if (Behavior is not 4 && IsOutsideMovementBounds(maximumAnimalX, adjustedMaximumY))
        {
            IsRemoved = true;
            return;
        }

        UpdateRandomizedMovement(random);
        if (
            ApplyAvoidanceMovement(random)
            || ApplyBehaviorThreeBoundaryRedirect(random)
            || ApplyAttractionOrBehaviorTwoRedirect(random)
        )
        {
            return;
        }

        CompleteMovement(random);
    }

    private bool BeginUpdate(GameRandom random)
    {
        if (IsRemoved)
            return true;

        MovementTimer--;
        AvoidanceLinger--;
        MirrorTimer--;
        MovementX = 0;
        MovementY = 0;

        if (Behavior is 4)
        {
            var lifetime = PhaseTimer;
            PhaseTimer++;
            if (lifetime > 108)
            {
                IsRemoved = true;
                return true;
            }
        }

        if (MovementState is 4)
        {
            UpdateLandingMovement();
            return true;
        }

        if (MovementState is not 5)
            return false;

        UpdateTakeoffMovement();
        return true;
    }

    private bool UpdateBehaviorThreeState(GameRandom random)
    {
        RefreshAttractionTarget();
        if (MovementState is not 3)
        {
            var stateTimer = PhaseTimer;
            PhaseTimer++;
            if (stateTimer > 16)
            {
                MovementState = 3;
                MovementTimer = random.NextInt(70) + 20;
                CachedAvoidanceIndex = -1;
                RefreshAvoidanceTarget();
                Heading = ResolveBehaviorThreeHeading(random);
            }
        }

        if (MovementState is 3 && (_effectivePositionX < -0x200 || _isInsideAttractionTarget))
        {
            IsRemoved = true;
            return true;
        }

        if (MovementState is 3 && _redirectRefreshPending)
        {
            var redirectTimer = PhaseTimer;
            PhaseTimer++;
            if (redirectTimer > 18)
            {
                _redirectRefreshPending = false;
                MovementTimer = random.NextInt(70) + 20;
            }
        }

        return false;
    }

    private bool IsOutsideMovementBounds(int maximumAnimalX, int adjustedMaximumY)
    {
        return _effectivePositionX < _minimumX
            || _effectivePositionX > maximumAnimalX
            || _effectivePositionY < _minimumY
            || _effectivePositionY > adjustedMaximumY;
    }

    private bool UpdateMovementState(GameRandom random)
    {
        if (Behavior is 0 && MovementState is 1 && DestinationX is 0 && DestinationY is 0)
        {
            Heading += HeadingStep;
            return false;
        }

        if (Behavior is 0 && MovementState is 2 && DestinationX is 0 && DestinationY is 0)
        {
            Heading -= HeadingStep;
            return false;
        }

        if (MovementState is not 3)
            return false;
        if (Behavior is 2 or 3)
        {
            if (MovementTimer <= 0)
                return false;

            if (ApplyPrimarySourceAvoidance())
            {
                PhaseTimer = 0;
                MovementState = 0;
            }

            return true;
        }
        if (MovementTimer > 0)
            return Behavior is 0 or 1;
        if (Behavior is 1)
        {
            BeginTakeoff(random);
            return true;
        }

        if (Behavior is 0)
        {
            AltitudeStep = random.NextInt(8) + 2;
            AltitudeStepChangeTimer = 25;
            Altitude = 8;
        }
        else
        {
            MovementState = 0;
            PhaseTimer = 0;
        }

        return false;
    }

    private void UpdateRandomizedMovement(GameRandom random)
    {
        RefreshAvoidanceTarget();
        if (Behavior is not 1 && ApplyPrimarySourceAvoidance())
            _hasAvoidanceTarget = true;
        RefreshAttractionTarget();

        if (MovementTimer < 1)
        {
            MovementState = sbyte.CreateTruncating(GetMovementState(random));
            HeadingStep = GetHeadingStep(random);
            MovementTimer = GetSpeedChangeTimer(random);
        }

        var speedChangeTimer = SpeedChangeTimer;
        SpeedChangeTimer--;
        if (SpeedChangeTimer is 0 || speedChangeTimer < 1)
        {
            Speed += GetSpeedChange(random);
            ClampSpeed();
            SpeedChangeTimer = GetNextSpeedChangeTimer(random);
        }

        var altitudeStepChangeTimer = AltitudeStepChangeTimer;
        AltitudeStepChangeTimer--;
        if (AltitudeStepChangeTimer is 0 || altitudeStepChangeTimer < 1)
        {
            AltitudeStep = Math.Clamp(
                AltitudeStep + GetAltitudeStepChange(random),
                Behavior is 1 ? -16 : -8,
                Behavior is 1 ? 16 : 8
            );
            AltitudeStepChangeTimer = GetNextAltitudeStepChangeTimer(random);
        }

        var angle = Heading / 8;
        MovementX = IntegerMath.GetSine(angle + 90) * Speed / 1024;
        MovementY = IntegerMath.GetSine(angle) * Speed / 1024;
        if (AvoidanceLinger < 1 && !_hasAvoidanceTarget)
        {
            MovementX += CleanupDriftX;
            MovementY += CleanupDriftY;
        }
    }

    private bool ApplyAvoidanceMovement(GameRandom random)
    {
        if (AvoidanceLinger < 1 && !_hasAvoidanceTarget || !_hasAvoidanceTarget && Behavior >= 2)
            return false;
        if (Behavior is 2 or 3)
        {
            var targetAngle = IntegerMath.GetVectorAngle(-AvoidanceX, -AvoidanceY);
            var movementAngle = IntegerMath.GetVectorAngle(MovementX, MovementY);
            var difference = Math.Abs(IntegerMath.GetAngleDifference(targetAngle - movementAngle));
            if (difference >= 120)
                return false;

            Redirect(random, AvoidanceX, AvoidanceY);
            return true;
        }

        AddNormalizedMovement(AvoidanceX, AvoidanceY, 5);
        _attractionPoints = [];
        return false;
    }

    private bool ApplyBehaviorThreeBoundaryRedirect(GameRandom random)
    {
        if (Behavior is not 3 || ZoneCleanup)
            return false;
        if (MovementY < 0 && _effectivePositionY + MovementY < 0)
        {
            Redirect(random, 0, 0x100);
            return true;
        }

        if (MovementX <= 0 || _effectivePositionX + MovementX <= 0x6000)
            return false;

        Redirect(random, -0x100, 0);
        return true;
    }

    private bool ApplyAttractionOrBehaviorTwoRedirect(GameRandom random)
    {
        if (
            HasAttractionTarget
            && !_isInsideAttractionTarget
            && Behavior is not 3
            && !_hasAvoidanceTarget
            && MovementState is not 3
        )
        {
            AddNormalizedMovement(AttractionX, AttractionY, 4);
            return false;
        }

        if (Behavior is not 2)
            return false;
        if (MovementX < 0 && _effectivePositionX + MovementX < 0)
        {
            Redirect(random, 0x100, 0);
            return true;
        }

        if (MovementY < 0 && _effectivePositionY + MovementY < 0)
        {
            Redirect(random, 0, 0x100);
            return true;
        }

        if (MovementX <= 0 || _effectivePositionX + MovementX <= 0x6000)
            return false;

        Redirect(random, -0x100, 0);
        return true;
    }

    private void CompleteMovement(GameRandom random)
    {
        if (Behavior is 0)
        {
            SteeringState = Math.Clamp(
                (SteeringState + (MovementX - MovementY) / 3) * 9 / 10,
                -80,
                80
            );
        }

        var previousAltitude = Altitude;
        Altitude = Math.Clamp(Altitude + AltitudeStep, 0, 792);
        if (Behavior is 1 && previousAltitude is not 0 && Altitude is 0)
        {
            AltitudeStepChangeTimer = 90;
            AltitudeStep = 0;
        }

        RefreshLandingTarget();
        if (Altitude is 0)
            UpdateGroundMovement(random);

        UpdateMirroring();
        WasInsideLandingTarget = _isInsideLandingTarget;
        CleanupDriftX = 0;
        CleanupDriftY = 0;
        GameObject.MoveTo(_effectivePositionX + MovementX, _effectivePositionY + MovementY);
    }

    private int GetMovementState(GameRandom random)
    {
        return Behavior switch
        {
            0 or 1 => random.NextInt(2) + 1,
            2 => random.NextInt(3),
            3 or 4 => 0,
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };
    }

    private int GetHeadingStep(GameRandom random)
    {
        return Behavior switch
        {
            0 => random.NextInt(20) * 8 + 16,
            1 => GetBirdHeadingStep(random),
            2 => random.NextInt(6) * 8,
            3 or 4 => 0,
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };
    }

    private static int GetBirdHeadingStep(GameRandom random)
    {
        var value = random.NextInt(3);
        return value is 0 ? random.NextInt(8) * 2 : value * 16;
    }

    private int GetSpeedChangeTimer(GameRandom random)
    {
        var headingStep = HeadingStep / 8;

        return Behavior switch
        {
            0 => random.NextInt(180 - 12 * headingStep) / 2,
            1 => random.NextInt(100 - 30 * headingStep) / 2,
            2 => (random.NextInt(400 - 200 * headingStep) + 10) / 2,
            3 or 4 => random.NextInt(60),
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };
    }

    private int GetSpeedChange(GameRandom random)
    {
        var change = Behavior switch
        {
            0 => random.NextInt(18) - 8,
            1 => random.NextInt(34) - 16,
            2 or 3 => random.NextInt(10) - 4,
            4 => 0,
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };

        return change * _speedMultiplier / 100;
    }

    private void ClampSpeed()
    {
        Speed = Behavior switch
        {
            0 => Math.Clamp(Speed, 10, 44),
            1 => Math.Clamp(Speed, 80, 120),
            2 => Math.Clamp(Speed, 16, 28),
            3 => Math.Clamp(Speed, 24, 36),
            4 => 0,
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };
        Speed = Speed * _speedMultiplier / 100;
    }

    private int GetNextSpeedChangeTimer(GameRandom random)
    {
        return Behavior switch
        {
            0 => random.NextInt(15),
            1 => random.NextInt(90) + 30,
            2 or 3 or 4 => random.NextInt(45) + 15,
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };
    }

    private int GetAltitudeStepChange(GameRandom random)
    {
        return Behavior switch
        {
            0 => random.NextInt(10) - 4,
            1 => GetBirdAltitudeStepChange(random),
            2 or 3 or 4 => 0,
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };
    }

    private int GetBirdAltitudeStepChange(GameRandom random)
    {
        if (random.NextInt(3) is 0)
            return random.NextInt(19) - 10;

        if (Altitude < 400)
            return random.NextInt(6) is 0 ? -14 : 14;

        return random.NextInt(6) is 0 ? 14 : -14;
    }

    private int GetNextAltitudeStepChangeTimer(GameRandom random)
    {
        return Behavior switch
        {
            0 => random.NextInt(5),
            1 => random.NextInt(2) + 5,
            2 or 3 or 4 => 999,
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };
    }

    private void Redirect(GameRandom random, int x, int y)
    {
        MovementState = 3;
        Speed = 16;
        MovementTimer = GetRedirectDuration(random);
        Heading = (random.NextInt(180) + IntegerMath.GetVectorAngle(x, y) - 90) << 3;
        var previousRedirectCount = RedirectCount;
        RedirectCount = unchecked(RedirectCount + 1);

        if (Behavior is 3)
            _redirectRefreshPending = true;
        else if (previousRedirectCount > 9)
            _attractionPoints = [];
    }

    private int ResolveBehaviorThreeHeading(GameRandom random)
    {
        if (_hasAvoidanceTarget)
            return IntegerMath.GetVectorAngle(AvoidanceX, AvoidanceY) << 3;

        if (!ZoneCleanup)
            return random.NextInt(360) << 3;

        return HasAttractionTarget
            ? IntegerMath.GetVectorAngle(AttractionX, AttractionY) << 3
            : 180 << 3;
    }

    private int GetRedirectDuration(GameRandom random)
    {
        return Behavior switch
        {
            0 => random.NextInt(400) + 100,
            1 => random.NextInt(1500) + 1000,
            2 => random.NextInt(200) + 100,
            3 or 4 => random.NextInt(70) + 20,
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };
    }

    private void UpdateLandingMovement()
    {
        var landingTimer = PhaseTimer;
        PhaseTimer++;

        if (landingTimer > 83)
        {
            MovementState = 3;
            DestinationX = 0;
            DestinationY = 0;
            return;
        }

        MovementX = LandingX / 85;
        MovementY = LandingY / 85;
        GameObject.MoveTo(_effectivePositionX + MovementX, _effectivePositionY + MovementY);
    }

    private void BeginTakeoff(GameRandom random)
    {
        PhaseTimer = 0;
        MovementState = 5;
        Altitude = 8;
        AltitudeStep = random.NextInt(8) + 2;
        AltitudeStepChangeTimer = 25;

        var heading = random.NextInt(360);

        for (var attempts = 359; attempts > 0 && IsDisallowedTakeoffHeading(heading); attempts--)
            heading = (heading + 1) % 360;

        Heading = heading << 3;
    }

    private void UpdateTakeoffMovement()
    {
        PhaseTimer++;
        Heading += HeadingStep;

        var progress = PhaseTimer * 4096 / 110;
        var movementSpeed = Speed * progress / 4096 + 1;
        var angle = Heading / 8;
        MovementX = IntegerMath.GetSine(angle + 90) * movementSpeed / 1024;
        MovementY = IntegerMath.GetSine(angle) * movementSpeed / 1024;
        GameObject.MoveTo(_effectivePositionX + MovementX, _effectivePositionY + MovementY);

        UpdateMirroring();

        if (PhaseTimer >= 110)
            MovementState = 0;
    }

    private static bool IsDisallowedTakeoffHeading(int heading)
    {
        return heading is 45 or >= 135 and <= 315;
    }

    private void UpdateGroundMovement(GameRandom random)
    {
        if (Behavior is 0)
        {
            if (
                (_landingPoints.Count is 0 || _isInsideLandingTarget && !WasInsideLandingTarget)
                && _effectivePositionX + MovementX > 0
                && !_hasAvoidanceTarget
            )
            {
                MovementState = 3;
                MovementTimer = GetRedirectDuration(random);
                return;
            }

            AltitudeStep = random.NextInt(8) + 2;
            AltitudeStepChangeTimer = 25;
            Altitude = 8;
            return;
        }

        var destinationReached = _isInsideLandingTarget && IsLandingDestinationReached();

        if (
            Behavior is not 1
            || !_isInsideLandingTarget
            || WasInsideLandingTarget && !destinationReached
            || _effectivePositionX + MovementX <= 0
            || _hasAvoidanceTarget
        )
        {
            return;
        }

        var targetAngle = IntegerMath.GetVectorAngle(LandingX, LandingY);
        var angleDifference = Math.Abs(IntegerMath.GetAngleDifference(Heading / 8 - targetAngle));

        if (angleDifference >= 30)
            return;

        MovementState = 4;
        PhaseTimer = 0;
        MovementTimer = GetRedirectDuration(random);
    }

    private bool IsLandingDestinationReached()
    {
        if (DestinationX is 0 && DestinationY is 0)
            return false;

        var x = LandingX + _effectivePositionX + MovementX;
        var y = LandingY + _effectivePositionY + MovementY;
        var reached = Math.Abs(x - DestinationX) < 100 && Math.Abs(y - DestinationY) < 100;

        if (reached)
            AltitudeStep = -32;

        return reached;
    }
}
