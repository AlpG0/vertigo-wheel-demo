using VertigoWheel.Rewards;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Sag alttaki nakit/altin bakiyesini gosteren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IWalletView
    {
        void SetBalance(CurrencyType type, int amount, bool animate); // animate: degisince kucuk bir "pop" yapsin mi
    }
}