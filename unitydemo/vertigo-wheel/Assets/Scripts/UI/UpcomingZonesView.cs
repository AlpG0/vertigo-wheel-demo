using System.Collections.Generic;
using UnityEngine;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Sagdaki rozet listesi. Kac ozel zone turu varsa o kadar rozet olusturur (yeni bir zone turu eklenirse kod degismez).
    /// </summary>
    public class UpcomingZonesView : MonoBehaviour, IUpcomingZonesView
    {
        [SerializeField] private UpcomingZoneBadgeView badgeTemplate; // her rozet bundan kopyalanir, kendisi pasif durur

        private readonly List<UpcomingZoneBadgeView> badges = new List<UpcomingZoneBadgeView>();

        private void OnValidate()
        {
            badgeTemplate = GetComponentInChildren<UpcomingZoneBadgeView>(true);
        }

        public void Show(IReadOnlyList<UpcomingZoneInfo> zones)
        {
            while (badges.Count < zones.Count)
            {
                UpcomingZoneBadgeView badge = Instantiate(badgeTemplate, badgeTemplate.transform.parent);
                badge.name = "ui_upcoming_zone_" + badges.Count;
                badge.gameObject.SetActive(true);
                badges.Add(badge);
            }

            for (int i = 0; i < badges.Count; i++)
            {
                bool used = i < zones.Count;
                badges[i].gameObject.SetActive(used);
                if (used)
                {
                    badges[i].Show(zones[i]);
                }
            }
        }
    }
}
