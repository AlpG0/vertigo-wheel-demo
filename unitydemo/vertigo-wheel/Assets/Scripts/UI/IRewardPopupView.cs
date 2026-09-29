using System; // Action burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Bomba patlayinca gosterilen popup'i yoneten siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IRewardPopupView
    {
        event Action OnGiveUpClicked; // GIVE UP butonuna basilinca tetiklenir, oduller gercekten kaybedilir
        event Action OnGoldReviveClicked; // altinla revive'a basilinca tetiklenir
        event Action OnAdReviveClicked; // reklamla revive'a basilinca tetiklenir

        void Show(string message, bool canAffordGoldRevive); // popup'i mesajla gosterir, altin yetmiyorsa altinli revive butonu pasif olur
        void Hide(); // popup'i gizler
    }
}
