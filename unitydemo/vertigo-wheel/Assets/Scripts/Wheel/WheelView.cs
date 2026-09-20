using System.Collections.Generic; // List burda tanimli
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
        }

        public void ShowConfig(WheelConfig config) // disaridan hangi config gosterilecekse bunu cagiracagiz
       {
            wheelConfig = config; // gosterilecek config'i guncelle
            DisplaySegments(); // ikonlari bu yeni config'e gore uygula
       }

        public void DisplaySegments() // config'teki ikonları segment görsellerine uygular
        {
            for (int i = 0; i < segmentImages.Length; i++) // her segment için tek tek
            {
                segmentImages[i].sprite = wheelConfig.Segments[i].Icon; // o segmentin ikonunu config'ten alıp uyguluyoruz.
            }
        }
    }
}