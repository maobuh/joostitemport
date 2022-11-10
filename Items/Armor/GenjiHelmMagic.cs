using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace joostitemport.Items.Armor
{
    class GenjiHelmMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Press " ;
        }
    }
}