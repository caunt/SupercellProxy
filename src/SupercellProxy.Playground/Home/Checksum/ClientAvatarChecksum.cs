using System.Globalization;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home.Checksum;

internal static class ClientAvatarChecksum
{
    public static void EncodeAbbreviated(ChecksumEncoder encoder, HomeState state)
    {
        EnsureNoUnsupportedFarmPassCapacityModifiers(state);

        var clientAvatar = state.ClientAvatar;
        encoder.WriteVarInt(state.AvatarTimestamp);
        WriteCurrentChecksum(encoder);
        encoder.WriteLongId(clientAvatar.HomeId);
        encoder.WriteLongId(clientAvatar.AccountId);
        encoder.WriteVarInt(clientAvatar.AvatarVersion);
        WriteCurrentChecksum(encoder);
        EncodeStorageCapacities(encoder, state);
        WriteCurrentChecksum(encoder);
        EncodeHelperCapacities(encoder, state);
        WriteCurrentChecksum(encoder);
    }

    private static void EncodeStorageCapacities(ChecksumEncoder encoder, HomeState state)
    {
        encoder.WriteVarInt(
            ResolveGroupedCapacity(state, "SiloRank", GameAssetFiles.Silos, "Silo", "Capacity")
        );
        encoder.WriteVarInt(
            ResolveGroupedCapacity(
                state,
                "WarehouseRank",
                GameAssetFiles.Warehouses,
                "Shed",
                "Capacity"
            )
        );
    }

    private static void EncodeHelperCapacities(ChecksumEncoder encoder, HomeState state)
    {
        encoder.WriteVarInt(
            ResolveGroupedCapacity(
                state,
                "TackleBoxRank",
                GameAssetFiles.TackleBox,
                "TackleBox",
                "Capacity"
            )
        );
        encoder.WriteVarInt(
            ResolveGroupedCapacity(
                state,
                "LobsterPoolRank",
                GameAssetFiles.LobsterPool,
                "LobsterPool",
                "Capacity"
            )
        );
        encoder.WriteVarInt(
            ResolveGroupedCapacity(
                state,
                "DuckSalonRank",
                GameAssetFiles.DuckSalon,
                "DuckSalon",
                "Capacity"
            )
        );
        WriteCurrentChecksum(encoder);
        encoder.WriteVarInt(
            ResolveCapacity(
                state,
                GameAssetFiles.Money,
                "ExpLevel",
                GameAssetFiles.ExperienceLevels,
                "CaretakerStorageCapacity"
            )
        );
        encoder.WriteVarInt(
            ResolveCapacity(
                state,
                GameAssetFiles.Money,
                "ExpLevel",
                GameAssetFiles.ExperienceLevels,
                "MillerStorageCapacity"
            )
        );
    }

    private static int ResolveGroupedCapacity(
        HomeState state,
        string rankName,
        string capacityFile,
        string capacityName,
        string capacityField
    )
    {
        if (!state.DataTableResolver.TryResolve(GameAssetFiles.Money, rankName, out var rankData))
            throw new InvalidDataException($"Missing native capacity data for {capacityName}.");

        var rank = state.Inventory.GetTotalValue(rankData);

        if (
            !state.DataTableResolver.TryResolveValueCount(
                capacityFile,
                capacityName,
                capacityField,
                out var rowCount
            )
            || !state.DataTableResolver.TryResolveInt(
                capacityFile,
                capacityName,
                capacityField,
                Math.Clamp(rank, 1, rowCount) - 1,
                out var capacity
            )
        )
        {
            throw new InvalidDataException(
                $"Missing capacity data {capacityField} for {capacityName}."
            );
        }

        return capacity;
    }

    public static void EncodeFull(ChecksumEncoder encoder, HomeState state)
    {
        EncodeFull(encoder, state.ClientAvatar, state.Inventory, state.ShopEventManager.Snapshot);
    }

