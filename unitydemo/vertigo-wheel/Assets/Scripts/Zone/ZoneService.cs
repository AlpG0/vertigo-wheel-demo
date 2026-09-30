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

        public IReadOnlyList<ZoneDefinition> Zones { get { return zonesByPriority; } } // buyuk araliktan kucuge sirali

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

        public int GetNextZoneNumber(ZoneDefinition zone, int afterZone) // bu zone turu bir sonraki kacinci zone'da gelecek
        {
            int limit = afterZone + zone.Interval * 100; // ayni araliga sahip iki tanim olursa sonsuz donmesin diye ust sinir
            for (int zoneNumber = afterZone + 1; zoneNumber <= limit; zoneNumber++)
            {
                if (GetZone(zoneNumber) == zone) // daha buyuk aralikli bir zone ustune binmis olabilir (30 hem 5'in katidir), o yuzden kontrol ediyoruz
                {
                    return zoneNumber;
                }
            }

            return -1;
        }
    }
}
