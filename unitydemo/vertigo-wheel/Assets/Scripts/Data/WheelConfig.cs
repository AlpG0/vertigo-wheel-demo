using System.Collections.Generic; // List burda tanımlı, o yüzden lazım
using UnityEngine; // ScriptableObject ve SerializeField burda

namespace VertigoWheel.Data // aynı data grubu
{
    [CreateAssetMenu(fileName = "WheelConfig", menuName = "VertigoWheel/Wheel Config")] // Project panelinde sağ tık > Create menüsüne bunu ekliyor
    /// <summary>
    /// Bir wheel'in butun dilimlerini tutan, Editor'den duzenlenebilir asset.
    /// </summary>
    public class WheelConfig : ScriptableObject // asset olarak duracak, sahnedeki bir objeye bağlı değil
    {
        [SerializeField] private List<WheelSegmentData> segments; // bu wheel'in bütün dilimleri, sıraysiyla, inspector'da görünmesi için SerializeField ile işaretledik

        public List<WheelSegmentData> Segments // dışarıdan okunabilsin diye
        {
            get { return segments; } // sadece okuma, değiştirme yok. Segments listesi ScriptableObject'te duruyor, sahnedeki objeye bağlı değil. Bu yüzden sahnedeki objeden değiştirilmemesi lazım.
        }
    }
}