using System.Collections; // IEnumerator burda, coroutine icin lazim
using UnityEngine;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Bir objeyi kisa sureligine buyutup eski boyuna dondurerek "pulse" efekti verir.
    /// </summary>
    public class PunchScaleAnimator : MonoBehaviour // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour
    {
        [SerializeField] private float punchScale = 1.25f; // en buyuk anda kac kat buyuyecek
        [SerializeField] private float duration = 0.25f; // efekt kac saniye surecek

        public void Play() // disaridan cagrilacak, pulse efektini baslatir
        {
            StopAllCoroutines(); // ust uste tetiklenirse eski animasyonu iptal edip yeniden basla
            StartCoroutine(PunchRoutine()); // coroutine'i baslat
        }

        private IEnumerator PunchRoutine() // zaman icinde buyuyup kuculen coroutine
        {
            float elapsed = 0f; // gecen sure
            Vector3 baseScale = Vector3.one; // normal boyut (1x)
            Vector3 peakScale = baseScale * punchScale; // en buyuk boyut

            while (elapsed < duration) // sure dolana kadar
            {
                elapsed += Time.deltaTime; // gecen zamani ekle
                float t = elapsed / duration; // 0 ile 1 arasinda ilerleme
                float curve = Mathf.Sin(t * Mathf.PI); // 0'dan 1'e cikip tekrar 0'a inen bir egri
                transform.localScale = Vector3.Lerp(baseScale, peakScale, curve); // egriye gore boyutu ayarla
                yield return null; // bir sonraki kareye kadar bekle
            }

            transform.localScale = baseScale; // sonunda tam olarak normal boyuta don, kucuk sapmalari duzelt
        }
    }
}
