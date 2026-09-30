namespace VertigoWheel.Utils
{
    /// <summary>
    /// Animasyonlarda ortak kullanilan hiz egrileri. Her animasyon kendi formulunu tekrar yazmasin diye tek yerde.
    /// </summary>
    public static class Easing
    {
        public static float EaseOutCubic(float t) // basta hizli, sona dogru yavaslayan egri (t: 0..1)
        {
            float inverse = 1f - t;
            return 1f - inverse * inverse * inverse;
        }
    }
}
