using UnityEngine;
using UnityEngine.UI;
using VertigoWheel.Utils; // HierarchyLookup, GameConstants burda

namespace VertigoWheel.Wheel
{
    /// <summary>
    /// Wheel'in govde ve indicator gorselini kendisine verilen sprite'larla degistiren sinif.
    /// Hangi zone'da hangi sprite kullanilacagini bilmez, bu karar ZoneDefinition asset'lerinde.
    /// </summary>
    public class WheelThemeView : MonoBehaviour, IWheelThemeView // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour; IWheelThemeView'i implement ediyor
    {
        [SerializeField] private Image wheelBaseImage; // wheel'in govde gorseli
        [SerializeField] private Image indicatorImage; // wheel'in indicator gorseli

        private void OnValidate() // referanslari elle suruklemeyelim diye otomatik bul
        {
            wheelBaseImage = HierarchyLookup.FindByName<Image>(this, GameConstants.UINames.WheelBaseImage); // yol yerine isim: obje baska bir parent'a tasinsa da bulur
            indicatorImage = HierarchyLookup.FindByName<Image>(this, GameConstants.UINames.WheelIndicatorImage);
        }

        public void ApplyTheme(Sprite baseSprite, Sprite indicatorSprite) // disaridan hangi sprite'lar verilirse onlari uygular
        {
            wheelBaseImage.sprite = baseSprite; // govde gorselini degistir
            indicatorImage.sprite = indicatorSprite; // indicator gorselini degistir
        }
    }
}
