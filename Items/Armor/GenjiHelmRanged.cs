using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace joostitemport.Items.Armor
{
    public class GenjiHelmRanged : ModItem
    { // buh
        int armorBuffTimer = 120;
        bool gRanged;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Genji Helm");
            Tooltip.SetDefault("50% Increased Ranged damage\nYou no longer consume ammo");
        }
        public override void SetDefaults()
        {
            Item.wornArmor = true;
            Item.headSlot = 1;
            Item.width = 28;
            Item.height = 26;
            Item.value = 10000000;
            Item.rare = ItemRarityID.Purple;
            Item.defense = 30;
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
            return body.type == ModContent.ItemType<GenjiChestRanged>() && legs.type == ModContent.ItemType<GenjiLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Press the Armor Ability key to sacrifice all your defense for increased ranged ability for 2 seconds with no cooldown";
            if (joostitemport.ArmorAbilityHotKey.JustPressed && armorBuffTimer == 120) {
                gRanged = true;
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RangedDamageClass>() += 0.50f;
            // player.GetModPlayer<JoostModPlayer>().ammoConsume = 0; // player shouldnt use ammo when they shoot idk how tho fck
            if (armorBuffTimer <= 0)
            {
                gRanged = false;
                armorBuffTimer = 120;
            }
            else if (gRanged)
            {
                player.GetDamage<RangedDamageClass>() *= 1 + (player.statDefense * 0.005f);
                player.statDefense = 0;
                armorBuffTimer--;
            }
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawShadowLokis = true;
            // if (player.HasBuff(mod.BuffType("gRangedBuff")))
            // {
            //     player.armorEffectDrawOutlines = true;
            // }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // recipe.AddIngredient();
            recipe.Register();
        }
    }
}