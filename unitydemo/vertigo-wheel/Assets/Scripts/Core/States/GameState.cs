namespace VertigoWheel.Core
{
    /// <summary>
    /// Oyun akisindaki bir durumun temeli. Her input icin varsayilan davranis "yok say":
    /// bir state sadece kendisinde anlamli olan input'u override eder, yanlis anda gelen tiklama hicbir sey yapmaz.
    /// </summary>
    public abstract class GameState
    {
        protected readonly GameContext context;
        protected readonly GameStateMachine machine;

        protected GameState(GameContext context, GameStateMachine machine)
        {
            this.context = context;
            this.machine = machine;
        }

        public virtual void Enter() { } // bu state'e girilince
        public virtual void Exit() { } // bu state'ten cikilinca

        public virtual void HandleSpinRequested() { }
        public virtual void HandleLeaveRequested() { }
        public virtual void HandleGiveUpRequested() { }
        public virtual void HandleGoldReviveRequested() { }
        public virtual void HandleAdReviveRequested() { }

        protected bool IsActive // animasyon callback'leri geldiginde hala bu state'te miyiz
        {
            get { return machine.Current == this; }
        }
    }
}
