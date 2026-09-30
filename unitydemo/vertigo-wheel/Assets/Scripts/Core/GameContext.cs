using VertigoWheel.Rewards;
using VertigoWheel.Wheel;
using VertigoWheel.Zone;

namespace VertigoWheel.Core
{
    /// <summary>
    /// State'lerin paylastigi her sey: servisler, view'lar ve o anki spin sonucu. Hepsi disaridan (GameInstaller'dan) verilir.
    /// </summary>
    public class GameContext
    {
        private readonly GameSettings settings;
        private readonly IZoneService zoneService;
        private readonly IPlayerRunState runState;
        private readonly IRunRewards runRewards;
        private readonly IWallet wallet;
        private readonly IRewardBank rewardBank;
        private readonly IWheelResultPicker resultPicker;
        private readonly GameViews views;
        private readonly ZonePresenter presenter;
        private SpinResult currentSpin; // son spin'in sonucu, animasyonlar boyunca state'ler arasinda tasinir

        public GameContext(GameSettings settings, IZoneService zoneService, IPlayerRunState runState, IRunRewards runRewards, IWallet wallet, IRewardBank rewardBank, IWheelResultPicker resultPicker, GameViews views, ZonePresenter presenter)
        {
            this.settings = settings;
            this.zoneService = zoneService;
            this.runState = runState;
            this.runRewards = runRewards;
            this.wallet = wallet;
            this.rewardBank = rewardBank;
            this.resultPicker = resultPicker;
            this.views = views;
            this.presenter = presenter;
        }

        public GameSettings Settings { get { return settings; } }
        public IPlayerRunState RunState { get { return runState; } }
        public IRunRewards RunRewards { get { return runRewards; } }
        public IWallet Wallet { get { return wallet; } }
        public IRewardBank RewardBank { get { return rewardBank; } }
        public IWheelResultPicker ResultPicker { get { return resultPicker; } }
        public GameViews Views { get { return views; } }
        public ZonePresenter Presenter { get { return presenter; } }
        public ZoneDefinition CurrentZone { get { return zoneService.GetZone(runState.CurrentZone); } }

        public SpinResult CurrentSpin
        {
            get { return currentSpin; }
            set { currentSpin = value; }
        }
    }
}
