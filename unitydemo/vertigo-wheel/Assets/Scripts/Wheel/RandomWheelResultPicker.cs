using UnityEngine; // Random burda tanımlı
using VertigoWheel.Data; // WheelConfig, WheelSegmentData burda

namespace VertigoWheel.Wheel //monobehavior degil cunku sahnede bir objeye bağlı değil, sadece bir class. Bu yüzden namespace ile ayırıyoruz. Wheel ile ilgili sınıflar burada olacak.
{
    /// <summary>
    /// IWheelResultPicker'i rastgele secim yaparak uygulayan sinif.
    /// </summary>
    public class RandomWheelResultPicker : IWheelResultPicker // arayüzü uyguluyoruz, bu sınıfın PickSegment metodunu implement etmesi lazım.
    {
        public WheelSegmentData PickSegment(WheelConfig config) // arayüzdeki imzayla aynı olmak zorunda
        {
            int randomIndex = Random.Range(0, config.Segments.Count); // 0 ile segment sayısı arasında rastgele bir sayı döndürür. Random.Range üst sınırı dahil etmez, yani 0 ile Count-1 arasında bir sayı döndürür.
            return config.Segments[randomIndex]; // o index'teki segmenti döndürüyoruz. Bu segment, sahnedeki objeye bağlı değil, ScriptableObject'te duruyor.
        }
    }
}