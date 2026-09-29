using NUnit.Framework;
using VertigoWheel.Rewards;

namespace VertigoWheel.Tests
{
    public class RewardEconomyTests
    {
        private class RecordingSink : IRewardSink
        {
            public RewardDefinition CollectedReward;
            public int CollectedAmount;
            public bool Exploded;

            public void Collect(RewardDefinition reward, int amount)
            {
                CollectedReward = reward;
                CollectedAmount = amount;
            }

            public void Explode()
            {
                Exploded = true;
            }
        }

        private int balanceChangedCount;

        [TearDown]
        public void TearDown()
        {
            TestData.DestroyAll();
            balanceChangedCount = 0;
        }

        [Test]
        public void ItemReward_IsCollected()
        {
            ItemRewardDefinition chest = TestData.CreateReward<ItemRewardDefinition>("Sandik");
            RecordingSink sink = new RecordingSink();

            chest.Apply(sink, 3);

            Assert.AreSame(chest, sink.CollectedReward);
            Assert.AreEqual(3, sink.CollectedAmount);
            Assert.IsFalse(sink.Exploded);
        }

        [Test]
        public void BombReward_Explodes_InsteadOfBeingCollected() // tur kontrolu yok: davranisi odulun kendisi belirliyor
        {
            BombRewardDefinition bomb = TestData.CreateReward<BombRewardDefinition>("Bomba");
            RecordingSink sink = new RecordingSink();

            bomb.Apply(sink, 1);

            Assert.IsTrue(sink.Exploded);
            Assert.IsNull(sink.CollectedReward);
        }

        [Test]
        public void SameRewardTwice_SumsIntoOneEntry()
        {
            ItemRewardDefinition grenade = TestData.CreateReward<ItemRewardDefinition>("El Bombasi");
            RunRewards run = new RunRewards();

            run.Add(grenade, 1);
            run.Add(grenade, 2);

            Assert.AreEqual(1, run.CollectedRewards.Count);
            Assert.AreEqual(3, run.GetAmount(grenade));
        }

        [Test]
        public void BankInto_SendsCurrencyToWallet_ItemsToInventory_AndClears()
        {
            CurrencyRewardDefinition cash = TestData.CreateCurrency("Nakit", CurrencyType.Cash);
            ItemRewardDefinition chest = TestData.CreateReward<ItemRewardDefinition>("Sandik");
            Wallet wallet = new Wallet(100, 0);
            PlayerBank bank = new PlayerBank(wallet);
            RunRewards run = new RunRewards();
            run.Add(cash, 200);
            run.Add(chest, 1);

            run.BankInto(bank);

            Assert.AreEqual(300, wallet.GetBalance(CurrencyType.Cash));
            Assert.AreEqual(1, bank.GetItemAmount(chest));
            Assert.AreEqual(0, run.CollectedRewards.Count);
        }

        [Test]
        public void TrySpend_WhenNotEnough_FailsWithoutChangingBalance()
        {
            Wallet wallet = new Wallet(0, 10);
            wallet.OnBalanceChanged += HandleBalanceChanged;

            bool spent = wallet.TrySpend(CurrencyType.Gold, 25);

            Assert.IsFalse(spent);
            Assert.AreEqual(10, wallet.GetBalance(CurrencyType.Gold));
            Assert.AreEqual(0, balanceChangedCount);
        }

        [Test]
        public void TrySpend_WhenEnough_DeductsAndNotifies()
        {
            Wallet wallet = new Wallet(0, 50);
            wallet.OnBalanceChanged += HandleBalanceChanged;

            bool spent = wallet.TrySpend(CurrencyType.Gold, 25);

            Assert.IsTrue(spent);
            Assert.AreEqual(25, wallet.GetBalance(CurrencyType.Gold));
            Assert.AreEqual(1, balanceChangedCount);
        }

        private void HandleBalanceChanged(CurrencyType type, int balance)
        {
            balanceChangedCount++;
        }
    }
}
