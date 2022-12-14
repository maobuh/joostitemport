using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class GenjiHelmMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Azure Genji Helm");
            Tooltip.SetDefault("65% Increased Magic damage\n35% Reduced Mana Usage");
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
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
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<GenjiChestMagic>() && legs.type == ModContent.ItemType<GenjiLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Press the Armor Ability Hotkey to cast Bitter End\n" +
                "This consumes all of your mana and cant be used while you have mana sickness";
            if (joostitemport.ArmorAbilityHotKey.JustPressed) {
                player.GetModPlayer<JoostPlayer>().gMagic = true;
            }
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawShadowLokis = true;
            // if (player.statMana >= player.statManaMax2 && !player.HasBuff(BuffID.ManaSickness) && player.ownedProjectileCounts[mod.ProjectileType("BitterEndFriendly")] + player.ownedProjectileCounts[mod.ProjectileType("BitterEndFriendly2")] <= 0)
            // {
            //     player.armorEffectDrawOutlines = true;
            // }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<MagicDamageClass>() += 0.65f;
            player.manaCost *= 0.65f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.Register();
        }
    }
}