    public static void EncodeFull(ChecksumEncoder encoder, ClientAvatar clientAvatar)
    {
        EncodeFull(
            encoder,
            clientAvatar,
            InventoryState.Create(clientAvatar),
            clientAvatar.UnknownManager0
        );
    }

    private static void EncodeFull(
        ChecksumEncoder encoder,
        ClientAvatar clientAvatar,
        InventoryState inventory,
        AvatarManagerA shopEventManager
    )
    {
        EncodeIdentityAndCollections(encoder, clientAvatar, inventory);
        EncodeLeagueAndManagers(encoder, clientAvatar, shopEventManager);
        EncodeAvatarTail(encoder, clientAvatar);
    }

    private static void EncodeIdentityAndCollections(
        ChecksumEncoder encoder,
        ClientAvatar clientAvatar,
        InventoryState inventory
    )
    {
        encoder.WriteVarInt(clientAvatar.Unknown0);
        encoder.WriteVarInt(clientAvatar.Unknown1);
        encoder.WriteVarInt(clientAvatar.AvatarVersion);
        encoder.WriteVarInt(clientAvatar.Unknown3);
        encoder.WriteNullableString(clientAvatar.Name);
        encoder.WriteLongId(clientAvatar.HomeId);
        encoder.WriteLongId(clientAvatar.AccountId);
        EncodeInventory(encoder, inventory);
        EncodeRoadsideShop(encoder, clientAvatar.RoadsideShop);
        EncodeNeighborhood(encoder, clientAvatar.Neighborhood);
        EncodeArray(encoder, clientAvatar.MailEntries, EncodeMailEntry);
        EncodeArray(
            encoder,
            clientAvatar.UnknownValues0,
            static (valueEncoder, value) => valueEncoder.WriteVarInt(value)
        );
        EncodeArray(encoder, clientAvatar.UnknownEntries0, EncodeAvatarEntryA);
        encoder.WriteBoolean(clientAvatar.TrainStationReady);
        encoder.WriteBoolean(clientAvatar.IsMuted);
        encoder.WriteBoolean(clientAvatar.CanEditFarm);
        EncodeArray(encoder, clientAvatar.UnknownEntries1, EncodeAvatarEntryB);
        EncodeArray(encoder, clientAvatar.PickedPassengers, EncodePickedPassenger);
        EncodeArray(encoder, clientAvatar.UnknownEntries2, EncodeAvatarEntryC);
        EncodeArray(encoder, clientAvatar.UnknownEntries3, EncodeAvatarEntryC);
    }

    private static void EncodeLeagueAndManagers(
        ChecksumEncoder encoder,
        ClientAvatar clientAvatar,
        AvatarManagerA shopEventManager
    )
    {
        if (
            clientAvatar.UnknownValues1.Length is not 11
            || clientAvatar.UnknownValues2.Length is not 6
        )
            throw new InvalidOperationException("Unexpected fixed avatar field count.");
        encoder.WriteVarInt(clientAvatar.UnknownNullableListCount);
        if (clientAvatar.UnknownNullableListCount > 0)
            throw new InvalidOperationException(
                "Cannot checksum the unsupported polymorphic avatar section."
            );

        EncodeOptionalLongId(encoder, clientAvatar.UnknownOptionalId0);
        EncodeOptionalLongId(encoder, clientAvatar.UnknownOptionalId1);
        encoder.WriteVarInt(clientAvatar.LeagueType);
        encoder.WriteVarInt(clientAvatar.UnknownLeagueValue);
        encoder.WriteVarInt(clientAvatar.LeagueScore);

        foreach (var value in clientAvatar.UnknownValues1)
            encoder.WriteVarInt(value);

        EncodeAvatarManagerA(encoder, shopEventManager);
        EncodeAvatarStringManager(encoder, clientAvatar.UnknownManager1);

        foreach (var value in clientAvatar.UnknownValues2)
            encoder.WriteVarInt(value);

        EncodeOptionalLongId(encoder, clientAvatar.MapGameId);
        EncodeOptionalLongId(encoder, clientAvatar.UnknownOptionalId3);
        encoder.WriteVarInt(clientAvatar.Unknown4);

        if (clientAvatar.MapGameId is not null)
            encoder.WriteBoolean(value: false);
    }

