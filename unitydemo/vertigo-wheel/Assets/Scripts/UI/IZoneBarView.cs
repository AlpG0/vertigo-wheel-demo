using System.Collections.Generic;
using UnityEngine;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Ustteki zone numaralari cubugunu gosteren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IZoneBarView
    {
        void ShowZone(int currentZone, IReadOnlyList<Color> zoneColors, bool animate); // zoneColors[0] = 1. zone'un rengi, animate: kayarak mi gecilsin
    }
}
