# `23245` 帧结构与字段含义对照

日期：2026-10-30

## 范围与结论

本次分析联合使用当前 Null 客户端运行的真实 server-to-client `23245` 抓包、
解密后的明文、Frida 字段读取 trace、当前 Null `libg.so`、较早官服
`libg.so` 和 iOS IPA 主程序的 IDA 反汇编、伪代码及 diagnostic strings。
不同构建的绝对地址和对象布局保持分离；没有把旧 IPA 或 SupercellProxy 的地址
当成当前构建的真值。

已经闭合的结论是：

1. 外层帧由 7 字节头、16 字节认证标签和 133844 字节密文组成。
2. 解密后的 133844 字节正文由 Object A、Object B 和 5 个外层 VarInt32 组成。
3. Object B 的前 30 字节与 AstralRoyale 的 `ClientAvatar` 逐字节一致：
   三组 `(0, 0x04426004)` LogicLong 后紧接长度前缀字符串 `CodexAITest`。因此
   `CodexAITest` 是当前玩家显示名、三组值共享同一个玩家/账户式 ID 的判断已
   有强外部格式证据；三组记录的独立角色仍未证明。
4. Object B 的 `B+0x1E=0x02` 是嵌套对象 registry selector，解析为对象类型
   `0x36`；它不是普通计数，也不是旧版 `ClientAvatar` 的布尔字段。
5. Object B 随后包含 9 个集合句柄。实测 9 个集合的 count 依次为
   `2,0,0,0,1,0,0,0,0`；元素线格式为固定 `u32be` 记录/句柄加 VarInt 值。
6. IPA 的 79 条绑定归一为 74 个不同成员名和 39 个 decode helper；当前 SO 对
   74 个名称都有精确 diagnostic，且都能从当前 HomeState decode 闭包到达。
   本次运行真实 trace 直接命中其中 48 个。名称与调用闭包可达不等于每个名称
   已有唯一 wire 位置，因此不能把 helper 内共存的数组强行拆成父字段。
7. Object A 是账户级大型游戏状态容器，包含卡牌、卡组、库存、奖励、赛事、
   赛季、徽章、banner、trail 和 deep-link 状态。它不是由 13 万字节各自对应的
   13 万个父字段组成，大量内容来自共享 helper 中的数组和子对象。
8. 外层 `+0xAC` 和 `+0xB8` 有计时器语义，`+0xB0=-1` 是 quickmatch 冷却关闭的
   强候选；`+0xA8` 和 `+0xB4` 仍未知。
9. 证据支持“OwnHomeData 家族的启动/玩家状态包”，Object A 是
   Home/game-state 家族对象，Object B 是强 `ClientAvatar` 家族前缀匹配。
   IPA 还独立给出 `0x560` Home-state-family 对象和 `0x208`
   ClientAvatar-family 对象，但当前 `23245/v10` 的精确类名仍未证明。
10. Null、旧官服和 IPA 共享同一套有符号 profile key 命名空间。联合 key
    decoder、ABI 写入偏移和完整 Object B 位置解码后，本次已经闭合 15 个
    当前存在字段和 4 个条件 clan 字段。`wire +31` 是 `scr -> +0x88`，
    `wire +32` 是条件标量 `-> +0x158`；此前两处反向不成立。

机器可读结果位于：

```text
D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis
\profile23245-real-field-trace-20261029\frame-schema.json
```

该文件的 SHA-256 是
`38E28C93F0CC02B3222585027D33989DE24E1A0E24645801E9E12E51138E6D25`。

三二进制联合分析、Object B 的 137 字节修正存储映射和 HomeState 成员映射位于：

- [三二进制联合分析](profile-three-binary-joint-analysis.md)

```text
D:\IDA Scripts\ida-ai\agent_reports\libg-profile-23245-joint-20261030
```

其中：

```text
object-b-storage-map-v2.json
SHA-256 6623EB91EA9171A5CD2FBB618498C718C8E41690C57EC8ADC70EE05A4B6A411A

profile-field-crosswalk.json
SHA-256 7502A0DCB439E5EE3CAD2500BA6B71B63EFBC7AA2FFD2A97C8893C1075A29009

homestate-member-map.json
SHA-256 9A7FA9B82E1461FEAB0E2B4743393EC0AEF1DF724F8F8A66454E8B4122BD26EB
```

