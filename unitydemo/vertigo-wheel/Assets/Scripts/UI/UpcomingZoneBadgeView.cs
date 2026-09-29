using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoWheel.Utils;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Sagdaki tek bir rozet: zone adi, kacinci zone'da gelecegi ve kucuk wheel ikonu. Numara degisince "pop" yapar.
    /// </summary>
    public class UpcomingZoneBadgeView : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image frame;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private Image icon;
        [SerializeField] private PunchScaleAnimator punch;

        private int shownZoneNumber = -1;

        private void OnValidate()
        {
            background = HierarchyLookup.FindByName<Image>(this, GameConstants.UINames.UpcomingZoneBackground);
            frame = HierarchyLookup.FindByName<Image>(this, GameConstants.UINames.UpcomingZoneFrame);
            titleText = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.UpcomingZoneTitle);
            numberText = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.UpcomingZoneNumber);
            icon = HierarchyLookup.FindByName<Image>(this, GameConstants.UINames.UpcomingZoneIcon);
            punch = GetComponent<PunchScaleAnimator>();
        }

        public void Show(UpcomingZoneInfo info)
        {
            titleText.text = info.Title;
            numberText.text = NumberFormatter.FormatAmount(info.ZoneNumber);
            icon.sprite = info.Icon;
            frame.color = info.Color;
            background.color = new Color(info.Color.r * 0.55f, info.Color.g * 0.55f, info.Color.b * 0.55f, 1f); // rengin koyu tonu, yazi okunur kalsin

            if (shownZoneNumber >= 0 && shownZoneNumber != info.ZoneNumber) // ilk gosterimde degil, sayi degistiginde
            {
                punch.Play();
            }

            shownZoneNumber = info.ZoneNumber;
        }
    }
}
