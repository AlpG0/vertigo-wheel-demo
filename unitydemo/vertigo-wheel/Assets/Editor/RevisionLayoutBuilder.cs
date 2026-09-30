using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using VertigoWheel.Rewards;
using VertigoWheel.UI;
using VertigoWheel.Utils;

namespace VertigoWheel.EditorTools
{
    /// <summary>
    /// Revizyondaki yeni layout'u (zone cubugu, sol odul paneli, sag rozetler, cuzdan, alt baslik, wheel ortasinda SPIN)
    /// acik sahnede Unity API'si ile kurar. Tek seferlik gecis araci: tum islemler Undo'ya kaydedilir, tekrar calistirilirsa once eskisini siler.
    /// </summary>
    public static class RevisionLayoutBuilder
    {
        private const string UndoName = "Build Revision Layout";
        private const string ImagesFolder = "Assets/images/";

        private static readonly Vector2 Center = new Vector2(0.5f, 0.5f);

        [MenuItem("Tools/Vertigo/Build Revision Layout")]
        public static void Build()
        {
            GameObject rootObject = GameObject.Find("ui_panel_root");
            if (rootObject == null)
            {
                Debug.LogError("ui_panel_root bulunamadi.");
                return;
            }

            RectTransform root = (RectTransform)rootObject.transform;
            TMP_FontAsset font = FindFont(root);
            RectTransform wheelContainer = FindDeep(root, "ui_panel_wheel_container");
            RectTransform popup = FindDeep(root, "ui_panel_reward_popup");
            RectTransform travel = FindDeep(root, "ui_image_reward_travel");

            RemoveOldLayout(root, wheelContainer);

            RectTransform background = BuildBackground(root);
            BuildWheel(wheelContainer);
            BuildZoneBar(root, font);
            BuildExitPanel(root, font);
            BuildUpcomingZones(root, font);
            BuildZoneInfo(root, font);
            BuildWallet(root, font);
            BuildPopupDim(popup);

            if (rootObject.GetComponent<ActionButtonsView>() == null) // butonlar artik farkli panellerde, ikisinin ortak ustu root
            {
                Undo.AddComponent<ActionButtonsView>(rootObject);
            }

            background.SetAsFirstSibling(); // en arkada
            popup.parent.SetAsLastSibling(); // popup ve ucan ikon her seyin ustunde
            travel.SetAsLastSibling();

            RunOnValidate(root);
            EditorSceneManager.MarkSceneDirty(rootObject.scene);
            Debug.Log("Revizyon layout'u kuruldu. Kontrol edip File > Save ile kaydet.");
        }

        private static void RemoveOldLayout(RectTransform root, RectTransform wheelContainer)
        {
            Undo.SetTransformParent(wheelContainer, root, UndoName); // wheel eski sarmalayicisindan cikip dogrudan root'a baglansin

            DestroyIfExists(root, "ui_panel_card_frame");
            DestroyWrapperOf(root, "ui_panel_header");
            DestroyWrapperOf(root, "ui_panel_actions");
            DestroyWrapperOf(root, "ui_panel_wheel");
            DestroyIfExists(root, "ui_image_total_circle");
            DestroyIfExists(root, "ui_text_total_value");
            DestroyIfExists(root, "ui_text_max_reward_value");

            string[] builtBefore = { "ui_image_background", "ui_panel_zone_bar", "ui_panel_exit", "ui_panel_upcoming_zones", "ui_panel_zone_info", "ui_panel_wallet", "ui_image_popup_dim", GameConstants.UINames.SpinButton };
            foreach (string name in builtBefore) // arac tekrar calistirilirsa once kendi urettiklerini siler
            {
                DestroyIfExists(root, name);
            }
        }

        private static RectTransform BuildBackground(RectTransform root)
        {
            RectTransform background = CreateStretch("ui_image_background", root, 0f);
            AddImage(background, "ui_background_corridor.png", Color.white, Image.Type.Simple);
            return background;
        }

