using System.Collections.Generic; // List burda
using UnityEngine; // MonoBehaviour, SerializeField burda
using VertigoWheel.Data; // WheelConfig, WheelSegmentData, RewardType, ZonePreset burda
using VertigoWheel.UI; // ActionButtonsView, HudView burda
using VertigoWheel.Wheel; // WheelView, IWheelResultPicker, RandomWheelResultPicker burda
using VertigoWheel.Zone; // ZoneManager, ZoneType burda

namespace VertigoWheel.Core
{
    /// <summary>
    /// Butun sistemleri (wheel, butonlar, hud, zone) birbirine baglayip oyunun akisini yoneten ana sinif.
    /// </summary>
    public class GameManager : MonoBehaviour // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour
    {
        private const string BombLostMessageFormat = "BOMBA! {0} odulu kaybettin."; // bomba mesaji artik tek yerde, string.Format ile dolduruluyor

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
        private IPlayerRunState runState; // oyuncunun anlik durumu, arayuz tipinde tutuyoruz
        private IZoneManager zoneManager; // su anki zone'un turunu hesaplayan mantik, arayuz tipinde tutuyoruz
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

            actionButtonsView.OnSpinClicked += HandleSpinClicked; // spin event'ine kendi metodumuzu bagliyoruz
            actionButtonsView.OnLeaveClicked += HandleLeaveClicked; // leave event'ine kendi metodumuzu bagliyoruz
            rewardPopupView.OnCloseClicked += HandleRewardPopupClosed; // popup kapatilinca kendi metodumuzu bagliyoruz
        }

