using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Projectiles;
using Terraria.DataStructures;

namespace joostitemport.Items
{
	public class LunarRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Rod");
			Tooltip.SetDefault("Fires multiple fishing hooks\n" + "Can fish up Lunar Fragments");
		}
		public override void SetDefaults()
		{
			Item.width = 56;
			Item.height = 32;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = 1;
			Item.value = 100000;
			Item.rare = 9;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<LunarFishHook2>();
			Item.shootSpeed = 17f;
			Item.fishingPole = 100;
		}
		public override void HoldItem (Player player)
		{
			player.GetModPlayer<JoostPlayer>().lunarRod = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ItemID.FragmentNebula, 3);
			recipe.AddIngredient(ItemID.FragmentSolar, 3);
			recipe.AddIngredient(ItemID.FragmentVortex, 3);
			recipe.AddIngredient(ItemID.FragmentStardust, 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();

		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const float spread = 45f * 0.0174f;
            float baseSpeed = velocity.Length();
            float startAngle = (float) Math.Atan2(velocity.X, velocity.Y) - (spread / 2);
            const float deltaAngle = spread / 32f;
            for (int i = 0; i < 4; i++)
            {
                float offsetAngle = startAngle + (deltaAngle * i);
                Vector2 direction = new((float)Math.Sin(offsetAngle), (float)Math.Cos(offsetAngle));
                Projectile.NewProjectile(source, position, baseSpeed * direction, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
	}
}

