using UnityEngine; // Sprite burda tanımlı, o yüzden lazım
using VertigoWheel.Rewards; // RewardDefinition burda

namespace VertigoWheel.Data // data ile ilgili sınıflar burada olacak
{
    [System.Serializable] // Inspector'da görünsün diye
    /// <summary>
    /// Bir wheel diliminin verisini (hangi odul, kac tane) tutar.
    /// </summary>
    public class WheelSegmentData // bir dilimin bilgisi burada tutuluyor
    {
        [SerializeField] private RewardDefinition reward; // dilim hangi odul (esya, para ya da bomba), turu artik enum degil odulun kendi sinifi
        [SerializeField] private int amount; // kaç tane/kaç para

        public RewardDefinition Reward // dışarıdan okunabilsin diye
        {
            get { return reward; } // sadece okuma, değiştirme yok
        }

        public int Amount
        {
            get { return amount; }
        }

        public Sprite Icon // ikon artik odulun kendisinde, ayni odulu her dilimde tekrar tekrar secmeyelim diye
        {
            get { return reward.Icon; }
        }
    }
}
