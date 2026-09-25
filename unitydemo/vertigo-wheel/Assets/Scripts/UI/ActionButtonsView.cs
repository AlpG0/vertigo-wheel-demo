using System; // Action tipi burda tanımlı
using UnityEngine; // MonoBehaviour, SerializeField burda
using UnityEngine.UI; // Button tipi burda

namespace VertigoWheel.UI // UI ile ilgili sınıflar burada olacak.
{
    /// <summary>
    /// Spin ve Leave butonlarinin tiklanmasini dinleyip disariya event olarak bildiren sinif.
    /// </summary>
    public class ActionButtonsView : MonoBehaviour, IActionButtonsView // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour; IActionButtonsView'i implement ediyor
    {
        [SerializeField] private Button spinButton; // spin butonunun Button component'i
        [SerializeField] private Button leaveButton; // leave butonunun Button component'i

        public event Action OnSpinClicked; // spin'e basılınca dışarıya haber vermek için
        public event Action OnLeaveClicked; // leave'e basılınca dışarıya haber vermek için

        private void OnValidate() // Editor'de otomatik çalışır, referansları elle sürüklemeyelim diye
        {
            Button[] buttons = GetComponentsInChildren<Button>(); // altındaki tüm butonları bul
            spinButton = buttons[0]; // ilk bulunan buton spin (hierarchy sırasına göre)
            leaveButton = buttons[1]; // ikinci bulunan buton leave
        }

        private void Awake() // sahne yüklenince, Start'tan önce çalışır
        {
            spinButton.onClick.AddListener(HandleSpinClicked); // butona tıklanınca hangi metodun çalışacağını kodla bağlıyoruz
            leaveButton.onClick.AddListener(HandleLeaveClicked); // aynı şekilde leave için
        }

        private void HandleSpinClicked() // spin butonuna tıklanınca çalışır
        {
            OnSpinClicked?.Invoke(); // soru isareti kullanmamim nedeni bu event'i dinleyen varsa haber ver,yoksa null reference hatası verme demektir.
        }

        private void HandleLeaveClicked() // leave butonuna tıklanınca çalışır
        {
            OnLeaveClicked?.Invoke(); // bu event'i dinleyen varsa haber ver yoksa null reference hatası verme demektir.
        }

        public void SetLeaveInteractable(bool interactable) // leave butonu tiklanabilir mi olacak
        {
            leaveButton.interactable = interactable; // Unity'nin kendi ozelligi: false ise tiklamalar hic calismaz
        }

        public void SetSpinInteractable(bool interactable) // spin butonu tiklanabilir mi olacak
        {
            spinButton.interactable = interactable; // animasyon sirasinda tekrar tiklanmasin diye kapatacagiz
        }
    }
}