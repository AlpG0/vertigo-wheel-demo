using System.Collections.Generic; // List burda tanimli
using TMPro; // TMP_Text burda tanimli
using UnityEngine; // MonoBehaviour, SerializeField burda
using UnityEngine.UI; // Image tipi burda
using VertigoWheel.Data; // WheelConfig burda

namespace VertigoWheel.Wheel
{
    /// <summary>
    /// Wheel'deki segment ikonlarini, verilen config'e gore ekranda gosteren sinif.
    /// </summary>
    public class WheelView : MonoBehaviour // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour
    {
        [SerializeField] private Image[] segmentImages; // 8 segmentin Image component'leri, array olarak tanımladık. Inspector'da görünmesi için SerializeField ile işaretledik.
        [SerializeField] private TMP_Text[] segmentValueTexts; // her segmentin altindaki "xN" yazilari
        [SerializeField] private TMP_Text maxRewardText; // wheel'in altindaki "Up To xN Rewards" yazisi
        [SerializeField] private WheelConfig wheelConfig; // hangi wheel config'i göstereceğiz ve bu config'teki ikonları segment görsellerine uygulayacağız. Inspector'da görünmesi için SerializeField ile işaretledik.

        private void OnValidate() // Editor'de bu obje seçilip bir değer değiştiğinde Unity otomatik çağırır
        {
            Image[] allImages = GetComponentsInChildren<Image>(); // altimdaki TUM Image'lari bul, artik wheel_base de dahil buna
            List<Image> filtered = new List<Image>(); // sadece gercek segment ikonlarini buraya toplayacagiz

            foreach (Image image in allImages) // her bulunan image icin tek tek bak, foreach asagidaki gibi calisir: once image = allImages[0], sonra image = allImages[1] ... sonuncuya kadar
            { // index ile say yerine listedeki her elemani tek tek aliyor, allImages icindeki her image'i tek tek aliyor ve asagidaki kodu calistiriyor
                if (image.gameObject.name != "ui_image_wheel_base") // wheel_base'in kendisi degilse
                {
                    filtered.Add(image); // listeye ekle
                }
            }

            segmentImages = filtered.ToArray(); // List'i tekrar array'e cevirip alana ata
            segmentValueTexts = GetComponentsInChildren<TMP_Text>(true); // altimda baska text olmadigi icin direkt 8'ini de buluyor, sirasi ikonlarla ayni

            // maxRewardText benim (ui_panel_segments'in) altimda degil, bir ust seviyede (ui_panel_wheel_container) duruyor,
            // o yuzden once parent'a cikip oradan ismiyle arıyoruz.
            Transform maxRewardTransform = transform.parent.Find("ui_text_max_reward_value");
            if (maxRewardTransform != null) // henuz sahnede yoksa null gelebilir, hata vermesin diye kontrol ediyoruz
            {
                maxRewardText = maxRewardTransform.GetComponent<TMP_Text>();
            }
        }

        public void ShowConfig(WheelConfig config) // disaridan hangi config gosterilecekse bunu cagiracagiz
       {
            wheelConfig = config; // gosterilecek config'i guncelle
            DisplaySegments(); // ikonlari bu yeni config'e gore uygula
       }

        public void DisplaySegments() // config'teki ikonları segment görsellerine uygular
        {
            int maxAmount = 0; // su ana kadarki en yuksek miktar, "Up To xN Rewards" icin lazim

            for (int i = 0; i < segmentImages.Length; i++) // her segment için tek tek
            {
                segmentImages[i].sprite = wheelConfig.Segments[i].Icon; // o segmentin ikonunu config'ten alıp uyguluyoruz.
                segmentValueTexts[i].text = "x" + wheelConfig.Segments[i].Amount; // o segmentin miktarini "xN" seklinde yaziyoruz

                if (wheelConfig.Segments[i].Amount > maxAmount) // bu segment simdiye kadarkilerden buyukse
                {
                    maxAmount = wheelConfig.Segments[i].Amount; // en buyuk degeri guncelle
                }
            }

            maxRewardText.text = "Up To x" + maxAmount + " Rewards"; // en yuksek miktari alt yaziya yaziyoruz
        }
    }
}