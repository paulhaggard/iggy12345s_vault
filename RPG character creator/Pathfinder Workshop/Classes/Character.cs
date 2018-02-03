using System;
using Pathfinder_Workshop.Classes.Item_Classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder_Workshop.Classes
{
    class Character : File
    {
        /*Description:
         * Describes any character that a player can play as. */

        // Property Definitions
        private Stats GeneralStats;
        private DescriptionsCluster Appearance;
        private Class[] Classes;
        private SavingThrows SavingThrowInfo;
        private Combat CombatInfo;
        private Item[] Gear;
        private Feat[] Feats;
        private SpellDataCluster SpellDataInfo;
        private MoneyCluster Money;

        internal Stats GeneralStats1 { get => GeneralStats; set => GeneralStats = value; }
        internal DescriptionsCluster Appearance1 { get => Appearance; set => Appearance = value; }
        internal Class[] Classes1 { get => Classes; set => Classes = value; }
        internal SavingThrows SavingThrowInfo1 { get => SavingThrowInfo; set => SavingThrowInfo = value; }
        internal Combat CombatInfo1 { get => CombatInfo; set => CombatInfo = value; }
        internal Item[] Gear1 { get => Gear; set => Gear = value; }
        internal Feat[] Feats1 { get => Feats; set => Feats = value; }
        internal SpellDataCluster SpellDataInfo1 { get => SpellDataInfo; set => SpellDataInfo = value; }
        internal MoneyCluster Money1 { get => Money; set => Money = value; }
    }
}