        private static void BuildWheel(RectTransform wheelContainer)
        {
            wheelContainer.anchorMin = Center;
            wheelContainer.anchorMax = Center;
            wheelContainer.pivot = Center;
            wheelContainer.anchoredPosition = new Vector2(0f, 20f);
            wheelContainer.localScale = Vector3.one * 1.1f;

            AlignSegmentsToPockets(wheelContainer);

            RectTransform spin = CreateRect(GameConstants.UINames.SpinButton, wheelContainer, Center, Center, Center, Vector2.zero, new Vector2(150f, 150f));
            Image spinImage = AddImage(spin, "ui_spin_generic_button.png", Color.white, Image.Type.Simple, true);
            spinImage.raycastTarget = true;
            AddButton(spin, spinImage); // wheel'in donen kismina (segments) degil container'a bagli, o yuzden SPIN donmez
            spin.SetAsLastSibling();
        }

        [MenuItem("Tools/Vertigo/Align Wheel Segments")]
        public static void AlignWheelSegments() // sadece dilimleri yeniden hizalamak icin, tum layout'u tekrar kurmadan
        {
            GameObject rootObject = GameObject.Find("ui_panel_root");
            if (rootObject == null)
            {
                Debug.LogError("ui_panel_root bulunamadi.");
                return;
            }

            AlignSegmentsToPockets(FindDeep(rootObject.transform, "ui_panel_wheel_container"));
            EditorSceneManager.MarkSceneDirty(rootObject.scene);
            Debug.Log("Wheel dilimleri yuvalara hizalandi. File > Save ile kaydet.");
        }

        [MenuItem("Tools/Vertigo/Update Bomb Popup")]
        public static void UpdateBombPopup() // popup butonlarina ikon ekler ve metinleri ekranin geri kalaniyla ayni dile (Turkce) getirir
        {
            GameObject rootObject = GameObject.Find("ui_panel_root");
            if (rootObject == null)
            {
                Debug.LogError("ui_panel_root bulunamadi.");
                return;
            }

            RectTransform popup = FindDeep(rootObject.transform, "ui_panel_reward_popup");

            SetText(popup, "ui_text_bomb_headline", "EYVAH, BOMBA ELİNDE PATLADI!");
            SetupPopupButton(popup, GameConstants.UINames.PopupGiveUpButton, "VAZGEÇ", "ui_icon_give_up", "ui_icon_trash.png", new Vector2(26f, 30f));
            SetupPopupButton(popup, GameConstants.UINames.PopupGoldReviveButton, "25 CANLAN", "ui_icon_gold_revive", "UI_icon_gold.png", new Vector2(34f, 30f));
            SetupPopupButton(popup, GameConstants.UINames.PopupAdReviveButton, "CANLAN", "ui_icon_ad_revive", "ui_icon_video.png", new Vector2(36f, 27f));

            EditorSceneManager.MarkSceneDirty(rootObject.scene);
            Debug.Log("Bomba popup'i guncellendi. File > Save ile kaydet.");
        }

        private static void SetupPopupButton(RectTransform popup, string buttonName, string label, string iconName, string iconFile, Vector2 iconSize)
        {
            RectTransform button = FindDeep(popup, buttonName);

            RectTransform icon = FindDeep(button, iconName);
            if (icon == null) // ilk calistirmada olustur, tekrar calistirilirsa var olani kullan
            {
                icon = CreateRect(iconName, button, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, iconSize);
                AddImage(icon, iconFile, Color.white, Image.Type.Simple, true);
            }

            Undo.RecordObject(icon, UndoName);
            icon.anchorMin = new Vector2(0f, 0.5f);
            icon.anchorMax = new Vector2(0f, 0.5f);
            icon.pivot = Center;
            icon.anchoredPosition = new Vector2(30f, 0f);
            icon.sizeDelta = iconSize;
            icon.SetAsLastSibling();

            TMP_Text text = button.GetComponentInChildren<TMP_Text>(true);
            RectTransform textRect = text.rectTransform;
            Undo.RecordObject(text, UndoName);
            Undo.RecordObject(textRect, UndoName);
            text.text = label;
            text.color = Color.white; // mavi/yesil/gri zeminde beyaz yazi ikonlarla ayni tonda ve daha okunakli
            text.fontSize = 22f;
            text.fontStyle = FontStyles.Bold;
            text.alignment = TextAlignmentOptions.Center;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(52f, 0f); // ikonun sagindan baslasin
            textRect.offsetMax = new Vector2(-8f, 0f);
        }

