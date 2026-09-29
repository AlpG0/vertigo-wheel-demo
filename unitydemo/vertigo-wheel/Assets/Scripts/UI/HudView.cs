using TMPro; // TMP_Text burda tanımlı
using UnityEngine;
using VertigoWheel.Utils; // HierarchyLookup, GameConstants, NumberFormatter burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Ekrandaki zone ve toplam odul yazilarini gunceleyen sinif.
    /// </summary>
    public class HudView : MonoBehaviour, IHudView // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour; IHudView'i implement ediyor
    {
        [SerializeField] private TMP_Text zoneValueText; // ust baslik yazisi, text mesh pro temel text tipi, bu yüzden TMP_Text
        [SerializeField] private TMP_Text totalValueText; // TOTAL yazisinin text component'i
        [SerializeField] private PunchScaleAnimator totalPunchAnimator; // total daire buyuyup kuculerek "pop" efekti versin diye

        private void OnValidate() // referanslari elle surüklemeyelim diye otomatik bul
        {
            // sahnede birden fazla "Canvas" isimli obje oldugu icin yol ile degil isimle ariyoruz, isimler GameConstants'ta tek yerde
            zoneValueText = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.ZoneTitleText);
            totalValueText = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.TotalValueText);
            totalPunchAnimator = GetComponentInChildren<PunchScaleAnimator>(true); // altimda tek tane var, direkt bulup atiyoruz
        }

        public Vector3 GetTotalWorldPosition() // odul gidis animasyonu icin ucusun bitecegi konum lazim
        {
            return totalPunchAnimator.transform.position;
        }

        public void SetSpinTitle(string title) // ust basligi gunceller ("GÜMÜŞ ÇEVİRME" gibi)
        {
            zoneValueText.text = title;
        }

        public void SetTotal(int total) // toplam odulu gunceller
        {
            totalValueText.text = NumberFormatter.FormatAmount(total); // format kurali tek yerde
            totalPunchAnimator.Play(); // sayi degisince daire kisa bir pulse yapsin, oduldu belli olsun
        }
    }
}
