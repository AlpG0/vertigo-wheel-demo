using UnityEngine;
using VertigoWheel.Rewards;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Sol paneldeki toplanan odul listesini gosteren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IRewardListView
    {
        Vector3 PrepareRow(RewardDefinition reward); // odulun satirini hazirla (yoksa olustur), ucan ikonun inecegi konumu dondur
        void SetAmount(RewardDefinition reward, int amount); // satirdaki miktari guncelle (sayarak artar)
        void Clear(); // tum satirlari sil
    }
}