        private static void SetText(Transform root, string objectName, string value)
        {
            TMP_Text text = FindDeep(root, objectName).GetComponent<TMP_Text>();
            Undo.RecordObject(text, UndoName);
            text.text = value;
        }

        // Wheel gorselinden (485px) olculen degerler: yuva merkezleri merkezden 143px uzakta, yuva ici ~77px.
        // Gorsel sahnede 450 birim cizildigi icin: yaricap ~133, yuva ~71 birim.
        private const float PocketRadius = 133f;
        private const float IconOutwardOffset = 7f; // ikon yuvanin dis tarafina biraz kaysin, alta yaziya yer kalsin
        private const float LabelInwardOffset = 21f; // "xN" yazisi yuvanin merkeze bakan tarafinda
        private const float IconSize = 42f;
        private const int SegmentCount = 8;

        private static void AlignSegmentsToPockets(RectTransform wheelContainer)
        {
            for (int i = 0; i < SegmentCount; i++)
            {
                float angle = i * (GameConstants.FullRotationDegrees / SegmentCount); // saat yonunde, tepeden baslayarak (WheelSpinAnimator ile ayni sira)
                float radians = angle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
                Quaternion facing = Quaternion.Euler(0f, 0f, -angle); // yuvanin icerigi merkeze donuk dursun, referanstaki gibi

                string number = (i + 1).ToString("00");
                RectTransform icon = FindDeep(wheelContainer, "ui_segment_" + number);
                RectTransform label = FindDeep(wheelContainer, "ui_text_segment_" + number + "_value");

                Undo.RecordObject(icon, UndoName);
                icon.anchoredPosition = direction * (PocketRadius + IconOutwardOffset);
                icon.localRotation = facing;
                icon.sizeDelta = new Vector2(IconSize, IconSize);

                TMP_Text labelText = label.GetComponent<TMP_Text>();
                Undo.RecordObject(label, UndoName);
                Undo.RecordObject(labelText, UndoName);
                label.anchoredPosition = direction * (PocketRadius - LabelInwardOffset);
                label.localRotation = facing;
                label.sizeDelta = new Vector2(60f, 18f);
                labelText.fontSize = 15f;
                labelText.fontStyle = FontStyles.Bold;
                labelText.alignment = TextAlignmentOptions.Center;
                labelText.textWrappingMode = TextWrappingModes.NoWrap; // "x2.000" tek satirda kalsin
            }
        }

        private static void BuildZoneBar(RectTransform root, TMP_FontAsset font)
        {
            RectTransform bar = CreateRect("ui_panel_zone_bar", root, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -34f), new Vector2(960f, 84f));
            AddImage(CreateStretch("ui_image_zone_bar_bg", bar, 0f), "ui_card_panel_zone_bg.png", Color.white, Image.Type.Sliced);

            RectTransform current = CreateRect("ui_image_zone_bar_current", bar, Center, Center, Center, Vector2.zero, new Vector2(84f, 84f));
            AddImage(current, "ui_card_panel_zone_white.png", new Color(0.42f, 0.45f, 0.52f, 1f), Image.Type.Sliced);
            current.gameObject.AddComponent<PunchScaleAnimator>();
            RectTransform brackets = CreateRect("ui_image_zone_bar_current_brackets", current, Center, Center, Center, Vector2.zero, new Vector2(100f, 100f));
            AddImage(brackets, "ui_card_zone_map_frame.png", Color.white, Image.Type.Sliced, false, false);

            RectTransform viewport = CreateStretch("ui_panel_zone_bar_viewport", bar, 6f);
            viewport.gameObject.AddComponent<RectMask2D>(); // cubugun disina tasan numaralar gorunmesin
            RectTransform content = CreateRect(GameConstants.UINames.ZoneBarContent, viewport, Center, Center, Center, Vector2.zero, new Vector2(80f, 84f));
            RectTransform cell = CreateRect(GameConstants.UINames.ZoneBarCellTemplate, content, Center, Center, Center, Vector2.zero, new Vector2(80f, 84f));
            AddText(cell, font, "1", 46f, Color.white, TextAlignmentOptions.Center);
            cell.gameObject.SetActive(false);

