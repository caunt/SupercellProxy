using SupercellProxy.Networking.Protocol.MessageEncoding;

using static SupercellProxy.Networking.Protocol.CommandEncoding.Registration.CommandRegistry;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

internal static class PrimitiveCommandSchemas
{
    internal static readonly CommandPrimitiveSchema[] Entries =
    [
        new(
            [
                512,
                CollectCalendarGiftCommandType,
                519,
                520,
                MarkCountyFairOpenedCommandType,
                538,
                556,
                558,
                559,
                565,
                597,
                602,
                605,
                611,
                617,
                618,
                MarkChainOfferSeenCommandType,
                ClaimDecoStickerBookCollectionRewardCommandType,
                627,
                629,
                632,
                651,
                656,
                BuyCropSeedsCommandType,
                667,
                693,
                696,
            ],
            new CommandFieldType[1],
            MessageDirection.Serverbound
        ),
        new( [ 561, 585, 591, 609, 673 ], [ CommandFieldType.Boolean ], MessageDirection.Serverbound ),
        new( [ 692 ], [ CommandFieldType.String ], MessageDirection.Serverbound ),
        new( [ 525, 625 ], [ CommandFieldType.Boolean, CommandFieldType.VarInt ], MessageDirection.Serverbound ),
        new( [ 679 ], [ CommandFieldType.String, CommandFieldType.VarInt ], MessageDirection.Serverbound ),
        new( [ 501, 523, 550, 568, 607, 620, 621, 642, 650, 655, 658 ], new CommandFieldType[2], MessageDirection.Serverbound ),
        new([ 608, 666 ], new CommandFieldType[3], MessageDirection.Serverbound),
        new( [ 839 ], [ CommandFieldType.Boolean, CommandFieldType.Boolean ], MessageDirection.Clientbound ),
        new( [ 130, SetBoyOfferFlagCommandType, 196 ], [ CommandFieldType.Boolean ], MessageDirection.Serverbound, baseFirst: false ),
        new( [ 138, 334 ], [ CommandFieldType.LongId ], MessageDirection.Serverbound, baseFirst: false ),
        new( [ 27 ], [ CommandFieldType.Byte ], MessageDirection.Serverbound, baseFirst: false ),
        new( [ 33 ], [ CommandFieldType.UInt16 ], MessageDirection.Serverbound, baseFirst: false ),
        new( [ 240, 300 ], [ CommandFieldType.Int32 ], MessageDirection.Serverbound, baseFirst: false ),
        new(
            [
                15,
                16,
                19,
                29,
                DiscardMysteryBoxCommandType,
                ClientCommand47Type,
                49,
                53,
                59,
                63,
                65,
                66,
                HireBoyCommandType,
                SelectBoyOfferCommandType,
                SearchWithBoyCommandType,
                WakePetCommandType,
                88,
                89,
                93,
                96,
                100,
                101,
                107,
                113,
                115,
                122,
                123,
                125,
                128,
                MarkNeighborhoodChatReadCommandType,
                150,
                155,
                157,
                160,
                161,
                166,
                183,
                185,
                187,
                188,
                189,
                193,
                194,
                208,
                209,
                215,
                217,
                218,
                219,
                TapSanctuaryAnimalCommandType,
                225,
                236,
                238,
                241,
                297,
                324,
                326,
                329,
                330,
                335,
                338,
                340,
                393,
                394,
            ],
            new CommandFieldType[1],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 54 ], new CommandFieldType[1], MessageDirection.Serverbound, baseFirst: false ),
        new( [ 152 ], [ CommandFieldType.VarIntArray ], MessageDirection.Serverbound, baseFirst: false ),
        new( [ 303, 318 ], [ CommandFieldType.DataReference ], MessageDirection.Serverbound, baseFirst: false ),
        new( [ 135 ], [ CommandFieldType.LongId ], MessageDirection.Clientbound, baseFirst: false ),
        new( [ 38, 181, 184, 269 ], new CommandFieldType[1], MessageDirection.Clientbound, baseFirst: false ),
        new( [ 25 ], [ CommandFieldType.VarInt, CommandFieldType.Byte ], MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 86 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.DataReference,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 91 ], [ CommandFieldType.UInt16, CommandFieldType.UInt16 ], MessageDirection.Serverbound, baseFirst: false ),
        new([137], [CommandFieldType.LongId, CommandFieldType.VarInt], MessageDirection.Clientbound, baseFirst: false),
        new( [ 272 ], [ CommandFieldType.OptionalLongId ], MessageDirection.Clientbound, baseFirst: false ),
        new(
            [24, 114, PlaceSanctuaryPuzzlePiecesCommandType],
            [CommandFieldType.VarInt, CommandFieldType.Boolean],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [
                90,
                120,
                126,
                162,
                165,
                191,
                216,
                223,
                237,
                254,
                255,
                259,
                260,
                264,
                363,
            ],
            new CommandFieldType[2],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 178, 213 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ UpdateTaskEventStateCommandType ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new([169, 250, 251, MarkTaskEventSeenCommandType], new CommandFieldType[3], MessageDirection.Serverbound, baseFirst: false),

        new(
            [ 179 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 171 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 207 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.LongId,
                CommandFieldType.LongId,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 211 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.Boolean,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new([273], [CommandFieldType.OptionalLongId, CommandFieldType.VarInt], MessageDirection.Clientbound, baseFirst: false),
        new( [ 302 ], [ CommandFieldType.DataReference, CommandFieldType.VarInt ], MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 316 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 190, 319 ], new CommandFieldType[4], MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 7, 85 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.String,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 267 ],
            [
                CommandFieldType.String,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 172, 173 ],
            [
                CommandFieldType.VarIntArray,
                CommandFieldType.VarIntArray,
                CommandFieldType.VarIntArray,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 342 ], new CommandFieldType[3], MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 350 ],
            [
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 351 ],
            [
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 392 ], new CommandFieldType[4], MessageDirection.Clientbound, baseFirst: false ),
        new(
            [ 154, 268 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ CommandRegistry.RequestShopPurchaseCommandType ],
            [
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 228 ],
            [
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VarInt,
                CommandFieldType.OptionalInt32String,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 234 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarIntArray,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 167 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.OptionalLongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 174 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.OptionalLongId,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 182 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.LongId,
                CommandFieldType.OptionalLongId,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 231 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.LongId,
                CommandFieldType.OptionalLongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 323 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.LongId,
                CommandFieldType.OptionalLongId,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [245],
            [CommandFieldType.Int32, CommandFieldType.String, CommandFieldType.Boolean, ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 246 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 249 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ RecordPromotionPopupStateCommandType ],
            [
                CommandFieldType.String,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),

        new( [ 325 ], [ CommandFieldType.String, CommandFieldType.Int32 ], MessageDirection.Clientbound, baseFirst: false ),
        new(
            [ 331 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 353 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 382 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Boolean,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 390 ], [ CommandFieldType.String ], MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 192 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new([ 235 ], new CommandFieldType[1], MessageDirection.Serverbound),
        new([ 317 ], new CommandFieldType[4], MessageDirection.Serverbound),
        new( [ 540, 563, 588 ], [ CommandFieldType.DataReference ], MessageDirection.Serverbound ),
        new([521], [CommandFieldType.VarInt, CommandFieldType.VarInt, CommandFieldType.DataReference, ], MessageDirection.Serverbound),
        new([579], [CommandFieldType.VarInt, CommandFieldType.DataReference, CommandFieldType.DataReference, ], MessageDirection.Serverbound),
        new( [ RemoveNewShopItemsCommandType, 670, 686 ], [ CommandFieldType.VarIntArray ], MessageDirection.Serverbound ),
        new( [ 226 ], [ CommandFieldType.Int32 ], MessageDirection.Clientbound ),
        new( [ 248 ], [ CommandFieldType.Int32, CommandFieldType.VarInt ], MessageDirection.Clientbound ),
        new( [ 299 ], [ CommandFieldType.LongId ], MessageDirection.Serverbound ),
        new([349], [CommandFieldType.String, CommandFieldType.Boolean, CommandFieldType.VarInt, ], MessageDirection.Clientbound),
        new([543], [CommandFieldType.VarInt, CommandFieldType.DataReference, CommandFieldType.VarInt, ], MessageDirection.Serverbound),
        new( [ 552 ], [ CommandFieldType.Int32, CommandFieldType.Int32 ], MessageDirection.Serverbound ),
        new(
            [ 771 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.String,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarLong,
            ],
            MessageDirection.Clientbound
        ),
        new(
            [ 39 ],
            [
                CommandFieldType.DataReferenceVarIntPairArray,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 149 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.OptionalLongId,
                CommandFieldType.VarIntArray,
                CommandFieldType.VarIntArray,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ CommandRegistry.ShopPurchaseServerCommandType ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.String,
                CommandFieldType.DataReference,
                CommandFieldType.VarIntPairArray,
                CommandFieldType.VarIntPairArray,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 233 ],
            [ CommandFieldType.DataReference, CommandFieldType.DataReferenceArray, CommandFieldType.VarInt, ],
            MessageDirection.Serverbound
        ),
        new(
            [ 266 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.DataReference,
                CommandFieldType.StringArray,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 313 ], new CommandFieldType[6], MessageDirection.Clientbound ),
        new([322], [CommandFieldType.VarInt, CommandFieldType.DataReferenceArray, ], MessageDirection.Serverbound, baseFirst: false),
        new( [ 344 ], [ CommandFieldType.DataReferenceVarIntPairArray ], MessageDirection.Clientbound, baseFirst: false ),
        new(
            [ 366 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarLong,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new([ 367 ], new CommandFieldType[2], MessageDirection.Serverbound),
        new(
            [ 368 ],
            [
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.NullableVarLongArray,
                CommandFieldType.VarLong,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 369, 370, 373, 374, 376, 377, 380 ], new CommandFieldType[1], MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 372 ],
            [
                CommandFieldType.VarIntPairArray,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.String,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 378 ], [ CommandFieldType.VarInt, CommandFieldType.Boolean ], MessageDirection.Clientbound, baseFirst: false ),
        new( [ 379 ], new CommandFieldType[1], MessageDirection.Clientbound, baseFirst: false ),


        new(
            [ 134 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.DataReference,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 136 ],
            [
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        ),
    ];
}
