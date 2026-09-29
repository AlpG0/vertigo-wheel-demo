using System;
using System.Collections.Generic;

namespace VertigoWheel.Rewards
{
    /// <summary>
    /// IRunRewards'in varsayilan uygulamasi. Ayni odulden tekrar gelirse yeni satir acmaz, miktarini artirir.
    /// </summary>
    public class RunRewards : IRunRewards
    {
        private readonly Dictionary<RewardDefinition, int> amounts = new Dictionary<RewardDefinition, int>(); // odul -> toplam miktar
        private readonly List<RewardDefinition> order = new List<RewardDefinition>(); // listede ilk kazanilan ustte dursun diye sirayi ayrica tutuyoruz

        public event Action<RewardDefinition, int> OnRewardChanged; // odul miktari degistiginde tetiklenir. (odul, yeni miktar)
        public event Action OnCleared; // tum oduller temizlendiginde tetiklenir

        public IReadOnlyList<RewardDefinition> CollectedRewards { get { return order; } } // odul sirasi, ilk kazanilan ustte dursun diye listede tutuyoruz

        public int GetAmount(RewardDefinition reward) // odulun toplam miktarini dondurur
        {
            int amount;
            amounts.TryGetValue(reward, out amount); // yoksa 0 kalir
            return amount;
        }

        public void Add(RewardDefinition reward, int amount) // odul miktarini artirir, yoksa ekler
        {
            if (!amounts.ContainsKey(reward)) // bu odul ilk defa geliyor
            {
                amounts[reward] = 0;
                order.Add(reward);
            }

            amounts[reward] += amount;
            OnRewardChanged?.Invoke(reward, amounts[reward]);
        }

        public void BankInto(IRewardBank bank) // odulleri cuzdana aktarir, envantere aktarmaz
        {
            foreach (RewardDefinition reward in order)
            {
                reward.Bank(bank, amounts[reward]); // nereye aktarilacagina odulun kendisi karar veriyor (para cuzdana, esya envantere)
            }

            Clear(); // aktarim sonrasi odulleri temizliyoruz
        }

        public void Clear() // odulleri temizler
        {
            amounts.Clear();
            order.Clear();
            OnCleared?.Invoke();
        }
    }
}
