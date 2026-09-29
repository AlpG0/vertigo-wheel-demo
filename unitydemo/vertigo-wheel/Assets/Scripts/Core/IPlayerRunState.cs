namespace VertigoWheel.Core
{
    /// <summary>
    /// Oyuncunun run durumunu okuyup degistiren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IPlayerRunState
    {
        int CurrentZone { get; } // disaridan okunabilsin diye

        void AdvanceZone(); // bir zone ilerleriz (spin basarili oldugunda)
        void ResetZone(); // yeni run icin zone 1'e don (bomba ya da cikis sonrasi), toplanan oduller artik RunRewards'ta
    }
}
