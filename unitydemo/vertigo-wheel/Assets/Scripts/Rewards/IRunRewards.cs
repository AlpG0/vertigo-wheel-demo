using System;
using System.Collections.Generic;

namespace VertigoWheel.Rewards
{
    /// <summary>
    /// Bu run'da toplanan, henuz guvende olmayan (bombayla kaybedilebilecek) oduller.
    /// </summary>
    public interface IRunRewards
    {
        event Action<RewardDefinition, int> OnRewardChanged; // bir odulun miktari degisince: (odul, yeni toplam)
        event Action OnCleared; // tum liste bosalinca

        IReadOnlyList<RewardDefinition> CollectedRewards { get; } // kazanilma sirasina gore

        int GetAmount(RewardDefinition reward);
        void Add(RewardDefinition reward, int amount);
        void BankInto(IRewardBank bank); // hepsini kalici hesaba aktar ve listeyi bosalt
        void Clear(); // bombayla her sey kaybedildi
    }
}
