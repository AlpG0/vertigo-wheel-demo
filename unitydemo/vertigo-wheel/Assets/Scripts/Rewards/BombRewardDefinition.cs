using UnityEngine;

namespace VertigoWheel.Rewards
{
    /// <summary>
    /// Bomba dilimi. Toplanmaz, akisa patladigini bildirir.
    /// </summary>
    [CreateAssetMenu(fileName = "Reward_Bomb", menuName = "VertigoWheel/Rewards/Bomb")]
    public class BombRewardDefinition : RewardDefinition
    {
        public override void Apply(IRewardSink sink, int amount) // toplamak yerine patla
        {
            sink.Explode();
        }
    }
}
