using UnityEngine;
using UnityEngine.UI;

namespace VertigoWheel.Wheel
{
    /// <summary>
    /// Wheel'in govde ve indicator gorselini kendisine verilen sprite'larla degistiren sinif.
    /// Hangi zone'da hangi sprite kullanilacagini bilmez, bu karar artik GameManager'daki ZonePreset'te.
    /// </summary>
    public class WheelThemeView : MonoBehaviour, IWheelThemeView // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour; IWheelThemeView'i implement ediyor
    {
        [SerializeField] private Image wheelBaseImage; // wheel'in govde gorseli
        [SerializeField] private Image indicatorImage; // wheel'in indicator gorseli

        private void OnValidate() // referanslari elle suruklemeyelim diye otomatik bul
        {
            wheelBaseImage = transform.Find("ui_panel_segments/ui_image_wheel_base").GetComponent<Image>(); // hiyerarsideki belirli bir yolu bulur
            indicatorImage = transform.Find("ui_image_wheel_indicator").GetComponent<Image>(); // direkt cocugu bulur
        }

        public void ApplyTheme(Sprite baseSprite, Sprite indicatorSprite) // disaridan hangi sprite'lar verilirse onlari uygular
        {
            wheelBaseImage.sprite = baseSprite; // govde gorselini degistir
            indicatorImage.sprite = indicatorSprite; // indicator gorselini degistir
        }
    }
}