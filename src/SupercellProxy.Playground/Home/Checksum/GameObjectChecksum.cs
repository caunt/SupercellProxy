namespace SupercellProxy.Playground.Home.Checksum;

internal static class GameObjectChecksum
{
    public static void EncodeBase(ChecksumEncoder encoder, GameObjectState gameObject)
    {
        encoder.WriteVarInt(gameObject.PositionX);
        encoder.WriteVarInt(gameObject.PositionY);
        encoder.WriteVarInt(gameObject.GlobalId);
        encoder.WriteVarInt(gameObject.Data.GlobalId);
        EncodeDimensions(encoder, gameObject);
        encoder.WriteBoolean(gameObject.Mirrored);
        EncodeBoosterList(encoder, gameObject.Snapshot.BoosterList);
        encoder.WriteVarInt(gameObject.Parent?.GlobalId ?? 0);
    }

    public static void EncodeSecondaryBase(ChecksumEncoder encoder, GameObjectState gameObject)
    {
        encoder.WriteVarInt(gameObject.GlobalId);
        encoder.WriteVarInt(gameObject.Data.GlobalId);
        encoder.WriteVarInt(gameObject.PositionX);
        encoder.WriteVarInt(gameObject.PositionY);
        EncodeDimensions(encoder, gameObject);
        encoder.WriteBoolean(gameObject.Mirrored);
        encoder.WriteVarInt(gameObject.Parent?.GlobalId ?? 0);
    }

    public static void EncodeSecondaryCar(ChecksumEncoder encoder, CarState car)
    {
        EncodeSecondaryBase(encoder, car.GameObject);
        encoder.WriteVarInt(car.State);
        encoder.WriteVarInt(car.ChecksumState0);
        encoder.WriteVarInt(car.ChecksumState1);
        encoder.WriteVarInt(car.RewardAmount);
        encoder.WriteVarInt(car.RewardCount);
        encoder.WriteVarInt(car.RewardType);
        EncodeCarPath(encoder, car.Path0);
        EncodeCarPath(encoder, car.Path1);
    }

    public static void EncodeSecondaryField(ChecksumEncoder encoder, FieldState field)
    {
        EncodeSecondaryBase(encoder, field.GameObject);
        encoder.WriteVarInt(field.GrowthTimer.StartSeconds);
        encoder.WriteVarInt(field.GrowthTimer.TicksLeft);
        encoder.WriteBoolean(field.IsHarvestStarted);
        encoder.WriteBoolean(field.IsHarvestGainApplied);
    }

    public static void EncodeSecondaryAmbientAnimal(
        ChecksumEncoder encoder,
        AmbientAnimalState animal
    )
    {
        EncodeSecondaryBase(encoder, animal.GameObject);
        encoder.WriteVarInt(animal.Heading);
        encoder.WriteVarInt(animal.SteeringState);
        encoder.WriteVarInt(animal.Altitude);
        encoder.WriteVarInt(animal.Speed);
        encoder.WriteVarInt(animal.MovementTimer);
        encoder.WriteVarInt(animal.SpeedChangeTimer);
        encoder.WriteVarInt(animal.AltitudeStepChangeTimer);
        encoder.WriteVarInt(animal.PhaseTimer);
        encoder.WriteVarInt(animal.HeadingStep);
        encoder.WriteVarInt(animal.AltitudeStep);
        encoder.WriteVarInt(animal.MovementState);
        encoder.WriteVarInt(animal.Behavior);
        encoder.WriteVarInt(animal.AvoidanceX);
        encoder.WriteVarInt(animal.AvoidanceY);
        encoder.WriteVarInt(animal.LandingX);
        encoder.WriteVarInt(animal.LandingY);
        encoder.WriteVarInt(animal.AttractionX);
        encoder.WriteVarInt(animal.AttractionY);
        encoder.WriteVarInt(animal.CleanupDriftX);
        encoder.WriteVarInt(animal.CleanupDriftY);
        encoder.WriteVarInt(animal.AvoidanceLinger);
        encoder.WriteVarInt(animal.RedirectCount);
        encoder.WriteVarInt(animal.MirrorTimer);
        encoder.WriteBoolean(animal.IsRemoved);
        encoder.WriteBoolean(animal.WasInsideLandingTarget);
        encoder.WriteBoolean(animal.HasAttractionTarget);
        encoder.WriteBoolean(animal.ZoneCleanup);
        EncodeIntPair(encoder, new IntPair(animal.MovementX, animal.MovementY));
        encoder.WriteVarInt(animal.CachedAvoidanceIndex);
    }

