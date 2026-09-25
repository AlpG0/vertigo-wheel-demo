using UnityEngine;

namespace VertigoWheel.Data
{
    /// <summary>
    /// Bir zone turune (Normal/Safe/Super) ait tum gorsel ve veri referanslarini bir arada tutar.
    /// </summary>
    [System.Serializable] // Unity Inspector'da gorunmesi icin
    public class ZonePreset
    {
        [SerializeField] private ZoneType zoneType; // hangi zone turu (Normal/Safe/Super)
        [SerializeField] private string spinTitle; // spin ekraninda gosterilecek baslik, ornegin "NORMAL SPIN"
        [SerializeField] private WheelConfig wheelConfig; // bu zone turu icin kullanilacak wheel config, segment sayisi, segmentler, segmentlerin yuzdesi vs.
        [SerializeField] private Sprite wheelBaseSprite; // wheel'in arka plan gorseli, segmentlerin uzerinde duracak
        [SerializeField] private Sprite indicatorSprite; // wheel'in ustunde donen ok gorseli, hangi segmentin secildigini gosterecek

        public ZoneType ZoneType { get { return zoneType; } } // zone turunu disariya ac, enum tipinde
        public string SpinTitle { get { return spinTitle; } } // spin ekraninda gosterilecek basligi disariya ac, string tipinde
        public WheelConfig WheelConfig { get { return wheelConfig; } } // wheel config'i disariya ac, WheelConfig tipinde
        public Sprite WheelBaseSprite { get { return wheelBaseSprite; } } // wheel'in arka plan gorselini disariya ac, Sprite tipinde
        public Sprite IndicatorSprite { get { return indicatorSprite; } } // wheel'in ustunde donen ok gorselini disariya ac, Sprite tipinde
    }
}