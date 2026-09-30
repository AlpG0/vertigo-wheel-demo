using UnityEngine;

namespace VertigoWheel.Utils
{
    /// <summary>
    /// View'larin OnValidate'te kendi cocuklarini isimle bulmasi icin ortak yardimci.
    /// Isim sahnede bulunamazsa sessizce null donmek yerine uyari verir, boylece kod ile sahne ayri dustugunde hemen fark edilir.
    /// </summary>
    public static class HierarchyLookup
    {
        public static T FindByName<T>(Component root, string objectName) where T : Component // root altinda objectName isimli T turunden bir obje bulur, bulamazsa null doner ve uyari verir
        {
            T[] candidates = root.GetComponentsInChildren<T>(true); // true: pasif objeler de dahil (popup gibi)

            foreach (T candidate in candidates) // sahnedeki obje ismi GameConstants.UINames ile uyusmuyor olabilir, bu durumda null doner ve uyari verir
            {
                if (candidate.gameObject.name == objectName)
                {
                    return candidate;
                }
            }

            Debug.LogWarning(string.Format("'{0}' altinda '{1}' isimli {2} bulunamadi. Sahnedeki obje ismi GameConstants.UINames ile uyusmuyor olabilir.", root.name, objectName, typeof(T).Name), root);
            return null;
        }
    }
}
