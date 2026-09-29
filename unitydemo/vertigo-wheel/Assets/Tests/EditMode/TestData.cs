using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VertigoWheel.Core;
using VertigoWheel.Data;
using VertigoWheel.Rewards;
using VertigoWheel.Zone;

namespace VertigoWheel.Tests
{
    /// <summary>
    /// Testler icin diske asset yazmadan bellekte ScriptableObject ureten yardimci.
    /// Private [SerializeField] alanlari Inspector'daki gibi SerializedObject ile dolduruyoruz, uretim koduna test icin setter eklemiyoruz.
    /// </summary>
    internal static class TestData
    {
        private static readonly List<Object> created = new List<Object>();

        public static T CreateReward<T>(string name) where T : RewardDefinition
        {
            T reward = ScriptableObject.CreateInstance<T>();
            reward.name = name;
            created.Add(reward);
            return reward;
        }

        public static CurrencyRewardDefinition CreateCurrency(string name, CurrencyType type)
        {
            CurrencyRewardDefinition reward = CreateReward<CurrencyRewardDefinition>(name);
            SerializedObject serialized = new SerializedObject(reward);
            serialized.FindProperty("currencyType").enumValueIndex = (int)type;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return reward;
        }

        public static WheelConfig CreateWheel(RewardDefinition[] rewards, int[] amounts)
        {
            WheelConfig wheel = ScriptableObject.CreateInstance<WheelConfig>();
            created.Add(wheel);

            SerializedObject serialized = new SerializedObject(wheel);
            SerializedProperty segments = serialized.FindProperty("segments");
            segments.arraySize = rewards.Length;
            for (int i = 0; i < rewards.Length; i++)
            {
                SerializedProperty segment = segments.GetArrayElementAtIndex(i);
                segment.FindPropertyRelative("reward").objectReferenceValue = rewards[i];
                segment.FindPropertyRelative("amount").intValue = amounts[i];
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            return wheel;
        }

        public static ZoneDefinition CreateZone(string title, int interval, bool canLeave, WheelConfig wheel)
        {
            ZoneDefinition zone = ScriptableObject.CreateInstance<ZoneDefinition>();
            zone.name = title;
            created.Add(zone);

            SerializedObject serialized = new SerializedObject(zone);
            serialized.FindProperty("title").stringValue = title;
            serialized.FindProperty("interval").intValue = interval;
            serialized.FindProperty("canLeave").boolValue = canLeave;
            serialized.FindProperty("wheelConfig").objectReferenceValue = wheel;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return zone;
        }

        public static GameSettings CreateSettings(ZoneDefinition[] zones, int reviveGoldCost)
        {
            GameSettings settings = ScriptableObject.CreateInstance<GameSettings>();
            created.Add(settings);

            SerializedObject serialized = new SerializedObject(settings);
            SerializedProperty zoneList = serialized.FindProperty("zones");
            zoneList.arraySize = zones.Length;
            for (int i = 0; i < zones.Length; i++)
            {
                zoneList.GetArrayElementAtIndex(i).objectReferenceValue = zones[i];
            }

            serialized.FindProperty("reviveGoldCost").intValue = reviveGoldCost;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return settings;
        }

        public static void DestroyAll()
        {
            foreach (Object item in created)
            {
                Object.DestroyImmediate(item);
            }

            created.Clear();
        }
    }
}
