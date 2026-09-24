using System.Collections.Generic; // List burda tanimli
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
            // sahnede birden fazla "Canvas" isimli obje oldugu icin transform.Find("Canvas/...") yanlis objeyi buluyordu (hata veriyordu).
            // bunun yerine altimdaki TUM TMP_Text'leri tarayip isme gore ESLESTIRIYORUZ, boylece hangi Canvas'in altinda olduklari onemli degil.
            TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>(true); // true: pasif objeleri de ara (reward popup gibi)

            foreach (TMP_Text text in allTexts) // her text icin tek tek bak
            {
                if (text.gameObject.name == "ui_text_zone_value") // ismi zone_value ise
                {
                    zoneValueText = text; // bu bizim baslik text'imiz
                }
                else if (text.gameObject.name == "ui_text_total_value") // ismi total_value ise
                {
                    totalValueText = text; // bu bizim total text'imiz
                }
            }
        }

        public void SetSpinTitle(string title) // ust basligi gunceller, artik zone numarasi degil "BRONZE SPIN" gibi bir baslik gosteriyoruz
        {
            zoneValueText.text = title; // metni disaridan gelen baslikla degistiriyoruz, burada "ZONE " gibi bir on ek eklemiyoruz artik
        }

        public void SetTotal(int total) // toplam odulu gunceller
        {
            totalValueText.text = total.ToString(); // artik "TOTAL: " on eki yok, referans gorseldeki gibi sade sayi (wheel'in ortasinda gosterilecek)
        }
    }
}