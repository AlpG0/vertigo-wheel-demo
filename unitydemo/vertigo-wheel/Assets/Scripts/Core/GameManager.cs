using UnityEngine; // MonoBehaviour, SerializeField burda
using VertigoWheel.Data; // WheelConfig, WheelSegmentData, RewardType burda
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
        [SerializeField] private WheelConfig wheelConfigNormal; // normal zone icin kullanacagimiz config, bunu elle surukleyecegiz
        [SerializeField] private WheelConfig wheelConfigSafe; // safe zone icin config, bombasiz
        [SerializeField] private WheelConfig wheelConfigSuper; // super zone icin config, bombasiz

        private IWheelResultPicker resultPicker; // sonucu secen mantik, arayuz tipinde tutuyoruz
        private PlayerRunState runState; // oyuncunun anlik durumu (zone, toplam odul)
        private ZoneManager zoneManager; // su anki zone'un turunu hesaplayan sinif
        private WheelSegmentData pendingResult; // spin animasyonu bitince uygulanacak sonucu gecici olarak burada tutuyoruz

        private void OnValidate() // sahnedeki diger 3 script'i otomatik bul
        {
            wheelView = GetComponentInChildren<WheelView>(); // altimdaki WheelView'i bul
            wheelSpinAnimator = GetComponentInChildren<WheelSpinAnimator>(); // altimdaki WheelSpinAnimator'i bul
            wheelThemeView = GetComponentInChildren<WheelThemeView>(); // altimdaki WheelThemeView'i bul
            actionButtonsView = GetComponentInChildren<ActionButtonsView>(); // altimdaki ActionButtonsView'i bul
            hudView = GetComponentInChildren<HudView>(); // altimdaki HudView'i bul
            rewardPopupView = GetComponentInChildren<RewardPopupView>(true); // true: popup pasif basladigi icin inactive de dahil ara
        }

        private void Awake() // sahne yuklenince ilk calisan metot
        {
            resultPicker = new RandomWheelResultPicker(); // gercek sinifi olusturup arayuz degiskenine atiyoruz
            runState = new PlayerRunState(); // oyuncunun durumunu olusturuyoruz
            zoneManager = new ZoneManager(); // zone turunu hesaplayacak sinifi olusturuyoruz

            actionButtonsView.OnSpinClicked += HandleSpinClicked; // spin event'ine kendi metodumuzu bagliyoruz
            actionButtonsView.OnLeaveClicked += HandleLeaveClicked; // leave event'ine kendi metodumuzu bagliyoruz
            rewardPopupView.OnCloseClicked += HandleRewardPopupClosed; // popup kapatilinca kendi metodumuzu bagliyoruz
        }

        private void Start() // Awake'ten sonra calisir
        {
            hudView.SetZone(runState.CurrentZone); // baslangic zone'unu ekrana yaz
            hudView.SetTotal(runState.TotalValue); // baslangic toplamini ekrana yaz
            wheelView.ShowConfig(GetCurrentWheelConfig()); // basta hangi wheel gosterilecek onu belirle
            wheelThemeView.ApplyZoneType(GetCurrentZoneType()); // basta hangi tema (bronze/silver/golden) gosterilecek onu belirle
            UpdateLeaveInteractable(); // leave butonu basta aktif mi kapali mi ayarla
        }

        private ZoneType GetCurrentZoneType() // su anki zone'un turunu dondurur, tek yerden hesapliyoruz
        {
            return zoneManager.GetZoneType(runState.CurrentZone); // zoneManager'a soruyoruz, zoneManager zone numarasina gore safe/super/normal donduruyor
        }

        private WheelConfig GetCurrentWheelConfig() // su anki zone'a gore dogru config'i secer
        {
            ZoneType zoneType = GetCurrentZoneType(); // once zone turunu ogreniyoruz

            if (zoneType == ZoneType.Super) // super zone ise
            {
                return wheelConfigSuper; // super zone config'ini dondur
            }

            if (zoneType == ZoneType.Safe) // safe zone ise
            {
                return wheelConfigSafe; // safe zone config'ini dondur
            }

            return wheelConfigNormal; // ikisi de degilse normal
        }

        private void UpdateLeaveInteractable() // leave butonunun tiklanabilir olup olmadigini gunceller
        {
            ZoneType zoneType = GetCurrentZoneType(); // su anki zone turu ne
            bool canLeave = zoneType == ZoneType.Safe || zoneType == ZoneType.Super; // pdf kurali: sadece safe/super zone'da leave edilebilir
            actionButtonsView.SetLeaveInteractable(canLeave); // butonu ac/kapat
        }

        private void HandleSpinClicked() // spin'e basilinca calisir
        {
            WheelConfig currentConfig = GetCurrentWheelConfig(); // bu spin icin dogru config hangisi
            WheelSegmentData result = resultPicker.PickSegment(currentConfig); // o config'ten sonuc sec
            int winningIndex = currentConfig.Segments.IndexOf(result); // sonucun listedeki index'i, animasyon icin lazim

            pendingResult = result; // animasyon bitince kullanmak icin sonucu sakla

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

                hudView.SetZone(runState.CurrentZone); // ekrani guncelle
                hudView.SetTotal(runState.TotalValue); // ekrani guncelle
                wheelView.ShowConfig(GetCurrentWheelConfig()); // yeni run'un wheel'ini goster
                wheelThemeView.ApplyZoneType(GetCurrentZoneType()); // yeni zone'a gore temayi guncelle

                rewardPopupView.Show("BOMBA! " + lostAmount + " odulu kaybettin."); // kaybi popup'ta goster
                return; // butonlari burada acmiyoruz, popup kapaninca acilacak
            }

            runState.AddReward(result.Amount); // odulu topluyoruz
            runState.AdvanceZone(); // bir zone ilerliyoruz

            hudView.SetZone(runState.CurrentZone); // ekrani guncelle
            hudView.SetTotal(runState.TotalValue); // ekrani guncelle
            wheelView.ShowConfig(GetCurrentWheelConfig()); // zone degismis olabilir, yeni zone'un wheel'ini goster
            wheelThemeView.ApplyZoneType(GetCurrentZoneType()); // yeni zone'a gore temayi guncelle
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

            hudView.SetZone(runState.CurrentZone); // ekrani guncelle
            hudView.SetTotal(runState.TotalValue); // ekrani guncelle
            wheelView.ShowConfig(GetCurrentWheelConfig()); // yeni run'un wheel'ini goster
            wheelThemeView.ApplyZoneType(GetCurrentZoneType()); // yeni zone'a gore temayi guncelle
            UpdateLeaveInteractable(); // yeni zone'a gore leave aktif mi kapali mi guncelle
        }
    }
}