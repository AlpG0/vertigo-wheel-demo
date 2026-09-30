using UnityEngine;

namespace VertigoWheel.Rewards
{
    /// <summary>
    /// Nakit veya altin odulu. Run icinde diger oduller gibi toplanir, cikista esya olarak degil cuzdana aktarilir.
    /// </summary>
    [CreateAssetMenu(fileName = "Reward_Currency", menuName = "VertigoWheel/Rewards/Currency")]
    public class CurrencyRewardDefinition : RewardDefinition
    {
        [SerializeField] private CurrencyType currencyType; // nakit mi altin mi

        public CurrencyType CurrencyType { get { return currencyType; } }

        public override void Bank(IRewardBank bank, int amount) // esya olarak degil, cuzdana para olarak gecsin
        {
            bank.AddCurrency(currencyType, amount);
        }
    }
}
