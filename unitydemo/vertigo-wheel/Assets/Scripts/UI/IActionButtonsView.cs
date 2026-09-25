using System; // Action burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Spin ve Leave butonlarini dinleyip disariya event olarak bildiren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IActionButtonsView
    {
        event Action OnSpinClicked; // spin'e basilinca tetiklenir
        event Action OnLeaveClicked; // leave'e basilinca tetiklenir

        void SetSpinInteractable(bool interactable); // spin butonunu ac/kapat
        void SetLeaveInteractable(bool interactable); // leave butonunu ac/kapat
    }
}
