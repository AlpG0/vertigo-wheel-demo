using System; // Action tipi burda
using System.Collections; // IEnumerator burda, coroutine icin lazim
using UnityEngine;
using UnityEngine.UI; // Image tipi burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Kazanilan odulun ikonunu wheel'deki segmentten TOTAL dairesine dogru ucurarak tasiyan sinif.
    /// </summary>
    public class RewardTravelAnimator : MonoBehaviour, IRewardTravelAnimator // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour; IRewardTravelAnimator'i implement ediyor
    {
        [SerializeField] private Image travelIcon; // ucacak gecici ikon, benim uzerimdeki Image component
        [SerializeField] private float duration = 0.5f; // ucus kac saniye surecek

        private void OnValidate() // kendi Image'imi otomatik bul
        {
            travelIcon = GetComponent<Image>(); // bu script'in kendi objesindeki Image component
        }

        public void Play(Sprite icon, Vector3 fromPosition, Vector3 toPosition, Action onComplete) // disaridan cagrilacak, ucusu baslatir
        {
            travelIcon.sprite = icon; // hangi ikon ucacak
            travelIcon.gameObject.SetActive(true); // ucus bitene kadar gorunur olsun
            travelIcon.transform.position = fromPosition; // baslangic noktasina isinlan

            StartCoroutine(TravelRoutine(fromPosition, toPosition, onComplete)); // coroutine'i baslat
        }

        private IEnumerator TravelRoutine(Vector3 fromPosition, Vector3 toPosition, Action onComplete) // zaman icinde konum degistiren coroutine
        {
            float elapsed = 0f; // gecen sure

            while (elapsed < duration) // sure dolana kadar
            {
                elapsed += Time.deltaTime; // gecen zamani ekle
                float t = elapsed / duration; // 0 ile 1 arasinda ilerleme
                float easedT = 1f - Mathf.Pow(1f - t, 3); // ease-out cubic, spin animasyonundaki gibi basta hizli sona dogru yavaslayan egri
                travelIcon.transform.position = Vector3.Lerp(fromPosition, toPosition, easedT); // egriye gore konumu ayarla
                yield return null; // bir sonraki kareye kadar bekle
            }

            travelIcon.transform.position = toPosition; // tam hedefe otur, kucuk sapmalari duzelt
            travelIcon.gameObject.SetActive(false); // ucus bitti, ikonu tekrar gizle
            onComplete?.Invoke(); // disariya haber ver, soru isareti: eger onComplete null degilse cagir demek
        }
    }
}
