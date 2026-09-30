using System.Collections.Generic;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Siradaki ozel zone'lari (altin, gumus...) gosteren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IUpcomingZonesView
    {
        void Show(IReadOnlyList<UpcomingZoneInfo> zones);
    }
}
