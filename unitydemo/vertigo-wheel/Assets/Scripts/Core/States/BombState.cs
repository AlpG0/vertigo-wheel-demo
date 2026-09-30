using VertigoWheel.Rewards;
using VertigoWheel.Utils;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Bombaya carpilinca acilan popup durumu: vazgec (her sey kaybedilir) ya da altin/reklamla devam.
    /// </summary>
    public class BombState : GameState
    {
        public BombState(GameContext context, GameStateMachine machine) : base(context, machine) { }

        public override void Enter()
        {
            string atRisk = NumberFormatter.FormatAmount(context.RunRewards.CollectedRewards.Count);
            string message = string.Format(GameConstants.Texts.BombRiskMessage, atRisk);
            context.Views.RewardPopup.Show(message, CanAffordGoldRevive());
        }

        public override void Exit()
        {
            context.Views.RewardPopup.Hide();
        }

        public override void HandleGiveUpRequested()
        {
            context.RunRewards.Clear();
            context.RunState.ResetZone();
            context.Presenter.Present(true); // cubuk 1. zone'a geri kaysin
            machine.ChangeState<IdleState>();
        }

        public override void HandleGoldReviveRequested()
        {
            if (!context.Wallet.TrySpend(CurrencyType.Gold, context.Settings.ReviveGoldCost)) // yetmiyorsa devam yok
            {
                return;
            }

            machine.ChangeState<IdleState>(); // hicbir sey kaybedilmedi, kalinan yerden devam
        }

        public override void HandleAdReviveRequested()
        {
            machine.ChangeState<IdleState>(); // reklam sistemi yok, izlenmis sayiyoruz
        }

        private bool CanAffordGoldRevive()
        {
            return context.Wallet.GetBalance(CurrencyType.Gold) >= context.Settings.ReviveGoldCost;
        }
    }
}
