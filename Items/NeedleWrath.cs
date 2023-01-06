using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;
using joostitemport.Projectiles;

namespace joostitemport.Items
{
    public class NeedleWrath : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Needle Wrath");
            Tooltip.SetDefault("Unleashes a Hurricane of needles");
        }
        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 2;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = 10000000;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item7;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Needle2>();
            Item.shootSpeed = 12f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const float spread = 15f * 0.0174f;
            float baseSpeed = velocity.Length();
            double baseAngle = Math.Atan2(velocity.X, velocity.Y);
            double randomAngle = baseAngle + ((Main.rand.NextFloat() - 0.5f) * spread);
            velocity = new(baseSpeed * (float)Math.Sin(randomAngle), baseSpeed * (float)Math.Cos(randomAngle));
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.itemAnimation % 4 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item7, position);
            }

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ThousandNeedles>(), 1); // something post moon lord
            recipe.AddIngredient(ItemID.FragmentNebula, 5);
            recipe.AddIngredient(ItemID.LunarBar, 1);
            recipe.Register();
        }
    }
}