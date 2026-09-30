using System; // Action tipi burda
using TMPro; // TMP_Text burda
using UnityEngine;
using UnityEngine.UI; // Button burda
using VertigoWheel.Utils; // GameConstants, HierarchyLookup burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Bomba patlayinca gosterilen odul popup'ini yoneten sinif.
    /// </summary>
    public class RewardPopupView : MonoBehaviour, IRewardPopupView // IRewardPopupView'i implement ediyor
    {
        [SerializeField] private TMP_Text messageText; // popup icindeki mesaj yazisi
        [SerializeField] private Button giveUpButton; // oduller gercekten kaybedilecekse basilan buton
        [SerializeField] private Button goldReviveButton; // altin harcayarak devam
        [SerializeField] private Button adReviveButton; // reklam izleyerek devam

        public event Action OnGiveUpClicked;
        public event Action OnGoldReviveClicked;
        public event Action OnAdReviveClicked;

        private void OnValidate() // referanslari elle suruklemeyelim diye otomatik bul, isimler GameConstants'ta tek yerde
        {
            messageText = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.PopupMessageText);
            giveUpButton = HierarchyLookup.FindByName<Button>(this, GameConstants.UINames.PopupGiveUpButton);
            goldReviveButton = HierarchyLookup.FindByName<Button>(this, GameConstants.UINames.PopupGoldReviveButton);
            adReviveButton = HierarchyLookup.FindByName<Button>(this, GameConstants.UINames.PopupAdReviveButton);
        }

        private void Awake() // sahne yuklenince calisir
        {
            giveUpButton.onClick.AddListener(HandleGiveUpClicked);
            goldReviveButton.onClick.AddListener(HandleGoldReviveClicked);
            adReviveButton.onClick.AddListener(HandleAdReviveClicked);
        }

        private void HandleGiveUpClicked()
        {
            OnGiveUpClicked?.Invoke();
        }

        private void HandleGoldReviveClicked()
        {
            OnGoldReviveClicked?.Invoke();
        }

        private void HandleAdReviveClicked()
        {
            OnAdReviveClicked?.Invoke();
        }

        public void Show(string message, bool canAffordGoldRevive) // popup'i mesajla birlikte goster
        {
            messageText.text = message;
            goldReviveButton.interactable = canAffordGoldRevive; // altin yetmiyorsa basilamasin, reklamli secenek hep acik
            gameObject.SetActive(true);
        }

        public void Hide() // popup'i gizle
        {
            gameObject.SetActive(false);
        }
    }
}