    private static void EncodeAvatarTail(ChecksumEncoder encoder, ClientAvatar clientAvatar)
    {
        encoder.WriteVarInt(clientAvatar.Unknown5);
        encoder.WriteString(clientAvatar.UnknownString0 ?? string.Empty);
        encoder.WriteBoolean(clientAvatar.StorePromotionAllowed);
        encoder.WriteString(clientAvatar.UnknownString1 ?? string.Empty);
        encoder.WriteBoolean(clientAvatar.UnknownBoolean1);
        EncodeAvatarManagerB(encoder, clientAvatar.UnknownManager2);
        EncodeSettings(encoder, clientAvatar.Settings);
    }

    public static void EncodeInventory(ChecksumEncoder encoder, ClientAvatar clientAvatar)
    {
        EncodeInventory(encoder, InventoryState.Create(clientAvatar));
    }

    public static void EncodeInventory(ChecksumEncoder encoder, InventoryState inventory)
    {
        foreach (var values in inventory.Values)
        {
            encoder.WriteVarInt(values.Length);

            foreach (var value in values)
                encoder.WriteVarInt(value);
        }

        foreach (var entries in inventory.DataReferenceValues)
            EncodeDataReferenceInventory(encoder, entries);

        encoder.WriteVarInt(inventory.DeprecatedDataCount);
        encoder.WriteVarInt(inventory.Unknown0);
    }

    private static void EncodeDataReferenceInventory(
        ChecksumEncoder encoder,
        IReadOnlyDictionary<int, int> entries
    )
    {
        var nonzeroValues = entries
            .Where(static entry => entry.Value is not 0)
            .OrderBy(static entry => entry.Key)
            .ToArray();

        encoder.WriteVarInt(nonzeroValues.Length);

        foreach (var (globalDataId, value) in nonzeroValues)
        {
            encoder.WriteVarInt(globalDataId);
            encoder.WriteVarInt(value);
        }
    }

    private static void EncodeRoadsideShop(ChecksumEncoder encoder, RoadsideShopEntry[] entries)
    {
        EncodeArray(
            encoder,
            entries,
            static (entryEncoder, entry) =>
            {
                EncodeOptionalLongId(entryEncoder, entry.BuyerId);
                entryEncoder.WriteBoolean(entry.IsSold);
                entryEncoder.WriteVarInt(entry.Price);
                entryEncoder.WriteVarInt(entry.Quantity);
                entryEncoder.WriteVarInt(entry.ItemGlobalId);
            }
        );
    }

    private static void EncodeNeighborhood(ChecksumEncoder encoder, NeighborhoodData? neighborhood)
    {
        encoder.WriteBoolean(neighborhood is not null);

        if (neighborhood is null)
            return;

        encoder.WriteLongId(neighborhood.NeighborhoodId);
        encoder.WriteNullableString(neighborhood.NeighborhoodName);
        encoder.WriteVarInt(neighborhood.NeighborhoodRole);
        encoder.WriteVarInt(neighborhood.BadgeUnknown0);
        encoder.WriteVarInt(neighborhood.BadgeUnknown1);
        encoder.WriteVarInt(neighborhood.BadgeUnknown2);
        encoder.WriteVarInt(neighborhood.Unknown0);
        encoder.WriteVarInt(neighborhood.Unknown1);
        encoder.WriteVarInt(neighborhood.Unknown2);
    }

