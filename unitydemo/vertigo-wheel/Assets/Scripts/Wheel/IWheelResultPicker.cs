using VertigoWheel.Data; // WheelConfig ve WheelSegmentData burada tanımlı

namespace VertigoWheel.Wheel // wheel ile ilgili sınıflar burada olacak
{
    /// <summary>
    /// Wheel spin sonucunda hangi segmentin kazanacagini secen siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IWheelResultPicker // arayüz: sadece imza var, içi boş. Arayuz, bir sınıfın hangi metodları içermesi gerektiğini belirler. Bu sayede farklı sınıflar aynı arayüzü implement edebilir ve aynı metodları kullanabilir.
    {
        WheelSegmentData PickSegment(WheelConfig config); // hangi segmentin kazanacağını döndürür
    }
}