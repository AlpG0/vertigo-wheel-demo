using System.Collections.Generic;
using UnityEngine;
using VertigoWheel.UI;
using VertigoWheel.Utils;
using VertigoWheel.Zone;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Su anki zone'a gore ekrani tek yerden gunceller: alttaki baslik, wheel dilimleri ve tema, ustteki zone cubugu, sagdaki rozetler.
    /// </summary>
    public class ZonePresenter
    {
        private readonly IZoneService zoneService;
        private readonly IPlayerRunState runState;
        private readonly GameViews views;
        private readonly List<Color> zoneColors = new List<Color>(); // her Present'te yeniden liste olusturmayalim diye tekrar kullaniyoruz
        private readonly List<UpcomingZoneInfo> upcomingZones = new List<UpcomingZoneInfo>();

        public ZonePresenter(IZoneService zoneService, IPlayerRunState runState, GameViews views)
        {
            this.zoneService = zoneService;
            this.runState = runState;
            this.views = views;
        }

        public void Present(bool animate) // animate: zone cubugu kayarak mi gecsin (oyun basinda aninda)
        {
            int currentZone = runState.CurrentZone;
            ZoneDefinition zone = zoneService.GetZone(currentZone);

            views.Hud.SetZoneInfo(zone.Title, zone.Subtitle);
            views.Wheel.ShowConfig(zone.WheelConfig);
            views.WheelTheme.ApplyTheme(zone.WheelBaseSprite, zone.IndicatorSprite);
            views.ZoneBar.ShowZone(currentZone, BuildZoneColors(currentZone + GameConstants.ZoneBarLookahead), animate);
            views.UpcomingZones.Show(BuildUpcomingZones(currentZone));
        }

        private List<Color> BuildZoneColors(int lastZone) // 1'den lastZone'a kadar her zone'un cubuktaki rengi
        {
            zoneColors.Clear();
            for (int zoneNumber = 1; zoneNumber <= lastZone; zoneNumber++)
            {
                zoneColors.Add(zoneService.GetZone(zoneNumber).ThemeColor);
            }

            return zoneColors;
        }

        private List<UpcomingZoneInfo> BuildUpcomingZones(int currentZone) // her ozel zone turu (araligi 1'den buyuk) icin siradaki numara
        {
            upcomingZones.Clear();
            foreach (ZoneDefinition zone in zoneService.Zones)
            {
                if (zone.Interval <= 1) // her zone gelen varsayilan tur, rozeti olmaz
                {
                    continue;
                }

                int nextZone = zoneService.GetNextZoneNumber(zone, currentZone);
                upcomingZones.Add(new UpcomingZoneInfo(zone.Title, nextZone, zone.ThemeColor, zone.WheelBaseSprite));
            }

            return upcomingZones;
        }
    }
}
