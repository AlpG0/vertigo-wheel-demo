# Vertigo Wheel Demo

A wheel-of-fortune style spin mechanic built in Unity 6 (6000.3.9f1) for the Vertigo Games Game Developer take-home assignment.

## Gameplay

- Press **SPIN** to spin the wheel. Landing on a reward adds it to your **TOTAL** and advances you one zone.
- Every **5th** zone is a **Safe** zone (silver wheel) — no bomb segment, higher rewards.
- Every **30th** zone is a **Super** zone (golden wheel) — no bomb segment, the highest rewards.
- Landing on the **bomb** segment (only possible in normal zones) opens a popup with three choices:
  - **GIVE UP** — the run really ends: total resets to 0 and you go back to zone 1.
  - **25 REVIVE** / **REVIVE** — nothing is lost; you keep your current total and zone and can keep spinning right away.
- **LEAVE** is only enabled in Safe/Super zones, and only while you're not mid-spin. It banks your current total and returns you to zone 1 for a fresh run, without losing anything.

## Project Structure

```
Assets/Scripts/
  Core/    GameManager (orchestrates the flow), PlayerRunState + IPlayerRunState (zone/total)
  Data/    RewardType, ZoneType, WheelSegmentData, WheelConfig (ScriptableObject), ZonePreset
  Wheel/   WheelView, WheelSpinAnimator, WheelThemeView, IWheelResultPicker + Random/FixedIndex implementations
  UI/      HudView, ActionButtonsView, RewardPopupView, RewardTravelAnimator, PunchScaleAnimator, PulsingGlowAnimator
  Zone/    ZoneManager + IZoneManager (maps a zone number to Normal/Safe/Super)
```

Each concrete MonoBehaviour that `GameManager` depends on has a matching interface (`IWheelView`, `IHudView`, `IActionButtonsView`, ...). `GameManager` keeps a `[SerializeField]` concrete reference for the Inspector (Unity can't serialize interface fields) and assigns it to an interface-typed field once in `Awake()`, so the rest of the code only ever talks to the abstraction.

Per-zone data (which `WheelConfig` to use, the spin title, and the wheel's base/indicator sprites) lives in a single `List<ZonePreset>` on `GameManager` instead of being spread across if/else chains — adding a new zone type is a data change, not a code change.

The bomb/spin/leave flow is driven by a small `GameState` enum (`Idle` / `Spinning` / `PopupOpen`) instead of loose boolean flags, so button interactability always derives from one place.

Wiring convention: every view auto-finds its own child references in `OnValidate()` (by hierarchy path or by GameObject name), so nothing is hand-bound through the Inspector's OnClick UI — only `GameManager`'s own view/config references are dragged in manually, since those are cross-object dependencies rather than a view's own children.

## Running It

1. Open the project (`unitydemo/vertigo-wheel`) in Unity **6000.3.9f1**.
2. Open `Assets/Main.unity` and press Play.

## Building

Standard Android build via **File > Build Settings > Android > Build**. A built APK is attached to the GitHub Release for this repo, along with a gameplay video and screenshots.

## Git Workflow

Each piece of work (visual pass, spin/top-bar/reward animations, SOLID refactor, bomb popup redesign) was done on its own `feature/...` branch and merged into `main` once verified in the Editor.
