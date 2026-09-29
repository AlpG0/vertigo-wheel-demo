using System;
using VertigoWheel.Rewards;
using VertigoWheel.UI;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Cuzdan bakiyesi degistikce sag alttaki gostergeleri gunceller.
    /// </summary>
    public class WalletPresenter : IDisposable
    {
        private readonly IWallet wallet;
        private readonly IWalletView view;

        public WalletPresenter(IWallet wallet, IWalletView view)
        {
            this.wallet = wallet;
            this.view = view;

            foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType))) // baslangic bakiyelerini animasyonsuz goster
            {
                view.SetBalance(type, wallet.GetBalance(type), false);
            }

            wallet.OnBalanceChanged += HandleBalanceChanged;
        }

        private void HandleBalanceChanged(CurrencyType type, int balance)
        {
            view.SetBalance(type, balance, true);
        }

        public void Dispose()
        {
            wallet.OnBalanceChanged -= HandleBalanceChanged;
        }
    }
}
