using VertigoWheel.Data;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Bir spin'in sonucu: hangi dilim kazandi ve wheel'deki sirasi.
    /// </summary>
    public readonly struct SpinResult // hangi dilim kazandi ve wheel'deki sirasi
    {
        private readonly WheelSegmentData segment; // kazanan dilim
        private readonly int segmentIndex; // kazanan dilimin wheel'deki sirasi

        public SpinResult(WheelSegmentData segment, int segmentIndex) // kazanan dilim ve wheel'deki sirasi
        {
            this.segment = segment;
            this.segmentIndex = segmentIndex;
        }

        public WheelSegmentData Segment { get { return segment; } } // kazanan dilim
        public int SegmentIndex { get { return segmentIndex; } } // kazanan dilimin wheel'deki sirasi
    }
}