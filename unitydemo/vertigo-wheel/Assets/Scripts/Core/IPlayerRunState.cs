namespace VertigoWheel.Core
{
    /// <summary>
    /// Oyuncunun run durumunu okuyup degistiren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IPlayerRunState
    {
        int CurrentZone { get; } // disaridan okunabilsin diye
        int TotalValue { get; } // disaridan okunabilsin diye

        void AddReward(int amount); // odul kazanildiginda cagrilir
        void ResetRun(); // bombaya carpinca her sey sifirlanir
        void AdvanceZone(); // bir zone ilerleriz (spin basarili oldugunda)
        void EndRun(); // leave ile basariyla run'u bitirince cagrilir
    }
}