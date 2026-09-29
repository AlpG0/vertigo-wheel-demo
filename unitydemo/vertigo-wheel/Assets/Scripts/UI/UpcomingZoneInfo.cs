using UnityEngine;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Sagdaki "ALTIN ÇEVİRME 30" gibi bir rozetin gostermesi gereken bilgi.
    /// </summary>
    public readonly struct UpcomingZoneInfo
    {
        private readonly string title;
        private readonly int zoneNumber;
        private readonly Color color;
        private readonly Sprite icon;

        public UpcomingZoneInfo(string title, int zoneNumber, Color color, Sprite icon)
        {
            this.title = title;
            this.zoneNumber = zoneNumber;
            this.color = color;
            this.icon = icon;
        }

        public string Title { get { return title; } }
        public int ZoneNumber { get { return zoneNumber; } }
        public Color Color { get { return color; } }
        public Sprite Icon { get { return icon; } }
    }
}