    public static void EncodeSecondaryAmbientAnimalSpawner(
        ChecksumEncoder encoder,
        AmbientAnimalSpawnerState spawner
    )
    {
        EncodeSecondaryBase(encoder, spawner.GameObject);
        encoder.WriteVarInt(spawner.SelectedZone);
        encoder.WriteVarInt(spawner.ConstructorRandomValue);
        encoder.WriteBoolean(AmbientAnimalSpawnerState.HasParent);
        encoder.WriteBoolean(AmbientAnimalSpawnerState.HasObjectManager);
        encoder.WriteVarInt(AmbientAnimalSpawnerState.ZoneCount);
        encoder.WriteBoolean(spawner.Initialized);
        encoder.WriteBoolean(spawner.RefreshPending);
        encoder.WriteVarInt(spawner.ChecksumState0);
        EncodeAmbientAnimalSpawnerPoints(encoder, spawner.Points0);
        EncodeAmbientAnimalSpawnerPoints(encoder, spawner.Points1);
        EncodeAmbientAnimalSpawnerPoints(encoder, spawner.Points2);

        for (var i = 0; i < AmbientAnimalSpawnerState.ZoneCount; i++)
        {
            EncodeAmbientAnimalSpawnerZone(encoder, spawner.ActiveZones[i]);
            EncodeAmbientAnimalSpawnerZone(encoder, spawner.TemplateZones[i]);
        }
    }

    public static void EncodeSecondaryDataFirstBase(
        ChecksumEncoder encoder,
        GameObjectState gameObject
    )
    {
        encoder.WriteVarInt(gameObject.GlobalId);
        encoder.WriteVarInt(gameObject.Data.GlobalId);
        encoder.WriteVarInt(gameObject.PositionX);
        encoder.WriteVarInt(gameObject.PositionY);
        EncodeDimensions(encoder, gameObject);
        encoder.WriteBoolean(gameObject.Mirrored);
        encoder.WriteVarInt(gameObject.Parent?.GlobalId ?? 0);
    }

    public static void EncodeSecondaryUpgradeable(
        ChecksumEncoder encoder,
        GameObjectState gameObject
    )
    {
        EncodeSecondaryBase(encoder, gameObject);
        encoder.WriteVarInt(gameObject.Snapshot.Rank);
        encoder.WriteBoolean(gameObject.Snapshot.BrokenParts is not null);
        EncodeTimer(encoder, TimerSnapshot.Decode(gameObject.Snapshot.UpgradeTimer));
        encoder.WriteBoolean(gameObject.Snapshot.UpgradeReady);
    }

    public static void EncodeAnimalHabitat(
        ChecksumEncoder encoder,
        AnimalHabitatState animalHabitat
    )
    {
        EncodeBase(encoder, animalHabitat.GameObject);
        encoder.WriteVarInt(animalHabitat.PieceCount);
        encoder.WriteVarInt(animalHabitat.AnimalCount);
        encoder.WriteVarInt(animalHabitat.PieceAndAnimalCount);
    }

    public static void EncodeSecondaryAnimalHabitat(
        ChecksumEncoder encoder,
        AnimalHabitatState animalHabitat
    )
    {
        EncodeSecondaryUpgradeable(encoder, animalHabitat.GameObject);
        encoder.WriteVarInt(animalHabitat.PieceCount);
        encoder.WriteVarInt(animalHabitat.AnimalCount);
        encoder.WriteVarInt(animalHabitat.PieceAndAnimalCount);
    }

