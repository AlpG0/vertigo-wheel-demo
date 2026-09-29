using VertigoWheel.Data;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Oyuncunun spin ya da cikis secebildigi bekleme durumu.
    /// </summary>
    public class IdleState : GameState
    {
        public IdleState(GameContext context, GameStateMachine machine) : base(context, machine) { }

        public override void Enter()
        {
            RefreshButtons();
        }

        public override void Exit()
        {
            context.Views.ActionButtons.SetSpinInteractable(false); // bu state disinda hicbir butona basilamaz
            context.Views.ActionButtons.SetLeaveInteractable(false);
        }

        public override void HandleSpinRequested()
        {
            WheelConfig config = context.CurrentZone.WheelConfig;
            WheelSegmentData segment = context.ResultPicker.PickSegment(config);

            context.CurrentSpin = new SpinResult(segment, config.Segments.IndexOf(segment));
            machine.ChangeState<SpinningState>();
        }

        public override void HandleLeaveRequested()
        {
            if (!context.CurrentZone.CanLeave) // buton zaten pasif olmali, ama kural buton durumuna guvenmesin
            {
                return;
            }

            context.RunRewards.BankInto(context.RewardBank); // toplananlar kalici hesaba
            context.RunState.ResetZone(); // yeni run
            context.Presenter.Present();
            RefreshButtons(); // Idle'dan cikmiyoruz ama yeni zone'a gore cikis butonu degisti
        }

        private void RefreshButtons()
        {
            context.Views.ActionButtons.SetSpinInteractable(true);
            context.Views.ActionButtons.SetLeaveInteractable(context.CurrentZone.CanLeave);
        }
    }
}
