using System;
using VertigoWheel.Rewards;
using VertigoWheel.UI;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Run'da toplanan oduller degistikce sol listeyi gunceller. Model view'i bilmez, view modeli bilmez; ikisini bu sinif baglar.
    /// </summary>
    public class RunRewardsPresenter : IDisposable
    {
        private readonly IRunRewards runRewards;
        private readonly IRewardListView view;

        public RunRewardsPresenter(IRunRewards runRewards, IRewardListView view)
        {
            this.runRewards = runRewards;
            this.view = view;

            runRewards.OnRewardChanged += HandleRewardChanged;
            runRewards.OnCleared += HandleCleared;
        }

        private void HandleRewardChanged(RewardDefinition reward, int amount)
        {
            view.SetAmount(reward, amount);
        }

        private void HandleCleared()
        {
            view.Clear();
        }

        public void Dispose()
        {
            runRewards.OnRewardChanged -= HandleRewardChanged;
            runRewards.OnCleared -= HandleCleared;
        }
    }
}