    public static void EncodeSecondaryPostman(ChecksumEncoder encoder, GameObjectState postman)
    {
        EncodeSecondaryBase(encoder, postman);
        encoder.WriteVarInt(postman.Snapshot.State);

        var timer = TimerSnapshot.Decode(postman.Snapshot.Timer);
        encoder.WriteVarInt(timer.StartSeconds);
        encoder.WriteVarInt(timer.TicksLeft);
    }

    public static void EncodeSecondaryGathererNest(
        ChecksumEncoder encoder,
        GathererNestState gathererNest
    )
    {
        EncodeSecondaryBase(encoder, gathererNest.GameObject);
        encoder.WriteVarInt(gathererNest.GathererCount);
    }

    public static void EncodeSecondaryGathererHabitat(
        ChecksumEncoder encoder,
        GathererHabitatState gathererHabitat
    )
    {
        EncodeSecondaryDataFirstBase(encoder, gathererHabitat.GameObject);
        encoder.WriteVarInt(gathererHabitat.NestCount);
        encoder.WriteVarInt(gathererHabitat.GameObject.Snapshot.MasteryGatherCount);
    }

    public static void EncodeSecondaryGatherer(ChecksumEncoder encoder, GathererState gatherer)
    {
        EncodeSecondaryBase(encoder, gatherer.GameObject);
        encoder.WriteVarInt(gatherer.GathererMineIndex);
        encoder.WriteVarInt(gatherer.GathererNestIndex);
        encoder.WriteVarInt(gatherer.AiState);
        encoder.WriteVarInt(gatherer.ChecksumState0);
        encoder.WriteVarInt(gatherer.TargetX);
        encoder.WriteVarInt(gatherer.TargetY);
        encoder.WriteBoolean(gatherer.ChecksumFlag0);
        encoder.WriteBoolean(gatherer.ChecksumFlag1);
        encoder.WriteVarInt(gatherer.Timer.StartSeconds);
        encoder.WriteVarInt(gatherer.Timer.TicksLeft);
        EncodePath(encoder, gatherer.Path);
    }

    public static void EncodeSecondaryHelperCharacter(
        ChecksumEncoder encoder,
        HelperCharacterState helperCharacter
    )
    {
        EncodeSecondaryBase(encoder, helperCharacter.GameObject);
        encoder.WriteVarInt(helperCharacter.ChecksumState0);
        EncodePath(encoder, helperCharacter.Path);
        encoder.WriteVarInt(helperCharacter.ChecksumState1);
        encoder.WriteVarInt(helperCharacter.ChecksumState2);
        encoder.WriteVarInt(helperCharacter.ChecksumState3);
    }

    public static void EncodeSecondaryConstructionBuilding(
        ChecksumEncoder encoder,
        ConstructionBuildingState constructionBuilding
    )
    {
        EncodeSecondaryUpgradeable(encoder, constructionBuilding.GameObject);
        encoder.WriteVarInt(constructionBuilding.ConstructionTimer.StartSeconds);
        encoder.WriteVarInt(constructionBuilding.ConstructionTimer.TicksLeft);
        encoder.WriteBoolean(constructionBuilding.ChecksumFlag0);
        encoder.WriteVarInt(constructionBuilding.TargetData?.GlobalId ?? 0);
    }

    public static void EncodeSecondaryBoyBox(ChecksumEncoder encoder, BoyBoxState boyBox)
    {
        EncodeSecondaryDataFirstBase(encoder, boyBox.GameObject);
        encoder.WriteVarInt(boyBox.Item?.GlobalId ?? 0);
        encoder.WriteVarInt(boyBox.Count);
    }