旧 `object-b-ordinal.json` 的
`1FCF8483B1FF585D6944A1F8F04BF266F6EC51BDDA825AFBDC76B033D1606E5B`
只用于保留历史，其中 `+47=collection count` 和 `+96 -> +0x9C` 的解释已被
后续 decoder 证据推翻。

外部项目对照的独立机器证据位于同目录的
`external-reference-comparison.json`，SHA-256 为
`07985BF562A64741C8D7F0285F321C95DDE520FFAAC1165A9DCF987C1FF9562F`。

## 传输帧

抓包的总长度是 133867 字节：

| 偏移 | 长度 | 编码 | 字段 | 本次值 |
| --- | ---: | --- | --- | --- |
| `0x0000` | 2 | `u16be` | message id | `0x5ACD`，即 23245 |
| `0x0002` | 3 | `u24be` | payload length | 133860 |
| `0x0005` | 2 | `u16be` | message version | 10 |
| `0x0007` | 16 | opaque | authentication tag/context | 每帧随机化 |
| `0x0017` | 133844 | ciphertext | 加密正文 | 解密后长度仍为 133844 |

闭合关系如下：

```text
capture length      = 7 + 133860 = 133867
payload length      = 16 tag + 133844 ciphertext = 133860
plaintext length    = 133844
```

因此，3 字节长度字段包含 tag 和 ciphertext，不包含 7 字节消息头。认证标签或
加密上下文会使同一明文的 sealed frame 哈希发生变化，所以传输帧哈希不能替代
明文哈希。

本次使用的身份：

| 文件 | SHA-256 |
| --- | --- |
| sealed frame | `49B65C687DA0AAFF73FBD5776BE996F35AD04599D4252C9B25A2185D1335CFCF` |
| plaintext | `16D983559A639552ABDBEB43814EA64FD29976086A2AAC6D55255107362BBD2E` |
| current `libg.so` | `110AA2B5CAC391C498645E072B0D88729428C2C2845E7E2737CA8EE979059783` |

## 明文的三段结构

| 明文偏移 | 长度 | 名称 | 解码入口 | 当前解释 |
| --- | ---: | --- | --- | --- |
| `0..133694` | 133694 | Object A | `0xF96F48` | 大型账户级游戏状态容器 |
| `133694..133831` | 137 | Object B | `0xCE0394` | 小型玩家/账户状态对象 |
| `133831..133844` | 13 | outer fields | `0x1052A98` | 5 个 VarInt32 状态/时间字段 |

外层解码器 `0x1052A98` 对 A、B 和五个整数的处理顺序可由伪代码直接确认：

```c
operator new(0x570); sub_F948A4(...); sub_F96F48(...);  // A
operator new(0x208); sub_CDF238(...); slot_0x20(...);   // B
message[0xA8] = read_varint32(...);
message[0xAC] = read_varint32(...);
message[0xB0] = read_varint32(...);
message[0xB4] = read_varint32(...);
message[0xB8] = read_varint32(...);
```

Object A 与 Object B 开头的 ID 不同，不能合并：

- A 的首个 LongId 是 `0x04604204`，十进制 73,548,292。
- B 中重复出现的是 `0x04426004`，十进制 71,458,820。

## Object B 逐字段

当前 ordinal 已用 92 条有序记录覆盖全部 137 字节，包括 VarInt、bit-packed
字段和固定宽度位置。它不是为每个字节制造一个业务字段：集合元素、位域和
未闭合的尾段分别保留各自边界。

| 相对偏移 | 原始值 | 类型/结构 | 当前解释 |
| --- | --- | --- | --- |
| `0..14` | 三组 `(0, 0x04426004)` | 三个 LogicLong，每个由两个 VarInt32 组成 | 与 AstralRoyale `ClientAvatar` 前三行同形；同一个玩家/账户式 ID 重复三次，独立角色未证明，置信度 0.93 |
| `15..29` | `0000000B 436F646578414954657374` | 4 字节长度前缀 + UTF-8 | `CodexAITest`，玩家显示名；当前 decoder 与 Astral 前缀共同支持，置信度 0.94 |
| `30` | `02` | arena-family registry selector | Android 当前 decoder 用对象类型 `0x36` 查询 class 54；当前表注册为 `csv_logic/arenas.csv`，选择 `Arena2`，结果存储在 `+0x160`，相邻行派生到 `+0x168`，置信度 0.97 |
| `31..46` | 16 个 VarInt 观测 | 多个标量和嵌套 selector | 可确认线宽和顺序的进入 ordinal；业务含义仍未证明 |
| `47` | `09` | VarInt32 | decoder 正常读取后直接丢弃；后面的九个 collection handle 是固定 `for(i=0; i!=9; ++i)`，因此它不是 collection count |
| `48..72` | 9 个集合 count 和 3 个元素 | 集合句柄 + `0x20` 字节运行时 item | count 为 `2,0,0,0,1,0,0,0,0`；元素是固定 `u32be` record/handle 加 VarInt value |
| `73..136` | 64 字节 | VarInt、bit-packed flags、固定宽度和尾部零值 | 顺序已保留；除少数类型事实外，字段语义仍未证明 |

