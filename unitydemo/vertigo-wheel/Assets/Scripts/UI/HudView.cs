using TMPro; // TMP_Text burda tanımlı
using UnityEngine;
using VertigoWheel.Utils; // HierarchyLookup, GameConstants burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Wheel'in altindaki zone basligini ve alt yazisini gunceleyen sinif.
    /// </summary>
    public class HudView : MonoBehaviour, IHudView // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour; IHudView'i implement ediyor
    {
        [SerializeField] private TMP_Text titleText; // "GÜMÜŞ ÇEVİRME" gibi buyuk baslik
        [SerializeField] private TMP_Text subtitleText; // altindaki kucuk aciklama

        private void OnValidate() // referanslari elle surüklemeyelim diye otomatik bul, isimler GameConstants'ta tek yerde
        {
            titleText = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.ZoneTitleText);
            subtitleText = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.ZoneSubtitleText);
        }

        public void SetZoneInfo(string title, string subtitle)
        {
            titleText.text = title;
            subtitleText.text = subtitle;
        }
    }
}
