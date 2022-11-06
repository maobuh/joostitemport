using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Projectiles
{
	public class BrawlersGlove : ModProjectile
	{
        // if the player has right clicked
        private bool grab = false;
        // the npc that the projectile has hit with a right click
        NPC grabTarget;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.damage = 100;
            Projectile.friendly = true;
			Projectile.tileCollide = false;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            // if player is right clicking
            if (player.altFunctionUse == 2)
            {
                grab = true;
            }
            if (grab)
            {
                grabTarget.Center = player.Center;
            }
            // if player is left clicking
            else
            {
                // slow down projectile
                if (Projectile.velocity != Vector2.Zero)
                {
                    Projectile.velocity = Vector2.Subtract(Projectile.velocity, Vector2.Normalize(Projectile.velocity));
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            player.immune = true;
            if (grab)
            {
                // 3 seconds of grabbing before you have to let go
                Projectile.timeLeft = 180;
                grabTarget = target;
            }
            // 10 frames of immunity if you hit someone
            player.immuneTime = 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            // add players velocity to projectile
            Player player = Main.player[Projectile.owner];
            Projectile.velocity = Vector2.Add(Projectile.velocity, player.velocity);
            // rotate based on tan of projectiles velocity
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }
    }
}