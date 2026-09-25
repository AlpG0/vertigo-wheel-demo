using VertigoWheel.Data; // ZoneType burda

namespace VertigoWheel.Zone
{
    /// <summary>
    /// Zone numarasindan zone turunu hesaplayan siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IZoneManager
    {
        ZoneType GetZoneType(int zone); // zone numarasini verince hangi zone turu (Normal/Safe/Super) oldugunu dondurur
    }
}