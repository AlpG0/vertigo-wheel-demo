using UnityEngine;

namespace VertigoWheel.Rewards
{
    /// <summary>
    /// Wheel'den cikabilecek her seyin (esya, para, bomba) ortak temeli.
    /// Kazanilinca ne olacagina odulun kendisi karar verir, akis tarafinda tur kontrolu yapilmaz.
    /// </summary>
    public abstract class RewardDefinition : ScriptableObject // abstract: tek basina asset olamaz, bir alt tipi secilmeli
    {
        [SerializeField] private string displayName; // ekranda gorunecek isim
        [SerializeField] private Sprite icon; // wheel'de ve odul listesinde gorunecek ikon

        public string DisplayName { get { return displayName; } }
        public Sprite Icon { get { return icon; } }

        public virtual void Apply(IRewardSink sink, int amount) // wheel bu odulde durunca cagrilir, varsayilan davranis: toplanir
        {
            sink.Collect(this, amount);
        }

        public virtual void Bank(IRewardBank bank, int amount) // oyuncu cikis yapinca odul kalici hesaba boyle aktarilir, varsayilan: esya olarak
        {
            bank.AddItem(this, amount);
        }
    }
}
