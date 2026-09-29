using System.Collections.Generic;

namespace VertigoWheel.Zone
{
    /// <summary>
    /// Zone numarasindan hangi ZoneDefinition'in gecerli oldugunu bulur. Kurallar koddan degil, verilen tanimlardan gelir.
    /// </summary>
    public class ZoneService : IZoneService
    {
        private readonly List<ZoneDefinition> zonesByPriority; // buyuk aralik once: 30, 5, 1. Hem 30'un hem 5'in kati olan zone 30'a dussun diye
        private readonly Dictionary<int, ZoneDefinition> cache = new Dictionary<int, ZoneDefinition>(); // ayni zone icin her ekran yenilemesinde tekrar aramayalim

        public ZoneService(IEnumerable<ZoneDefinition> zones)
        {
            zonesByPriority = new List<ZoneDefinition>(zones);
            zonesByPriority.Sort(CompareByIntervalDescending);
        }

        private static int CompareByIntervalDescending(ZoneDefinition a, ZoneDefinition b)
        {
            return b.Interval.CompareTo(a.Interval);
        }

        public ZoneDefinition GetZone(int zoneNumber)
        {
            ZoneDefinition cached;
            if (cache.TryGetValue(zoneNumber, out cached))
            {
                return cached;
            }

            ZoneDefinition result = zonesByPriority[zonesByPriority.Count - 1]; // hic eslesme olmazsa en kucuk aralikli (varsayilan) zone
            foreach (ZoneDefinition zone in zonesByPriority)
            {
                if (zoneNumber % zone.Interval == 0)
                {
                    result = zone;
                    break;
                }
            }

            cache[zoneNumber] = result;
            return result;
        }
    }
}
