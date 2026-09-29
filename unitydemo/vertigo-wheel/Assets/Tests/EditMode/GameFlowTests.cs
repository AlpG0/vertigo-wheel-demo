using NUnit.Framework;
using VertigoWheel.Core;
using VertigoWheel.Data;
using VertigoWheel.Rewards;
using VertigoWheel.Wheel;
using VertigoWheel.Zone;

namespace VertigoWheel.Tests
{
    /// <summary>
    /// Oyun akisini bastan sona, sahne olmadan test eder: GameController + gercek state'ler + gercek servisler,
    /// view'lar sahte, sonuc secici ise rastgele yerine FixedIndexResultPicker / QueuedResultPicker ile sabitlenmis.
    /// </summary>
    public class GameFlowTests
    {
        private const int BombIndex = 0;
        private const int CashIndex = 1;
        private const int CashAmount = 200;
        private const int ReviveCost = 25;

        private FakeGameViews views;
        private Wallet wallet;
        private RunRewards runRewards;
        private PlayerRunState runState;
        private GameController controller;

        [TearDown]
        public void TearDown()
        {
            if (controller != null)
            {
                controller.Dispose();
            }

            TestData.DestroyAll();
        }

        private void StartGame(IWheelResultPicker picker, int startingGold)
        {
            RewardDefinition[] rewards =
            {
                TestData.CreateReward<BombRewardDefinition>("Bomba"),
                TestData.CreateCurrency("Nakit", CurrencyType.Cash)
            };
            WheelConfig wheel = TestData.CreateWheel(rewards, new[] { 1, CashAmount });
            ZoneDefinition[] zones =
            {
                TestData.CreateZone("Bronz", 1, false, wheel),
                TestData.CreateZone("Gumus", 5, true, wheel)
            };
            GameSettings settings = TestData.CreateSettings(zones, ReviveCost);

            views = new FakeGameViews();
            GameViews gameViews = views.ToGameViews();
            wallet = new Wallet(0, startingGold);
            runRewards = new RunRewards();
            runState = new PlayerRunState();
            ZoneService zoneService = new ZoneService(settings.Zones);
            ZonePresenter presenter = new ZonePresenter(zoneService, runState, gameViews);

            GameContext context = new GameContext(settings, zoneService, runState, runRewards, wallet, new PlayerBank(wallet), picker, gameViews, presenter);
            controller = new GameController(context);
            controller.Start();
        }

        [Test]
        public void WinningASpin_CollectsTheReward_AndAdvancesTheZone()
        {
            StartGame(new FixedIndexResultPicker(CashIndex), 0);

            views.ClickSpin();
            views.ClickSpin();

            Assert.AreEqual(CashAmount * 2, runRewards.GetAmount(runRewards.CollectedRewards[0]));
            Assert.AreEqual(3, runState.CurrentZone);
            Assert.AreEqual(3, views.CurrentZoneShown);
            Assert.IsTrue(views.SpinInteractable);
        }

        [Test]
        public void HittingTheBomb_OpensThePopup_AndLocksTheSpinButton()
        {
            StartGame(new FixedIndexResultPicker(BombIndex), 0);

            views.ClickSpin();

            Assert.IsTrue(views.PopupVisible);
            Assert.IsFalse(views.SpinInteractable);
        }

        [Test]
        public void SpinClick_WhilePopupIsOpen_IsIgnoredByTheState()
        {
            StartGame(new FixedIndexResultPicker(BombIndex), 0);
            views.ClickSpin();

            views.ClickSpin(); // buton kilitli olsa bile event gelirse state kabul etmemeli

            Assert.AreEqual(1, views.SpinCount);
            Assert.IsTrue(views.PopupVisible);
        }

        [Test]
        public void GiveUp_LosesTheRunRewards_AndGoesBackToZoneOne()
        {
            StartGame(new QueuedResultPicker(CashIndex, BombIndex), 0);
            views.ClickSpin();
            views.ClickSpin();

            views.ClickGiveUp();

            Assert.IsFalse(views.PopupVisible);
            Assert.AreEqual(0, runRewards.CollectedRewards.Count);
            Assert.AreEqual(1, runState.CurrentZone);
            Assert.IsTrue(views.SpinInteractable);
        }

        [Test]
        public void GoldRevive_SpendsGold_AndKeepsEverything()
        {
            StartGame(new QueuedResultPicker(CashIndex, BombIndex), 50);
            views.ClickSpin();
            views.ClickSpin();

            views.ClickGoldRevive();

            Assert.IsFalse(views.PopupVisible);
            Assert.AreEqual(50 - ReviveCost, wallet.GetBalance(CurrencyType.Gold));
            Assert.AreEqual(1, runRewards.CollectedRewards.Count);
            Assert.AreEqual(2, runState.CurrentZone);
        }

        [Test]
        public void GoldRevive_WithoutEnoughGold_DoesNothing()
        {
            StartGame(new FixedIndexResultPicker(BombIndex), 10);
            views.ClickSpin();

            views.ClickGoldRevive();

            Assert.IsFalse(views.PopupCanAffordGoldRevive);
            Assert.IsTrue(views.PopupVisible);
            Assert.AreEqual(10, wallet.GetBalance(CurrencyType.Gold));
        }

        [Test]
        public void AdRevive_IsFree_AndKeepsEverything()
        {
            StartGame(new QueuedResultPicker(CashIndex, BombIndex), 0);
            views.ClickSpin();
            views.ClickSpin();

            views.ClickAdRevive();

            Assert.IsFalse(views.PopupVisible);
            Assert.AreEqual(1, runRewards.CollectedRewards.Count);
        }

        [Test]
        public void Leave_IsOnlyAllowedInASafeZone_AndBanksTheRewards()
        {
            StartGame(new FixedIndexResultPicker(CashIndex), 0);

            views.ClickLeave(); // zone 1: cikisa izin yok
            Assert.AreEqual(0, wallet.GetBalance(CurrencyType.Cash));

            for (int i = 0; i < 4; i++) // zone 5'e (gumus) kadar ilerle
            {
                views.ClickSpin();
            }

            Assert.IsTrue(views.LeaveInteractable);
            views.ClickLeave();

            Assert.AreEqual(CashAmount * 4, wallet.GetBalance(CurrencyType.Cash));
            Assert.AreEqual(0, runRewards.CollectedRewards.Count);
            Assert.AreEqual(1, runState.CurrentZone);
        }
    }
}
