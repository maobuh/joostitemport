using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace joostitemport.Projectiles
{
	public class EnkiduWindFriendly : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enkidu's Wind");
			Main.projFrames[Projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.alpha = 150;
			Projectile.extraUpdates = 1;
			Projectile.aiStyle = ProjectileID.Bullet;
		}
		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 4)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 6;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.EntitySpriteDraw((Texture2D) ModContent.Request<Texture2D>("joostitemport/Projectiles/EnkiduWindFriendly"), Projectile.Center, new Rectangle(0, 0, 28, 42),
				Color.White, Projectile.rotation, new Vector2(28 / 2, 42 / 2), 10, 0, 0);
			return false;
		}
	}
}