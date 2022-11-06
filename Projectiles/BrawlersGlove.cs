using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace joostitemport.Projectiles
{
	public class BrawlersGlove : ModProjectile
	{
        // the number of times the left click projectile has been slowed down 
        private int slowdowns = 1;
        // if the player has right clicked
        private bool grab = false;
        // the npc that the projectile has hit with a right click
        NPC grabTarget = null;
        Player grabTargetPlayer = null;
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
            if (grab && grabTarget != null)
            {
                // teleport the target to the player
                grabTarget.Center = player.Center;
            }
            else if (grab && grabTargetPlayer != null)
            {
                grabTargetPlayer.Center = player.Center;
            }
            // if player is left clicking
            else
            {
                if (Projectile.velocity != Vector2.Zero)
                {
                    // add players velocity to projectile so that it stays kind of close to player
                    Projectile.velocity = Vector2.Add(Projectile.velocity, Vector2.Divide(player.velocity, slowdowns * 2));
                    // slow down projectile
                    Projectile.velocity = Vector2.Subtract(Projectile.velocity, Vector2.Normalize(Projectile.velocity));
                    slowdowns++;
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
            else
            {
                Vector2 addedVelocity = Vector2.Multiply(Vector2.Normalize(Projectile.velocity), 10);
                // knock them back a bit but in the direction of where the projectile hits them
                target.velocity = Vector2.Add(addedVelocity, target.velocity);
                // make same velocity on you so you stick on them
                player.velocity = target.velocity;
            }
            // 15 frames of immunity if you hit something
            player.immuneTime = 15;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            player.immune = true;
            if (grab)
            {
                // 3 seconds of grabbing before you have to let go
                Projectile.timeLeft = 180;
                grabTargetPlayer = target;
            }
            else
            {
                Vector2 addedVelocity = Vector2.Multiply(Vector2.Normalize(Projectile.velocity), 10);
                // knock them back a bit but in the direction of where the projectile hits them
                target.velocity = Vector2.Add(addedVelocity, target.velocity);
                // make same velocity on you so you stick on them
                player.velocity = target.velocity;
            }
            // 15 frames of immunity if you hit someone
            player.immuneTime = 15;
        }

        public override void OnSpawn(IEntitySource source)
        {
            // rotate based on tan of projectiles velocity
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }
    }
}