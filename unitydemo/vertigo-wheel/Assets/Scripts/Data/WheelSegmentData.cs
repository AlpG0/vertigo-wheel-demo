using UnityEngine; // Sprite burda tanımlı, o yüzden lazım

namespace VertigoWheel.Data // data ile ilgili sınıflar burada olacak
{
    [System.Serializable] // Inspector'da görünsün diye
    /// <summary>
    /// Bir wheel diliminin verisini (odul turu, miktar, ikon) tutar.
    /// </summary>
    public class WheelSegmentData // bir dilimin bilgisi burada tutuluyor
    {
        [SerializeField] private RewardType rewardType; // dilim ne tip (bomba/para/item)
        [SerializeField] private int amount; // kaç tane/kaç para
        [SerializeField] private Sprite icon; // dilimin resmi

        public RewardType RewardType // dışarıdan okunabilsin diye
        {
            get { return rewardType; } // sadece okuma, değiştirme yok
        }

        public int Amount
        {
            get { return amount; }
        }

        public Sprite Icon
        {
            get { return icon; }
        }
    }
}