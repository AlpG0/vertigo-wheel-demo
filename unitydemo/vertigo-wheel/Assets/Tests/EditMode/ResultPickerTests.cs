using NUnit.Framework;
using VertigoWheel.Data;
using VertigoWheel.Rewards;
using VertigoWheel.Wheel;

namespace VertigoWheel.Tests
{
    public class ResultPickerTests
    {
        private WheelConfig wheel;

        [SetUp]
        public void SetUp()
        {
            RewardDefinition[] rewards =
            {
                TestData.CreateReward<BombRewardDefinition>("Bomba"),
                TestData.CreateReward<ItemRewardDefinition>("Sandik"),
                TestData.CreateReward<ItemRewardDefinition>("Molotof")
            };
            wheel = TestData.CreateWheel(rewards, new[] { 1, 1, 2 });
        }

        [TearDown]
        public void TearDown()
        {
            TestData.DestroyAll();
        }

        [Test]
        public void FixedIndexPicker_AlwaysReturnsTheSameSegment()
        {
            FixedIndexResultPicker picker = new FixedIndexResultPicker(2);

            Assert.AreSame(wheel.Segments[2], picker.PickSegment(wheel));
            Assert.AreSame(wheel.Segments[2], picker.PickSegment(wheel));
        }

        [Test]
        public void FixedIndexPicker_WrapsAnIndexLargerThanTheWheel()
        {
            FixedIndexResultPicker picker = new FixedIndexResultPicker(4); // 3 dilim var, 4 -> 1

            Assert.AreSame(wheel.Segments[1], picker.PickSegment(wheel));
        }

        [Test]
        public void RandomPicker_OnlyReturnsSegmentsOfTheWheel()
        {
            RandomWheelResultPicker picker = new RandomWheelResultPicker();

            for (int i = 0; i < 50; i++)
            {
                CollectionAssert.Contains(wheel.Segments, picker.PickSegment(wheel));
            }
        }
    }
}
