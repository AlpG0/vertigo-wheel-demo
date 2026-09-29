using VertigoWheel.Rewards;
using VertigoWheel.Zone;

namespace VertigoWheel.Core
{
    /// <summary>
    /// Su anki zone'a gore ekrani (baslik, wheel dilimleri, tema) tek yerden gunceller.
    /// </summary>
    public class ZonePresenter
    {
        private readonly IZoneService zoneService;
        private readonly IPlayerRunState runState;
        private readonly IRunRewards runRewards;
        private readonly GameViews views;

        public ZonePresenter(IZoneService zoneService, IPlayerRunState runState, IRunRewards runRewards, GameViews views)
        {
            this.zoneService = zoneService;
            this.runState = runState;
            this.runRewards = runRewards;
            this.views = views;
        }

        public void Present()
        {
            ZoneDefinition zone = zoneService.GetZone(runState.CurrentZone);

            views.Hud.SetSpinTitle(zone.Title);
            views.Hud.SetTotal(runRewards.CollectedRewards.Count); // gecici: yeni layout'ta sol odul listesi gelecek
            views.Wheel.ShowConfig(zone.WheelConfig);
            views.WheelTheme.ApplyTheme(zone.WheelBaseSprite, zone.IndicatorSprite);
        }
    }
}