    private static void EncodeMailEntry(ChecksumEncoder encoder, MailEntry entry)
    {
        encoder.WriteVarInt(entry.Unknown0);
        encoder.WriteVarInt(entry.Unknown1);
        encoder.WriteVarLong(entry.Unknown2);
        encoder.WriteNullableString(entry.SenderAvatarName);
        encoder.WriteVarInt(entry.Unknown3);
        encoder.WriteVarInt(entry.Unknown4);
        encoder.WriteVarInt(entry.Unknown5);
        encoder.WriteVarInt(entry.Unknown6);
        encoder.WriteVarInt(entry.Unknown7);
        encoder.WriteNullableString(entry.Subject);
        encoder.WriteNullableString(entry.Body);
        encoder.WriteVarInt(entry.Unknown8);
        encoder.WriteNullableString(entry.FacebookId);
        encoder.WriteNullableString(entry.GameCenterId);
        encoder.WriteVarInt(entry.Unknown9);
        encoder.WriteVarInt(entry.Unknown10);
        encoder.WriteVarInt(entry.Unknown11);
        encoder.WriteVarInt(entry.Unknown12);
        encoder.WriteVarInt(entry.Unknown13);
        encoder.WriteVarInt(entry.Unknown14);
        encoder.WriteNullableString(entry.CustomSubject);
        encoder.WriteNullableString(entry.CustomBody);
        encoder.WriteVarInt(entry.Unknown15);
        encoder.WriteVarInt(entry.Unknown16);
        encoder.WriteNullableString(entry.UnknownString0);
        encoder.WriteNullableString(entry.UnknownString1);
    }

    private static void EncodeAvatarEntryA(ChecksumEncoder encoder, AvatarEntryA entry)
    {
        encoder.WriteVarInt(entry.Unknown0);
        encoder.WriteVarInt(entry.Unknown1);
        encoder.WriteVarInt(entry.Unknown2);
        EncodeOptionalLongId(encoder, entry.UnknownId);
    }

    private static void EncodeAvatarEntryB(ChecksumEncoder encoder, AvatarEntryB entry)
    {
        EncodeOptionalLongId(encoder, entry.UnknownId);
        encoder.WriteVarInt(entry.Unknown0);
        encoder.WriteBoolean(entry.Unknown1);
    }

    private static void EncodePickedPassenger(ChecksumEncoder encoder, PickedPassenger passenger)
    {
        encoder.WriteVarInt(passenger.Unknown0);
        encoder.WriteVarInt(passenger.Unknown1);
        encoder.WriteVarInt(passenger.Unknown2);
        encoder.WriteLongId(passenger.UnknownId0);
        encoder.WriteLongId(passenger.UnknownId1);
        encoder.WriteNullableString(passenger.UnknownString0);
    }

    private static void EncodeAvatarEntryC(ChecksumEncoder encoder, AvatarEntryC entry)
    {
        EncodeOptionalLongId(encoder, entry.UnknownId);
        encoder.WriteVarInt(entry.Unknown0);
        encoder.WriteVarInt(entry.Unknown1);
        encoder.WriteBoolean(entry.Unknown2);
    }

    private static void EncodeAvatarManagerA(ChecksumEncoder encoder, AvatarManagerA manager)
    {
        encoder.WriteVarInt(manager.Version);
        encoder.WriteBoolean(manager.Optional is not null);

        if (manager.Optional is { } optional)
        {
            encoder.WriteVarInt(optional.Unknown0);
            EncodeArray(encoder, optional.Entries, EncodeAvatarManagerASpecial);
        }

        EncodeArray(
            encoder,
            manager.FixedValues.OrderBy(static entry => entry.Key),
            static (entryEncoder, entry) =>
            {
                entryEncoder.WriteVarInt(entry.Key);
                entryEncoder.WriteInt32(entry.Value);
            }
        );
        EncodeArray(
            encoder,
            manager.Pairs.OrderBy(static entry => entry.Key),
            static (entryEncoder, entry) =>
            {
                entryEncoder.WriteVarInt(entry.Key);
                entryEncoder.WriteVarInt(entry.Value);
            }
        );
        EncodeArray(
            encoder,
            manager.UnknownValues0,
            static (valueEncoder, value) => valueEncoder.WriteVarInt(value)
        );
        EncodeArray(
            encoder,
            manager.UnknownValues1,
            static (valueEncoder, value) => valueEncoder.WriteVarInt(value)
        );
        EncodeArray(
            encoder,
            manager.Strings.OrderBy(static entry => entry.Key),
            static (entryEncoder, entry) =>
            {
                entryEncoder.WriteVarInt(entry.Key);
                entryEncoder.WriteString(entry.Value ?? string.Empty);
            }
        );
        EncodeArray(encoder, manager.UnknownEntries0, EncodeAvatarManagerAItem);
        EncodeArray(encoder, manager.UnknownEntries1, EncodeAvatarManagerAItem);
        EncodeArray(
            encoder,
            manager.Triples,
            static (entryEncoder, entry) =>
            {
                entryEncoder.WriteVarInt(entry.Unknown0);
                entryEncoder.WriteVarInt(entry.Unknown1);
                entryEncoder.WriteVarInt(entry.Unknown2);
            }
        );
    }

