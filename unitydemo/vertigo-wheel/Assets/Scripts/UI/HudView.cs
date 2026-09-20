using TMPro; // TMP_Text burda tanımlı
using UnityEngine;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Ekrandaki zone ve toplam odul yazilarini gunceleyen sinif.
    /// </summary>
    public class HudView : MonoBehaviour // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour
    {
        [SerializeField] private TMP_Text zoneValueText; // ZONE yazisinin text component'i, text mesh pro temel text tipi, bu yüzden TMP_Text. Inspector'da görünmesi için SerializeField ile işaretledik.
        [SerializeField] private TMP_Text totalValueText; // TOTAL yazisinin text component'i

        private void OnValidate() // referanslari elle surüklemeyelim diye otomatik bul
        {
            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(); // altindaki tum text component'lerini bul
            zoneValueText = texts[0]; // hierarchy'de once gelen zone_value
            totalValueText = texts[1]; // sonra gelen total_value
        }

        public void SetZone(int zone) // zone numarasini gunceller
        {
            zoneValueText.text = "ZONE " + zone; // metni yeni zone numarasiyla degistiriyoruz
        }

        public void SetTotal(int total) // toplam odulu gunceller
        {
            totalValueText.text = "TOTAL: " + total; // metni yeni toplamla degistiriyoruz
        }
    }
}