### 联合 keyed codec 已闭合的字段

三份二进制都包含同一套 19 个 profile key。当前抓包存在其中 15 个字段：

| wire 偏移 | key | 当前 Android ABI | 类型 | 本次值 |
| --- | --- | ---: | --- | ---: |
| `+5..+6` | `accountID.hi` | `+0x20` | VarInt32 | 0 |
| `+6..+10` | `accountID.lo` | `+0x24` | VarInt32 | 71458820 |
| `+15..+30` | `name` | `+0x78` | 长度前缀 UTF-8 | `CodexAITest` |
| `+30` | `arena` | `+0x160` | registry VarInt | 2，解析为 `Arena2` |
| `+31` | `scr` | `+0x88` | u32 | 0 |
| `+79` | `expPoints` | `+0xEC` | u32 | 0 |
| `+80` | `totalExpPoints` | `+0xF0` | u32 | 0 |
| `+83` | `expLevel` | `+0xE8` | u32 | 1 |
| `+96..+100` | `spi` | `+0x194` | u32be | 0 |
| `+100..+102` | `lti` | `+0x58` | VarInt32 | 6047 |
| `+102` | `npc` | `+0x19C` | bool | false |
| `+103..+108` | `birth_date` | `+0x1A0` | Unix seconds VarInt32 | 946674000 |
| `+123` | `rnk_sc` | `+0x1D0` | VarInt32 | 0 |
| `+125` | `rnk_ps` | `+0x1D8` | VarInt32 | 0 |
| `+136` | `spt` | `+0x198` | VarInt32 | 0 |

`rnk_sc` is now a proven gameplay scalar: the official profile UI reads
`+0x1D0`, selects the `ranked_score` node, and formats that value as the score.
The encoder and decoder independently expose the same u32 binding. A
non-probabilistic name for it is therefore `currentLadderScore`; the wire key
remains `rnk_sc`.

`rnk_ps` remains unresolved even though it is also rank-prefixed. The official
seasonal-profile UI reads previous-season values from `+0x1EC/+0x1F0` and
best-season values from `+0x1FC/+0x200`, not from `+0x1D8`. Neither the `ps`
suffix, the adjacent layout, nor that UI path proves
`previousSeasonScore`, `bestSeasonScore`, or `peakLadderScore`.

`spt` 虽然对应较靠前的 ABI 偏移，但实际在 wire 最后读取；storage offset 与
wire order 不能互相推导。`spi > 0 && spi == lti` 是代码已证明的条件，
本次两个值为 `0` 和 `6047`，所以条件为 false；其 UI 行为仍未证明。

同一对象数据流进一步证明：`lti/+0x58` 是单调前进的 progression index/value；
购买命令把当前 `lti` 写入 `spi/+0x194`，随后把命令结果映射成 `1..3` 并写入
`spt/+0x198`；delivery 路径只有在记录中的 progression 值等于 `lti` 时才恢复
`spi/spt`，另一分支把两者清零。因此 `spi/spt` 应作为一对 progression
snapshot/tag 建模，而不是根据字母猜成 shop index 或 season point。

`scr/+0x88` 被 profile UI 复制并由 `rating` 或 `trophies_autochess` 节点展示；
另一个 profile-list consumer 还将它与计算得到的分数比较并取最大值。可以
把它描述为 score/rating value，但 `scr` 的原始展开和单一玩法含义仍未证明。

`birth_date=946674000` 对应 UTC `1999-12-31T21:00:00Z`，换算上海时间为
`2000-01-01T05:00:00+08:00`。它符合“新账号哨兵值”的形状，但当前没有 UI
consumer 把这个解释闭合。

`wire +84` 是可选 clan 块开关。本抓包为 false，因此以下绑定已知但没有对应
wire value：

