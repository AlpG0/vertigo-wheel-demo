using System; // Action tipi burda
using System.Collections.Generic; // List burda
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
        private const string GiveUpButtonName = "ui_button_reward_popup_give_up"; // GIVE UP butonunu digerlerinden ayirt etmek icin isim sabiti

        [SerializeField] private TMP_Text messageText; // popup icindeki mesaj yazisi
        [SerializeField] private Button giveUpButton; // oduller gercekten kaybedilecekse basilan buton
        [SerializeField] private Button[] reviveButtons; // gold REVIVE ve reklam REVIVE - ikisi de hicbir sey kaybettirmez

        public event Action OnGiveUpClicked; // GIVE UP'a basilinca disariya haber verir
        public event Action OnReviveClicked; // REVIVE butonlarindan birine basilinca disariya haber verir

        private void OnValidate() // referanslari elle suruklemeyelim diye otomatik bul
        {
            TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>(true); // true: inactive objeleri de ara, cunku popup kapali baslıyor
            foreach (TMP_Text text in allTexts) // artik butonlarin da kendi label'lari oldugu icin isme gore secmemiz lazim
            {
                if (text.gameObject.name == "ui_text_reward_popup_value") // asil mesaj yazisi bu isimde
                {
                    messageText = text;
                }
            }

            Button[] allButtons = GetComponentsInChildren<Button>(true); // true: popup kapali basladigi icin inactive de dahil ara
            List<Button> revives = new List<Button>(); // GIVE UP disindaki butonlari buraya toplayacagiz

            foreach (Button button in allButtons) // her butonu tek tek kontrol et
            {
                if (button.gameObject.name == GiveUpButtonName) // ismi GIVE UP mi
                {
                    giveUpButton = button;
                }
                else // degilse revive butonlarindan biridir (gold veya reklam)
                {
                    revives.Add(button);
                }
            }

            reviveButtons = revives.ToArray(); // List'i tekrar array'e cevirip alana ata
        }

        private void Awake() // sahne yuklenince calisir
        {
            giveUpButton.onClick.AddListener(HandleGiveUpClicked); // GIVE UP'a tiklaninca hangi metot calisacak

            foreach (Button button in reviveButtons) // her iki revive butonunu da ayni metoda bagliyoruz
            {
                button.onClick.AddListener(HandleReviveClicked);
            }
        }

        private void HandleGiveUpClicked() // GIVE UP'a tiklaninca calisir
        {
            OnGiveUpClicked?.Invoke(); // dinleyen varsa haber ver
        }

        private void HandleReviveClicked() // iki revive butonundan birine tiklaninca calisir
        {
            OnReviveClicked?.Invoke(); // dinleyen varsa haber ver
        }

        public void Show(string message) // popup'i mesajla birlikte goster
        {
            messageText.text = message; // mesaji guncelle
            gameObject.SetActive(true); // objeyi aktif yap, goruncur olsun
        }

        public void Hide() // popup'i gizle
        {
            gameObject.SetActive(false); // objeyi pasif yap, kaybolsun
        }
    }
}