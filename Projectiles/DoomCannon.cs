using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace joostitemport.Projectiles
{
    public class DoomCannon : ModProjectile
    {
        private const int CHARGE_INCREMENT = 30;
        private const int TOTAL_FRAMES = CHARGE_INCREMENT * 11;
        // frame of the animation at which charging the cannon slows down the player
        private const int SLOW_FRAME = 5;
        private const int INIT_SPD = 10;
        private const float SPD_INC = 0.5f;
        private const int INIT_DMG = 1;
        private const float DMG_INC = 0.25f;
        private const int DOOM2_FRAME = 180;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doom Cannon");
			Main.projFrames[Projectile.type] = 12;
		}
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool CanHitPvp(Player target)
        {
            return false;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            if (player.controlUseTile)
            {
                Projectile.ai[0] = 0;
                Projectile.netUpdate = true;
                Projectile.netUpdate2 = true;
                Projectile.Kill();
            }
            bool channeling = player.channel && !player.noItems && !player.CCed;
            if (channeling)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    float scaleFactor6 = 1f;
                    if (player.inventory[player.selectedItem].shoot == Projectile.type)
                    {
                        scaleFactor6 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
                    }
                    Vector2 dir = Main.MouseWorld - playerPos;
                    Projectile.direction = dir.X > 0 ? 1 : -1;
                    if (Vector2.Distance(Main.MouseWorld, playerPos) > 40)
                    {
                        dir = Main.MouseWorld - Projectile.Center;
                    }
                    dir.Normalize();
                    if (dir.HasNaNs())
                    {
                        dir = Vector2.UnitX * player.direction;
                    }
                    dir *= scaleFactor6;
                    if (dir.X != Projectile.velocity.X || dir.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = dir;
                }
            }
            else
            {
                Projectile.Kill();
            }
            if (Projectile.ai[0] < TOTAL_FRAMES) {
                Projectile.frame = (int) Projectile.ai[0] / CHARGE_INCREMENT;
                // slow down the player at some charge of the cannon
                if (Projectile.frame >= SLOW_FRAME) {
                    player.velocity.X *= 0.99f - ((Projectile.frame - SLOW_FRAME) * 0.15f);
                    if (player.velocity.Y * player.gravDir < 0)
                    {
                        player.velocity.Y *= 0.99f - ((Projectile.frame - SLOW_FRAME) * 0.15f);
                    }
                }
            }
            else
            {
                player.velocity.X *= 0.9f;
                if (player.velocity.Y * player.gravDir < 0)
                {
                    player.velocity.Y *= 0.9f;
                }
                Vector2 vel = Vector2.Normalize(Projectile.velocity);
                if (Math.Abs(vel.X) < 0.15f)
                {
                    Projectile.velocity.X = 0;
                }
                if (Math.Abs(vel.Y) < 0.15f)
                {
                    Projectile.velocity.Y = 0;
                }
                Projectile.frame = 11;
                int dust2 = Dust.NewDust(new Vector2(player.Center.X - 4, player.Center.Y + (player.height / 2 * player.gravDir)), 1, 1, 261, 5, -3 * player.gravDir, 0, default, 1);
                Main.dust[dust2].noGravity = true;
                int dust3 = Dust.NewDust(new Vector2(player.Center.X - 4, player.Center.Y + (player.height / 2 * player.gravDir)), 1, 1, 261, -5, -3 * player.gravDir, 0, default, 1);
                Main.dust[dust3].noGravity = true;
            }
            if (Projectile.ai[0] <= TOTAL_FRAMES)
            {
                Projectile.ai[0]++;
            }
            if (Projectile.ai[0] == TOTAL_FRAMES)
            {
                SoundEngine.PlaySound(SoundID.Item42, Projectile.position); // i think the sounds wrong
            }
            if (Projectile.ai[0] % CHARGE_INCREMENT == 0)
            {
                SoundEngine.PlaySound(SoundID.Item75, Projectile.position);
                if (Projectile.ai[0] >= 360)
                {
                    SoundEngine.PlaySound(SoundID.Item114, Projectile.position);
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.direction == -1 ? 3.14f : 0);
            Projectile.position = playerPos - (Projectile.Size / 2) + new Vector2(-14, 0) + (new Vector2((float)Math.Cos(Projectile.rotation - (Math.PI / 2)), (float)Math.Sin(Projectile.rotation - (Math.PI / 2))) * 14);
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner && !player.dead && !Main.mouseRight && Projectile.ai[0] >= 60)
            {
                SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
                Vector2 pos = Projectile.Center + (Projectile.velocity * 26);
                int type = ModContent.ProjectileType<DoomSkull>();
                float speed;
                float mult;
                if (Projectile.ai[0] < TOTAL_FRAMES && Projectile.frame >= 1) {
                    speed = INIT_SPD + (SPD_INC * (Projectile.frame - 1));
                    mult = INIT_DMG + (DMG_INC * (Projectile.frame - 1));
                    if (Projectile.ai[0] > DOOM2_FRAME) {
                        type = ModContent.ProjectileType<DoomSkull2>();
                    }
                }
                else
                {
                    type = ModContent.ProjectileType<DoomSkull3>();
                    speed = 7;
                    mult = INIT_DMG + (DMG_INC * (Projectile.frame - 1));
                    SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
                    pos = Projectile.Center + (Projectile.velocity * 140);
                }
                if (float.IsNaN(Projectile.velocity.X) || float.IsNaN(Projectile.velocity.Y))
                {
                    Projectile.velocity = -Vector2.UnitY;
                }
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, Projectile.velocity * speed, type, (int)(Projectile.damage * mult), Projectile.knockBack * mult, Projectile.owner);
            }
        }
    }
}