using System; // Action burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Bomba patlayinca gosterilen popup'i yoneten siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IRewardPopupView
    {
        event Action OnCloseClicked; // kapatma butonuna basilinca tetiklenir

        void Show(string message); // popup'i mesajla birlikte gosterir
        void Hide(); // popup'i gizler
    }
}