            AddImage(CreateStretch("ui_image_zone_bar_frame", bar, 0f), "ui_card_frame_4px_zone.png", Color.white, Image.Type.Sliced, false, false);
            bar.gameObject.AddComponent<ZoneBarView>();
        }

        private static void BuildExitPanel(RectTransform root, TMP_FontAsset font)
        {
            RectTransform exit = CreateRect("ui_panel_exit", root, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(46f, 0f), new Vector2(290f, 720f));
            AddImage(exit, "ui_card_frame_gardient.png", Color.white, Image.Type.Sliced);

            RectTransform leave = CreateRect(GameConstants.UINames.LeaveButton, exit, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -24f), new Vector2(250f, 80f));
            Image leaveImage = AddImage(leave, "UI_button_grey_standard.png", Color.white, Image.Type.Sliced);
            leaveImage.raycastTarget = true;
            AddButton(leave, leaveImage);
            AddText(CreateStretch("ui_text_leave_label", leave, 0f), font, "ÇIKIŞ", 40f, Color.white, TextAlignmentOptions.Center);

            RectTransform viewport = CreateStretch("ui_panel_reward_list_viewport", exit, 0f);
            viewport.offsetMin = new Vector2(18f, 18f);
            viewport.offsetMax = new Vector2(-18f, -124f);
            Image viewportImage = AddImage(viewport, null, new Color(0f, 0f, 0f, 0.01f), Image.Type.Simple);
            viewportImage.raycastTarget = true; // ScrollRect'in surukleme alabilmesi icin
            viewport.gameObject.AddComponent<RectMask2D>();

            RectTransform content = CreateRect(GameConstants.UINames.RewardListContent, viewport, new Vector2(0f, 1f), Vector2.one, new Vector2(0.5f, 1f), Vector2.zero, Vector2.zero);
            VerticalLayoutGroup layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 8f;
            layout.padding = new RectOffset(0, 0, 4, 4);
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            ContentSizeFitter fitter = content.gameObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            ScrollRect scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.viewport = viewport;
            scroll.content = content;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 20f;