        protected virtual IWheelResultPicker CreateResultPicker() // hangi IWheelResultPicker kullanilacagini belirler
        {
            return new RandomWheelResultPicker(); // varsayilan olarak rastgele secen picker'i kullaniyoruz
        } // virtual oldugu icin bir alt sinif (ornegin test amacli) bunu ezip farkli bir picker donduebilir, boylece interface gercekten ise yariyor

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
            return zoneManager.GetZoneType(runState.CurrentZone); // zoneManager'a soruyoruz, zoneManager zone numarasina gore safe/super/normal donduruyor
        }

        private ZonePreset GetCurrentPreset() // su anki zone'a ait veri paketini (config+baslik+sprite) listeden bulur
        {
            ZoneType zoneType = GetCurrentZoneType(); // once zone turunu ogreniyoruz

            foreach (ZonePreset preset in zonePresets) // listedeki her preset icin tek tek bak
            {
                if (preset.ZoneType == zoneType) // aradigimiz zone turu bu mu
                {
                    return preset; // bulduk, dondur
                }
            }

            return zonePresets[0]; // buraya gelinmemeli (her zone turu icin bir preset olmali), guvenlik icin ilkini donduruyoruz
        }

        private void RefreshViewsForCurrentZone() // su anki zone'a gore tum ekranlari tek yerden gunceller
        {
            ZonePreset preset = GetCurrentPreset(); // once bu zone'a ait veri paketini al

            hudView.SetSpinTitle(preset.SpinTitle); // ust basligi guncelle
            hudView.SetTotal(runState.TotalValue); // toplam odulu guncelle
            wheelView.ShowConfig(preset.WheelConfig); // wheel'in segmentlerini guncelle
            wheelThemeView.ApplyTheme(preset.WheelBaseSprite, preset.IndicatorSprite); // wheel'in govde/indicator gorselini guncelle
        }

        private bool CanLeaveCurrentZone() // pdf kurali: sadece safe/super zone'da leave edilebilir
        {
            ZoneType zoneType = GetCurrentZoneType(); // su anki zone turu ne
            return zoneType == ZoneType.Safe || zoneType == ZoneType.Super;
        }

        private void SetState(GameState newState) // oyunun durumunu degistirir, buton etkilesimi buradan turer
        {
            currentState = newState; // yeni durumu sakla

            bool isIdle = currentState == GameState.Idle; // sadece Idle'dayken butonlara basilabilir
            actionButtonsView.SetSpinInteractable(isIdle); // spin sadece Idle'da acik
            actionButtonsView.SetLeaveInteractable(isIdle && CanLeaveCurrentZone()); // leave hem Idle hem safe/super zone gerektirir
        }

        private void HandleSpinClicked() // spin'e basilinca calisir
        {
            WheelConfig currentConfig = GetCurrentPreset().WheelConfig; // bu spin icin dogru config hangisi
            WheelSegmentData result = resultPicker.PickSegment(currentConfig); // o config'ten sonuc sec
            int winningIndex = currentConfig.Segments.IndexOf(result); // sonucun listedeki index'i, animasyon icin lazim

            pendingResult = result; // animasyon bitince kullanmak icin sonucu sakla
            pendingWinningIndex = winningIndex; // odul gidis animasyonu bu index'ten baslayacak, onu da sakla

            SetState(GameState.Spinning); // animasyon bitene kadar hicbir butona basilamasin

            wheelSpinAnimator.SpinTo(winningIndex, currentConfig.Segments.Count, HandleSpinAnimationComplete); // wheel'i dondur, bitince HandleSpinAnimationComplete'i cagir (parametresiz, o yuzden metot adini direkt verebiliyoruz)
        }

        private void HandleSpinAnimationComplete() // animasyon bitince calisir, sonucu pendingResult'tan okur
        {
            WheelSegmentData result = pendingResult; // sakladigimiz sonucu geri al

            if (result.RewardType == RewardType.Bomb) // bombaya carptiysak
            {
                HandleBombResult(); // bomba akisi artik kendi metodunda
                return; // butonlari burada acmiyoruz, popup kapaninca acilacak
            }

            HandleRewardResult(result); // odul akisi artik kendi metodunda
        }

        private void HandleBombResult() // bombaya carpinca yapilacak her seyi burada topluyoruz
        {
            int lostAmount = runState.TotalValue; // sifirlanmadan once kaybedilen miktari not al
            runState.ResetRun(); // her sey sifirlanir

            RefreshViewsForCurrentZone(); // yeni run'un ekranini kur
            SetState(GameState.PopupOpen); // popup acik oldugu surece butonlar kapali kalsin

            rewardPopupView.Show(string.Format(BombLostMessageFormat, lostAmount)); // kaybi popup'ta goster
        }

        private void HandleRewardResult(WheelSegmentData result) // odul kazaninca yapilacak her seyi burada topluyoruz
        {
            runState.AddReward(result.Amount); // odulu topluyoruz
            runState.AdvanceZone(); // bir zone ilerliyoruz

            Vector3 fromPosition = wheelView.GetSegmentWorldPosition(pendingWinningIndex); // ucusun baslayacagi yer, kazanan segmentin ikonu
            Vector3 toPosition = hudView.GetTotalWorldPosition(); // ucusun bitecegi yer, TOTAL dairesi
            rewardTravelAnimator.Play(result.Icon, fromPosition, toPosition, HandleRewardTravelComplete); // ikon ucsun, bitince HandleRewardTravelComplete cagrilsin (parametresiz, o yuzden metot adini direkt verebiliyoruz)
        }

        private void HandleRewardTravelComplete() // odul ikonu TOTAL'e ulasinca calisir
        {
            RefreshViewsForCurrentZone(); // yeni zone'un ekranini kur (SetTotal burada da cagrilir, daire pulse atar)
            SetState(GameState.Idle); // spin tekrar tiklanabilir olsun
        }

        private void HandleRewardPopupClosed() // popup'taki TAMAM'a basilinca calisir
        {
            rewardPopupView.Hide(); // popup'i gizle
            SetState(GameState.Idle); // spin tekrar tiklanabilir olsun
        }

        private void HandleLeaveClicked() // leave'e basilinca calisir (sadece safe/super zone'da tiklanabilir oldugu icin buraya gelmesi zaten guvenli)
        {
            runState.EndRun(); // total'i koruyarak zone'u basa al

            RefreshViewsForCurrentZone(); // yeni run'un ekranini kur
            SetState(GameState.Idle); // yeni run icin butonlari guncelle
        }
    }
}