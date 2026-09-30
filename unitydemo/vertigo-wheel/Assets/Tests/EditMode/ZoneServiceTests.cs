using NUnit.Framework;
using VertigoWheel.Zone;

namespace VertigoWheel.Tests
{
    public class ZoneServiceTests
    {
        private ZoneDefinition bronze;
        private ZoneDefinition silver;
        private ZoneDefinition gold;
        private ZoneService service;

        [SetUp]
        public void SetUp()
        {
            bronze = TestData.CreateZone("Bronz", 1, false, null);
            silver = TestData.CreateZone("Gumus", 5, true, null);
            gold = TestData.CreateZone("Altin", 30, true, null);
            service = new ZoneService(new[] { silver, bronze, gold }); // sira bilerek karisik: servis araliga gore kendisi siralamali
        }

        [TearDown]
        public void TearDown()
        {
            TestData.DestroyAll();
        }

        [TestCase(1)]
        [TestCase(7)]
        [TestCase(29)]
        public void OrdinaryZone_UsesDefaultZone(int zoneNumber)
        {
            Assert.AreSame(bronze, service.GetZone(zoneNumber));
        }

        [TestCase(5)]
        [TestCase(25)]
        [TestCase(35)]
        public void EveryFifthZone_IsSafe(int zoneNumber)
        {
            Assert.AreSame(silver, service.GetZone(zoneNumber));
        }

        [TestCase(30)]
        [TestCase(60)]
        public void EveryThirtiethZone_IsSuper_EvenThoughItIsAlsoAMultipleOfFive(int zoneNumber)
        {
            Assert.AreSame(gold, service.GetZone(zoneNumber));
        }

        [Test]
        public void NextSafeZone_SkipsTheZoneTakenBySuper()
        {
            Assert.AreEqual(35, service.GetNextZoneNumber(silver, 25));
        }

        [Test]
        public void NextSuperZone_FromTheStart_IsThirty()
        {
            Assert.AreEqual(30, service.GetNextZoneNumber(gold, 1));
        }

        [Test]
        public void NewZoneKind_IsJustData_NoCodeChange() // OCP: yeni zone turu = yeni tanim, servise dokunmadan
        {
            ZoneDefinition diamond = TestData.CreateZone("Elmas", 100, true, null);
            ZoneService withDiamond = new ZoneService(new[] { bronze, silver, gold, diamond });

            Assert.AreSame(diamond, withDiamond.GetZone(100));
            Assert.AreSame(gold, withDiamond.GetZone(90));
        }
    }
}
