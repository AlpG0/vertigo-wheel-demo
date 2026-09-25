using System; // Action burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Bomba patlayinca gosterilen popup'i yoneten siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IRewardPopupView
    {
        event Action OnGiveUpClicked; // GIVE UP butonuna basilinca tetiklenir, oduller gercekten kaybedilir
        event Action OnReviveClicked; // REVIVE butonlarindan (gold veya reklam) birine basilinca tetiklenir, hicbir sey kaybedilmez

        void Show(string message); // popup'i mesajla birlikte gosterir
        void Hide(); // popup'i gizler
    }
}