    public static void EncodeSecondaryPhotographer(
        ChecksumEncoder encoder,
        PhotographerState photographer
    )
    {
        EncodeSecondaryBase(encoder, photographer.GameObject);
        encoder.WriteVarInt(
            Convert.ToInt32(photographer.State, System.Globalization.CultureInfo.InvariantCulture)
        );
        encoder.WriteVarInt(photographer.StateTimer);
        encoder.WriteVarInt(photographer.NextPoint);
        encoder.WriteVarInt(photographer.RuntimeStateA);
        encoder.WriteVarInt(photographer.RuntimeStateB);
        encoder.WriteBoolean(photographer.PathComplete);
        encoder.WriteBoolean(photographer.LifecycleEnabled);
        EncodeIntPair(encoder, photographer.MovementVector);
        EncodeIntPair(encoder, photographer.CandidateMovementVector);
        EncodeIntPair(encoder, photographer.PersistentMovementVector);
        encoder.WriteBoolean(PhotographerState.HasManager);
        EncodeIntPairs(encoder, photographer.EntryRoute);
        EncodeIntPairs(encoder, photographer.ExitRoute);
    }

    public static void EncodeSecondaryPerson(ChecksumEncoder encoder, PersonState person)
    {
        EncodeSecondaryBase(encoder, person.GameObject);
        EncodePersonFields(encoder, person);
    }

    public static void EncodePrimaryPerson(ChecksumEncoder encoder, PersonState person)
    {
        EncodeBase(encoder, person.GameObject);
        EncodePersonFields(encoder, person);
    }

    private static void EncodePersonFields(ChecksumEncoder encoder, PersonState person)
    {
        encoder.WriteVarInt(person.State);
        encoder.WriteVarInt(person.Timer);
        encoder.WriteVarInt(person.NextPoint);
        encoder.WriteVarInt(person.GoodAmount);
        encoder.WriteVarInt(person.Good.GlobalId);
        encoder.WriteVarInt(person.TargetX);
        encoder.WriteVarInt(person.TargetY);
        encoder.WriteVarInt(person.PaymentObjectAmount);
        encoder.WriteVarInt(person.PaymentObject.GlobalId);
        encoder.WriteVarInt(person.ExperienceReward);
        encoder.WriteBoolean(person.MovementComplete);
        encoder.WriteBoolean(person.Active);
    }

    public static void EncodeSecondaryOrderTable(
        ChecksumEncoder encoder,
        OrderTableState orderTable
    )
    {
        EncodeSecondaryDataFirstBase(encoder, orderTable.GameObject);

        for (var slot = 0; slot < orderTable.Orders.Length; slot++)
        {
            if (slot is 7)
                continue;

            EncodeOrder(encoder, orderTable.Orders[slot]);
        }
    }

    public static void EncodeSecondaryWheel(ChecksumEncoder encoder, WheelState wheel)
    {
        EncodeSecondaryBase(encoder, wheel.GameObject);
        encoder.WriteVarInt(wheel.State);
        encoder.WriteVarInt(wheel.ChecksumState0);
        encoder.WriteVarInt(wheel.LastInitDayIndex);
        encoder.WriteVarInt(wheel.JackpotCount);
        encoder.WriteVarInt(wheel.PrizeType);
        encoder.WriteVarInt(wheel.PrizeGlobalId);
        encoder.WriteVarInt(wheel.PrizeCount);
        encoder.WriteVarInt(wheel.BoughtSpins);
        encoder.WriteVarInt(wheel.NumSpins);
        encoder.WriteVarInt(wheel.LastSpinDayIndex);
        encoder.WriteVarInt(wheel.ConsecutiveSpinDays);
        encoder.WriteVarInt(wheel.BoughtSpinsDaily);
        encoder.WriteVarInt(wheel.FarmPassSpins);
        encoder.WriteVarInt(wheel.AdsSpins);
        encoder.WriteVarInt(wheel.Prizes.Length);
        encoder.WriteVarInt(wheel.SlotCount);

        foreach (var row in wheel.Prizes)
        {
            foreach (var prize in row)
                encoder.WriteVarInt(prize);
        }

        foreach (var row in wheel.Amounts)
        {
            foreach (var amount in row)
                encoder.WriteVarInt(amount);
        }
    }

