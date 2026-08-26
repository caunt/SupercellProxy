namespace SupercellProxy.Playground.Network.Protocol;

/// <summary>
/// Defines the supported <c>AppStore</c> values.
/// </summary>
public enum AppStore
{
    /// <summary>
    /// Identifies the <c>Unknown</c> option.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Identifies the <c>AppleAppStore</c> option.
    /// </summary>
    AppleAppStore = 1,

    /// <summary>
    /// Identifies the <c>GooglePlay</c> option.
    /// </summary>
    GooglePlay = 2,

    /// <summary>
    /// Identifies the <c>Kunlun360</c> option.
    /// </summary>
    Kunlun360 = 3,

    /// <summary>
    /// Identifies the <c>KunlunUc</c> option.
    /// </summary>
    KunlunUc = 4,

    /// <summary>
    /// Identifies the <c>KunlunBaiduDuoku</c> option.
    /// </summary>
    KunlunBaiduDuoku = 5,

    /// <summary>
    /// Identifies the <c>KunlunXiaomi</c> option.
    /// </summary>
    KunlunXiaomi = 6,

    /// <summary>
    /// Identifies the <c>KunlunOppo</c> option.
    /// </summary>
    KunlunOppo = 7,

    /// <summary>
    /// Identifies the <c>KunlunHuawei</c> option.
    /// </summary>
    KunlunHuawei = 8,

    /// <summary>
    /// Identifies the <c>KunlunDownjoy</c> option.
    /// </summary>
    KunlunDownjoy = 9,

    /// <summary>
    /// Identifies the <c>KunlunWandoujia</c> option.
    /// </summary>
    KunlunWandoujia = 10,

    /// <summary>
    /// Identifies the <c>KunlunLenovo</c> option.
    /// </summary>
    KunlunLenovo = 11,

    /// <summary>
    /// Identifies the <c>Kunlun91</c> option.
    /// </summary>
    Kunlun91 = 12,

    /// <summary>
    /// Identifies the <c>Unknown13</c> option.
    /// </summary>
    Unknown13 = 13,

    /// <summary>
    /// Identifies the <c>KunlunLandingPage</c> option.
    /// </summary>
    KunlunLandingPage = 14,

    /// <summary>
    /// Identifies the <c>KunlunAnzhi</c> option.
    /// </summary>
    KunlunAnzhi = 15,

    /// <summary>
    /// Identifies the <c>KunlunVivo</c> option.
    /// </summary>
    KunlunVivo = 16,

    /// <summary>
    /// Identifies the <c>KunlunAzsc</c> option.
    /// </summary>
    KunlunAzsc = 17,

    /// <summary>
    /// Identifies the <c>KunlunBaiduTieba</c> option.
    /// </summary>
    KunlunBaiduTieba = 18,

    /// <summary>
    /// Identifies the <c>KunlunBaidu</c> option.
    /// </summary>
    KunlunBaidu = 19,

    /// <summary>
    /// Identifies the <c>Kunlun4399</c> option.
    /// </summary>
    Kunlun4399 = 20,

    /// <summary>
    /// Identifies the <c>KunlunAppChina</c> option.
    /// </summary>
    KunlunAppChina = 21,

    /// <summary>
    /// Identifies the <c>KunlunYouku</c> option.
    /// </summary>
    KunlunYouku = 22,

    /// <summary>
    /// Identifies the <c>KunlunChinaMobile</c> option.
    /// </summary>
    KunlunChinaMobile = 23,

    /// <summary>
    /// Identifies the <c>KunlunChinaTelecom</c> option.
    /// </summary>
    KunlunChinaTelecom = 24,

    /// <summary>
    /// Identifies the <c>KunlunChinaUnicom</c> option.
    /// </summary>
    KunlunChinaUnicom = 25,

    /// <summary>
    /// Identifies the <c>KunlunKingsoft</c> option.
    /// </summary>
    KunlunKingsoft = 26,

    /// <summary>
    /// Identifies the <c>Samsung</c> option.
    /// </summary>
    Samsung = 27,

    /// <summary>
    /// Identifies the <c>Amazon</c> option.
    /// </summary>
    Amazon = 28,

    /// <summary>
    /// Identifies the <c>Unknown29</c> option.
    /// </summary>
    Unknown29 = 29,

    /// <summary>
    /// Identifies the <c>KunlunMha</c> option.
    /// </summary>
    KunlunMha = 30,

    /// <summary>
    /// Identifies the <c>KunlunGionee</c> option.
    /// </summary>
    KunlunGionee = 31,

    /// <summary>
    /// Identifies the <c>KunlunCoolpad</c> option.
    /// </summary>
    KunlunCoolpad = 32,

    /// <summary>
    /// Identifies the <c>KunlunMeizu</c> option.
    /// </summary>
    KunlunMeizu = 33,

    /// <summary>
    /// Identifies the <c>Tencent</c> option.
    /// </summary>
    Tencent = 34,

    /// <summary>
    /// Identifies the <c>GooglePlayInstantApp</c> option.
    /// </summary>
    GooglePlayInstantApp = 35,

