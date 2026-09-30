namespace VertigoWheel.Rewards
{
    /// <summary>
    /// Oyuncu cikis yapinca run'da toplanan odullerin aktarildigi kalici hesap.
    /// </summary>
    public interface IRewardBank
    {
        void AddCurrency(CurrencyType type, int amount); // para cuzdana
        void AddItem(RewardDefinition item, int amount); // esya envantere
    }
}
