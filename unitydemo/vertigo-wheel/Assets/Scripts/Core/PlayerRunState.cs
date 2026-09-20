namespace VertigoWheel.Core
{
    /// <summary>
    /// Oyuncunun mevcut run durumunu temsil eder.
    /// </summary>
    public class PlayerRunState // MonoBehaviour degil, sahneye bagli olmayan duz bir veri sinifi
    {
        private int currentZone = 1; // oyuncu hangi zone'da, 1'den basliyor
        private int totalValue; // su ana kadar toplanan odul miktari

        public int CurrentZone
        {
            get { return currentZone; }// disaridan okunabilsin diye
        }

        public int TotalValue
        {
            get { return totalValue; } // disaridan okunabilsin diye
        }

        public void AddReward(int amount) // odul kazanildiginda cagrilir
        {
            totalValue += amount; // mevcut toplama ekliyoruz
        }

        public void ResetRun() // bombaya carpinca her sey sifirlanir
        {
            totalValue = 0; // odul sifirlaniyor
            currentZone = 1; // basa donuyoruz
        }

        public void AdvanceZone() // bir zone ilerleriz (spin basarili oldugunda)
        {
            currentZone++; // zone sayacini bir arttir
        }

        public void EndRun() // leave ile basariyla run'u bitirince cagrilir
        {
            currentZone = 1; // yeni bir run icin zone'u basa aliyoruz
            // totalValue'ya dokunmuyoruz cunku leave'de kazanilan odul korunuyor, bomba gibi kaybetmiyoruz
        }
    }
}