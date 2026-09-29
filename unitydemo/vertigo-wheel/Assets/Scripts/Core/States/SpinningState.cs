using VertigoWheel.Rewards;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Wheel donerken. Donus bitince sonucu odulun kendisine sorar: toplanacak mi, patlayacak mi.
    /// </summary>
    public class SpinningState : GameState, IRewardSink
    {
        public SpinningState(GameContext context, GameStateMachine machine) : base(context, machine) { }

        public override void Enter()
        {
            SpinResult spin = context.CurrentSpin;
            int segmentCount = context.CurrentZone.WheelConfig.Segments.Count;
            context.Views.WheelSpinAnimator.SpinTo(spin.SegmentIndex, segmentCount, HandleSpinFinished);
        }

        private void HandleSpinFinished()
        {
            if (!IsActive) // animasyon gecikmeli gelebilir, artik bu state'te degilsek yok say
            {
                return;
            }

            SpinResult spin = context.CurrentSpin;
            spin.Segment.Reward.Apply(this, spin.Segment.Amount); // tur kontrolu yok: esya/para Collect'i, bomba Explode'u cagirir
        }

        public void Collect(RewardDefinition reward, int amount) // IRewardSink
        {
            machine.ChangeState<CollectingState>();
        }

        public void Explode() // IRewardSink
        {
            machine.ChangeState<BombState>();
        }
    }
}
