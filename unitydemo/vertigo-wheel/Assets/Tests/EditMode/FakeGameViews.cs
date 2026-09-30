using System;
using System.Collections.Generic;
using UnityEngine;
using VertigoWheel.Core;
using VertigoWheel.Data;
using VertigoWheel.Rewards;
using VertigoWheel.UI;
using VertigoWheel.Wheel;

namespace VertigoWheel.Tests
{
    /// <summary>
    /// Butun view arayuzlerini tek sinifta taklit eder. Animasyonlar beklemeden hemen biter, tiklamalar metotla tetiklenir,
    /// gosterilen seyler alanlara yazilir. Oyun akisi arayuzlere bagli oldugu icin sahne olmadan test edilebiliyor.
    /// </summary>
    internal class FakeGameViews : IWheelView, IWheelSpinAnimator, IWheelThemeView, IActionButtonsView, IHudView,
        IZoneBarView, IUpcomingZonesView, IRewardListView, IWalletView, IRewardPopupView, IRewardTravelAnimator
    {
        public event Action OnSpinClicked;
        public event Action OnLeaveClicked;
        public event Action OnGiveUpClicked;
        public event Action OnGoldReviveClicked;
        public event Action OnAdReviveClicked;

        public bool SpinInteractable;
        public bool LeaveInteractable;
        public bool PopupVisible;
        public bool PopupCanAffordGoldRevive;
        public int SpinCount;
        public int CurrentZoneShown;
        public string ZoneTitleShown;

        public GameViews ToGameViews()
        {
            return new GameViews(this, this, this, this, this, this, this, this, this, this, this);
        }

        public void ClickSpin() { if (OnSpinClicked != null) OnSpinClicked(); }
        public void ClickLeave() { if (OnLeaveClicked != null) OnLeaveClicked(); }
        public void ClickGiveUp() { if (OnGiveUpClicked != null) OnGiveUpClicked(); }
        public void ClickGoldRevive() { if (OnGoldReviveClicked != null) OnGoldReviveClicked(); }
        public void ClickAdRevive() { if (OnAdReviveClicked != null) OnAdReviveClicked(); }

        public void ShowConfig(WheelConfig config) { }
        public Vector3 GetSegmentWorldPosition(int index) { return Vector3.zero; }

        public void SpinTo(int segmentIndex, int segmentCount, Action onComplete) // animasyon yok, hemen bitti say
        {
            SpinCount++;
            onComplete();
        }

        public void ApplyTheme(Sprite baseSprite, Sprite indicatorSprite) { }
        public void SetSpinInteractable(bool interactable) { SpinInteractable = interactable; }
        public void SetLeaveInteractable(bool interactable) { LeaveInteractable = interactable; }
        public void SetZoneInfo(string title, string subtitle) { ZoneTitleShown = title; }
        public void ShowZone(int currentZone, IReadOnlyList<Color> zoneColors, bool animate) { CurrentZoneShown = currentZone; }
        public void Show(IReadOnlyList<UpcomingZoneInfo> zones) { }
        public Vector3 PrepareRow(RewardDefinition reward) { return Vector3.zero; }
        public void SetAmount(RewardDefinition reward, int amount) { }
        public void Clear() { }
        public void SetBalance(CurrencyType type, int amount, bool animate) { }

        public void Show(string message, bool canAffordGoldRevive)
        {
            PopupVisible = true;
            PopupCanAffordGoldRevive = canAffordGoldRevive;
        }

        public void Hide() { PopupVisible = false; }

        public void Play(Sprite icon, Vector3 fromPosition, Vector3 toPosition, Action onComplete) // ucus animasyonu yok, hemen indi say
        {
            onComplete();
        }
    }

    /// <summary>
    /// Sirayla verilen dilimleri donduren picker. IWheelResultPicker'in ucuncu bir uygulamasi: akis hangisi geldigini bilmeden calisir.
    /// </summary>
    internal class QueuedResultPicker : IWheelResultPicker
    {
        private readonly Queue<int> indices;

        public QueuedResultPicker(params int[] indices)
        {
            this.indices = new Queue<int>(indices);
        }

        public WheelSegmentData PickSegment(WheelConfig config)
        {
            return config.Segments[indices.Dequeue()];
        }
    }
}