| key | ABI offset |
| --- | ---: |
| `clan_id` | `+0x108` |
| `badge` | `+0x110` |
| `clan_name` | `+0x120` |
| `welo` | `+0x130` |

`welo/+0x130` 的确位于可选 clan block，并会被 profile UI 复制到对象内字段；
三个 command execute 路径和一条重置路径会写它，但本轮没有找到已解码的 UI
consumer，因此只能称为 clan-adjacent profile state。`rnk_ps/+0x1D8` 仍没有
直接 consumer。完整的同对象证据和允许采用的临时字段名见
[profile abbreviated scalar evidence](profile-abbreviated-scalar-evidence.md)。

`accepted_tos_version`、`new_user`、`tutorial_state` 等来自持久化 key 字符串
命名空间，不能据此直接断言它们存在于本包或决定某个启动 UI。

后段仍未闭合的标量不能只凭数值相同就去其他函数搜索。当前扫描已经证明
“相同 ABI 偏移”会大量命中不同 receiver、不同对象和不同的逻辑地址表达式；
后续候选必须保留 receiver 数据流，或再找到一个 keyed encoder/decoder 绑定。

3 个集合元素在本次抓包中的值是：

```text
collection 0, item 0: handle 5000001 -> value 500
collection 0, item 1: handle 5000002 -> value 10
collection 4, item 0: handle 5000008 -> value 8
```

Android 当前 decoder 与较早官服 decoder 在地址归一化后结构等价：元素对象为
`0x20` 字节，包含固定 record/handle 和 VarInt value。当前版本资源表已经闭合
GlobalID 命名空间：

```text
5000001 -> class 5, Resources[1], Gold,            value 500
5000002 -> class 5, Resources[2], ChestOrderIndex, value 10
5000008 -> class 5, Resources[8], CardCount,       value 8
```

当前注册表把 class 5 映射为 `csv_logic/resources.csv`。`F74718` 用
`global_id / 1,000,000` 取 class，`F74734` 用 `global_id % 1,000,000`
取 item index。因此上述值应建模为对应 resource 的协议值；不能把所有 value
都泛化成 count，尤其 `ChestOrderIndex=10` 更像档位/索引。

这里最容易产生的误判是把 `m_decks`、`m_cards` 等 Object A 成员名硬套到
Object B，或者把集合元素的 `500000x` 句柄拆成普通 Object B 标量。当前证据
不支持这样做。Object B 的前缀已足以升格为
“`ClientAvatar` 家族强匹配”：

```text
0084809344 0084809344 0084809344 0000000B 436F646578414954657374
```

这是三组相同 LogicLong 加 `CodexAITest` 的 30 字节；AstralRoyale 的
`ClientAvatar.encode()` 恰好依次写三组 `player.highID/lowID` 和玩家名。
不过 `23245/v10` 与参考消息 `28502/v1` 不同，当前 `B+0x1E=02` 已证明为
registry selector，而不是旧版后续字段。因此只能继承前缀的线格式语义，不能把
整个类或后续偏移直接复制。

## Object A 的粗粒度结构

Object A 的 trace 有 6876 次字段读取、1302 个读取段，涉及 81 个函数；按字节
计算的覆盖率为 66.99%。未覆盖部分主要是固定宽度字段、未被 hook 的读取和共享
helper 内部的循环区间，所以不能根据“没有 trace”推断字段不存在。

| 相对区间 | trace 读取 | 函数数 | 字节覆盖率 | 工作区解释 | 代表内容 |
| --- | ---: | ---: | ---: | --- | --- |
| `0..151` | 117 | 7 | 76.16% | 头字段与同形小记录 | 首个 LongId、flags、重复 4 字节间隙 |
| `151..44770` | 1334 | 1 | 15.04% | 事件、挑战、礼包和显示状态集合 | `Classic 2v2`、Challenge、Friendly、Emote |
| `44770..45050` | 187 | 1 | 100% | 小型记录或集合边界 | 以整数和 flag 为主 |
| `45050..68515` | 1107 | 36 | 74.40% | 配置、统计、资源、ID 和 flags | `Royals`、`Classic2v2`、`grandchallenge` |
| `68515..68629` | 53 | 8 | 73.68% | 窄的具名状态区 | consumable、spell、mastery、badge 集合 |
| `68629..132954` | 3789 | 5 | 99.98% | 大型重复 banner/resource/card 集合 | `.sc` 资源路径、banner 标题和描述 |
| `132954..133694` | 289 | 36 | 80.68% | 奖励、赛季、进度和 deep-link 尾部 | trail、claimed reward、augment、deep link |

