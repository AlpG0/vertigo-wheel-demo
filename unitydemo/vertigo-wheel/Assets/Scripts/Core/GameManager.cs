using System.Collections.Generic; // List burda
using UnityEngine; // MonoBehaviour, SerializeField burda
using VertigoWheel.Data; // WheelConfig, WheelSegmentData, ZonePreset burda
using VertigoWheel.Rewards; // RewardDefinition, IRunRewards, IWallet, IRewardSink burda
using VertigoWheel.UI; // ActionButtonsView, HudView burda
using VertigoWheel.Wheel; // WheelView, IWheelResultPicker, RandomWheelResultPicker burda
using VertigoWheel.Zone; // ZoneManager, ZoneType burda

namespace VertigoWheel.Core
{
    /// <summary>
    /// Butun sistemleri (wheel, butonlar, hud, zone, oduller) birbirine baglayip oyunun akisini yoneten ana sinif.
    /// </summary>
    public class GameManager : MonoBehaviour, IRewardSink // odul sonucunu (topla / patla) bu sinif teslim aliyor
    {
        private const string BombRiskMessageFormat = "Topladığın {0} ödülü kaybetme!"; // popup acikken oduller henuz kaybedilmedi, GIVE UP'a basilirsa kaybedilecek
        private const int ReviveGoldCost = 25; // altinla devam etmenin bedeli
        private const int StartingCash = 10000; // oyuncunun baslangic bakiyesi
        private const int StartingGold = 950;

        private enum GameState // oyunun su anki akis durumu, artik dagilmis bool bayraklar yerine tek yerden yonetiliyor
        {
            Idle, // spin'e basilabilir
            Spinning, // wheel donuyor veya odul ucuyor, hicbir butona basilamaz
            PopupOpen // bomba popup'i acik
        }

        [SerializeField] private WheelView wheelViewRef; // Inspector'dan surukleyecegimiz somut referans
        [SerializeField] private WheelSpinAnimator wheelSpinAnimatorRef; // Inspector'dan surukleyecegimiz somut referans
        [SerializeField] private WheelThemeView wheelThemeViewRef; // Inspector'dan surukleyecegimiz somut referans
        [SerializeField] private ActionButtonsView actionButtonsViewRef; // Inspector'dan surukleyecegimiz somut referans
        [SerializeField] private HudView hudViewRef; // Inspector'dan surukleyecegimiz somut referans
        [SerializeField] private RewardPopupView rewardPopupViewRef; // Inspector'dan surukleyecegimiz somut referans
        [SerializeField] private RewardTravelAnimator rewardTravelAnimatorRef; // Inspector'dan surukleyecegimiz somut referans
        [SerializeField] private List<ZonePreset> zonePresets; // her zone turu (Normal/Safe/Super) icin config+baslik+sprite paketi, bunu elle dolduracagiz

        private IWheelView wheelView; // GameManager'in gercekten kullandigi tip: Unity Inspector interface tutamadigi icin somut Ref'ten burada atiyoruz
        private IWheelSpinAnimator wheelSpinAnimator; // ayni sebeple arayuz tipinde
        private IWheelThemeView wheelThemeView; // ayni sebeple arayuz tipinde
        private IActionButtonsView actionButtonsView; // ayni sebeple arayuz tipinde
        private IHudView hudView; // ayni sebeple arayuz tipinde
        private IRewardPopupView rewardPopupView; // ayni sebeple arayuz tipinde
        private IRewardTravelAnimator rewardTravelAnimator; // ayni sebeple arayuz tipinde

        private IWheelResultPicker resultPicker; // sonucu secen mantik, arayuz tipinde tutuyoruz
        private IPlayerRunState runState; // oyuncunun hangi zone'da oldugu
        private IZoneManager zoneManager; // su anki zone'un turunu hesaplayan mantik, arayuz tipinde tutuyoruz
        private IRunRewards runRewards; // bu run'da toplanan, bombayla kaybedilebilecek oduller
        private IWallet wallet; // kalici nakit/altin bakiyesi
        private IRewardBank rewardBank; // cikista odullerin aktarildigi yer
        private WheelSegmentData pendingResult; // spin animasyonu bitince uygulanacak sonucu gecici olarak burada tutuyoruz
        private int pendingWinningIndex; // odul gidis animasyonu icin kazanan segmentin index'ini de sakliyoruz
        private GameState currentState = GameState.Idle; // oyun basta bekleme durumunda baslar

