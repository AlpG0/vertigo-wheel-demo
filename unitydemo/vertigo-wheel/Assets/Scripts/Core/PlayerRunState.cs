namespace VertigoWheel.Core
{
    /// <summary>
    /// Oyuncunun mevcut run durumunu (hangi zone'da oldugunu) temsil eder.
    /// </summary>
    public class PlayerRunState : IPlayerRunState // MonoBehaviour degil, sahneye bagli olmayan duz bir veri sinifi; IPlayerRunState'i implement ediyor
    {
        private int currentZone = 1; // oyuncu hangi zone'da, 1'den basliyor

        public int CurrentZone
        {
            get { return currentZone; } // disaridan okunabilsin diye
        }

        public void AdvanceZone() // bir zone ilerleriz (spin basarili oldugunda)
        {
            currentZone++; // zone sayacini bir arttir
        }

        public void ResetZone() // bomba ya da cikis sonrasi yeni run basliyor
        {
            currentZone = 1;
        }
    }
}
