using UnityEngine;
using UnityEngine.UI;
using VertigoWheel.Data; // ZoneType burda

namespace VertigoWheel.Wheel
{
    /// <summary>
    /// Wheel'in govde ve indicator gorselini zone turune gore (bronze/silver/golden) degistiren sinif.
    /// </summary>
    public class WheelThemeView : MonoBehaviour // sahnedeki bir objeye eklenecek, o yüzden MonoBehaviour
    {
        [SerializeField] private Image wheelBaseImage; // wheel'in govde gorseli
        [SerializeField] private Image indicatorImage; // wheel'in indicator gorseli

        [SerializeField] private Sprite bronzeBaseSprite; // normal zone govdesi
        [SerializeField] private Sprite bronzeIndicatorSprite; // normal zone indicator'u
        [SerializeField] private Sprite silverBaseSprite; // safe zone govdesi
        [SerializeField] private Sprite silverIndicatorSprite; // safe zone indicator'u
        [SerializeField] private Sprite goldenBaseSprite; // super zone govdesi
        [SerializeField] private Sprite goldenIndicatorSprite; // super zone indicator'u

        private void OnValidate() // referanslari elle suruklemeyelim diye otomatik bul
        {
            wheelBaseImage = transform.Find("ui_panel_segments/ui_image_wheel_base").GetComponent<Image>(); // hiyerarsideki belirli bir yolu bulur
            indicatorImage = transform.Find("ui_image_wheel_indicator").GetComponent<Image>(); // direkt cocugu bulur
        }

        public void ApplyZoneType(ZoneType zoneType) // zone turune gore dogru gorselleri uygular
        {
            if (zoneType == ZoneType.Super) // super zone ise
            {
                wheelBaseImage.sprite = goldenBaseSprite; // govdeyi golden yap
                indicatorImage.sprite = goldenIndicatorSprite; // indicator'u golden yap
                return; // burada bitir, altina inme
            }

            if (zoneType == ZoneType.Safe) // safe zone ise
            {
                wheelBaseImage.sprite = silverBaseSprite; // govdeyi silver yap
                indicatorImage.sprite = silverIndicatorSprite; // indicator'u silver yap
                return; // burada bitir
            }

            wheelBaseImage.sprite = bronzeBaseSprite; // ikisi de degilse normal, bronze yap
            indicatorImage.sprite = bronzeIndicatorSprite;
        }
    }
}