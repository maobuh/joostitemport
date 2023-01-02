using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Items.Armor
{
    public class GenjiChestSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver Genji Armor");
            Tooltip.SetDefault("Increases your max number of minions by 7\nMax Life increased by 150");
        }

        public override void SetDefaults()
        {
            Item.wornArmor = true;
            Item.bodySlot = 1;
            Item.width = 44;
            Item.height = 50;
            Item.value = 10000000;
            Item.rare = ItemRarityID.Purple;
            Item.defense = 25;
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
            player.statLifeMax2 += 150;
            player.maxMinions += 7;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // recipe.AddIngredient();
            recipe.Register();
        }
    }
}