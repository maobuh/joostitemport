using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;
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
        // or the player if it hits a player instead
        Player grabTargetPlayer = null;
        // rotation value for right click projectile in radians
        double rot = Math.PI / 2;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.damage = 100;
            Projectile.friendly = true;
			Projectile.tileCollide = false;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (grab && grabTarget?.active == true)
            {
                // teleport the target to the player
                grabTarget.Center = player.Center;
            }
            else if (grab && grabTargetPlayer?.active == true)
            {
                // teleport the target to the player
                grabTargetPlayer.Center = player.Center;
            }
            // kill the projectile if the target dies
            else if (grabTarget != null || grabTargetPlayer != null) {
                if (!grabTarget.active || !grabTargetPlayer.active)
                {
                    Projectile.Kill();
                }
            }
            else if (grab)
            {
                // projectile moves in a quarter circle foward depending on the direction the player is facing
                // for some reason this is actually what the center of the player feels like rather than player.Center
                Vector2 actualCenter = new(player.position.X + (player.width * 0.25f), player.position.Y - (player.height * 0.5f));
                // rotation direction - unit vector that is in the direction of the rot value with the X value opposite if player is facing left
                Vector2 rotDirection = new((float)Math.Cos(rot) * player.direction, (float)Math.Sin(rot));
                // relative position from player
                Vector2 relativePos = Vector2.Multiply(rotDirection, 40);
                // the projectiles position is based off the actual centre and not the relative position though
                Projectile.position = Vector2.Add(relativePos, actualCenter);
                // rotate the projectile based on rot but 90 degrees off like it looks like in the original
                Projectile.rotation = (float)((rot - (Math.PI * player.direction / 2)) * player.direction);
                // decrement the rotation so that the projectile actually gets somewhere
                rot -= Math.PI / 20;
            }
            // if player is left clicking
            else
            {
                if (Projectile.velocity != Vector2.Zero)
                {
                    // add players velocity divided by the amount of times the projectile has been slowed down to projectile so that it stays kind of close to player
                    Projectile.velocity = Vector2.Add(Projectile.velocity, Vector2.Divide(player.velocity, slowdowns * 2));
                    // slow down projectile
                    Projectile.velocity = Vector2.Subtract(Projectile.velocity, Vector2.Normalize(Projectile.velocity));
                    slowdowns++;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            // boom
            SoundEngine.PlaySound(RandomiseSound(), Projectile.position);
            Player player = Main.player[Projectile.owner];
            player.immune = true;
            // you cannot grab bosses
            if (grab && !target.boss)
            {
                // 3 seconds of grabbing before you have to let go
                Projectile.timeLeft = 180;
                grabTarget = target;
            }
            else
            {
                Vector2 addedVelocity = Vector2.Multiply(Vector2.Normalize(Projectile.velocity), knockback);
                // knock them back a bit but in the direction of where the projectile hits them
                target.velocity = Vector2.Add(addedVelocity, target.velocity);
                // make same velocity on you so you stick on them
                player.velocity = target.velocity;
            }
            // 10 frames of immunity if you hit something
            player.immuneTime = 10;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            // boom
            SoundEngine.PlaySound(RandomiseSound(), Projectile.position);
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
                Vector2 addedVelocity = Vector2.Multiply(Vector2.Normalize(Projectile.velocity), Projectile.knockBack);
                // knock them back a bit but in the direction of where the projectile hits them
                target.velocity = Vector2.Add(addedVelocity, target.velocity);
                // make same velocity on you so you stick on them
                player.velocity = target.velocity;
            }
            // 10 frames of immunity if you hit someone
            player.immuneTime = 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            // if player is right clicking it is a grab and the projectile will not die when it hits something and the projectiles position will be synced with all clients on a server
            if (player.controlUseTile)
            {
                Projectile.penetrate = -1;
                grab = true;
                Projectile.netUpdate = true;
            }
            // otherwise its just punch
            else
            {
                // rotate based on tan of projectiles velocity
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
                // play whiff sound
                SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
            }
        }

        public static SoundStyle RandomiseSound()
        {
            Random rand = new();
            SoundStyle[] sounds = {SoundID.Dig, SoundID.Item14, SoundID.Item10};
            return sounds[rand.Next(0, sounds.Length)];
        }
    }
}