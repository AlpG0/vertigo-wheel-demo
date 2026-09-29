using UnityEngine;
using VertigoWheel.Rewards;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Sag alttaki bakiye gostergeleri. Her para turu icin bir CurrencyDisplayView var; yeni para turu eklemek yeni bir gosterge demek.
    /// </summary>
    public class WalletView : MonoBehaviour, IWalletView
    {
        [SerializeField] private CurrencyDisplayView[] displays;

        private void OnValidate()
        {
            displays = GetComponentsInChildren<CurrencyDisplayView>(true);
        }

        public void SetBalance(CurrencyType type, int amount, bool animate)
        {
            foreach (CurrencyDisplayView display in displays)
            {
                if (display.CurrencyType == type)
                {
                    display.SetAmount(amount, animate);
                }
            }
        }
    }
}
