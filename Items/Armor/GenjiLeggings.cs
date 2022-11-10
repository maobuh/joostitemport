using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace joostitemport.Items.Armor
{
    class GenjiLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Genji Leggings");
            Tooltip.SetDefault("so fast...");
        }
        public override void SetDefaults()
        {
            Item.wornArmor = true;
            Item.legSlot = 1;
        }
    }
}