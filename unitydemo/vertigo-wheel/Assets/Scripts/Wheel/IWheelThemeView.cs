using UnityEngine; // Sprite burda

namespace VertigoWheel.Wheel
{
    /// <summary>
    /// Wheel'in govde/indicator gorselini degistiren siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IWheelThemeView
    {
        void ApplyTheme(Sprite baseSprite, Sprite indicatorSprite); // verilen sprite'lari uygular
    }
}
