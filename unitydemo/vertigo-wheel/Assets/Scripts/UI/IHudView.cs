namespace VertigoWheel.UI
{
    /// <summary>
    /// Ekranin altindaki zone basligi ve alt yazisini gosteren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IHudView
    {
        void SetZoneInfo(string title, string subtitle); // ornegin "GÜMÜŞ ÇEVİRME" / "DAHA İYİ ÖDÜLLER - BOMBA RİSKİ YOK"
    }
}
