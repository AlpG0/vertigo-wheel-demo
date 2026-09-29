using System;
using System.Collections.Generic;

namespace VertigoWheel.Rewards
{
    /// <summary>
    /// IWallet'in varsayilan uygulamasi, bakiyeleri bellekte tutar.
    /// </summary>
    public class Wallet : IWallet
    {
        private readonly Dictionary<CurrencyType, int> balances = new Dictionary<CurrencyType, int>(); // para turu -> bakiye

        public event Action<CurrencyType, int> OnBalanceChanged; // bakiye degistiginde tetiklenir. (para turu, yeni bakiye)

        public Wallet(int startingCash, int startingGold) // baslangic bakiyesini disaridan aliyoruz, sabit degerler burada gomulu kalmasin
        {
            balances[CurrencyType.Cash] = startingCash; // baslangic bakiyesini disaridan aliyoruz, sabit degerler burada gomulu kalmasin
            balances[CurrencyType.Gold] = startingGold; // baslangic bakiyesini disaridan aliyoruz, sabit degerler burada gomulu kalmasin
        }

        public int GetBalance(CurrencyType type) // bakiyeyi dondurur
        {
            int balance;
            balances.TryGetValue(type, out balance);
            return balance;
        }

        public void Add(CurrencyType type, int amount) // bakiyeyi artirir
        {
            balances[type] = GetBalance(type) + amount;
            OnBalanceChanged?.Invoke(type, balances[type]);
        }

        public bool TrySpend(CurrencyType type, int amount) // bakiyeyi azaltir, yeterli bakiye yoksa hic dokunmaz
        {
            if (GetBalance(type) < amount) // yetmiyor, hic dokunma
            {
                return false;
            }

            balances[type] = GetBalance(type) - amount;
            OnBalanceChanged?.Invoke(type, balances[type]);
            return true;
        }
    }
}
