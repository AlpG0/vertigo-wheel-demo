using System; // Action burda

namespace VertigoWheel.Wheel
{
    /// <summary>
    /// Wheel'i dondurup kazanan segmente hizalayan siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IWheelSpinAnimator
    {
        void SpinTo(int segmentIndex, int segmentCount, Action onComplete); // wheel'i dondurur, bitince onComplete cagrilir
    }
}
