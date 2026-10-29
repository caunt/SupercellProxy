# `23245` 帧结构与字段含义对照

日期：2026-10-29

## 范围与结论

本次分析使用当前 Null 客户端运行的真实 server-to-client `23245` 抓包、解密后的
明文、Frida 字段读取 trace，以及当前 `libg.so` 的 IDA 反汇编和伪代码。没有把
旧 IPA 或 SupercellProxy 的地址当成当前构建的真值。

已经闭合的结论是：

1. 外层帧由 7 字节头、16 字节认证标签和 133844 字节密文组成。
2. 解密后的 133844 字节正文由 Object A、Object B 和 5 个外层 VarInt32 组成。
3. Object B 中的 `CodexAITest` 是玩家/账户显示名的高置信度候选。
4. 当前 SO 的精确 decode diagnostic 字符串与本次执行到的 23 个 helper 对应，
   直接覆盖 48 个成员名。
5. Object A 是账户级大型游戏状态容器，包含卡牌、卡组、库存、奖励、赛事、
   赛季、徽章、banner、trail 和 deep-link 状态。它不是由 13 万字节各自对应的
   13 万个父字段组成，大量内容来自共享 helper 中的数组和子对象。
6. 外层 `+0xAC` 和 `+0xB8` 有计时器语义，`+0xB0=-1` 是 quickmatch 冷却关闭的
   强候选；`+0xA8` 和 `+0xB4` 仍未知。
7. 证据支持“启动/玩家状态包”，尚不足以把 `23245` 直接命名为
   `OwnHomeDataMessage`，也不足以把 Object B 直接命名为 `ClientAvatar`。

机器可读结果位于：

```text
D:\IDA Scripts\clash-royale-supply-chain\work\null-libg-analysis
\profile23245-real-field-trace-20261029\frame-schema.json
```

该文件的 SHA-256 是
`AED02D5A97DE277A10E82C84630C12BB259B0D57756EAE40E35354A85F1DB7E6`。

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

Object B 的覆盖不是完全连续。trace 捕获了 137 字节中的 121 字节，占 88.32%；
未捕获的是四个固定 4 字节位置：

```text
B+49..52  B+55..58  B+64..67  B+96..99
```

其中前三个可以直接从原始明文读取，第四个全零。

| 相对偏移 | 原始值 | 类型/结构 | 当前对照 |
| --- | --- | --- | --- |
| `0..14` | 三组 `(0, 0x04426004)` | VarInt32 对 | 三组 identity-like 记录；角色尚未证明 |
| `15..29` | `0000000B 436F646578414954657374` | 4 字节长度前缀 + UTF-8 | `CodexAITest`，玩家/账户显示名候选，置信度 0.90 |
| `30` | `02` | VarInt32 | count/state 候选，单一语义未知 |
| `31..46` | 16 个零 VarInt32 | 原始整数块 | 具体字段未知 |
| `47` | `09` | VarInt32 | 数值状态，语义未知 |
| `48` | `02` | VarInt32 | 数值状态，语义未知 |
| `49..52` | `004C4B41` | 固定 `u32be` 5,000,001 | content/card ID 候选，置信度 0.48 |
| `53..54` | `B4 07` | VarInt32 500 | 数值字段 |
| `55..58` | `004C4B42` | 固定 `u32be` 5,000,002 | content/card ID 候选，置信度 0.48 |
| `59` | `0A` | VarInt32 10 | 数值字段 |
| `60..63` | `00 00 00 01` | VarInt32 + bit read | 混合整数/标志区 |
| `64..67` | `004C4B48` | 固定 `u32be` 5,000,008 | content/card ID 候选，置信度 0.48 |
| `68` | `08` | VarInt32 8 | 数值字段 |
| `69..72` | 四个零 VarInt32 | 原始整数块 | 具体字段未知 |
| `73..75` | `80 89 7A` | VarInt32 1,000,000 | 阈值/数量候选 |
| `76..78` | `80 89 7A` | VarInt32 1,000,000 | 与前一值成对，语义未知 |
| `79..81` | 三个零 VarInt32 | 数值块 | 具体字段未知 |
| `82..85` | bit-packed flags 与单独 VarInt32 | bit stream | 不能按字节边界当成 4 个独立字段 |
| `91` | `05` | VarInt32 5 | 数值字段 |
| `94` | `01` | VarInt32 1 | 数值字段 |
| `100..101` | `9F 5E` | VarInt32 6047 | 数值字段 |
| `103..107` | `90 E5 E8 86 07` | VarInt32 946,674,000 | 数值/时间候选，尚未证明 |
| `109..112` | `FF FF FF FF` | optional string sentinel | 明确空值/缺失 |
| `116..136` | 21 个零 VarInt32 | 零初始化 primitive tail | 单个字段含义未知 |

这里最容易产生的误判是把 `m_decks`、`m_cards` 等 Object A 成员名硬套到
Object B。当前证据不支持这样做。Object B 只能较稳妥地描述为
“小型玩家/账户状态对象”，它与 Hay Day `ClientAvatar` 的架构相似，但没有
当前构建的线格式等价证明。

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

## 当前 SO 名称对照

当前 SO 中有 246 条
`Too many items in array decode, var:<member>` diagnostic，分布在 122 个
decode helper。本次 trace 执行到其中 23 个 helper，这些 helper 的精确名称与
实际读取区间如下。区间是 helper 观测范围的最小值到最大值，不代表区间内每一
字节都属于某个同名数组。

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

## 与 IPA 和 SupercellProxy 对照

旧 Clash Royale IPA 的 `23245` 分析给出：

- 79 条字段绑定；
- 39 个 decode helper；
- 74 个不同成员名。

本次当前构建 trace 命中其中 48 个名称，即 64.86%。IPA 的绝对地址来自另一
版本，只能保留为 reference-only；成员名必须通过当前 SO 的 exact diagnostic
和实际 helper 执行重新验证。当前 schema 已记录命中的 48 个名称及未命中的
26 个名称。

SupercellProxy 没有 `23245` 的直接实现。最近的结构类比是 Hay Day
`OwnHomeDataMessage(24180)`：同样包含账户/主场景状态、时间值和多个复杂
子对象。但两边消息 ID、顶层对象数量和线格式不同，因此只能采用“启动状态包”
这一层架构类比：

- 不把 `23245` 重命名为 `OwnHomeDataMessage`；
- 不把 Object B 直接重命名为 `ClientAvatar`；
- 不从 Hay Day 类字段回填当前构建偏移；
- 可以把 `m_badgesShownInProfile` 等当前 SO 已证明的字段作为理解 profile
  内容的本地证据。

## 仍未决

1. Object A 和 Object B 的正式业务类名。
2. 外层字段 0、3 的具体消费者和语义。
3. Object B `+0/+5/+10` 三组 identity-like 记录的关系。
4. `5,000,001`、`5,000,002`、`5,000,008` 是否均为 content/card ID。
5. Object A 中共享 helper 对应的 sibling array 的确切父偏移和元素类型。
6. 946,674,000 是时间、金额、经验值还是其他整数状态。

下一最小实验应直接观察 Object B 的显示名消费者，同时选取一个带明确 count
和元素边界的 banner/reward helper，将至少一个 helper-scope 名称转换为精确
父偏移。这样既能验证 `CodexAITest`，也能验证重复数组模型。
