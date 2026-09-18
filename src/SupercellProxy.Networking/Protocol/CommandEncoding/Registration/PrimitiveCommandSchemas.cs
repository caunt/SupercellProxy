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
                516,
                519,
                520,
                522,
                538,
                556,
                558,
                559,
                565,
                569,
                576,
                597,
                602,
                605,
                611,
                617,
                618,
                623,
                MarkChainOfferSeenCommandType,
                ClaimDecoStickerBookCollectionRewardCommandType,
                627,
                629,
                632,
                651,
                656,
                BuyCropSeedsCommandType,
                667,
                669,
                693,
                696,
            ],
            new CommandFieldType[1],
            direction: MessageDirection.Serverbound
        ),
        new( [ 561, 585, 591, 609, 673 ], [ CommandFieldType.Boolean ], direction: MessageDirection.Serverbound ),
        new( [ 692 ], [ CommandFieldType.String ], direction: MessageDirection.Serverbound ),
        new( [ 525, 625 ], [ CommandFieldType.Boolean, CommandFieldType.VariableInt ], direction: MessageDirection.Serverbound ),
        new( [ 679 ], [ CommandFieldType.String, CommandFieldType.VariableInt ], direction: MessageDirection.Serverbound ),
        new( [ 560 ], [ CommandFieldType.VariableInt, CommandFieldType.Boolean ], direction: MessageDirection.Serverbound ),
        new( [ 501, 523, 550, 568, 607, 620, 621, 642, 650, 655, 658 ], new CommandFieldType[2], direction: MessageDirection.Serverbound ),
        new([ 608, 666 ], new CommandFieldType[3], direction: MessageDirection.Serverbound),
        new( [ 839 ], [ CommandFieldType.Boolean, CommandFieldType.Boolean ], direction: MessageDirection.Clientbound ),
        new( [ 130, SetBoyOfferFlagCommandType, 196 ], [ CommandFieldType.Boolean ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new( [ 138, 334 ], [ CommandFieldType.LongIdentifier ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new( [ 27 ], [ CommandFieldType.Byte ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new( [ 33 ], [ CommandFieldType.UInt16 ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new( [ 240, 300 ], [ CommandFieldType.Int32 ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new(
            [
                6,
                15,
                16,
                19,
                29,
                42,
                43,
                DiscardMysteryBoxCommandType,
                47,
                49,
                53,
                59,
                61,
                63,
                MineCommandType,
                65,
                66,
                HireBoyCommandType,
                SelectBoyOfferCommandType,
                SearchWithBoyCommandType,
                87,
                88,
                89,
                93,
                96,
                98,
                100,
                101,
                107,
                113,
                115,
                119,
                121,
                122,
                123,
                125,
                128,
                139,
                141,
                150,
                155,
                157,
                159,
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
                202,
                208,
                209,
                215,
                217,
                218,
                219,
                220,
                225,
                236,
                238,
                241,
                286,
                288,
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
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 54 ], new CommandFieldType[1], direction: MessageDirection.Serverbound, baseFirst: false ),
        new( [ 152 ], [ CommandFieldType.VariableIntArray ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new( [ 285, 303, 318 ], [ CommandFieldType.DataReference ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new( [ 135 ], [ CommandFieldType.LongIdentifier ], direction: MessageDirection.Clientbound, baseFirst: false ),
        new( [ 38, 181, 184, 269 ], new CommandFieldType[1], direction: MessageDirection.Clientbound, baseFirst: false ),
        new( [ 25 ], [ CommandFieldType.VariableInt, CommandFieldType.Byte ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 86 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.DataReference,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 91 ], [ CommandFieldType.UInt16, CommandFieldType.UInt16 ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 137 ],
            [ CommandFieldType.LongIdentifier, CommandFieldType.VariableInt ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 272 ], [ CommandFieldType.OptionalLongIdentifier ], direction: MessageDirection.Clientbound, baseFirst: false ),
        new(
            [24, 60, 114, 221],
            [CommandFieldType.VariableInt, CommandFieldType.Boolean],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [
                90,
                118,
                120,
                126,
                140,
                142,
                143,
                144,
                147,
                162,
                165,
                191,
                199,
                204,
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
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 178, 213 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 131, UpdateTaskEventStateCommandType ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 104, 111, 145, 169, 250, 251, MarkTaskEventSeenCommandType ],
            new CommandFieldType[3],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 102 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.LongIdentifier,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 179 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.LongIdentifier,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 171 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.LongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 207 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.LongIdentifier,
                CommandFieldType.LongIdentifier,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 211 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.LongIdentifier,
                CommandFieldType.Boolean,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 214 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.Int32,
                CommandFieldType.Boolean,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [273],
            [CommandFieldType.OptionalLongIdentifier, CommandFieldType.VariableInt],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 302 ], [ CommandFieldType.DataReference, CommandFieldType.VariableInt ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new(
            [384],
            [CommandFieldType.Int32, CommandFieldType.Int32, CommandFieldType.Int32, ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 110 ],
            [
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 316 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 190, 319 ], new CommandFieldType[4], direction: MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 7, 85 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.String,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 267 ],
            [
                CommandFieldType.String,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 172, 173 ],
            [
                CommandFieldType.VariableIntArray,
                CommandFieldType.VariableIntArray,
                CommandFieldType.VariableIntArray,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 342 ], new CommandFieldType[3], direction: MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 350 ],
            [
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 351 ],
            [
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 392 ], new CommandFieldType[4], direction: MessageDirection.Clientbound, baseFirst: false ),
        new(
            [ 154, 268 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 227 ],
            [
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 228 ],
            [
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VariableInt,
                CommandFieldType.OptionalInt32String,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 234 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.LongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableIntArray,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 167 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.OptionalLongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 174 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.OptionalLongIdentifier,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 182 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.LongIdentifier,
                CommandFieldType.OptionalLongIdentifier,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [205],
            [CommandFieldType.Byte, CommandFieldType.ByteCountedVariableIntArray, ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 231 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.LongIdentifier,
                CommandFieldType.OptionalLongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 323 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.LongIdentifier,
                CommandFieldType.OptionalLongIdentifier,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [245],
            [CommandFieldType.Int32, CommandFieldType.String, CommandFieldType.Boolean, ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 246 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.LongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 249 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.LongIdentifier,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ RecordPromotionPopupStateCommandType ],
            [
                CommandFieldType.String,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 304 ],
            [
                CommandFieldType.OptionalLongIdentifier,
                CommandFieldType.OptionalLongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 325 ], [ CommandFieldType.String, CommandFieldType.Int32 ], direction: MessageDirection.Clientbound, baseFirst: false ),
        new(
            [ 331 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 353 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.LongIdentifier,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 382 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Boolean,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 390 ], [ CommandFieldType.String ], direction: MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 192 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new([ 235 ], new CommandFieldType[1], direction: MessageDirection.Serverbound),
        new([ 270 ], new CommandFieldType[2], direction: MessageDirection.Serverbound),
        new([ 317 ], new CommandFieldType[4], direction: MessageDirection.Serverbound),
        new( [ 540, 563, 588 ], [ CommandFieldType.DataReference ], direction: MessageDirection.Serverbound ),
        new(
            [ 521 ],
            [ CommandFieldType.VariableInt, CommandFieldType.VariableInt, CommandFieldType.DataReference, ],
            direction: MessageDirection.Serverbound
        ),
        new(
            [ 579 ],
            [ CommandFieldType.VariableInt, CommandFieldType.DataReference, CommandFieldType.DataReference, ],
            direction: MessageDirection.Serverbound
        ),
        new( [ RemoveNewShopItemsCommandType, 670, 686 ], [ CommandFieldType.VariableIntArray ], direction: MessageDirection.Serverbound ),
        new( [ 226 ], [ CommandFieldType.Int32 ], direction: MessageDirection.Clientbound ),
        new( [ 248 ], [ CommandFieldType.Int32, CommandFieldType.VariableInt ], direction: MessageDirection.Clientbound ),
        new( [ 299 ], [ CommandFieldType.LongIdentifier ], direction: MessageDirection.Serverbound ),
        new(
            [ 349 ],
            [ CommandFieldType.String, CommandFieldType.Boolean, CommandFieldType.VariableInt, ],
            direction: MessageDirection.Clientbound
        ),
        new(
            [ 543 ],
            [ CommandFieldType.VariableInt, CommandFieldType.DataReference, CommandFieldType.VariableInt, ],
            direction: MessageDirection.Serverbound
        ),
        new( [ 552 ], [ CommandFieldType.Int32, CommandFieldType.Int32 ], direction: MessageDirection.Serverbound ),
        new(
            [ 771 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.String,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableLong,
            ],
            direction: MessageDirection.Clientbound
        ),
        new(
            [ 39 ],
            [
                CommandFieldType.DataReferenceVariableIntPairArray,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new(
            [ 149 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.OptionalLongIdentifier,
                CommandFieldType.VariableIntArray,
                CommandFieldType.VariableIntArray,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 229 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.String,
                CommandFieldType.DataReference,
                CommandFieldType.VariableIntPairArray,
                CommandFieldType.VariableIntPairArray,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 233 ],
            [ CommandFieldType.DataReference, CommandFieldType.DataReferenceArray, CommandFieldType.VariableInt, ],
            direction: MessageDirection.Serverbound
        ),
        new(
            [ 266 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.DataReference,
                CommandFieldType.StringArray,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 313 ], new CommandFieldType[6], direction: MessageDirection.Clientbound ),
        new(
            [322],
            [CommandFieldType.VariableInt, CommandFieldType.DataReferenceArray, ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 344 ], [ CommandFieldType.DataReferenceVariableIntPairArray ], direction: MessageDirection.Clientbound, baseFirst: false ),
        new(
            [ 366 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.VariableLong,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new([ 367 ], new CommandFieldType[2], direction: MessageDirection.Serverbound),
        new(
            [ 368 ],
            [
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.NullableVariableLongArray,
                CommandFieldType.VariableLong,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Serverbound,
            baseFirst: false
        ),
        new( [ 369, 370, 373, 374, 376, 377, 380 ], new CommandFieldType[1], direction: MessageDirection.Serverbound, baseFirst: false ),
        new(
            [ 372 ],
            [
                CommandFieldType.VariableIntPairArray,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.String,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 378 ], [ CommandFieldType.VariableInt, CommandFieldType.Boolean ], direction: MessageDirection.Clientbound, baseFirst: false ),
        new( [ 379 ], new CommandFieldType[1], direction: MessageDirection.Clientbound, baseFirst: false ),
        new(
            [ VisitedBoatHelpServerCommandType, 387 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.LongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 389 ],
            [
                CommandFieldType.OptionalLongIdentifier,
                CommandFieldType.OptionalLongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new( [ 636 ], [ CommandFieldType.VariableIntArray, CommandFieldType.VariableLongArray ], direction: MessageDirection.Serverbound ),
        new(
            [ 134 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.DataReference,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.Boolean,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
        new(
            [ 136 ],
            [
                CommandFieldType.LongIdentifier,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
                CommandFieldType.VariableInt,
            ],
            direction: MessageDirection.Clientbound,
            baseFirst: false
        ),
    ];

}
