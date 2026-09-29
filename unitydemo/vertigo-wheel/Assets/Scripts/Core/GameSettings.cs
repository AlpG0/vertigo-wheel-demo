using System.Collections.Generic;
using UnityEngine;
using VertigoWheel.Zone; // ZoneDefinition burda

namespace VertigoWheel.Core
{
    /// <summary>
    /// Oyunun ayarlanabilir butun degerleri tek bir asset'te: zone'lar, baslangic bakiyesi, revive bedeli.
    /// </summary>
    [CreateAssetMenu(fileName = "GameSettings", menuName = "VertigoWheel/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private List<ZoneDefinition> zones; // sirasi onemli degil, ZoneService araliga gore siralar
        [SerializeField] private int startingCash = 10000;
        [SerializeField] private int startingGold = 950;
        [SerializeField] private int reviveGoldCost = 25;
        [SerializeField] private int debugForcedSegmentIndex = -1; // QA icin: 0 veya uzeri verilirse wheel hep o dilimde durur (FixedIndexResultPicker)

        public IReadOnlyList<ZoneDefinition> Zones { get { return zones; } }
        public int StartingCash { get { return startingCash; } }
        public int StartingGold { get { return startingGold; } }
        public int ReviveGoldCost { get { return reviveGoldCost; } }
        public int DebugForcedSegmentIndex { get { return debugForcedSegmentIndex; } }
    }
}