    private static void EncodeAvatarManagerASpecial(
        ChecksumEncoder encoder,
        AvatarManagerASpecial entry
    )
    {
        if (entry.UnknownValues.Length is not 11)
            throw new InvalidOperationException("Unexpected manager field count.");

        encoder.WriteBoolean(entry.UsesCompressedData);

        if (entry.UsesCompressedData)
            encoder.WriteNullableByteArray(entry.CompressedData);
        else
            encoder.WriteNullableString(entry.Text);

        encoder.WriteVarInt(entry.Unknown0);
        encoder.WriteVarInt(entry.Unknown1);
        encoder.WriteString(entry.UnknownString0 ?? string.Empty);

        foreach (var value in entry.UnknownValues)
            encoder.WriteVarInt(value);

        encoder.WriteString(entry.UnknownString1 ?? string.Empty);
    }

    private static void EncodeAvatarManagerAItem(ChecksumEncoder encoder, AvatarManagerAItem entry)
    {
        encoder.WriteVarInt(entry.Unknown0);
        encoder.WriteVarInt(entry.Kind);
        encoder.WriteVarInt(entry.Unknown1);

        if (entry.Kind is 1)
            encoder.WriteVarInt(
                entry.KindValue
                    ?? throw new InvalidOperationException($"{nameof(entry.KindValue)} is null.")
            );

        encoder.WriteVarInt(entry.Unknown2);
    }

    private static void EncodeAvatarStringManager(
        ChecksumEncoder encoder,
        AvatarStringManager manager
    )
    {
        encoder.WriteString(manager.UnknownString0 ?? string.Empty);
        encoder.WriteString(manager.UnknownString1 ?? string.Empty);
        encoder.WriteString(manager.UnknownString2 ?? string.Empty);
    }

    private static void EncodeAvatarManagerB(ChecksumEncoder encoder, AvatarManagerB manager)
    {
        encoder.WriteVarInt(manager.Version);

        if (manager.Version <= 0)
            return;

        EncodeArray(
            encoder,
            manager.Entries,
            static (entryEncoder, entry) =>
            {
                entryEncoder.WriteInt64(entry.Unknown0);
                entryEncoder.WriteVarInt(entry.Unknown1);
                entryEncoder.WriteVarInt(entry.Unknown2);
                EncodeArray(
                    entryEncoder,
                    entry.Values,
                    static (valueEncoder, value) =>
                    {
                        valueEncoder.WriteInt32(value.Key);
                        valueEncoder.WriteVarInt(value.Value);
                    }
                );
            }
        );
        EncodeAvatarManagerBState(encoder, manager.State);
        EncodeArray(
            encoder,
            manager.UnknownEntries0.OrderBy(static entry => entry.Key),
            EncodeAvatarManagerBMapEntry
        );
        EncodeArray(
            encoder,
            manager.UnknownEntries1.OrderBy(static entry => entry.Key),
            EncodeAvatarManagerBMapEntry
        );
    }

