using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VertigoWheel.Rewards;
using VertigoWheel.Utils;

namespace VertigoWheel.UI
{
    /// <summary>
    /// Sol paneldeki toplanan odul listesi. Her odul turu icin tek satir; ayni odul tekrar gelirse satirin miktari artar.
    /// </summary>
    public class RewardListView : MonoBehaviour, IRewardListView
    {
        [SerializeField] private RectTransform content; // satirlarin dizildigi yer (VerticalLayoutGroup)
        [SerializeField] private RewardRowView rowTemplate; // her satir bundan kopyalanir, kendisi pasif durur
        [SerializeField] private ScrollRect scrollRect; // satirlar sigmazsa kaydirilabilsin

        private readonly Dictionary<RewardDefinition, RewardRowView> rows = new Dictionary<RewardDefinition, RewardRowView>();

        private void OnValidate()
        {
            content = HierarchyLookup.FindByName<RectTransform>(this, GameConstants.UINames.RewardListContent);
            rowTemplate = GetComponentInChildren<RewardRowView>(true);
            scrollRect = GetComponentInChildren<ScrollRect>(true);
        }

        public Vector3 PrepareRow(RewardDefinition reward)
        {
            RewardRowView row = GetOrCreateRow(reward);
            LayoutRebuilder.ForceRebuildLayoutImmediate(content); // yeni satirin konumu hemen hesaplansin, ucan ikon dogru yere gitsin
            scrollRect.verticalNormalizedPosition = 0f; // en alttaki (yeni) satir gorunur olsun
            return row.IconWorldPosition;
        }

        public void SetAmount(RewardDefinition reward, int amount)
        {
            GetOrCreateRow(reward).SetAmount(amount);
        }

        public void Clear()
        {
            foreach (RewardRowView row in rows.Values)
            {
                Destroy(row.gameObject);
            }

            rows.Clear();
        }

        private RewardRowView GetOrCreateRow(RewardDefinition reward)
        {
            RewardRowView row;
            if (rows.TryGetValue(reward, out row))
            {
                return row;
            }

            row = Instantiate(rowTemplate, content);
            row.name = "ui_reward_row_" + reward.name;
            row.gameObject.SetActive(true);
            row.Setup(reward.Icon);
            rows[reward] = row;
            return row;
        }
    }
}