        private void OnValidate() // sahnedeki diger scriptleri otomatik bul
        {
            wheelViewRef = GetComponentInChildren<WheelView>(); // altimdaki WheelView'i bul
            wheelSpinAnimatorRef = GetComponentInChildren<WheelSpinAnimator>(); // altimdaki WheelSpinAnimator'i bul
            wheelThemeViewRef = GetComponentInChildren<WheelThemeView>(); // altimdaki WheelThemeView'i bul
            actionButtonsViewRef = GetComponentInChildren<ActionButtonsView>(); // altimdaki ActionButtonsView'i bul
            hudViewRef = GetComponentInChildren<HudView>(); // altimdaki HudView'i bul
            rewardPopupViewRef = GetComponentInChildren<RewardPopupView>(true); // true: popup pasif basladigi icin inactive de dahil ara
            rewardTravelAnimatorRef = GetComponentInChildren<RewardTravelAnimator>(true); // true: ucus objesi basta pasif basliyor
        }

        private void Awake() // sahne yuklenince ilk calisan metot
        {
            wheelView = wheelViewRef; // somut MonoBehaviour ayni zamanda arayuzu de implemente ediyor, direkt atanabilir
            wheelSpinAnimator = wheelSpinAnimatorRef;
            wheelThemeView = wheelThemeViewRef;
            actionButtonsView = actionButtonsViewRef;
            hudView = hudViewRef;
            rewardPopupView = rewardPopupViewRef;
            rewardTravelAnimator = rewardTravelAnimatorRef;

            resultPicker = CreateResultPicker(); // hangi picker kullanilacagini bu metoda birakiyoruz, direkt burada new'lemiyoruz
            runState = CreatePlayerRunState(); // ayni sekilde run state'i de fabrika metodundan aliyoruz
            zoneManager = CreateZoneManager(); // ayni sekilde zone manager'i da fabrika metodundan aliyoruz
            runRewards = new RunRewards();
            wallet = new Wallet(StartingCash, StartingGold);
            rewardBank = new PlayerBank(wallet);

            actionButtonsView.OnSpinClicked += HandleSpinClicked; // spin event'ine kendi metodumuzu bagliyoruz
            actionButtonsView.OnLeaveClicked += HandleLeaveClicked; // leave event'ine kendi metodumuzu bagliyoruz
            rewardPopupView.OnGiveUpClicked += HandleBombGiveUp;
            rewardPopupView.OnGoldReviveClicked += HandleGoldRevive;
            rewardPopupView.OnAdReviveClicked += HandleAdRevive;
        }

        protected virtual IWheelResultPicker CreateResultPicker() // hangi IWheelResultPicker kullanilacagini belirler
        {
            return new RandomWheelResultPicker(); // varsayilan olarak rastgele secen picker'i kullaniyoruz
        }

        protected virtual IPlayerRunState CreatePlayerRunState() // hangi IPlayerRunState kullanilacagini belirler
        {
            return new PlayerRunState(); // varsayilan gercek uygulamayi kullaniyoruz
        }

        protected virtual IZoneManager CreateZoneManager() // hangi IZoneManager kullanilacagini belirler
        {
            return new ZoneManager(); // varsayilan gercek uygulamayi kullaniyoruz
        }

        private void Start() // Awake'ten sonra calisir
        {
            RefreshViewsForCurrentZone(); // baslangic ekranini su anki zone'a gore kur
            SetState(GameState.Idle); // basta spin'e basilabilir durumdayiz
        }

        private ZoneType GetCurrentZoneType() // su anki zone'un turunu dondurur, tek yerden hesapliyoruz
        {
            return zoneManager.GetZoneType(runState.CurrentZone);
        }

        private ZonePreset GetCurrentPreset() // su anki zone'a ait veri paketini (config+baslik+sprite) listeden bulur
        {
            ZoneType zoneType = GetCurrentZoneType();

            foreach (ZonePreset preset in zonePresets)
            {
                if (preset.ZoneType == zoneType)
                {
                    return preset;
                }
            }

            return zonePresets[0]; // buraya gelinmemeli (her zone turu icin bir preset olmali), guvenlik icin ilkini donduruyoruz
        }

