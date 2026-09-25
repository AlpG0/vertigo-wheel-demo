using System; // Action burda
using UnityEngine; // Sprite, Vector3 burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Kazanilan odulun ikonunu wheel'den TOTAL'e ucuran siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IRewardTravelAnimator
    {
        void Play(Sprite icon, Vector3 fromPosition, Vector3 toPosition, Action onComplete); // ucusu baslatir, bitince onComplete cagrilir
    }
}
