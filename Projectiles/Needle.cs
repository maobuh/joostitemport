using System;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;

namespace joostitemport.Projectiles
{
	public class Needle : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.damage = 50;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 2;
			Projectile.height = 16;
			Projectile.knockBack = 0;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			// If the projectile hits the left or right side of the tile, reverse the X velocity
			if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) {
				Projectile.velocity.X = -oldVelocity.X;
			}

			// If the projectile hits the top or bottom side of the tile, reverse the Y velocity
			if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) {
				Projectile.velocity.Y = -oldVelocity.Y;
			}

			return false;
		}

		

		// public override void OnSpawn(IEntitySource source) {
		// 	Vector2 NewPosition = Vector2.Multiply(Projectile.position, i);
		// 	Projectile.NewProjectile(Projectile.InheritSource(Projectile), NewPosition, Projectile.velocity, ModContent.ProjectileType<Needle2>(), 1, 0);
		// }
	}
}