    private static void EncodeAvatarManagerBMapEntry(
        ChecksumEncoder encoder,
        AvatarManagerBMapEntry entry
    )
    {
        encoder.WriteVarInt(entry.Key);
        EncodeAvatarManagerBState(encoder, entry.State);
    }

    private static void EncodeAvatarManagerBState(
        ChecksumEncoder encoder,
        AvatarManagerBState state
    )
    {
        encoder.WriteVarInt(state.Unknown0);
        encoder.WriteVarInt(state.Unknown1);
        encoder.WriteVarInt(state.Unknown2);
        encoder.WriteInt64(state.Unknown3);
    }

    private static void EncodeSettings(ChecksumEncoder encoder, AvatarSettings? settings)
    {
        encoder.WriteBoolean(settings is not null);

        if (settings is null)
            return;

        if (settings.Entries.Length is not 9)
            throw new InvalidOperationException("Unexpected avatar setting count.");

        encoder.WriteVarInt(settings.Version);
        encoder.WriteVarInt(9);

        foreach (var setting in settings.Entries)
        {
            encoder.WriteBoolean(setting.Enabled);
            encoder.WriteVarInt(setting.Value);
        }

        encoder.WriteBoolean(settings.Unknown0);
    }

    private static void EncodeOptionalLongId(ChecksumEncoder encoder, LongId? value)
    {
        encoder.WriteBoolean(value is not null);

        if (value is not null)
            encoder.WriteLongId(value.Value);
    }

    private static int ResolveCapacity(
        HomeState state,
        string rankFile,
        string rankName,
        string capacityFile,
        string capacityField
    )
    {
        if (!state.DataTableResolver.TryResolve(rankFile, rankName, out var rankData))
            throw new InvalidDataException($"Unable to resolve {rankName} from {rankFile}.");

        var rank = state.Inventory.GetTotalValue(rankData);

        if (
            !state.DataTableResolver.TryResolvePhysicalRowCount(capacityFile, out var rowCount)
            || rowCount < 1
        )
            throw new InvalidDataException($"Unable to resolve physical rows from {capacityFile}.");

        var rowIndex = Math.Clamp(rank, 1, rowCount) - 1;

        if (
            !state.DataTableResolver.TryResolveInt(
                capacityFile,
                rowIndex,
                capacityField,
                out var capacity
            )
        )
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unable to resolve {capacityField} from physical row {rowIndex} of {capacityFile}."
                )
            );

        return capacity;
    }

    private static void EnsureNoUnsupportedFarmPassCapacityModifiers(HomeState state)
    {
        var farmPass = state.AvatarData.AvatarDataObjects.Common.FarmPassManager;

        if (farmPass is null)
            return;

        foreach (var perk in farmPass.Perks)
        {
            if (
                !state.DataTableResolver.TryResolve(perk.PerkDataId, out var perkData)
                || !state.DataTableResolver.TryResolveString(
                    perk.PerkDataId,
                    "Type",
                    out var perkType
                )
            )
            {
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Unable to resolve Farm Pass perk data global ID {perk.PerkDataId}."
                    )
                );
            }

            var mayModifyCapacity = perkType switch
            {
                "ExtraSiloCapacity" or "ExtraWarehouseCapacity" => perk.Active,
                "ExtraSiloCapacityPercent" or "ExtraBarnCapacityPercent" => perk.Active
                    || perk.SubscriptionPerk,
                _ => false,
            };

            if (mayModifyCapacity)
                throw new InvalidOperationException(
                    $"Farm Pass capacity perk {perkData.Name} is not supported by the abbreviated avatar checksum."
                );
        }
    }

    private static void WriteCurrentChecksum(ChecksumEncoder encoder)
    {
        encoder.WriteVarInt(encoder.Checksum);
    }

    private static void EncodeArray<T>(
        ChecksumEncoder encoder,
        IEnumerable<T> values,
        Action<ChecksumEncoder, T> encode
    )
    {
        var array = values as T[] ?? values.ToArray();
        encoder.WriteVarInt(array.Length);

        foreach (var value in array)
            encode(encoder, value);
    }
}
