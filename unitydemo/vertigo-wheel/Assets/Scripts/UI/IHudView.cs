using UnityEngine; // Vector3 burda

namespace VertigoWheel.UI
{
    /// <summary>
    /// Ekrandaki zone ve toplam odul yazilarini gunceleyen siniflarin uymasi gereken sozlesme.
    /// </summary>
    public interface IHudView
    {
        void SetSpinTitle(string title); // ust basligi gunceller
        void SetTotal(int total); // toplam odulu gunceller
        Vector3 GetTotalWorldPosition(); // TOTAL dairesinin ekrandaki konumunu dondurur
    }
}