`68629..132954` 区间中的同一个 helper `0xF811EC` 被反复执行 271 次，一次 trace
可能处于 `m_newBanners`、`m_chronosBannerOverrides`、`m_ownedBanners` 或
`m_favoriteBanners` 的子对象中。把每次 helper 执行都当成父对象新字段，会产生
数量级错误。

Object A 的相对偏移 15、30、45、60、75、90 处还出现固定 `u32be`：
`26000000`、`26000001`、`26000005`、`28000001`、`28000000`、
`26000003`。这组值与 `clash-royale-research` 的
`starting_home.json` 示例卡组 SCID 完全同域，支持这里是卡牌 content/deck
相关数据。它同时说明 Object B 的 `5,000,00x` 不应继续当作同类 content ID。

## 当前 SO 名称对照

当前 SO 中有 246 条
`Too many items in array decode, var:<member>` diagnostic，分布在 122 个
decode helper。IPA 的 79 条绑定归一为 74 个不同成员名；当前 SO 对这 74 个
名称均有精确 diagnostic，且 74 个都能从当前 HomeState decode 闭包到达。
本次 trace 执行到其中 23 个 helper，并直接命中 48 个名称。下表区间是 helper
观测范围的最小值到最大值，不代表区间内每一字节都属于某个同名数组。

| Helper | Object A 范围 | trace 读取 | 本次命中的成员名 |
| --- | ---: | ---: | --- |
| `0xD21BF4` | 68583..68607 | 13 | `m_itemsGet`, `m_itemsGive`, `m_usedGetSpellIdsCommon`, `m_usedGetSpellIdsEpic`, `m_usedGetSpellIdsLegendary`, `m_usedGetSpellIdsRare`, `m_usedGiveSpellIds` |
| `0xD24184` | 45703..45705 | 2 | `m_deckUsage` |
| `0xF76880` | 133686..133687 | 1 | `m_progress` |
| `0xF7BEA0` | 68468..68506 | 7 | `m_badgesShownInProfile`, `m_pendingNotSeenUpgradedBadges` |
| `0xF811EC` | 68629..132982 | 271 | `m_chronosBannerOverrides`, `m_favoriteBanners`, `m_newBanners`, `m_ownedBanners` |
| `0xF884B0` | 133001..133002 | 1 | `m_rewardList` |
| `0xF94224` | 133649..133656 | 7 | `m_claimedStepIndices` |
| `0xFACDD4` | 68573..68581 | 9 | `m_consumableCounts`, `m_consumableIds`, `m_maxedConsumables`, `m_newUnseenConsumables`, `m_seenConsumables` |
| `0xFBFC0C` | 133671..133672 | 1 | `m_popupOpenedEventIds` |
| `0xFD6B74` | 133032..133038 | 6 | `m_previousSeasonPassUncollectedNodes` |
| `0xFE3C54` | 133624..133632 | 4 | `m_newRulerSkins`, `m_newRulerVFXs`, `m_ownedRulerSkins`, `m_ownedRulerVFXs` |
| `0xFE6B50` | 68388..68415 | 6 | `m_loadedOverrideNodes` |
| `0xFEE450` | 133016..133032 | 13 | `m_deckEventIds`, `m_eventDecks`, `m_eventDecksHaveBeenEdited`, `m_rewardsPurchasedAmounts`, `m_rewardsPurchasedIndexes` |
| `0xFF0E80` | 133684..133686 | 2 | `m_openedDeepLinkEventEndTimestamps`, `m_openedDeepLinkEventIds` |
| `0xFF6214` | 133052..133577 | 105 | `m_cachedBadges` |
| `0xFFBB6C` | 133043..133583 | 13 | `m_trails` |
| `0xFFE5D4` | 133113..133581 | 7 | `m_claimedRewards` |
| `0x1004F50` | 68581..68583 | 4 | `m_masteriesProgress` |
| `0x1008558` | 133005..133007 | 3 | `m_collectedOneTimeRewardsLeagueDataIndices`, `m_collectedSeasonalRewardsLeagueDataIndices` |
| `0x100EB50` | 133042..133043 | 1 | `m_chainOfferStates` |
| `0x10253BC` | 133687..133689 | 2 | `m_activeStates`, `m_unclaimedRewards` |
| `0x1107A7C` | 133634..133637 | 2 | `m_augments` |
| `0x1161368` | 133007..133016 | 5 | `m_keys`, `m_values` |

