using UnityEngine;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Kazanilan odulun ikonu sol listedeki satirina ucarken. Ikon varinca odul listeye eklenir ve bir sonraki zone'a gecilir.
    /// </summary>
    public class CollectingState : GameState
    {
        public CollectingState(GameContext context, GameStateMachine machine) : base(context, machine) { }

        public override void Enter()
        {
            context.RunState.AdvanceZone();

            SpinResult spin = context.CurrentSpin;
            Vector3 from = context.Views.Wheel.GetSegmentWorldPosition(spin.SegmentIndex);
            Vector3 to = context.Views.RewardList.PrepareRow(spin.Segment.Reward); // odulun sol listedeki satiri (yoksa olusturulur)
            context.Views.RewardTravel.Play(spin.Segment.Icon, from, to, HandleTravelFinished);
        }

        private void HandleTravelFinished()
        {
            if (!IsActive)
            {
                return;
            }

            SpinResult spin = context.CurrentSpin;
            context.RunRewards.Add(spin.Segment.Reward, spin.Segment.Amount);
            context.Presenter.Present(true); // zone cubugu kayarak bir sonraki zone'a gecsin
            machine.ChangeState<IdleState>();
        }
    }
}
