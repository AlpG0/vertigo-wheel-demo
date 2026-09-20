using System; // Action tipi burda
using System.Collections; // IEnumerator burda, coroutine icin lazim
using UnityEngine;


namespace VertigoWheel.Wheel
{
    /// <summary>
    /// Wheel objesinin donme animasyonunu yapan sinif.
    /// </summary>
    public class WheelSpinAnimator : MonoBehaviour // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour
    {
        [SerializeField] private float spinDuration = 2f; // animasyon kac saniye surecek
        [SerializeField] private int extraFullSpins = 5; // gorsel etki icin kac tam tur atsin

        public void SpinTo(int segmentIndex, int segmentCount, Action onComplete) // disaridan cagrilacak, hangi segment kazandi
        {
            StartCoroutine(SpinRoutine(segmentIndex, segmentCount, onComplete)); // coroutine'i baslat
        }

        private IEnumerator SpinRoutine(int segmentIndex, int segmentCount, Action onComplete) // coroutine: zaman icinde calisan metot
        {
            float segmentAngle = 360f / segmentCount; // her segmentin kapladigi aci
            float targetAngle = extraFullSpins * 360f + segmentIndex * segmentAngle; // hedef aci: tam turlar + segmentin kendi acisi

            transform.localRotation = Quaternion.identity; // her spin basinda aciyi sifirla
            float elapsed = 0f; // gecen sure

            while (elapsed < spinDuration) // sure dolana kadar dongu
            {
                elapsed += Time.deltaTime; // her karede gecen zamani ekle
                float t = elapsed / spinDuration; // 0 ile 1 arasinda ilerleme orani
                float currentAngle = Mathf.Lerp(0f, targetAngle, t); // su anki ara aci
                transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle); // wheel'i o aciya dondur
                yield return null; // bir sonraki kareye kadar bekle
            }

            transform.localRotation = Quaternion.Euler(0f, 0f, targetAngle); // tam hedef acida bitir, kucuk sapmalari duzelt
            onComplete?.Invoke(); // animasyon bitti, disariya haber ver, soru isareti: eger onComplete null degilse cagir demek
        }
    }
}