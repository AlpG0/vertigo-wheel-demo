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
        [SerializeField] private WheelView wheelView; // wheel'i gosteren sinif, sahnedeki WheelView objesini tutar ve inspector'da görünmesi için SerializeField ile işaretledik
        [SerializeField] private WheelSpinAnimator wheelSpinAnimator; // wheel'i dondurup kazanan segmente hizalayan sinif
        [SerializeField] private WheelThemeView wheelThemeView; // wheel'in bronze/silver/golden gorselini degistiren sinif
        [SerializeField] private ActionButtonsView actionButtonsView; // spin/leave butonlarini dinleyen sinif
        [SerializeField] private HudView hudView; // zone/total yazilarini gunceleyen sinif
        [SerializeField] private RewardPopupView rewardPopupView; // bomba patlayinca gosterilen popup
        [SerializeField] private RewardTravelAnimator rewardTravelAnimator; // kazanilan odul ikonunu wheel'den TOTAL'e ucuran sinif
        [SerializeField] private List<ZonePreset> zonePresets; // her zone turu (Normal/Safe/Super) icin config+baslik+sprite paketi, bunu elle dolduracagiz

        private IWheelResultPicker resultPicker; // sonucu secen mantik, arayuz tipinde tutuyoruz
        private PlayerRunState runState; // oyuncunun anlik durumu (zone, toplam odul)
        private ZoneManager zoneManager; // su anki zone'un turunu hesaplayan sinif
        private WheelSegmentData pendingResult; // spin animasyonu bitince uygulanacak sonucu gecici olarak burada tutuyoruz
        private int pendingWinningIndex; // odul gidis animasyonu icin kazanan segmentin index'ini de sakliyoruz

        private void OnValidate() // sahnedeki diger 3 script'i otomatik bul
        {
            wheelView = GetComponentInChildren<WheelView>(); // altimdaki WheelView'i bul
            wheelSpinAnimator = GetComponentInChildren<WheelSpinAnimator>(); // altimdaki WheelSpinAnimator'i bul
            wheelThemeView = GetComponentInChildren<WheelThemeView>(); // altimdaki WheelThemeView'i bul
            actionButtonsView = GetComponentInChildren<ActionButtonsView>(); // altimdaki ActionButtonsView'i bul
            hudView = GetComponentInChildren<HudView>(); // altimdaki HudView'i bul
            rewardPopupView = GetComponentInChildren<RewardPopupView>(true); // true: popup pasif basladigi icin inactive de dahil ara
            rewardTravelAnimator = GetComponentInChildren<RewardTravelAnimator>(true); // true: ucus objesi basta pasif basliyor
        }

        private void Awake() // sahne yuklenince ilk calisan metot
        {
            resultPicker = CreateResultPicker(); // hangi picker kullanilacagini bu metoda birakiyoruz, direkt burada new'lemiyoruz
            runState = new PlayerRunState(); // oyuncunun durumunu olusturuyoruz
            zoneManager = new ZoneManager(); // zone turunu hesaplayacak sinifi olusturuyoruz

            actionButtonsView.OnSpinClicked += HandleSpinClicked; // spin event'ine kendi metodumuzu bagliyoruz
            actionButtonsView.OnLeaveClicked += HandleLeaveClicked; // leave event'ine kendi metodumuzu bagliyoruz
            rewardPopupView.OnCloseClicked += HandleRewardPopupClosed; // popup kapatilinca kendi metodumuzu bagliyoruz
        }

        protected virtual IWheelResultPicker CreateResultPicker() // hangi IWheelResultPicker kullanilacagini belirler
        {
            return new RandomWheelResultPicker(); // varsayilan olarak rastgele secen picker'i kullaniyoruz
        } // virtual oldugu icin bir alt sinif (ornegin test amacli) bunu ezip farkli bir picker donduebilir, boylece interface gercekten ise yariyor

        private void Start() // Awake'ten sonra calisir
        {
            RefreshViewsForCurrentZone(); // baslangic ekranini su anki zone'a gore kur
            UpdateLeaveInteractable(); // leave butonu basta aktif mi kapali mi ayarla
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

        private void UpdateLeaveInteractable() // leave butonunun tiklanabilir olup olmadigini gunceller
        {
            ZoneType zoneType = GetCurrentZoneType(); // su anki zone turu ne
            bool canLeave = zoneType == ZoneType.Safe || zoneType == ZoneType.Super; // pdf kurali: sadece safe/super zone'da leave edilebilir
            actionButtonsView.SetLeaveInteractable(canLeave); // butonu ac/kapat
        }

        private void HandleSpinClicked() // spin'e basilinca calisir
        {
            WheelConfig currentConfig = GetCurrentPreset().WheelConfig; // bu spin icin dogru config hangisi
            WheelSegmentData result = resultPicker.PickSegment(currentConfig); // o config'ten sonuc sec
            int winningIndex = currentConfig.Segments.IndexOf(result); // sonucun listedeki index'i, animasyon icin lazim

            pendingResult = result; // animasyon bitince kullanmak icin sonucu sakla
            pendingWinningIndex = winningIndex; // odul gidis animasyonu bu index'ten baslayacak, onu da sakla

            actionButtonsView.SetSpinInteractable(false); // animasyon bitene kadar tekrar tiklanamasin
            actionButtonsView.SetLeaveInteractable(false); // animasyon bitene kadar leave da tiklanamasin

            wheelSpinAnimator.SpinTo(winningIndex, currentConfig.Segments.Count, HandleSpinAnimationComplete); // wheel'i dondur, bitince HandleSpinAnimationComplete'i cagir (parametresiz, o yuzden metot adini direkt verebiliyoruz)
        }

        private void HandleSpinAnimationComplete() // animasyon bitince calisir, sonucu pendingResult'tan okur
        {
            WheelSegmentData result = pendingResult; // sakladigimiz sonucu geri al

            if (result.RewardType == RewardType.Bomb) // bombaya carptiysak
            {
                int lostAmount = runState.TotalValue; // sifirlanmadan once kaybedilen miktari not al
                runState.ResetRun(); // her sey sifirlanir

                RefreshViewsForCurrentZone(); // yeni run'un ekranini kur

                rewardPopupView.Show("BOMBA! " + lostAmount + " odulu kaybettin."); // kaybi popup'ta goster
                return; // butonlari burada acmiyoruz, popup kapaninca acilacak
            }

            runState.AddReward(result.Amount); // odulu topluyoruz
            runState.AdvanceZone(); // bir zone ilerliyoruz

            Vector3 fromPosition = wheelView.GetSegmentWorldPosition(pendingWinningIndex); // ucusun baslayacagi yer, kazanan segmentin ikonu
            Vector3 toPosition = hudView.GetTotalWorldPosition(); // ucusun bitecegi yer, TOTAL dairesi
            rewardTravelAnimator.Play(result.Icon, fromPosition, toPosition, HandleRewardTravelComplete); // ikon ucsun, bitince HandleRewardTravelComplete cagrilsin (parametresiz, o yuzden metot adini direkt verebiliyoruz)
        }

        private void HandleRewardTravelComplete() // odul ikonu TOTAL'e ulasinca calisir
        {
            RefreshViewsForCurrentZone(); // yeni zone'un ekranini kur (SetTotal burada da cagrilir, daire pulse atar)
            actionButtonsView.SetSpinInteractable(true); // spin tekrar tiklanabilir olsun
            UpdateLeaveInteractable(); // yeni zone'a gore leave aktif mi kapali mi guncelle
        }

        private void HandleRewardPopupClosed() // popup'taki TAMAM'a basilinca calisir
        {
            rewardPopupView.Hide(); // popup'i gizle
            actionButtonsView.SetSpinInteractable(true); // spin tekrar tiklanabilir olsun
            UpdateLeaveInteractable(); // yeni zone'a gore leave aktif mi kapali mi guncelle
        }

        private void HandleLeaveClicked() // leave'e basilinca calisir (sadece safe/super zone'da tiklanabilir oldugu icin buraya gelmesi zaten guvenli)
        {
            runState.EndRun(); // total'i koruyarak zone'u basa al

            RefreshViewsForCurrentZone(); // yeni run'un ekranini kur
            UpdateLeaveInteractable(); // yeni zone'a gore leave aktif mi kapali mi guncelle
        }
    }
}