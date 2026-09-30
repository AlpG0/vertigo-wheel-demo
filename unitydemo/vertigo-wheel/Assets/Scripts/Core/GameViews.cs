using VertigoWheel.UI;
using VertigoWheel.Wheel;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Oyun akisinin konustugu butun view'lar, sadece arayuz tipleriyle. Hangi somut sinifin geldigini sadece GameInstaller bilir.
    /// </summary>
    public class GameViews
    {
        private readonly IWheelView wheel;
        private readonly IWheelSpinAnimator wheelSpinAnimator;
        private readonly IWheelThemeView wheelTheme;
        private readonly IActionButtonsView actionButtons;
        private readonly IHudView hud;
        private readonly IZoneBarView zoneBar;
        private readonly IUpcomingZonesView upcomingZones;
        private readonly IRewardListView rewardList;
        private readonly IWalletView wallet;
        private readonly IRewardPopupView rewardPopup;
        private readonly IRewardTravelAnimator rewardTravel;

        public GameViews(IWheelView wheel, IWheelSpinAnimator wheelSpinAnimator, IWheelThemeView wheelTheme, IActionButtonsView actionButtons, IHudView hud,
            IZoneBarView zoneBar, IUpcomingZonesView upcomingZones, IRewardListView rewardList, IWalletView wallet, IRewardPopupView rewardPopup, IRewardTravelAnimator rewardTravel)
        {
            this.wheel = wheel;
            this.wheelSpinAnimator = wheelSpinAnimator;
            this.wheelTheme = wheelTheme;
            this.actionButtons = actionButtons;
            this.hud = hud;
            this.zoneBar = zoneBar;
            this.upcomingZones = upcomingZones;
            this.rewardList = rewardList;
            this.wallet = wallet;
            this.rewardPopup = rewardPopup;
            this.rewardTravel = rewardTravel;
        }

        public IWheelView Wheel { get { return wheel; } }
        public IWheelSpinAnimator WheelSpinAnimator { get { return wheelSpinAnimator; } }
        public IWheelThemeView WheelTheme { get { return wheelTheme; } }
        public IActionButtonsView ActionButtons { get { return actionButtons; } }
        public IHudView Hud { get { return hud; } }
        public IZoneBarView ZoneBar { get { return zoneBar; } }
        public IUpcomingZonesView UpcomingZones { get { return upcomingZones; } }
        public IRewardListView RewardList { get { return rewardList; } }
        public IWalletView Wallet { get { return wallet; } }
        public IRewardPopupView RewardPopup { get { return rewardPopup; } }
        public IRewardTravelAnimator RewardTravel { get { return rewardTravel; } }
    }
}
