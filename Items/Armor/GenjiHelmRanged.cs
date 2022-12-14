using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace joostitemport.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class GenjiHelmRanged : ModItem
    {
        int armorBuffTimer = 120;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Genji Helm");
            Tooltip.SetDefault("50% Increased Ranged damage\nYou no longer consume ammo");
        }
        public override void SetDefaults()
        {
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
                player.GetModPlayer<JoostPlayer>().gRanged = true;
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RangedDamageClass>() += 0.50f;
            if (armorBuffTimer <= 0)
            {
                player.GetModPlayer<JoostPlayer>().gRanged = false;
                armorBuffTimer = 120;
            }
            else if (player.GetModPlayer<JoostPlayer>().gRanged)
            {
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
            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.Register();
        }
    }
}