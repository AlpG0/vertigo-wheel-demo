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
            public const string WheelBaseImage = "ui_image_wheel_base"; // wheel'in altindaki daire
            public const string WheelIndicatorImage = "ui_image_wheel_indicator"; // wheel'in ustundeki ok
            public const string SpinButton = "ui_button_spin"; // wheel'i ceviren buton
            public const string LeaveButton = "ui_button_leave"; // oyunu terk eden buton
            public const string ZoneTitleText = "ui_text_zone_value"; // zone numarasini gosteren text
            public const string ZoneSubtitleText = "ui_text_zone_subtitle"; // zone alt basligini gosteren text
            public const string ZoneBarContent = "ui_panel_zone_bar_content"; // zone cubugunun icindeki content objesi
            public const string ZoneBarCellTemplate = "ui_text_zone_cell_template"; // zone cubugundaki bir cell'in template'i
            public const string RewardListContent = "ui_panel_reward_list_content"; // odul listesi icindeki content objesi
            public const string RewardRowIcon = "ui_image_reward_row_icon"; // odul satirindaki icon
            public const string RewardRowAmount = "ui_text_reward_row_amount"; // odul satirindaki miktar text'i
            public const string UpcomingZoneBackground = "ui_image_upcoming_zone_bg"; // yaklasan zone'un arka plani
            public const string UpcomingZoneFrame = "ui_image_upcoming_zone_frame"; // yaklasan zone'un cercevesi
            public const string UpcomingZoneTitle = "ui_text_upcoming_zone_title"; // yaklasan zone'un basligi
            public const string UpcomingZoneNumber = "ui_text_upcoming_zone_number"; // yaklasan zone'un sira numarasi
            public const string UpcomingZoneIcon = "ui_image_upcoming_zone_icon"; // yaklasan zone'un icon'u
            public const string CurrencyAmountText = "ui_text_currency_amount"; // altin/nakit miktarini gosteren text
            public const string PopupMessageText = "ui_text_reward_popup_value"; // odul popup'indaki mesaj text'i
            public const string PopupGiveUpButton = "ui_button_reward_popup_give_up"; // odul popup'indaki vazgec butonu
            public const string PopupGoldReviveButton = "ui_button_reward_popup_gold_revive"; // odul popup'indaki altin ile revive butonu
            public const string PopupAdReviveButton = "ui_button_reward_popup_ad_revive"; // odul popup'indaki reklam izleyerek revive butonu
            public const string ZoneBarCellPrefix = "ui_text_zone_cell_"; // sonuna zone numarasi eklenir
            public const string RewardRowPrefix = "ui_panel_reward_row_"; // sonuna odulun adi eklenir
            public const string UpcomingZoneBadgePrefix = "ui_panel_upcoming_zone_"; // sonuna sira numarasi eklenir
        }
    }
}