            RectTransform row = CreateRect("ui_panel_reward_row_template", content, new Vector2(0f, 1f), Vector2.one, new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, 58f));
            LayoutElement rowLayout = row.gameObject.AddComponent<LayoutElement>();
            rowLayout.minHeight = 58f;
            rowLayout.preferredHeight = 58f;
            row.gameObject.AddComponent<CanvasGroup>();
            row.gameObject.AddComponent<PunchScaleAnimator>();
            RectTransform icon = CreateRect(GameConstants.UINames.RewardRowIcon, row, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(6f, 0f), new Vector2(78f, 54f));
            AddImage(icon, null, Color.white, Image.Type.Simple, true);
            RectTransform amount = CreateStretch(GameConstants.UINames.RewardRowAmount, row, 0f);
            amount.offsetMin = new Vector2(98f, 0f);
            AddText(amount, font, "0", 38f, Color.white, TextAlignmentOptions.Left);
            row.gameObject.AddComponent<RewardRowView>();
            row.gameObject.SetActive(false);

            exit.gameObject.AddComponent<RewardListView>();
        }

        private static void BuildUpcomingZones(RectTransform root, TMP_FontAsset font)
        {
            RectTransform upcoming = CreateRect("ui_panel_upcoming_zones", root, Vector2.one, Vector2.one, Vector2.one, new Vector2(-40f, -130f), new Vector2(340f, 230f));
            VerticalLayoutGroup layout = upcoming.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 16f;
            layout.childAlignment = TextAnchor.UpperRight;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            RectTransform badge = CreateRect("ui_panel_upcoming_zone_template", upcoming, new Vector2(0f, 1f), Vector2.one, new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, 100f));
            LayoutElement badgeLayout = badge.gameObject.AddComponent<LayoutElement>();
            badgeLayout.minHeight = 100f;
            badgeLayout.preferredHeight = 100f;
            badge.gameObject.AddComponent<PunchScaleAnimator>();
            AddImage(CreateStretch(GameConstants.UINames.UpcomingZoneBackground, badge, 0f), "ui_card_panel_zone_white.png", Color.white, Image.Type.Sliced);
            AddImage(CreateStretch(GameConstants.UINames.UpcomingZoneFrame, badge, 0f), "ui_card_frame_4px_zone.png", Color.white, Image.Type.Sliced, false, false);
            RectTransform title = CreateRect(GameConstants.UINames.UpcomingZoneTitle, badge, Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 0.5f), new Vector2(18f, 0f), new Vector2(128f, 0f));
            AddText(title, font, "ALTIN ÇEVİRME", 24f, Color.white, TextAlignmentOptions.Left);
            RectTransform number = CreateRect(GameConstants.UINames.UpcomingZoneNumber, badge, Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 0.5f), new Vector2(150f, 0f), new Vector2(96f, 0f));
            AddText(number, font, "30", 54f, Color.white, TextAlignmentOptions.Left);
            RectTransform icon = CreateRect(GameConstants.UINames.UpcomingZoneIcon, badge, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-12f, 0f), new Vector2(76f, 76f));
            AddImage(icon, null, Color.white, Image.Type.Simple, true);
            badge.gameObject.AddComponent<UpcomingZoneBadgeView>();
            badge.gameObject.SetActive(false);

            upcoming.gameObject.AddComponent<UpcomingZonesView>();
        }

        private static void BuildZoneInfo(RectTransform root, TMP_FontAsset font)
        {
            RectTransform info = CreateRect("ui_panel_zone_info", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 120f), new Vector2(1100f, 120f));
            RectTransform title = CreateRect(GameConstants.UINames.ZoneTitleText, info, new Vector2(0f, 1f), Vector2.one, new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, 66f));
            AddText(title, font, "BRONZ ÇEVİRME", 54f, Color.white, TextAlignmentOptions.Center);
            RectTransform subtitle = CreateRect(GameConstants.UINames.ZoneSubtitleText, info, Vector2.zero, new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 6f), new Vector2(0f, 40f));
            AddText(subtitle, font, "BOMBA RİSKİ VAR - DİKKATLİ ÇEVİR", 26f, new Color(0.86f, 0.86f, 0.92f, 1f), TextAlignmentOptions.Center);
        }

        private static void BuildWallet(RectTransform root, TMP_FontAsset font)
        {
            RectTransform wallet = CreateRect("ui_panel_wallet", root, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-40f, 26f), new Vector2(460f, 60f));
            HorizontalLayoutGroup layout = wallet.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 24f;
            layout.childAlignment = TextAnchor.MiddleRight;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;

            BuildCurrencySlot(wallet, font, CurrencyType.Cash, "UI_icon_cash.png", new Vector2(62f, 34f), 170f);
            BuildCurrencySlot(wallet, font, CurrencyType.Gold, "UI_icon_gold.png", new Vector2(46f, 40f), 110f);
            wallet.gameObject.AddComponent<WalletView>();
        }

        private static void BuildCurrencySlot(RectTransform wallet, TMP_FontAsset font, CurrencyType type, string iconFile, Vector2 iconSize, float textWidth)
        {
            RectTransform slot = CreateRect("ui_currency_" + type.ToString().ToLowerInvariant(), wallet, Center, Center, Center, Vector2.zero, new Vector2(iconSize.x + 10f + textWidth, 60f));
            RectTransform icon = CreateRect("ui_image_currency_icon", slot, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), Vector2.zero, iconSize);
            AddImage(icon, iconFile, Color.white, Image.Type.Simple, true);
            RectTransform amount = CreateRect(GameConstants.UINames.CurrencyAmountText, slot, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(iconSize.x + 10f, 0f), new Vector2(textWidth, 60f));
            AddText(amount, font, "0", 36f, Color.white, TextAlignmentOptions.Left);
            amount.gameObject.AddComponent<PunchScaleAnimator>();

            CurrencyDisplayView display = slot.gameObject.AddComponent<CurrencyDisplayView>();
            SerializedObject serialized = new SerializedObject(display); // private alan, Inspector'daki gibi SerializedObject ile ayarliyoruz
            serialized.FindProperty("currencyType").enumValueIndex = (int)type;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void BuildPopupDim(RectTransform popup)
        {
            RectTransform dim = CreateRect("ui_image_popup_dim", popup, Center, Center, Center, Vector2.zero, new Vector2(5000f, 5000f));
            Image dimImage = AddImage(dim, null, new Color(0f, 0f, 0f, 0.72f), Image.Type.Simple);
            dimImage.raycastTarget = true; // popup acikken arkadaki butonlara tiklanamasin
            dim.SetAsFirstSibling();
        }

        private static void RunOnValidate(Transform root) // yeni kurulan view'lar referanslarini kendileri bulsun
        {
            foreach (MonoBehaviour behaviour in root.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (behaviour == null || behaviour.GetType().Namespace == null || !behaviour.GetType().Namespace.StartsWith("VertigoWheel"))
                {
                    continue;
                }

                MethodInfo onValidate = behaviour.GetType().GetMethod("OnValidate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (onValidate != null)
                {
                    onValidate.Invoke(behaviour, null);
                }

                EditorUtility.SetDirty(behaviour);
            }
        }

        private static RectTransform CreateRect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 size)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            Undo.RegisterCreatedObjectUndo(go, UndoName);
            go.layer = LayerMask.NameToLayer("UI");

            RectTransform rect = (RectTransform)go.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
            return rect;
        }

        private static RectTransform CreateStretch(string name, Transform parent, float inset)
        {
            RectTransform rect = CreateRect(name, parent, Vector2.zero, Vector2.one, Center, Vector2.zero, Vector2.zero);
            rect.offsetMin = new Vector2(inset, inset);
            rect.offsetMax = new Vector2(-inset, -inset);
            return rect;
        }

        private static Image AddImage(RectTransform rect, string spriteFile, Color color, Image.Type type, bool preserveAspect = false, bool fillCenter = true)
        {
            Image image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(spriteFile);
            image.color = color;
            image.type = type;
            image.preserveAspect = preserveAspect;
            image.fillCenter = fillCenter;
            image.raycastTarget = false; // sadece butonlar tiklansin
            return image;
        }

        private static TextMeshProUGUI AddText(RectTransform rect, TMP_FontAsset font, string text, float size, Color color, TextAlignmentOptions alignment)
        {
            TextMeshProUGUI label = rect.gameObject.AddComponent<TextMeshProUGUI>();
            label.font = font;
            label.text = text;
            label.fontSize = size;
            label.color = color;
            label.alignment = alignment;
            label.fontStyle = FontStyles.Bold;
            label.raycastTarget = false;
            return label;
        }

        private static void AddButton(RectTransform rect, Image target)
        {
            Button button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = target;
        }

        private static Sprite LoadSprite(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return null;
            }

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(ImagesFolder + file);
            if (sprite == null)
            {
                Debug.LogWarning("Sprite bulunamadi ya da Sprite olarak import edilmemis: " + file);
            }

            return sprite;
        }

        private static TMP_FontAsset FindFont(Transform root) // sahnede zaten kullanilan fontu kullan, gorunum tutarli kalsin
        {
            TMP_Text any = root.GetComponentInChildren<TMP_Text>(true);
            return any != null ? any.font : TMP_Settings.defaultFontAsset;
        }

        private static RectTransform FindDeep(Transform root, string name)
        {
            foreach (RectTransform rect in root.GetComponentsInChildren<RectTransform>(true))
            {
                if (rect.name == name)
                {
                    return rect;
                }
            }

            return null;
        }

        private static void DestroyIfExists(Transform root, string name)
        {
            RectTransform rect = FindDeep(root, name);
            if (rect != null)
            {
                Undo.DestroyObjectImmediate(rect.gameObject);
            }
        }

        private static void DestroyWrapperOf(Transform root, string childName) // eski layout'taki "Canvas" isimli sarmalayicilarla birlikte sil
        {
            RectTransform child = FindDeep(root, childName);
            if (child == null)
            {
                return;
            }

            Transform target = child.parent != root ? child.parent : child;
            Undo.DestroyObjectImmediate(target.gameObject);
        }
    }
}
