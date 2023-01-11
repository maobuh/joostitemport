using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace joostitemport.Projectiles
{
    public class BitterEndFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bitter End");
            Main.projFrames[Projectile.type] = 18;
        }
        public override void SetDefaults()
        {
            Projectile.width = 124;
            Projectile.height = 124;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.light = 0.95f;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 18;
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
            }
        }
        public override bool CanHitPvp(Player target)
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void Kill(int timeLeft)
        {
            const int shootNum = 36;
            const float shootSpread = 360f;
            const float spread = shootSpread * 0.0174f;
            float baseSpeed = (float)Math.Sqrt((7f * 7f) + (7f * 7f));
            double startAngle = Math.Atan2(7f, 7f) - (spread / shootNum);
            const double deltaAngle = spread / shootNum;
            double offsetAngle;
            int i;
            for (i = 0; i < shootNum; i++)
            {
                offsetAngle = startAngle + deltaAngle * i;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle)), ModContent.ProjectileType<BitterEndFriendly2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
        }
    }
}
