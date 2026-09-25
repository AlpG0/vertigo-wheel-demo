using UnityEngine; // Vector3 burda
using VertigoWheel.Data; // WheelConfig burda

namespace VertigoWheel.Wheel
{
    /// <summary>
    /// Wheel'in segmentlerini gosteren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IWheelView
    {
        void ShowConfig(WheelConfig config); // verilen config'e gore segmentleri gosterir
        Vector3 GetSegmentWorldPosition(int index); // belirtilen segmentin ekrandaki konumunu dondurur
    }
}
