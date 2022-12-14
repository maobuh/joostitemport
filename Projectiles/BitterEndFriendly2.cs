using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Projectiles
{
	public class BitterEndFriendly2 : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bitter End");
		}
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 400;
			Projectile.alpha = 150;
			Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.timeLeft * Projectile.velocity.Length() * 0.0174f;
        }
    }
}