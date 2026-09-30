using UnityEngine;
using VertigoWheel.Data; // WheelConfig burda

namespace VertigoWheel.Zone
{
    /// <summary>
    /// Bir zone turunun (bronz/gumus/altin...) butun kurali ve gorunumu. Yeni bir zone turu eklemek kod degil, yeni bir asset demek.
    /// </summary>
    [CreateAssetMenu(fileName = "Zone", menuName = "VertigoWheel/Zone Definition")]
    public class ZoneDefinition : ScriptableObject
    {
        [SerializeField] private string title; // ornegin "GÜMÜŞ ÇEVİRME"
        [SerializeField] private string subtitle; // ornegin "DAHA İYİ ÖDÜLLER - BOMBA RİSKİ YOK"
        [SerializeField] private int interval = 1; // her kacinci zone'da gelir: 30, 5... 1 = varsayilan (her zone)
        [SerializeField] private bool canLeave; // bu zone'da cikis yapilabilir mi
        [SerializeField] private WheelConfig wheelConfig; // bu zone'da hangi dilimler var
        [SerializeField] private Sprite wheelBaseSprite; // wheel govdesi
        [SerializeField] private Sprite indicatorSprite; // wheel ustundeki ok
        [SerializeField] private Color themeColor = Color.white; // zone cubugundaki numara ve yan paneldeki rozet rengi

        public string Title { get { return title; } }
        public string Subtitle { get { return subtitle; } }
        public int Interval { get { return Mathf.Max(1, interval); } } // 0 girilirse bolme hatasi olmasin diye en az 1
        public bool CanLeave { get { return canLeave; } }
        public WheelConfig WheelConfig { get { return wheelConfig; } }
        public Sprite WheelBaseSprite { get { return wheelBaseSprite; } }
        public Sprite IndicatorSprite { get { return indicatorSprite; } }
        public Color ThemeColor { get { return themeColor; } }
    }
}
