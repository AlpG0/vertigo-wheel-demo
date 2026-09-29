using VertigoWheel.Data; // WheelConfig, WheelSegmentData burda

namespace VertigoWheel.Wheel
{
    /// <summary>
    /// IWheelResultPicker'i her zaman ayni index'teki segmenti dondurerek uygulayan sinif.
    /// Rastgele degil, sabit bir sonuc verir - test/QA senaryolarinda belirli bir sonucu tekrar tekrar uretmek icin kullanilir.
    /// </summary>
    public class FixedIndexResultPicker : IWheelResultPicker // ayni arayuzu RandomWheelResultPicker ile paylasiyor, oyun akisi hangisini kullandigini bilmeden calisir
    {
        private readonly int fixedIndex; // her zaman donecegimiz segmentin index'i

        public FixedIndexResultPicker(int fixedIndex) // disaridan hangi index sabitlenecekse onu aliyoruz
        {
            this.fixedIndex = fixedIndex;
        }

        public WheelSegmentData PickSegment(WheelConfig config) // arayuzdeki imzayla ayni olmak zorunda
        {
            int safeIndex = fixedIndex % config.Segments.Count; // index dilim sayisindan buyuk verilirse tasmasin
            return config.Segments[safeIndex]; // rastgele secmek yerine hep ayni index'i donduruyoruz
        }
    }
}
