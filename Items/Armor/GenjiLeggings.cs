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
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000000;
			Item.rare = ItemRarityID.Purple;
			Item.defense = 20;
			Item.lifeRegen = 8;
        }
        public override void UpdateEquip(Player player)
		{
			player.moveSpeed *= 3;
			player.accRunSpeed *= 3;
			player.maxRunSpeed *= 3;
		}
    }
}