这些名称的置信度高于从参考项目猜名，因为名称来自当前 SO 的精确字符串，
且对应 helper 在本次 trace 中确实执行。一个 helper 含多个名称时，本轮证据
只能把名称提升到 helper scope，不能直接给出父对象中的唯一字段偏移。

## 外层 5 个 VarInt32

| ID | 明文偏移 | 原始字节 | 有符号值 | 当前解释 | 置信度 |
| --- | ---: | --- | ---: | --- | ---: |
| 0 | 133831 | `00` | 0 | 业务含义未知 | 0.30 |
| 1 | 133832 | `8B EF 96 AE 0D` | 1793252299 | session/timer base timestamp 候选 | 0.92 |
| 2 | 133837 | `7F` | -1 | quickmatch cooldown disabled/sentinel 强候选 | 0.88 |
| 3 | 133838 | `00` | 0 | 已安装到 state，但消费者未知 | 0.30 |
| 4 | 133839 | `8A 9F B2 AF 0D` | 1794525130 | season/event end timestamp 候选 | 0.86 |

两个时间值分别是：

```text
1793252299 = 2026-10-29 05:38:19 UTC
1794525130 = 2026-11-12 23:12:10 UTC
差值       = 1,272,831 秒（约 14 天 17 小时 33 分 51 秒）
```

IDA 证据把字段 1 和字段 4 与倒计时联系起来：

```c
sub_BDB074(a2[0xB8] - a2[0xAC]);
sub_C8E054(..., a2[0xA8], a2[0xAC], a2[0xB4], a2[0xB8]);
```

`0xBDA02C` 计算该计时器剩余时间，UI 消费者使用
`TID_SEASON_ENDS_IN`。字段 `+0xB0=-1` 被送入 `0xBDB09C`，
`0xBDB0C4` 计算剩余 quickmatch cooldown，并以 0 为下限；UI 使用
`TID_QUICKMATCH_COOLDOWN`。这条证据支持“负值代表关闭/无冷却”，但仍不是
对业务状态机的完整证明。

## 与 AstralRoyale、clash-royale-research、IPA 和 SupercellProxy 对照

### AstralRoyale

AstralRoyale 的 `OwnHomeDataMessage` 使用消息 `28502`、版本 `1`，编码顺序
严格为 `ClientHome` 后接 `ClientAvatar`。这与当前明文按“大 Object A → 小
Object B”排列一致，支持 `23245/v10` 属于同一 OwnHomeData 家族。

最强的证据是 Object B 的 30 字节精确前缀：

- `ClientAvatar.encode()` 前三行重复写当前玩家的
  `writeLogicLong(highID, lowID)`；
- `writeLogicLong` 的实现正是连续写两个 VarInt；
- 第四行写玩家名，当前 wire 是 `u32be` 长度后接 UTF-8；
- 当前值 `(0, 0x04426004)` 重复三次，随后是 `CodexAITest`，与这套顺序逐字节吻合。

这给出两个确定性层级：

1. 结构事实：前三对是两个 VarInt 组成的 LogicLong，且三对值完全相同；字符串
   是玩家显示名。
2. 来源推断：在 AstralRoyale 中这些位置分别是 player highID、lowID 和名字。
   当前构建是否把三个逻辑 ID 分别解释为 account、avatar、home identity 仍未
   证明，不能仅凭重复关系拆出三个角色。

Object A 只有家族级匹配。Astral `ClientHome` 也是 LogicLong 开头，随后写
`14, 0, LogicTimer`；当前 Object A 的第一个 LogicLong 是
`(0, 73548292)`，后面是 `0, -1, 0, 8`。前缀已经不同，所以当前 Object A 的
字段必须继续由当前 SO/trace 证明，不能照搬 Astral 的绝对偏移。

### clash-royale-research

该项目没有线上协议实现，`GOAL.md` 明确把 accounts 和 Supercell 服务器交互
排除在范围外。它对本启动包的作用是资产和 ID 语义，而不是 wire schema：

- `starting_home.json` 使用 `26000000`、`28000000` 系列作为卡牌 SCID，和
  Object A 观察到的六个卡牌 ID 同域；
