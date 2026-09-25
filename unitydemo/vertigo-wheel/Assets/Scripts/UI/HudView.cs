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
        [SerializeField] private PunchScaleAnimator totalPunchAnimator; // total daire buyuyup kuculerek "pop" efekti versin diye

        private void OnValidate() // referanslari elle surüklemeyelim diye otomatik bul
        {
            // sahnede birden fazla "Canvas" isimli obje oldugu icin transform.Find("Canvas/...") yanlis objeyi buluyordu (hata veriyordu).
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

            totalPunchAnimator = GetComponentInChildren<PunchScaleAnimator>(true); // altimda tek tane var, direkt bulup atiyoruz
        }

        public Vector3 GetTotalWorldPosition() // odul gidis animasyonu icin ucusun bitecegi konum lazim
        {
            return totalPunchAnimator.transform.position;
        }

        public void SetSpinTitle(string title) // ust basligi gunceller, artik zone numarasi degil "BRONZE SPIN" gibi bir baslik gosteriyoruz
        {
            zoneValueText.text = title; // metni disaridan gelen baslikla degistiriyoruz, burada "ZONE " gibi bir on ek eklemiyoruz artik
        }

        public void SetTotal(int total) // toplam odulu gunceller
        {
            totalValueText.text = total.ToString(); // artik "TOTAL: " on eki yok, referans gorseldeki gibi sade sayi (wheel'in ortasinda gosterilecek)
            totalPunchAnimator.Play(); // sayi degisince daire kisa bir pulse yapsin, oduldu belli olsun
        }
    }
}