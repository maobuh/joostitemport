using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace joostitemport.Items.Armor
{
    public class GenjiChestMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Genji Armor");
            Tooltip.SetDefault("Enemies are most likely to target you\nMax Life increased by 250");
        }
        public override void SetDefaults()
        {
            Item.wornArmor = true;
            Item.bodySlot = 1;
            Item.width = 44;
            Item.height = 50;
            Item.value = 10000000;
            Item.rare = ItemRarityID.Purple;
            Item.defense = 35;
        }
        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.OverrideColor = new Color(0, 255, 0);
                }
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 250;
            player.aggro += 300;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // recipe.AddIngredient();
            recipe.Register();
        }
    }
}