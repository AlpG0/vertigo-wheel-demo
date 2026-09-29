using System;

namespace VertigoWheel.Rewards
{
    /// <summary>
    /// Oyuncunun kalici nakit/altin bakiyesi.
    /// </summary>
    public interface IWallet
    {
        event Action<CurrencyType, int> OnBalanceChanged; // (para turu, yeni bakiye)

        int GetBalance(CurrencyType type);
        void Add(CurrencyType type, int amount);
        bool TrySpend(CurrencyType type, int amount); // yetmezse hic harcamaz, false doner
    }
}
