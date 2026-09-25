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
        [SerializeField] private TMP_Text messageText; // popup icindeki mesaj yazisi
        [SerializeField] private Button closeButton; // kapatma butonu

        public event Action OnCloseClicked; // kapatma butonuna basilinca disariya haber verir

        private void OnValidate() // referanslari elle suruklemeyelim diye otomatik bul
        {
            messageText = GetComponentInChildren<TMP_Text>(true); // true: inactive objeleri de ara, cunku popup kapali baslıyor
            closeButton = GetComponentInChildren<Button>(true); // ayni sebeple true
        }

        private void Awake() // sahne yuklenince calisir
        {
            closeButton.onClick.AddListener(HandleCloseClicked); // butona tiklaninca hangi metot calisacak
        }

        private void HandleCloseClicked() // butona tiklaninca calisir
        {
            OnCloseClicked?.Invoke(); // dinleyen varsa haber ver
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