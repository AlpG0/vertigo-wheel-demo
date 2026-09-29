namespace VertigoWheel.Zone
{
    /// <summary>
    /// Zone numarasindan o zone'un tanimini (kural + gorunum) veren servis.
    /// </summary>
    public interface IZoneService // zone numarasindan o zone'un tanimini (kural + gorunum) veren servis
    {
        ZoneDefinition GetZone(int zoneNumber); 
    }
}