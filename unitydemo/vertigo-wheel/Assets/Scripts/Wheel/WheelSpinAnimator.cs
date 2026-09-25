using System; // Action tipi burda
using System.Collections; // IEnumerator burda, coroutine icin lazim
using UnityEngine;


namespace VertigoWheel.Wheel
{
    /// <summary>
    /// Wheel objesinin donme animasyonunu yapan sinif.
    /// </summary>
    public class WheelSpinAnimator : MonoBehaviour, IWheelSpinAnimator // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour; IWheelSpinAnimator'i implement ediyor
    {
        [SerializeField] private float spinDuration = 2f; // animasyon kac saniye surecek
        [SerializeField] private int extraFullSpins = 5; // gorsel etki icin kac tam tur atsin

        private Coroutine spinCoroutine; // su an calisan spin coroutine'i, yarida kesmek icin referansini tutuyoruz
        private Action pendingOnComplete; // spin yarida kesilirse yine de cagirmamiz gereken callback

        public void SpinTo(int segmentIndex, int segmentCount, Action onComplete) // disaridan cagrilacak, hangi segment kazandi
        {
            pendingOnComplete = onComplete; // obje disable olursa diye callback'i ayrica sakliyoruz
            spinCoroutine = StartCoroutine(SpinRoutine(segmentIndex, segmentCount, onComplete)); // coroutine'i baslat, referansini tut
        }

        private void OnDisable() // obje herhangi bir sebeple pasif olursa Unity bunu cagirir, coroutine'leri de otomatik durdurur
        {
            if (spinCoroutine == null) // devam eden bir spin yoksa yapacak bir sey yok
            {
                return;
            }

            spinCoroutine = null; // referansi temizle
            Action callback = pendingOnComplete; // callback'i gecici degiskene al
            pendingOnComplete = null; // saklanani temizle, tekrar cagrilmasin

            callback?.Invoke(); // spin yarida kalsa bile disariya "bitti" haberi ver, yoksa Spin/Leave butonlari sonsuza kadar kilitli kalir
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
                float t = elapsed / spinDuration; // 0 ile 1 arasinda ilerleme orani (dogrusal, hala sabit hizda ilerliyor)
                float easedT = 1f - Mathf.Pow(1f - t, 3); // "ease-out cubic": basta hizli, sona dogru yavaslayan bir egri
                float currentAngle = Mathf.Lerp(0f, targetAngle, easedT); // artik t yerine easedT kullaniyoruz, gercek bir cark gibi yavaslayarak dursun diye
                transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle); // wheel'i o aciya dondur
                yield return null; // bir sonraki kareye kadar bekle
            }

            transform.localRotation = Quaternion.Euler(0f, 0f, targetAngle); // tam hedef acida bitir, kucuk sapmalari duzelt

            spinCoroutine = null; // normal bittigi icin referansi temizle, OnDisable tekrar cagirmasin
            pendingOnComplete = null; // saklanani da temizle

            onComplete?.Invoke(); // animasyon bitti, disariya haber ver, soru isareti: eger onComplete null degilse cagir demek
        }
    }
}