namespace VertigoWheel.Data
{
    /// <summary>
    /// Bir zone'un turunu belirtir (Normal, Safe, Super).
    /// </summary>
    public enum ZoneType // bir zone'un turu, 3 ihtimalden biri cunku enum ile tanimladik. Bu sayede zoneType degiskeni sadece bu 3 degeri alabilir.
    {
        Normal, // bombali, normal spin
        Safe, // her 5. zone, bombasiz, silver spin
        Super // her 30. zone, bombasiz, ozel odulller, golden spin
    }
}