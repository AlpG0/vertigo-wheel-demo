using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VertigoWheel.Utils;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Ustteki zone cubugu: numaralar yan yana dizili, mevcut zone ortadaki kutunun icinde durur.
    /// Zone degisince cubuk kayarak yeni zone'u kutuya getirir, kutu kucuk bir "pop" yapar.
    /// </summary>
    public class ZoneBarView : MonoBehaviour, IZoneBarView
    {
        [SerializeField] private RectTransform content; // numaralarin dizildigi, kayan kisim
        [SerializeField] private TMP_Text cellTemplate; // her numara bundan kopyalanir, kendisi pasif durur
        [SerializeField] private PunchScaleAnimator currentFramePunch; // ortadaki mevcut zone kutusu
        [SerializeField] private float cellWidth = 80f; // iki numara arasi mesafe
        [SerializeField] private float slideDuration = 0.35f; // kayma animasyonu suresi
        [SerializeField] private float passedZoneAlpha = 0.35f; // gecilmis zone'lar soluk gorunsun

        private readonly List<TMP_Text> cells = new List<TMP_Text>(); // numara hucresinin kopyalari
        private Coroutine slideRoutine; // kayma animasyonu coroutine'i, birden fazla animasyon baslamasini engellemek icin tutuluyor

        private void OnValidate() // inspector'da degisiklik yapildiginda, sahnede gorunmesi icin
        {
            content = HierarchyLookup.FindByName<RectTransform>(this, GameConstants.UINames.ZoneBarContent);
            cellTemplate = HierarchyLookup.FindByName<TMP_Text>(this, GameConstants.UINames.ZoneBarCellTemplate);
            currentFramePunch = GetComponentInChildren<PunchScaleAnimator>(true);
        }

        public void ShowZone(int currentZone, IReadOnlyList<Color> zoneColors, bool animate) // mevcut zone'u ve zone renklerini goster, kaydirarak animasyon yap
        {
            EnsureCells(zoneColors.Count);

            for (int i = 0; i < cells.Count; i++)
            {
                int zoneNumber = i + 1;
                Color color = i < zoneColors.Count ? zoneColors[i] : Color.white;
                if (zoneNumber < currentZone) // gecilen zone'lar soluk
                {
                    color.a = passedZoneAlpha;
                }

                cells[i].color = color;
            }

            float targetX = -(currentZone - 1) * cellWidth; // mevcut zone'un hucresi tam ortaya gelsin

            if (slideRoutine != null)
            {
                StopCoroutine(slideRoutine);
                slideRoutine = null;
            }

            if (animate && isActiveAndEnabled)
            {
                slideRoutine = StartCoroutine(SlideTo(targetX));
            }
            else
            {
                content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
            }
        }

        private void EnsureCells(int count) // gereken kadar numara hucresi yoksa olustur
        {
            while (cells.Count < count)
            {
                int zoneNumber = cells.Count + 1;
                TMP_Text cell = Instantiate(cellTemplate, content);
                cell.name = GameConstants.UINames.ZoneBarCellPrefix + zoneNumber;
                cell.text = NumberFormatter.FormatAmount(zoneNumber);
                cell.rectTransform.anchoredPosition = new Vector2((zoneNumber - 1) * cellWidth, 0f);
                cell.gameObject.SetActive(true);
                cells.Add(cell);
            }
        }

        private IEnumerator SlideTo(float targetX) // mevcut zone kutusunu kaydirarak yeni zone'a getir
        {
            float startX = content.anchoredPosition.x;
            float elapsed = 0f;

            while (elapsed < slideDuration)
            {
                elapsed += Time.deltaTime;
                float t = Easing.EaseOutCubic(Mathf.Clamp01(elapsed / slideDuration));
                content.anchoredPosition = new Vector2(Mathf.Lerp(startX, targetX, t), content.anchoredPosition.y);
                yield return null;
            }

            content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
            currentFramePunch.Play(); // yeni zone kutuya oturdu
            slideRoutine = null;
        }
    }
}