- 模拟器曾把 `5,000,000` 用作动态 unit ID 起始值；这个范围相似性只是早期
  线索，当前版本 `resources.csv` 的 GlobalID 映射优先，已证明 Object B
  这三个值属于资源表，而不是模拟器的动态实例命名约定。

因此现在的分层是：Object A 的 `26,000,000/28,000,000` 是卡牌 content
SCID 候选；Object B 的 `5,000,001/2/8` 是 class 5 resources GlobalID，
分别对应 Gold、ChestOrderIndex 和 CardCount。参考模拟器的 5,000,000
动态实例约定不再用于命名当前 wire 值。

### IPA 和 SupercellProxy

旧 Clash Royale IPA 的 `23245` 分析给出：

- 79 条字段绑定；
- 39 个 decode helper；
- 74 个不同成员名。

本次当前构建 trace 直接命中其中 48 个名称。进一步做当前 SO diagnostic 与
decode 调用闭包匹配后，74/74 个名称都有精确 diagnostic，且都能从当前
HomeState decoder 到达；另外 26 个名称属于新增可达项。该结果只证明名称及
调用闭包，`homestate-member-map.json` 中
`unique_wire_positions_proven` 仍为 `0`。IPA 的绝对地址来自另一版本，只能
保留为 reference-only。

IPA 主程序还给出独立结构桥：

```text
0x100374BC8 profile-response handler
  -> 0x100461F9C / 0x1004620B4
     -> 0x100749244
```

`0x100749244` 读取 version `11`、分配 `0x560` 字节 Home-state-family 对象，
通过 `0x1006A6AFC` 构造并调用 `0x1006A8B00` 解码。IPA 的
ClientAvatar-family 对象为 `0x208` 字节，constructor/encode/decode 分别是
`0x10046FC30` / `0x100470058` / `0x10047094C`；其 vtable `0x1014532E0`
的 `+00/+08/+10/+18/+20/+28/+30/+38` 槽分别对应 destructor、deleting
destructor、release、encode、decode、set context、`+0x19C` byte getter 和
round-trip probe。

IPA LoginOK 的 `24742` 不等于 Android 启动消息 `23245`，Android LoginOK 是
`20963`。因此当前 IPA 证据证明的是 ClientAvatar/Home-state 家族和序列化
实现，不足以把 IPA 的另一条 profile message 直接命名为 `23245`。

SupercellProxy 没有 `23245` 的直接实现。Hay Day
`OwnHomeDataMessage(24180)`：同样包含账户/主场景状态、时间值和多个复杂
子对象。但两边消息 ID、顶层对象数量和线格式不同，因此只能采用“启动状态包”
这一层架构类比：

- 可以把 `23245` 标注为 OwnHomeData 家族候选，但不能证明精确类名；
- 可以把 Object B 标注为 `ClientAvatar` 家族强匹配，并继承前 30 字节语义；
- 不从 Hay Day 类字段回填当前构建偏移；
- 可以把 `m_badgesShownInProfile` 等当前 SO 已证明的字段作为理解 profile
  内容的本地证据。

## 仍未决

1. Object A 和 Object B 的精确当前业务类名；现有证据只到 Home 家族和
   `ClientAvatar` 强前缀匹配。
2. 外层字段 0、3 的具体消费者和语义。
3. Object B `+0/+5/+10` 三组 identity-like 记录的关系。
4. 已证明 `5,000,001/2/8` 分别对应 Gold、ChestOrderIndex、CardCount；仍需
   继续追踪这些资源值如何进入 player/home 状态和 UI。
5. Object A 中共享 helper 对应的 sibling array 的确切父偏移和元素类型。
6. 946,674,000 是时间、金额、经验值还是其他整数状态。

下一最小实验应收取或构造同一构建的第二份有效 `23245` 启动包，至少改变一个
显示名或一个资源记录值，然后在保持本启动包范围不变的前提下做差异：
观察 Object B 前三组 LogicLong、名字和这些固定宽度记录各自如何变化；同时
对 Object A 开头 100 余字节做同样的卡牌 ID 对照。不要扩展到后续 gameplay
消息，也不要为了这次对照导出无关函数。

服务器侧持久化约束和数据建模要求已整理到
[玩家系统数据库契约](player-system-database-contract.md)。该文档把原始快照、
未知字段、LogicLong、卡牌 content SCID、动态实例 ID、Home 状态和时间字段分
成独立职责，避免当前尚未证明的 `B+0x1E` registry selector 之后布局被提前
固化成数据库表。