    /// <summary>
    /// Identifies the <c>KunlunEwan</c> option.
    /// </summary>
    KunlunEwan = 36,

    /// <summary>
    /// Identifies the <c>Yoozoo</c> option.
    /// </summary>
    Yoozoo = 37,

    /// <summary>
    /// Identifies the <c>KunlunBilibili</c> option.
    /// </summary>
    KunlunBilibili = 38,

    /// <summary>
    /// Identifies the <c>KunlunGuopan</c> option.
    /// </summary>
    KunlunGuopan = 39,

    /// <summary>
    /// Identifies the <c>KunlunJinli</c> option.
    /// </summary>
    KunlunJinli = 40,

    /// <summary>
    /// Identifies the <c>YoozooYouzu</c> option.
    /// </summary>
    YoozooYouzu = 41,

    /// <summary>
    /// Identifies the <c>YoozooTaptap</c> option.
    /// </summary>
    YoozooTaptap = 42,

    /// <summary>
    /// Identifies the <c>YoozooHaoyou</c> option.
    /// </summary>
    YoozooHaoyou = 43,

    /// <summary>
    /// Identifies the <c>YoozooHuawei</c> option.
    /// </summary>
    YoozooHuawei = 44,

    /// <summary>
    /// Identifies the <c>YoozooOppo</c> option.
    /// </summary>
    YoozooOppo = 45,

    /// <summary>
    /// Identifies the <c>YoozooVivo</c> option.
    /// </summary>
    YoozooVivo = 46,

    /// <summary>
    /// Identifies the <c>YoozooMeizu</c> option.
    /// </summary>
    YoozooMeizu = 47,

    /// <summary>
    /// Identifies the <c>YoozooLenovo</c> option.
    /// </summary>
    YoozooLenovo = 48,

    /// <summary>
    /// Identifies the <c>YoozooGionee</c> option.
    /// </summary>
    YoozooGionee = 49,

    /// <summary>
    /// Identifies the <c>YoozooCoolpad</c> option.
    /// </summary>
    YoozooCoolpad = 50,

    /// <summary>
    /// Identifies the <c>YoozooBilibili</c> option.
    /// </summary>
    YoozooBilibili = 51,

    /// <summary>
    /// Identifies the <c>YoozooXiaomi</c> option.
    /// </summary>
    YoozooXiaomi = 52,

    /// <summary>
    /// Identifies the <c>Yoozoo4399</c> option.
    /// </summary>
    Yoozoo4399 = 53,

    /// <summary>
    /// Identifies the <c>YoozooUc</c> option.
    /// </summary>
    YoozooUc = 54,

    /// <summary>
    /// Identifies the <c>Yoozoo360</c> option.
    /// </summary>
    Yoozoo360 = 55,

    /// <summary>
    /// Identifies the <c>YoozooGuopan</c> option.
    /// </summary>
    YoozooGuopan = 56,

    /// <summary>
    /// Identifies the <c>YoozooTt</c> option.
    /// </summary>
    YoozooTt = 57,

    /// <summary>
    /// Identifies the <c>YoozooNubia</c> option.
    /// </summary>
    YoozooNubia = 58,

    /// <summary>
    /// Identifies the <c>YoozooBaidu</c> option.
    /// </summary>
    YoozooBaidu = 59,

    /// <summary>
    /// Identifies the <c>YoozooGuopanLiuliu</c> option.
    /// </summary>
    YoozooGuopanLiuliu = 60,

    /// <summary>
    /// Identifies the <c>YoozooHisense</c> option.
    /// </summary>
    YoozooHisense = 61,

    /// <summary>
    /// Identifies the <c>YoozooHtc</c> option.
    /// </summary>
    YoozooHtc = 62,

    /// <summary>
    /// Identifies the <c>YoozooMeitu</c> option.
    /// </summary>
    YoozooMeitu = 63,

    /// <summary>
    /// Identifies the <c>YoozooMgtv</c> option.
    /// </summary>
    YoozooMgtv = 64,

    /// <summary>
    /// Identifies the <c>YoozooRenrenGame</c> option.
    /// </summary>
    YoozooRenrenGame = 65,

    /// <summary>
    /// Identifies the <c>YoozooSamsung</c> option.
    /// </summary>
    YoozooSamsung = 66,

    /// <summary>
    /// Identifies the <c>YoozooSmartisanUnion</c> option.
    /// </summary>
    YoozooSmartisanUnion = 67,

    /// <summary>
    /// Identifies the <c>YoozooSogouCom</c> option.
    /// </summary>
    YoozooSogouCom = 68,

    /// <summary>
    /// Identifies the <c>YoozooGamerskyCom</c> option.
    /// </summary>
    YoozooGamerskyCom = 69,

    /// <summary>
    /// Identifies the <c>YoozooTzsy</c> option.
    /// </summary>
    YoozooTzsy = 70,

    /// <summary>
    /// Identifies the <c>YoozooYuewen</c> option.
    /// </summary>
    YoozooYuewen = 71,
}
