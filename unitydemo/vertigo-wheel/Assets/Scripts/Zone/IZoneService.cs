using System.Collections.Generic;

namespace VertigoWheel.Zone
{
    /// <summary>
    /// Zone numarasindan o zone'un tanimini (kural + gorunum) veren servis.
    /// </summary>
    public interface IZoneService // zone numarasindan o zone'un tanimini (kural + gorunum) veren servis
    {
        IReadOnlyList<ZoneDefinition> Zones { get; } // tum zone turleri, buyuk araliktan kucuge
        ZoneDefinition GetZone(int zoneNumber);
        int GetNextZoneNumber(ZoneDefinition zone, int afterZone); // bu zone turu bir sonraki kacinci zone'da gelecek
    }
}
