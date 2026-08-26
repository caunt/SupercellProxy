using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed partial class AmbientAnimalState
{
    private void Update(GameRandom random)
    {
        if (BeginUpdate(random))
            return;

        if (Behavior is 3 && UpdateBehaviorThreeState(random))
            return;

        if (UpdateMovementState(random))
            return;

        var extraBoundary = Behavior < 2 ? birdExtraTiles << 9 : 0;
        var maximumAnimalX = maximumX + extraBoundary;
        var maximumAnimalY = maximumY + extraBoundary;
        var adjustedMaximumY = Behavior is 2 ? maximumAnimalY - 500 : maximumAnimalY;

        if (Behavior is not 4 && IsOutsideMovementBounds(maximumAnimalX, adjustedMaximumY))
        {
            ChecksumFlag0 = true;
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
        if (ChecksumFlag0)
            return true;

        ChecksumState2--;
        ChecksumState13--;
        ChecksumState16--;
        MovementX = 0;
        MovementY = 0;

        if (Behavior is 4)
        {
            var lifetime = ChecksumState5;
            ChecksumState5++;
            if (lifetime > 108)
            {
                ChecksumFlag0 = true;
                return true;
            }
        }

        if (ChecksumByte0 is 4)
        {
            UpdateLandingMovement();
            return true;
        }

        if (ChecksumByte0 is not 5)
            return false;

        UpdateTakeoffMovement();
        return true;
    }

    private bool UpdateBehaviorThreeState(GameRandom random)
    {
        RefreshAttractionTarget();
        if (ChecksumByte0 is not 3)
        {
            var stateTimer = ChecksumState5;
            ChecksumState5++;
            if (stateTimer > 16)
            {
                ChecksumByte0 = 3;
                ChecksumState2 = random.NextInt(70) + 20;
                ChecksumState14 = -1;
                RefreshAvoidanceTarget();
                Heading = ResolveBehaviorThreeHeading(random);
            }
        }

        if (ChecksumByte0 is 3 && (GameObject.PositionX < -0x200 || isInsideAttractionTarget))
        {
            ChecksumFlag0 = true;
            return true;
        }

        if (ChecksumByte0 is 3 && redirectRefreshPending)
        {
            var redirectTimer = ChecksumState5;
            ChecksumState5++;
            if (redirectTimer > 18)
            {
                redirectRefreshPending = false;
                ChecksumState2 = random.NextInt(70) + 20;
            }
        }

        return false;
    }

    private bool IsOutsideMovementBounds(int maximumAnimalX, int adjustedMaximumY)
    {
        return GameObject.PositionX < minimumX
            || GameObject.PositionX > maximumAnimalX
            || GameObject.PositionY < minimumY
            || GameObject.PositionY > adjustedMaximumY;
    }

    private bool UpdateMovementState(GameRandom random)
    {
        if (Behavior is 0 && ChecksumByte0 is 1 && DestinationX is 0 && DestinationY is 0)
        {
            Heading += HeadingStep;
            return false;
        }

        if (Behavior is 0 && ChecksumByte0 is 2 && DestinationX is 0 && DestinationY is 0)
        {
            Heading -= HeadingStep;
            return false;
        }

        if (ChecksumByte0 is not 3)
            return false;
        if (Behavior is 3)
            return false;
        if (ChecksumState2 > 0)
            return Behavior is 0 or 1;
        if (Behavior is 1)
        {
            BeginTakeoff(random);
            return true;
        }

        if (Behavior is 0)
        {
            ChecksumState6 = random.NextInt(8) + 2;
            ChecksumState4 = 25;
            ChecksumState1 = 8;
        }
        else
        {
            ChecksumByte0 = 0;
            ChecksumState5 = 0;
        }

        return false;
    }

    private void UpdateRandomizedMovement(GameRandom random)
    {
        RefreshAvoidanceTarget();
        RefreshAttractionTarget();
        if (ChecksumState2 < 1)
        {
            ChecksumByte0 = sbyte.CreateTruncating(GetMovementState(random));
            HeadingStep = GetHeadingStep(random);
            ChecksumState2 = GetSpeedChangeTimer(random);
        }

        var speedChangeTimer = ChecksumState3;
        ChecksumState3--;
        if (ChecksumState3 is 0 || speedChangeTimer < 1)
        {
            Speed += GetSpeedChange(random);
            ClampSpeed();
            ChecksumState3 = GetNextSpeedChangeTimer(random);
        }

        var headingChangeTimer = ChecksumState4;
        ChecksumState4--;
        if (ChecksumState4 is 0 || headingChangeTimer < 1)
        {
            ChecksumState6 = Math.Clamp(
                ChecksumState6 + GetHeadingChange(random),
                Behavior is 1 ? -16 : -8,
                Behavior is 1 ? 16 : 8
            );
            ChecksumState4 = GetNextHeadingChangeTimer(random);
        }

        var angle = Heading / 8;
        MovementX = IntegerMath.GetSine(angle + 90) * Speed / 1024;
        MovementY = IntegerMath.GetSine(angle) * Speed / 1024;
        if (ChecksumState13 < 1 && !hasAvoidanceTarget)
        {
            MovementX += ChecksumState11;
            MovementY += ChecksumState12;
        }
    }

    private bool ApplyAvoidanceMovement(GameRandom random)
    {
        if (ChecksumState13 < 1 && !hasAvoidanceTarget || !hasAvoidanceTarget && Behavior >= 2)
            return false;
        if (Behavior is 2 or 3)
        {
            var targetAngle = IntegerMath.GetVectorAngle(-ChecksumState7, -ChecksumState8);
            var movementAngle = IntegerMath.GetVectorAngle(MovementX, MovementY);
            var difference = Math.Abs(IntegerMath.GetAngleDifference(targetAngle - movementAngle));
            if (difference >= 120)
                return false;

            Redirect(random, ChecksumState7, ChecksumState8);
            return true;
        }

        AddNormalizedMovement(ChecksumState7, ChecksumState8, 5);
        attractionPoints = [];
        return false;
    }

    private bool ApplyBehaviorThreeBoundaryRedirect(GameRandom random)
    {
        if (Behavior is not 3 || ChecksumFlag3)
            return false;
        if (MovementY < 0 && GameObject.PositionY + MovementY < 0)
        {
            Redirect(random, 0, 0x100);
            return true;
        }

        if (MovementX <= 0 || GameObject.PositionX + MovementX <= 0x6000)
            return false;

        Redirect(random, -0x100, 0);
        return true;
    }

    private bool ApplyAttractionOrBehaviorTwoRedirect(GameRandom random)
    {
        if (
            hasAttractionTarget
            && !isInsideAttractionTarget
            && Behavior is not 3
            && !hasAvoidanceTarget
            && ChecksumByte0 is not 3
        )
        {
            AddNormalizedMovement(TargetY, ChecksumState10, 4);
            return false;
        }

        if (Behavior is not 2)
            return false;
        if (MovementX < 0 && GameObject.PositionX + MovementX < 0)
        {
            Redirect(random, 0x100, 0);
            return true;
        }

        if (MovementY < 0 && GameObject.PositionY + MovementY < 0)
        {
            Redirect(random, 0, 0x100);
            return true;
        }

        if (MovementX <= 0 || GameObject.PositionX + MovementX <= 0x6000)
            return false;

        Redirect(random, -0x100, 0);
        return true;
    }

    private void CompleteMovement(GameRandom random)
    {
        if (Behavior is 0)
        {
            ChecksumState0 = Math.Clamp(
                (ChecksumState0 + (MovementX - MovementY) / 3) * 9 / 10,
                -80,
                80
            );
        }

        var previousAltitude = ChecksumState1;
        ChecksumState1 = Math.Clamp(ChecksumState1 + ChecksumState6, 0, 792);
        if (Behavior is 1 && previousAltitude is not 0 && ChecksumState1 is 0)
        {
            ChecksumState4 = 90;
            ChecksumState6 = 0;
        }

        RefreshLandingTarget();
        if (ChecksumState1 is 0)
            UpdateGroundMovement(random);

        UpdateMirroring();
        ChecksumFlag1 = isInsideLandingTarget;
        ChecksumState11 = 0;
        ChecksumState12 = 0;
        GameObject.MoveTo(GameObject.PositionX + MovementX, GameObject.PositionY + MovementY);
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

        return change * speedMultiplier / 100;
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
        Speed = Speed * speedMultiplier / 100;
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

    private int GetHeadingChange(GameRandom random)
    {
        return Behavior switch
        {
            0 => random.NextInt(10) - 4,
            1 => GetBirdHeadingChange(random),
            2 or 3 or 4 => 0,
            _ => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unsupported ambient-animal behavior {Behavior}."
                )
            ),
        };
    }

    private int GetBirdHeadingChange(GameRandom random)
    {
        if (random.NextInt(3) is 0)
            return random.NextInt(19) - 10;

        if (ChecksumState1 < 400)
            return random.NextInt(6) is 0 ? -14 : 14;

        return random.NextInt(6) is 0 ? 14 : -14;
    }

    private int GetNextHeadingChangeTimer(GameRandom random)
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
        ChecksumByte0 = 3;
        Speed = 16;
        ChecksumState2 = GetRedirectDuration(random);
        Heading = (random.NextInt(180) + IntegerMath.GetVectorAngle(x, y) - 90) << 3;
        var previousRedirectCount = ChecksumState15;
        ChecksumState15 = unchecked(ChecksumState15 + 1);

        if (Behavior is 3)
            redirectRefreshPending = true;
        else if (previousRedirectCount > 9)
            attractionPoints = [];
    }

    private int ResolveBehaviorThreeHeading(GameRandom random)
    {
        if (hasAvoidanceTarget)
            return IntegerMath.GetVectorAngle(ChecksumState7, ChecksumState8) << 3;

        if (!ChecksumFlag3)
            return random.NextInt(360) << 3;

        return hasAttractionTarget
            ? IntegerMath.GetVectorAngle(TargetY, ChecksumState10) << 3
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
        var landingTimer = ChecksumState5;
        ChecksumState5++;

        if (landingTimer > 83)
        {
            ChecksumByte0 = 3;
            DestinationX = 0;
            DestinationY = 0;
            return;
        }

        MovementX = ChecksumState9 / 85;
        MovementY = TargetX / 85;
        GameObject.MoveTo(GameObject.PositionX + MovementX, GameObject.PositionY + MovementY);
    }

    private void BeginTakeoff(GameRandom random)
    {
        ChecksumState5 = 0;
        ChecksumByte0 = 5;
        ChecksumState1 = 8;
        ChecksumState6 = random.NextInt(8) + 2;
        ChecksumState4 = 25;

        var heading = random.NextInt(360);

        for (var attempts = 359; attempts > 0 && IsDisallowedTakeoffHeading(heading); attempts--)
            heading = (heading + 1) % 360;

        Heading = heading << 3;
    }

    private void UpdateTakeoffMovement()
    {
        ChecksumState5++;
        Heading += HeadingStep;

        var progress = ChecksumState5 * 4096 / 110;
        var movementSpeed = Speed * progress / 4096 + 1;
        var angle = Heading / 8;
        MovementX = IntegerMath.GetSine(angle + 90) * movementSpeed / 1024;
        MovementY = IntegerMath.GetSine(angle) * movementSpeed / 1024;
        GameObject.MoveTo(GameObject.PositionX + MovementX, GameObject.PositionY + MovementY);

        UpdateMirroring();

        if (ChecksumState5 >= 110)
            ChecksumByte0 = 0;
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
                (landingPoints.Count is 0 || isInsideLandingTarget && !ChecksumFlag1)
                && GameObject.PositionX + MovementX > 0
                && !hasAvoidanceTarget
            )
            {
                ChecksumByte0 = 3;
                ChecksumState2 = GetRedirectDuration(random);
                return;
            }

            ChecksumState6 = random.NextInt(8) + 2;
            ChecksumState4 = 25;
            ChecksumState1 = 8;
            return;
        }

        var destinationReached = isInsideLandingTarget && IsLandingDestinationReached();

        if (
            Behavior is not 1
            || !isInsideLandingTarget
            || ChecksumFlag1 && !destinationReached
            || GameObject.PositionX + MovementX <= 0
            || hasAvoidanceTarget
        )
        {
            return;
        }

        var targetAngle = IntegerMath.GetVectorAngle(ChecksumState9, TargetX);
        var angleDifference = Math.Abs(IntegerMath.GetAngleDifference(Heading / 8 - targetAngle));

        if (angleDifference >= 30)
            return;

        ChecksumByte0 = 4;
        ChecksumState5 = 0;
        ChecksumState2 = GetRedirectDuration(random);
    }

    private bool IsLandingDestinationReached()
    {
        if (DestinationX is 0 && DestinationY is 0)
            return false;

        var x = ChecksumState9 + GameObject.PositionX + MovementX;
        var y = TargetX + GameObject.PositionY + MovementY;
        var reached = Math.Abs(x - DestinationX) < 100 && Math.Abs(y - DestinationY) < 100;

        if (reached)
            ChecksumState6 = -32;

        return reached;
    }
}
