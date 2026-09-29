namespace VertigoWheel.Utils
{
    /// <summary>
    /// Koddaki sabitlerin tek adresi: metin sablonlari, sahne obje isimleri, matematiksel sabitler.
    /// Ayarlanabilir oyun degerleri (baslangic bakiyesi, revive bedeli, zone'lar) burada degil, GameSettings asset'inde.
    /// </summary>
    public static class GameConstants
    {
        public const float FullRotationDegrees = 360f; // bir tam tur
        public const int ZoneBarLookahead = 12; // zone cubugunda mevcut zone'dan sonra kac zone hazir dursun

        public static class Texts // ekranda gorunen metin sablonlari
        {
            public const string BombRiskMessage = "Topladığın {0} ödülü kaybetme!";
            public const string MultiplierFormat = "x{0}";
        }

        public static class UINames // kod ile sahne arasindaki sozlesme: bu isimler degisirse HierarchyLookup uyari verir
        {
            public const string WheelBaseImage = "ui_image_wheel_base";
            public const string WheelIndicatorImage = "ui_image_wheel_indicator";
            public const string SpinButton = "ui_button_spin";
            public const string LeaveButton = "ui_button_leave";
            public const string ZoneTitleText = "ui_text_zone_value";
            public const string ZoneSubtitleText = "ui_text_zone_subtitle";
            public const string ZoneBarContent = "ui_panel_zone_bar_content";
            public const string ZoneBarCellTemplate = "ui_text_zone_cell_template";
            public const string RewardListContent = "ui_panel_reward_list_content";
            public const string RewardRowIcon = "ui_image_reward_row_icon";
            public const string RewardRowAmount = "ui_text_reward_row_amount";
            public const string UpcomingZoneBackground = "ui_image_upcoming_zone_bg";
            public const string UpcomingZoneFrame = "ui_image_upcoming_zone_frame";
            public const string UpcomingZoneTitle = "ui_text_upcoming_zone_title";
            public const string UpcomingZoneNumber = "ui_text_upcoming_zone_number";
            public const string UpcomingZoneIcon = "ui_image_upcoming_zone_icon";
            public const string CurrencyAmountText = "ui_text_currency_amount";
            public const string PopupMessageText = "ui_text_reward_popup_value";
            public const string PopupGiveUpButton = "ui_button_reward_popup_give_up";
            public const string PopupGoldReviveButton = "ui_button_reward_popup_gold_revive";
            public const string PopupAdReviveButton = "ui_button_reward_popup_ad_revive";
        }
    }
}
