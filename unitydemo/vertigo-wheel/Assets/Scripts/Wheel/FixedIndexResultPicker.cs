using VertigoWheel.Data; // WheelConfig, WheelSegmentData burda

namespace VertigoWheel.Wheel
{
    /// <summary>
    /// IWheelResultPicker'i her zaman ayni index'teki segmenti dondurerek uygulayan sinif.
    /// Rastgele degil, sabit bir sonuc verir - test/QA senaryolarinda belirli bir sonucu tekrar tekrar uretmek icin kullanilir.
    /// </summary>
    public class FixedIndexResultPicker : IWheelResultPicker // ayni arayuzu RandomWheelResultPicker ile paylasiyor, GameManager hangisini kullandigini bilmeden calisir
    {
        private readonly int fixedIndex; // her zaman donecegimiz segmentin index'i

        public FixedIndexResultPicker(int fixedIndex) // disaridan hangi index sabitlenecekse onu aliyoruz
        {
            this.fixedIndex = fixedIndex;
        }

        public WheelSegmentData PickSegment(WheelConfig config) // arayuzdeki imzayla ayni olmak zorunda
        {
            return config.Segments[fixedIndex]; // rastgele secmek yerine hep ayni index'i donduruyoruz
        }
    }
}