    public static void EncodeSecondarySpawner(ChecksumEncoder encoder, SpawnerState spawner)
    {
        EncodeSecondaryBase(encoder, spawner.GameObject);
        encoder.WriteBoolean(spawner.PointsInitialized);
        encoder.WriteVarInt(spawner.SpawnTimer);
        encoder.WriteVarInt(spawner.SpawnInterval);
        EncodeRequiredIntPairs(encoder, spawner.Points0);
        EncodeRequiredIntPairs(encoder, spawner.Points1);
        encoder.WriteBoolean(SpawnerState.HasParent);
    }

    public static void EncodeSecondaryBoy(ChecksumEncoder encoder, BoyState boy)
    {
        EncodeSecondaryBase(encoder, boy.GameObject);
        encoder.WriteVarInt(boy.State);
        EncodeIntPair(encoder, boy.ChecksumPair0);
        EncodeIntPair(encoder, boy.ChecksumPair1);
        EncodeTimer(encoder, boy.HireTimer);
        EncodeTimer(encoder, boy.CooldownTimer);
        encoder.WriteBoolean(boy.ChecksumFlag0);
        encoder.WriteBoolean(boy.FreeReEngagementAvailable);
        encoder.WriteBoolean(boy.HireEnded);
        encoder.WriteBoolean(boy.ChecksumFlag1);
        encoder.WriteBoolean(boy.IntervalOfferActive);
        encoder.WriteBoolean(boy.ChecksumFlag2);
        encoder.WriteVarInt(boy.ChecksumState0);
    }

    private static void EncodeDimensions(ChecksumEncoder encoder, GameObjectState gameObject)
    {
        if (
            gameObject.TileWidth is not int tileWidth
            || gameObject.TileHeight is not int tileHeight
        )
            throw new InvalidOperationException(
                $"Dimensions for {gameObject.Data.File} are not implemented."
            );

        encoder.WriteVarInt(gameObject.Mirrored ? tileHeight : tileWidth);
        encoder.WriteVarInt(gameObject.Mirrored ? tileWidth : tileHeight);
    }

    private static void EncodeAmbientAnimalSpawnerPoints(
        ChecksumEncoder encoder,
        List<AmbientAnimalSpawnerPoint>[] rows
    )
    {
        encoder.WriteVarInt(rows.Length);

        foreach (var row in rows)
        {
            encoder.WriteVarInt(row.Count);

            foreach (var point in row)
            {
                encoder.WriteVarInt(point.X);
                encoder.WriteVarInt(point.Y);
                encoder.WriteVarInt(point.RadiusSquared);
            }
        }
    }

    private static void EncodeAmbientAnimalSpawnerZone(
        ChecksumEncoder encoder,
        AmbientAnimalSpawnerZoneState zone
    )
    {
        encoder.WriteBoolean(zone.SpawnCycleActive);
        encoder.WriteVarInt(zone.SpawnDelayThreshold);
        encoder.WriteVarInt(zone.CleanupDelayThreshold);
        encoder.WriteVarInt(zone.SpawnActivationCounter);
        encoder.WriteVarInt(zone.SpawnDelayCounter);
        encoder.WriteVarInt(zone.SpawnAttemptCounter);
        encoder.WriteVarInt(zone.CleanupDelayCounter);
        encoder.WriteVarInt(zone.CleanupActivationInterval);
        encoder.WriteVarInt(zone.CleanupPassCounter);
    }

    private static void EncodeBoosterList(ChecksumEncoder encoder, BoosterSnapshot[]? boosters)
    {
        if (boosters is null)
            return;

        encoder.WriteVarInt(boosters.Length);

        foreach (var booster in boosters)
        {
            if (booster.BoosterDataGlobalId is not 0)
                encoder.WriteVarInt(booster.BoosterDataGlobalId);

            encoder.WriteVarInt(booster.Timer.StartSeconds);
            encoder.WriteVarInt(booster.Timer.TicksLeft);
        }
    }

