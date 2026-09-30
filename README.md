# Vertigo Wheel Demo

A wheel-of-fortune spin game built in Unity 6 (6000.3.9f1) for the Vertigo Games Game Developer take-home assignment. The in-game UI is in Turkish, matching the reference screen.

## Gameplay

- Press **SPIN** in the middle of the wheel. The wheel lands on a reward, the reward's icon flies to its row in the list on the left, and you advance one zone on the zone bar at the top.
- Rewards vary: cash, gold, weapon points, weapons, grenades, consumables, chests. The same reward won again stacks into its existing row.
- Every **5th** zone is a **Silver** zone and every **30th** a **Gold** zone: no bomb, better rewards. The badges on the right show the next silver and gold zone numbers.
- In normal (bronze) zones one segment is a **bomb**. Hitting it opens a popup:
  - **VAZGEÇ** (give up): everything collected this run is lost and you go back to zone 1.
  - **25 CANLAN**: spend 25 gold to continue where you were. Disabled if you can't afford it.
  - **CANLAN** (ad icon): continue for free. There is no ad SDK; watching is simulated.
- **ÇIKIŞ** (leave) is only allowed in silver/gold zones. It banks the run: cash and gold go to the wallet (bottom right), items go to the inventory, and a new run starts at zone 1.

## Architecture

```
Assets/Scripts/            (VertigoWheel.asmdef)
  Core/     GameInstaller (composition root), GameController, GameContext, GameViews,
            ZonePresenter, RunRewardsPresenter, WalletPresenter, GameSettings, SpinResult,
            PlayerRunState
  Core/States/  GameStateMachine, GameState, IdleState, SpinningState, CollectingState, BombState
  Rewards/  RewardDefinition (+ Item / Currency / Bomb), RunRewards, Wallet, PlayerBank
  Zone/     ZoneDefinition, ZoneService
  Wheel/    WheelView, WheelSpinAnimator, WheelThemeView, IWheelResultPicker (+ Random / FixedIndex)
  UI/       ZoneBarView, RewardListView, UpcomingZonesView, WalletView, HudView, ActionButtonsView,
            RewardPopupView, RewardTravelAnimator, PunchScaleAnimator, PulsingGlowAnimator
  Utils/    GameConstants, NumberFormatter, HierarchyLookup, Easing
Assets/Configs/            GameSettings, zone definitions, wheel configs, reward definitions
Assets/Tests/EditMode/     30 EditMode tests
Assets/Editor/             RevisionLayoutBuilder (tool that built the scene layout)
```

**Composition root.** `GameInstaller` is the only class that knows concrete types. It creates the services, looks views up by interface (`GetComponentInChildren<IHudView>()`), and passes everything through constructors into `GameController` and a shared `GameContext`. Nothing past the installer depends on a concrete view or service.

**State machine.** The flow lives in `IdleState`, `SpinningState`, `CollectingState` and `BombState`. `GameController` declares which transitions are allowed; any other transition is logged and rejected. View input is routed to the current state. Each state handles only the input that makes sense for it, so a click in the wrong state does nothing even if a button were left interactable. Late animation callbacks are dropped once their state is no longer active.

**Rewards are polymorphic.** Each wheel segment points to a `RewardDefinition` asset. When the wheel stops, the flow calls `reward.Apply(sink, amount)`: items and currency collect themselves, the bomb explodes. On leave, each reward banks itself (currency to the wallet, items to the inventory). Adding a new reward kind means adding a subclass; no `if (type == Bomb)` checks.

**Zones are data.** A `ZoneDefinition` asset holds a zone kind's interval, title, subtitle, leave permission, wheel config, sprites and color. `ZoneService` picks the matching definition with the largest interval, so 30 wins over 5. A new zone kind, for example one every 100 zones, is a new asset with no code change; one of the tests shows this.

**Views are passive.** Presenters push state into them: `ZonePresenter` for the zone bar, title, wheel and badges, `RunRewardsPresenter` for the rewards list, `WalletPresenter` for balances. Views auto-wire their own children in `OnValidate()` by name. The names live in `GameConstants.UINames`, and `HierarchyLookup` logs a warning if code and scene drift apart. No button is bound through the Inspector's OnClick list.

**Shared helpers.** `NumberFormatter` is the only place numbers are formatted (`10.000`, `x5`). `Easing` is shared by every animation. Tunable values (starting balance, revive cost, zones) live in the `GameSettings` asset.

## Running

1. Open `unitydemo/vertigo-wheel` in Unity **6000.3.9f1**.
2. Open `Assets/Main.unity` and press Play.

**QA shortcut:** set **Debug Forced Segment Index** on `Assets/Configs/GameSettings` to a segment index (0 is the bomb on the bronze wheel). The installer then uses `FixedIndexResultPicker`, and every spin lands on that segment. Set it back to `-1` for normal play.

## Tests

**Window > General > Test Runner > EditMode > Run All.** There are 30 tests:

- Zone rules and next-zone lookup
- The reward/economy layer
- Both result pickers
- State machine transition validation
- The full game flow through `GameController`, with fake views and fixed pickers: win, bomb, ignored input while the popup is open, give up, gold/ad revive, leave

## Building

**File > Build Settings > Android > Build.** The APK, gameplay video link and screenshots (20:9, 16:9, 4:3) are attached to the latest GitHub Release.

## Git Workflow

Each piece of work (reward/economy layer, architecture refactor, new layout, revive icons, tests, this README) was done on its own `feature/...` branch and merged into `main` after being checked in the Editor.
