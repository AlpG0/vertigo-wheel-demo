using UnityEngine;
using UnityEngine.UI;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Bir Image'in alfa degerini surekli nefes alir gibi (0-1 arasi) degistirir.
    /// PunchScaleAnimator'dan farkli olarak tek seferlik degil, obje aktif oldugu surece dongude calisir.
    /// </summary>
    public class PulsingGlowAnimator : MonoBehaviour // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour
    {
        [SerializeField] private Image targetImage; // alfasini degistirecegimiz Image, kendi uzerimdeki component
        [SerializeField] private float minAlpha = 0.25f; // en soluk anda alfa
        [SerializeField] private float maxAlpha = 0.6f; // en belirgin anda alfa
        [SerializeField] private float speed = 1.5f; // nefes alma hizi

        private void OnValidate() // kendi Image'imi otomatik bul
        {
            targetImage = GetComponent<Image>();
        }

        private void Update() // her karede calisir, obje aktif oldugu surece
        {
            float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f; // sin -1..1 araligini 0..1 araligina cekiyoruz
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, t); // 0..1 degerini min-max alfa araligina yerlestiriyoruz

            Color color = targetImage.color; // once mevcut rengi al, sadece alfayi degistirecegiz
            color.a = alpha;
            targetImage.color = color;
        }
    }
}
