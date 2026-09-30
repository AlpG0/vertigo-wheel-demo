using System.Collections.Generic;

namespace VertigoWheel.Rewards
{
    /// <summary>
    /// Cikista aktarilan odullerin gittigi yer: para cuzdana, esyalar envantere.
    /// </summary>
    public class PlayerBank : IRewardBank // odullerin gidecegi yer. Cuzdani disaridan aliyor, kendisi olusturmuyor. Envanter yok, sadece cuzdana aktariliyor.
    {
        private readonly IWallet wallet; // cuzdani disaridan aliyoruz, kendisi olusturmuyor
        private readonly Dictionary<RewardDefinition, int> items = new Dictionary<RewardDefinition, int>(); // envanter yok, sadece cuzdana aktariliyor

        public PlayerBank(IWallet wallet) // cuzdani disaridan aliyoruz, kendisi olusturmuyor
        {
            this.wallet = wallet;
        }

        public void AddCurrency(CurrencyType type, int amount) // cuzdana aktariliyor
        {
            wallet.Add(type, amount);
        }

        public void AddItem(RewardDefinition item, int amount) 
        {
            int current;
            items.TryGetValue(item, out current); 
            items[item] = current + amount;
        }

        public int GetItemAmount(RewardDefinition item) 
        {
            int amount;
            items.TryGetValue(item, out amount);
            return amount;
        }
    }
}
