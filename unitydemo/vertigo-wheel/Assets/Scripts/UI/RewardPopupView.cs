using System; // Action tipi burda
using TMPro; // TMP_Text burda
using UnityEngine;
using UnityEngine.UI; // Button burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Bomba patlayinca gosterilen odul popup'ini yoneten sinif.
    /// </summary>
    public class RewardPopupView : MonoBehaviour, IRewardPopupView // IRewardPopupView'i implement ediyor
    {
        private const string MessageTextName = "ui_text_reward_popup_value"; // asil mesaj yazisinin obje ismi
        private const string GiveUpButtonName = "ui_button_reward_popup_give_up"; // butonlari birbirinden ayirt etmek icin isim sabitleri
        private const string GoldReviveButtonName = "ui_button_reward_popup_gold_revive";
        private const string AdReviveButtonName = "ui_button_reward_popup_ad_revive";

        [SerializeField] private TMP_Text messageText; // popup icindeki mesaj yazisi
        [SerializeField] private Button giveUpButton; // oduller gercekten kaybedilecekse basilan buton
        [SerializeField] private Button goldReviveButton; // altin harcayarak devam
        [SerializeField] private Button adReviveButton; // reklam izleyerek devam

        public event Action OnGiveUpClicked;
        public event Action OnGoldReviveClicked;
        public event Action OnAdReviveClicked;

        private void OnValidate() // referanslari elle suruklemeyelim diye otomatik bul
        {
            TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>(true); // true: popup kapali basladigi icin inactive de dahil ara
            foreach (TMP_Text text in allTexts) // butonlarin da kendi label'lari oldugu icin isme gore seciyoruz
            {
                if (text.gameObject.name == MessageTextName)
                {
                    messageText = text;
                }
            }

            Button[] allButtons = GetComponentsInChildren<Button>(true);
            foreach (Button button in allButtons) // her butonu ismine gore kendi alanina koy
            {
                if (button.gameObject.name == GiveUpButtonName)
                {
                    giveUpButton = button;
                }
                else if (button.gameObject.name == GoldReviveButtonName)
                {
                    goldReviveButton = button;
                }
                else if (button.gameObject.name == AdReviveButtonName)
                {
                    adReviveButton = button;
                }
            }
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
