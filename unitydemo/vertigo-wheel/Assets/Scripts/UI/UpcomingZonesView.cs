using System.Collections.Generic;
using UnityEngine;
using VertigoWheel.Utils;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Sagdaki rozet listesi. Kac ozel zone turu varsa o kadar rozet olusturur (yeni bir zone turu eklenirse kod degismez).
    /// </summary>
    public class UpcomingZonesView : MonoBehaviour, IUpcomingZonesView
    {
        [SerializeField] private UpcomingZoneBadgeView badgeTemplate; // her rozet bundan kopyalanir, kendisi pasif durur

        private readonly List<UpcomingZoneBadgeView> badges = new List<UpcomingZoneBadgeView>();

        private void OnValidate() // inspector'da degisiklik yapildiginda, sahnede gorunmesi icin
        {
            badgeTemplate = GetComponentInChildren<UpcomingZoneBadgeView>(true);
        }

        public void Show(IReadOnlyList<UpcomingZoneInfo> zones) //kac ozel zone varsa o kadar rozet olustur, her rozet zone'un rengini ve sira numarasini gostersin
        {
            while (badges.Count < zones.Count) // yeterli sayida rozet olustur
            {
                UpcomingZoneBadgeView badge = Instantiate(badgeTemplate, badgeTemplate.transform.parent);
                badge.name = GameConstants.UINames.UpcomingZoneBadgePrefix + badges.Count;
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
