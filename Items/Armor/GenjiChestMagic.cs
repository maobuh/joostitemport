using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace joostitemport.Items.Armor
{
    class GenjiChestMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Azure Genji Armor");
            Tooltip.SetDefault("Max Mana increased by 200\nMax Life increased by 175");
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
            player.statManaMax2 += 200;
            player.statLifeMax2 += 175;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // recipe.AddIngredient();
            recipe.Register();
        }
    }
}