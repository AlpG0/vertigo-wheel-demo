using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoWheel.Utils;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Sol listedeki tek bir odul satiri (ikon + miktar). Miktar degisince sayarak artar ve satir kisa bir "pop" yapar.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class RewardRowView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private PunchScaleAnimator punch;
        [SerializeField] private CanvasGroup canvasGroup; // ikon ucarken satir yeri hazir ama gorunmez dursun diye
        [SerializeField] private float countDuration = 0.4f; // sayinin artma suresi

        private int shownAmount;
        private Coroutine countRoutine;

        public Vector3 IconWorldPosition { get { return icon.transform.position; } } // ucan ikonun inecegi yer

        private void OnValidate()
        {
            icon = HierarchyLookup.FindByName<Image>(this, GameConstants.UINames.RewardRowIcon);
            amountText = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.RewardRowAmount);
            punch = GetComponent<PunchScaleAnimator>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Setup(Sprite sprite) // yeni satir: ikonu koy, odul inene kadar gizli kalsin
        {
            icon.sprite = sprite;
            shownAmount = 0;
            amountText.text = string.Empty;
            canvasGroup.alpha = 0f;
        }

        public void SetAmount(int amount)
        {
            canvasGroup.alpha = 1f;

            if (countRoutine != null)
            {
                StopCoroutine(countRoutine);
            }

            countRoutine = StartCoroutine(CountTo(amount));
            punch.Play();
        }

        private IEnumerator CountTo(int target)
        {
            int start = shownAmount;
            float elapsed = 0f;

            while (elapsed < countDuration)
            {
                elapsed += Time.deltaTime;
                float t = Easing.EaseOutCubic(Mathf.Clamp01(elapsed / countDuration));
                shownAmount = Mathf.RoundToInt(Mathf.Lerp(start, target, t));
                amountText.text = NumberFormatter.FormatAmount(shownAmount);
                yield return null;
            }

            shownAmount = target;
            amountText.text = NumberFormatter.FormatAmount(target);
            countRoutine = null;
        }
    }
}
