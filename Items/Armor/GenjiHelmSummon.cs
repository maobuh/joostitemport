using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Buffs;

namespace joostitemport.Items.Armor
{
    public class GenjiHelmSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver Genji Helm");
            Tooltip.SetDefault("75% Increased Minion damage and Knockback\nMax sentries increased by 4");
        }

        public override void SetDefaults()
        {
            Item.wornArmor = true;
            Item.headSlot = 1;
            Item.width = 26;
            Item.height = 24;
            Item.value = 10000000;
            Item.rare = ItemRarityID.Purple;
            Item.defense = 20;
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
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<GenjiChestSummon>() && legs.type == ModContent.ItemType<GenjiLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Enkidu will fight for you";
            player.GetModPlayer<JoostPlayer>().gSummon = true;
            player.AddBuff(ModContent.BuffType<EnkiduMinion>(), 2);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<SummonDamageClass>() += 0.75f;
            player.GetKnockback<SummonDamageClass>() *= 1.75f;
            player.maxTurrets += 4;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawShadowLokis = true;
            player.armorEffectDrawOutlines = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // recipe.AddIngredient();
            recipe.Register();
        }
    }
}