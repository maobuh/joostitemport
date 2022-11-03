using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using log4net.DateFormatter;

namespace joostitemport.Items
{
	public class ThousandNeedles : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("1000 Needles"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("way too many needles... \n what to do with all these needles?");
		}

		public override void SetDefaults()
		{
			Item.damage = 1;
			Item.noMelee = true;
			Item.crit = -100;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 10;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 2;
			Item.useAnimation = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Needle>();
			Item.shootSpeed = 1000;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Cactus, 10);
			recipe.AddIngredient(ItemID.Stinger);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// 15 degrees but in radians for the spread of the needles
			float spread = 0.261799f;
			// makes 4 needles when shoot
			for (int i = 0; i < 4; i++)
			{
				// math for making random new angle
				float baseSpeed = velocity.Length();
				double baseAngle = Math.Atan2(velocity.X, velocity.Y);
				double randomAngle = baseAngle + ((Main.rand.NextFloat() - 0.5f) * spread);
				Vector2 newVelocity = new(baseSpeed * (float)Math.Sin(randomAngle), baseSpeed * (float)Math.Cos(randomAngle));
				// make new projectile
				Projectile.NewProjectile(player.GetSource_OpenItem(ModContent.ItemType<HundredNeedles>()), position, newVelocity, ModContent.ProjectileType<Needle>(), 1, 0, player.whoAmI);
			}
			return false;
		}
	}
}