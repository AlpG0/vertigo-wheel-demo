using UnityEngine;
using VertigoWheel.Rewards;
using VertigoWheel.UI;
using VertigoWheel.Wheel;
using VertigoWheel.Zone;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Composition root: hangi arayuzun hangi somut sinifla karsilanacagini bilen TEK yer.
    /// Servisleri olusturur, view'lari arayuz tipiyle bulur ve hepsini constructor ile GameController'a verir.
    /// </summary>
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private GameSettings settings; // zone'lar, baslangic bakiyesi, revive bedeli

        private GameController controller;

        private void Awake()
        {
            GameViews views = new GameViews(
                FindView<IWheelView>(),
                FindView<IWheelSpinAnimator>(),
                FindView<IWheelThemeView>(),
                FindView<IActionButtonsView>(),
                FindView<IHudView>(),
                FindView<IRewardPopupView>(),
                FindView<IRewardTravelAnimator>());

            IWallet wallet = new Wallet(settings.StartingCash, settings.StartingGold);
            IRunRewards runRewards = new RunRewards();
            IRewardBank rewardBank = new PlayerBank(wallet);
            IZoneService zoneService = new ZoneService(settings.Zones);
            IPlayerRunState runState = new PlayerRunState();
            ZonePresenter presenter = new ZonePresenter(zoneService, runState, runRewards, views);

            GameContext context = new GameContext(settings, zoneService, runState, runRewards, wallet, rewardBank, CreateResultPicker(), views, presenter);
            controller = new GameController(context);
        }

        private void Start()
        {
            controller.Start();
        }

        private void OnDestroy()
        {
            if (controller != null)
            {
                controller.Dispose();
            }
        }

        private IWheelResultPicker CreateResultPicker()
        {
            if (settings.DebugForcedSegmentIndex >= 0) // QA: hep ayni dilim (ornegin bomba akisini tekrar tekrar test etmek icin)
            {
                return new FixedIndexResultPicker(settings.DebugForcedSegmentIndex);
            }

            return new RandomWheelResultPicker();
        }

        private T FindView<T>() where T : class // view'i somut tipiyle degil arayuzuyle ariyoruz, bu sinif da view siniflarina bagimli olmasin
        {
            T view = GetComponentInChildren<T>(true); // true: popup gibi pasif baslayan objeler dahil
            if (view == null)
            {
                Debug.LogError(string.Format("{0} sahnede bulunamadi.", typeof(T).Name), this);
            }

            return view;
        }
    }
}
