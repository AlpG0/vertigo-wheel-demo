using VertigoWheel.Data; // ZoneType burda tanimli

namespace VertigoWheel.Zone
{
    /// <summary>
    /// Verilen zone numarasinin turunu (Normal/Safe/Super) hesaplayan sinif.
    /// </summary>
    public class ZoneManager // duz sinif, MonoBehaviour degil, sahneyle ilgisi yok
    {
        public ZoneType GetZoneType(int zone) // verilen zone numarasinin turunu dondurur
        {
            if (zone % 30 == 0) // once 30'un kati mi diye bakiyoruz cunku 30 hem 5'in hem de 30'un kati oldugu icin once onu kontrol etmeliyiz
            {
                return ZoneType.Super; // 30, 60, 90... super zone
            }

            if (zone % 5 == 0) // 30 degilse, 5'in kati mi diye bakiyoruz
            {
                return ZoneType.Safe; // 5, 10, 15, 20, 25... safe zone
            }

            return ZoneType.Normal; // ikisi de degilse normal zone
        }
    }
}