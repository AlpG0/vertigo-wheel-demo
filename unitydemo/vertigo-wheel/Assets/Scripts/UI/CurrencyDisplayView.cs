using TMPro;
using UnityEngine;
using VertigoWheel.Rewards;
using VertigoWheel.Utils;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Cuzdandaki tek bir para turunun gostergesi (ikon + bakiye). Hangi para turunu gosterdigi Inspector'dan secilir.
    /// </summary>
    public class CurrencyDisplayView : MonoBehaviour
    {
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private PunchScaleAnimator punch;

        public CurrencyType CurrencyType { get { return currencyType; } }

        private void OnValidate()
        {
            amountText = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.CurrencyAmountText);
            punch = GetComponentInChildren<PunchScaleAnimator>(true);
        }

        public void SetAmount(int amount, bool animate)
        {
            amountText.text = NumberFormatter.FormatAmount(amount);
            if (animate)
            {
                punch.Play();
            }
        }
    }
}