        private void RefreshViewsForCurrentZone() // su anki zone'a gore tum ekranlari tek yerden gunceller
        {
            ZonePreset preset = GetCurrentPreset();

            hudView.SetSpinTitle(preset.SpinTitle);
            hudView.SetTotal(runRewards.CollectedRewards.Count); // gecici: yeni layout'ta (3. adim) bu dairenin yerine sol odul listesi gelecek
            wheelView.ShowConfig(preset.WheelConfig);
            wheelThemeView.ApplyTheme(preset.WheelBaseSprite, preset.IndicatorSprite);
        }

        private bool CanLeaveCurrentZone() // pdf kurali: sadece safe/super zone'da leave edilebilir
        {
            ZoneType zoneType = GetCurrentZoneType();
            return zoneType == ZoneType.Safe || zoneType == ZoneType.Super;
        }

        private void SetState(GameState newState) // oyunun durumunu degistirir, buton etkilesimi buradan turer
        {
            currentState = newState;

            bool isIdle = currentState == GameState.Idle;
            actionButtonsView.SetSpinInteractable(isIdle);
            actionButtonsView.SetLeaveInteractable(isIdle && CanLeaveCurrentZone());
        }

        private void HandleSpinClicked() // spin'e basilinca calisir
        {
            WheelConfig currentConfig = GetCurrentPreset().WheelConfig;
            WheelSegmentData result = resultPicker.PickSegment(currentConfig);
            int winningIndex = currentConfig.Segments.IndexOf(result);

            pendingResult = result;
            pendingWinningIndex = winningIndex;

            SetState(GameState.Spinning);

            wheelSpinAnimator.SpinTo(winningIndex, currentConfig.Segments.Count, HandleSpinAnimationComplete);
        }

        private void HandleSpinAnimationComplete() // animasyon bitince calisir
        {
            pendingResult.Reward.Apply(this, pendingResult.Amount); // turu biz sormuyoruz: esyaysa Collect, bombaysa Explode kendiliginden cagrilir
        }

        public void Collect(RewardDefinition reward, int amount) // IRewardSink: bir odul kazanildi
        {
            runState.AdvanceZone();

            Vector3 fromPosition = wheelView.GetSegmentWorldPosition(pendingWinningIndex);
            Vector3 toPosition = hudView.GetTotalWorldPosition();
            rewardTravelAnimator.Play(reward.Icon, fromPosition, toPosition, HandleRewardTravelComplete); // odul listeye ikon yerine varinca eklensin
        }

        public void Explode() // IRewardSink: bombaya carpildi
        {
            SetState(GameState.PopupOpen);

            string message = string.Format(BombRiskMessageFormat, runRewards.CollectedRewards.Count);
            bool canAffordGoldRevive = wallet.GetBalance(CurrencyType.Gold) >= ReviveGoldCost;
            rewardPopupView.Show(message, canAffordGoldRevive);
        }

        private void HandleRewardTravelComplete() // odul ikonu hedefe ulasinca calisir
        {
            runRewards.Add(pendingResult.Reward, pendingResult.Amount);
            RefreshViewsForCurrentZone();
            SetState(GameState.Idle);
        }

        private void HandleBombGiveUp() // GIVE UP: bu run'da toplanan her sey kaybedilir
        {
            runRewards.Clear();
            runState.ResetZone();

            rewardPopupView.Hide();
            RefreshViewsForCurrentZone();
            SetState(GameState.Idle);
        }

        private void HandleGoldRevive() // altinla devam: bedeli cuzdandan dusulur
        {
            if (!wallet.TrySpend(CurrencyType.Gold, ReviveGoldCost)) // buton zaten pasif olmali ama yine de guvenlik
            {
                return;
            }

            Revive();
        }

        private void HandleAdRevive() // reklamla devam: bedava (reklam sistemi yok, izlenmis sayiyoruz)
        {
            Revive();
        }

        private void Revive() // hicbir sey kaybedilmez, kalinan yerden devam
        {
            rewardPopupView.Hide();
            SetState(GameState.Idle);
        }

        private void HandleLeaveClicked() // cikis: toplanan oduller kalici hesaba aktarilir, yeni run baslar
        {
            runRewards.BankInto(rewardBank);
            runState.ResetZone();

            RefreshViewsForCurrentZone();
            SetState(GameState.Idle);
        }
    }
}