    private static void EncodePath(ChecksumEncoder encoder, PathState path)
    {
        encoder.WriteVarInt(path.ChecksumState2);
        encoder.WriteVarInt(path.ChecksumState0);
        encoder.WriteVarInt(path.ChecksumState1);
        encoder.WriteVarInt(path.ChecksumState7);
        encoder.WriteVarInt(path.ChecksumValues.Length);

        foreach (var value in path.ChecksumValues)
            encoder.WriteInt16(value);

        encoder.WriteVarInt(path.ChecksumCapacity);
        encoder.WriteVarInt(path.ChecksumState3);
        encoder.WriteVarInt(path.ChecksumState4);
        encoder.WriteVarInt(path.ChecksumState5);
        encoder.WriteVarInt(path.ChecksumState6);
    }

    private static void EncodeCarPath(ChecksumEncoder encoder, CarPathState path)
    {
        encoder.WriteVarInt(path.X);
        encoder.WriteVarInt(path.Y);
        encoder.WriteVarInt(path.PointIndex);
        encoder.WriteVarInt(path.ChecksumState0);
        encoder.WriteVarInt(path.Points.Length);

        foreach (var point in path.Points)
            encoder.WriteVarInt(point);
    }

    private static void EncodeIntPair(ChecksumEncoder encoder, IntPair pair)
    {
        encoder.WriteBoolean(value: true);
        encoder.WriteVarInt(pair.First);
        encoder.WriteVarInt(pair.Second);
    }

    private static void EncodeTimer(ChecksumEncoder encoder, TimerSnapshot timer)
    {
        encoder.WriteVarInt(timer.StartSeconds);
        encoder.WriteVarInt(timer.TicksLeft);
    }

    private static void EncodeIntPairs(ChecksumEncoder encoder, IntPair[]? pairs)
    {
        encoder.WriteBoolean(pairs is not null);

        if (pairs is null)
            return;

        encoder.WriteVarInt(pairs.Length);

        foreach (var pair in pairs)
            EncodeIntPair(encoder, pair);
    }

    private static void EncodeRequiredIntPairs(ChecksumEncoder encoder, IntPair[] pairs)
    {
        encoder.WriteVarInt(pairs.Length);

        foreach (var pair in pairs)
            EncodeIntPair(encoder, pair);
    }

    private static void EncodeOrder(ChecksumEncoder encoder, OrderState order)
    {
        encoder.WriteVarInt(order.Items.Length);

        foreach (var item in order.Items)
            encoder.WriteVarInt(item.GlobalId);

        encoder.WriteVarInt(order.Amounts.Length);

        foreach (var amount in order.Amounts)
            encoder.WriteVarInt(amount);

        encoder.WriteVarInt(order.Cash);
        encoder.WriteVarInt(order.Experience);
        encoder.WriteVarInt(order.Slot);
        encoder.WriteVarInt(order.Level);
        encoder.WriteBoolean(order.IsNew);
        encoder.WriteVarInt(order.Voucher);
        encoder.WriteVarInt(order.CashExperienceMultiplier);
        encoder.WriteBoolean(order.BonusRewardEnabled);
        encoder.WriteVarInt(order.BonusEventId);
        encoder.WriteVarInt(order.BonusReward?.GlobalId ?? 0);
        encoder.WriteVarInt(order.BonusCount);
        encoder.WriteVarInt(order.Receiver.GlobalId);
        encoder.WriteVarInt(order.Timer.StartSeconds);
        encoder.WriteVarInt(order.Timer.TicksLeft);
        encoder.WriteBoolean(order.ReviverAvatarId is not null);
        encoder.WriteVarInt(order.ChecksumState0);
        encoder.WriteVarInt(order.ChecksumState1);
        encoder.WriteBoolean(order.ChecksumFlag0);
        encoder.WriteBoolean(order.ChecksumFlag1);
        encoder.WriteBoolean(order.HasSeasonalCurrency);
        encoder.WriteBoolean(order.ChecksumFlag2);
    }
}
