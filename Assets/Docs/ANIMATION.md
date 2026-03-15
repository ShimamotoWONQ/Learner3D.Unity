# Animation System

## Overview

アニメーションは 4 層構造で管理される。

```
AnimationManager          ← StepManager と UI の橋渡し
  └─ AnimationSequence    ← step 内の node 列を管理
       └─ AnimationStep   ← 並列再生する node のグループ
            └─ AnimationStepNode  ← Animator を持つ 1 オブジェクト
```

---

## Classes

### AnimationStepNode
`Scripts/Animation/AnimationStepNode.cs`

- `Animator` を持つ GameObject に付ける。`RequireComponent(typeof(Animator))`。
- `Awake()` で `_animator.speed = 0` にして自動再生を止める。
- `Play()` で `Rebind()` してから `speed = 1` で再生開始。
- **完了検出は `Update()` のポーリング方式。** Animation Event は使わない。
  - `normalizedTime` が前フレームから `> 0.9 → < 0.1` に急落したらループ境界と判断し `OnCompleted` を発火。
  - Animator Controller 側に Exit Time トリガーの遷移があると `normalizedTime` が `1.0` に達せずスキップされるため、`>= 1f` チェックではなくこの差分検出を採用している。
- Animation Clip の **Loop Time は ON のままでよい**（スクリプト側で 1 周で止める）。

**公開イベント:**
| イベント | タイミング |
|---------|----------|
| `OnCompleted` | アニメーション 1 周完了時 |

**公開メソッド:**
| メソッド | 動作 |
|---------|------|
| `Play()` | Entry から再生 |
| `ResetAndPlay()` | `Play()` と同じ（Back 用エイリアス） |
| `Stop()` | 停止（`_isPlaying = false`, `speed = 0`） |
| `Pause()` | `speed = 0`（`_isPlaying` は維持） |
| `Resume()` | `speed = 1` |

---

### AnimationStep
`Scripts/Animation/AnimationStep.cs`

- `[Serializable]` クラス（MonoBehaviour ではない）。`AnimationSequence` の `[SerializeField]` リストに入る。
- 複数の `AnimationStepNode` を **並列再生** する。全 node が `OnCompleted` を返したら `OnStepCompleted` を発火。

**公開イベント:**
| イベント | タイミング |
|---------|----------|
| `OnStepCompleted` | 全 node 完了時 |

---

### AnimationSequence
`Scripts/Animation/AnimationSequence.cs`

- step のリスト（`List<AnimationStep>`）を順番に管理する MonoBehaviour。
- **node 完了後は自動進行しない。** `Next()` が呼ばれるまで待機する。
- `Next()` は再生中・待機中問わず現 step を止めて次へ進む。最後の step で呼ぶと `OnSequenceEnded` を発火。
- `Back()` は前 step へ戻る。先頭で呼ぶと `OnAtFirstStep` を発火（step 遷移は自身では行わない）。
- `PlayFromEnd()` は最後の step から再生を開始する（前 step からの Prev 境界越え時に使う）。

**公開イベント:**
| イベント | タイミング |
|---------|----------|
| `OnSequenceEnded` | 最後の step で `Next()` を呼んだとき |
| `OnAtFirstStep` | 先頭の step で `Back()` を呼んだとき |

**公開メソッド:**
| メソッド | 動作 |
|---------|------|
| `Play()` | index 0 から再生 |
| `PlayFromEnd()` | 最後の index から再生 |
| `Next()` | 現 step を stop して次へ（最後なら `OnSequenceEnded`） |
| `Back()` | 現 step を stop して前へ（先頭なら `OnAtFirstStep`） |
| `Pause()` / `Resume()` | 現 step に委譲 |

---

### AnimationManager
`Scripts/Animation/AnimationManager.cs`

- `StepManager` と `AnimationSequence` の中間層。
- `StepManager.LoadStep()` から `PlayAnimation(stepIndex)` を呼ばれる。
  - 対象 step の holder から `AnimationSequence` を取得。
  - `_currentSequence` が null（非アニメーション step）の場合、`Next()`/`Back()` は即 step 遷移イベントを発火する。
- `_shouldPlayFromEnd` フラグで、次の `PlayAnimation()` 呼び出し時に `PlayFromEnd()` を使うかを制御する。

**Inspector フィールド:**
| フィールド | 説明 |
|-----------|------|
| `objectManager` | step holder の取得元 |

**公開イベント:**
| イベント | タイミング |
|---------|----------|
| `OnNextStepRequested` | 最後 node で `Next()` / 非アニメーション step で `Next()` |
| `OnPrevStepRequested` | 先頭 node で `Back()` / 非アニメーション step で `Back()` |

**公開メソッド:**
| メソッド | 動作 |
|---------|------|
| `Init()` | 初期化（現状は空） |
| `PlayAnimation(int stepIndex)` | 指定 step のシーケンスを再生 |
| `Next()` | 現シーケンスに委譲、または即 step 遷移 |
| `Back()` | 現シーケンスに委譲、または即 step 遷移 |

---

## Data Flow

### Next を押したとき

```
UI / InputManager
  → AnimationManager.Next()
      [sequence あり] → AnimationSequence.Next()
          [中間 step] → 次の AnimationStep を再生（待機へ）
          [最後 step] → OnSequenceEnded
                          → AnimationManager.HandleSequenceEnded()
                              → OnNextStepRequested
                                  → StepManager.SkipToNextStep()
      [sequence なし] → OnNextStepRequested → StepManager.SkipToNextStep()
```

### Back を押したとき（先頭 node）

```
UI / InputManager
  → AnimationManager.Back()
      → AnimationSequence.Back()
          [先頭] → OnAtFirstStep
                    → AnimationManager.HandleAtFirstStep()
                        → _shouldPlayFromEnd = true
                        → OnPrevStepRequested
                            → StepManager.SkipToPrevStep()
                                → LoadStep()
                                    → AnimationManager.PlayAnimation()
                                        → PlayFromEnd()  ← _shouldPlayFromEnd が true
```

---

## Unity Setup

1. シーンに `AnimationManager` コンポーネントを持つ GameObject を作成。
2. `AnimationManager.objectManager` に `ObjectManager` をアサイン。
3. `StepManager.animationManager` に上記をアサイン。
4. アニメーションあり step の holder 配下に `AnimationSequence` を持つ GameObject を置く。
5. `AnimationSequence.steps` に `AnimationStep` を列挙し、各 step の `nodes` に `AnimationStepNode` をアサイン。
6. 非アニメーション step は `AnimationSequence` を持たなければ自動的に素通りになる。

### Animator Controller の注意事項

- Animation Clip の **Loop Time は ON のままでよい**。
- Exit Time トリガーの遷移があっても問題ない（スクリプト側でループ境界を検出して停止する）。
- `AnimationStepNode` は Animation Event を使わない。Clip に `NotifyCompleted` イベントが残っていても無害。
