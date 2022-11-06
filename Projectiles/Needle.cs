using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace joostitemport.Projectiles
{
	public class Needle : ModProjectile
	{
		private int bunces = 3;
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
			Projectile.timeLeft = 180;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (bunces > 0)
			{
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				// If the projectile hits the left or right side of the tile, reverse the X velocity
				if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) {
					Projectile.velocity.X = -(oldVelocity.X * 0.5f);
				}

				// If the projectile hits the top or bottom side of the tile, reverse the Y velocity
				if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) {
					Projectile.velocity.Y = -(oldVelocity.Y * 0.5f);
				}
				bunces--;
			}
			else
			{
				Projectile.Kill();
			}

			return false;
		}
	}
}