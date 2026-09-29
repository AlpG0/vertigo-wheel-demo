using System;

namespace VertigoWheel.Core
{
    /// <summary>
    /// State machine'i kurar, gecerli gecisleri tanimlar ve view'lardan gelen input'u o anki state'e iletir.
    /// Hicbir somut sinifi bilmez, her sey GameContext uzerinden arayuz olarak gelir.
    /// </summary>
    public class GameController : IDisposable
    {
        private readonly GameContext context;
        private readonly GameStateMachine machine = new GameStateMachine();

        public GameController(GameContext context)
        {
            this.context = context;

            machine.AddState(new IdleState(context, machine));
            machine.AddState(new SpinningState(context, machine));
            machine.AddState(new CollectingState(context, machine));
            machine.AddState(new BombState(context, machine));

            machine.AllowTransition<IdleState, SpinningState>(); // oyunun izin verilen butun akisi burada, tek bakista
            machine.AllowTransition<SpinningState, CollectingState>();
            machine.AllowTransition<SpinningState, BombState>();
            machine.AllowTransition<CollectingState, IdleState>();
            machine.AllowTransition<BombState, IdleState>();

            context.Views.ActionButtons.OnSpinClicked += HandleSpinClicked;
            context.Views.ActionButtons.OnLeaveClicked += HandleLeaveClicked;
            context.Views.RewardPopup.OnGiveUpClicked += HandleGiveUpClicked;
            context.Views.RewardPopup.OnGoldReviveClicked += HandleGoldReviveClicked;
            context.Views.RewardPopup.OnAdReviveClicked += HandleAdReviveClicked;
        }

        public void Start()
        {
            context.Presenter.Present();
            machine.Start<IdleState>();
        }

        private void HandleSpinClicked() { machine.Current.HandleSpinRequested(); } // karari o anki state verir, yanlis state'te yok sayilir
        private void HandleLeaveClicked() { machine.Current.HandleLeaveRequested(); }
        private void HandleGiveUpClicked() { machine.Current.HandleGiveUpRequested(); }
        private void HandleGoldReviveClicked() { machine.Current.HandleGoldReviveRequested(); }
        private void HandleAdReviveClicked() { machine.Current.HandleAdReviveRequested(); }

        public void Dispose() // sahne kapanirken event aboneliklerini birak
        {
            context.Views.ActionButtons.OnSpinClicked -= HandleSpinClicked;
            context.Views.ActionButtons.OnLeaveClicked -= HandleLeaveClicked;
            context.Views.RewardPopup.OnGiveUpClicked -= HandleGiveUpClicked;
            context.Views.RewardPopup.OnGoldReviveClicked -= HandleGoldReviveClicked;
            context.Views.RewardPopup.OnAdReviveClicked -= HandleAdReviveClicked;
        }
    }
}
