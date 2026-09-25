using VertigoWheel.Data; // ZoneType burda tanimli

namespace VertigoWheel.Zone
{
    /// <summary>
    /// Verilen zone numarasinin turunu (Normal/Safe/Super) hesaplayan sinif.
    /// </summary>
    public class ZoneManager : IZoneManager // duz sinif, MonoBehaviour degil, sahneyle ilgisi yok; IZoneManager'i implement ediyor
    {
        private const int SafeZoneInterval = 5; // her kacinci zone'da bir safe zone gelecek
        private const int SuperZoneInterval = 30; // her kacinci zone'da bir super zone gelecek

        public ZoneType GetZoneType(int zone) // verilen zone numarasinin turunu dondurur
        {
            if (zone % SuperZoneInterval == 0) // once super araligina bakiyoruz cunku 30 hem 5'in hem de 30'un kati oldugu icin once onu kontrol etmeliyiz
            {
                return ZoneType.Super; // 30, 60, 90... super zone
            }

            if (zone % SafeZoneInterval == 0) // super degilse, safe araligina bakiyoruz
            {
                return ZoneType.Safe; // 5, 10, 15, 20, 25... safe zone
            }

            return ZoneType.Normal; // ikisi de degilse normal zone
        }
    }
}