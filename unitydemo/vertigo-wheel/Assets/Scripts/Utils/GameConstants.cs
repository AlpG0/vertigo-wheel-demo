namespace VertigoWheel.Utils
{
    /// <summary>
    /// Koddaki sabitlerin tek adresi: metin sablonlari, sahne obje isimleri, matematiksel sabitler.
    /// Ayarlanabilir oyun degerleri (baslangic bakiyesi, revive bedeli, zone'lar) burada degil, GameSettings asset'inde.
    /// </summary>
    public static class GameConstants 
    {
        public const float FullRotationDegrees = 360f; // bir tam tur

        public static class Texts // ekranda gorunen metin sablonlari
        {
            public const string BombRiskMessage = "Topladığın {0} ödülü kaybetme!";
            public const string MultiplierFormat = "x{0}";
            public const string MaxRewardFormat = "Up To x{0} Rewards";
        }

        public static class UINames // kod ile sahne arasindaki sozlesme: bu isimler degisirse HierarchyLookup uyari verir
        {
            public const string WheelBaseImage = "ui_image_wheel_base";
            public const string WheelIndicatorImage = "ui_image_wheel_indicator";
            public const string MaxRewardText = "ui_text_max_reward_value";
            public const string ZoneTitleText = "ui_text_zone_value";
            public const string TotalValueText = "ui_text_total_value";
            public const string PopupMessageText = "ui_text_reward_popup_value";
            public const string PopupGiveUpButton = "ui_button_reward_popup_give_up";
            public const string PopupGoldReviveButton = "ui_button_reward_popup_gold_revive";
            public const string PopupAdReviveButton = "ui_button_reward_popup_ad_revive";
        }
    }
}
