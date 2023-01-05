using System;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Projectiles.Minions
{
	public class EnkiduMinion : ModProjectile
	{
		const float overlapVelocity = 0.04f;
		const int frameSpeed = 5;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enkidu");
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}
		public override void AI() {
			// active check (checks if player is alive, despawns minion if the player died)
			Player player = Main.player[Projectile.owner];
			if (player.dead) {
				player.ClearBuff(ModContent.BuffType<Buffs.EnkiduMinion>());
				player.GetModPlayer<JoostPlayer>().gSummon = false;
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.EnkiduMinion>())) {
				Projectile.timeLeft = 2;
			}
			if (player.ownedProjectileCounts[ModContent.ProjectileType<EnkiduMinion>()] > 1 || !player.GetModPlayer<JoostPlayer>().gSummon) {
				Projectile.Kill();
			}
			// general behaviour
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 80f; // Go up 80 coordinates (5 tiles from the center of the player)
			idlePosition.X -= 80 * player.direction;

			// All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

			// Teleport to player if distance is too big
			Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f) {
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the Projectile,
				// and then set netUpdate to true
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}

			// If your minion is flying, you want to do this independently of any conditions
			for (int i = 0; i < Main.maxProjectiles; i++) {
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width) {
					if (Projectile.position.X < other.position.X) Projectile.velocity.X -= overlapVelocity;
					else Projectile.velocity.X += overlapVelocity;

					if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
					else Projectile.velocity.Y += overlapVelocity;
				}
			}
			// find target
			
			// movement
			// divided by 100 because it feels right :)
			// magical number
			float speed = distanceToIdlePosition / 16;
			float inertia = 5f * distanceToIdlePosition / 16;
			// only move toward player if its far away
			if (distanceToIdlePosition > 20f) {
				vectorToIdlePosition.Normalize();
				vectorToIdlePosition *= speed;
				Projectile.velocity = ((Projectile.velocity * (inertia - 1)) + vectorToIdlePosition) / inertia;
			}

			// animation and visuals
			// rotate slightly in the direction its moving
			Projectile.rotation = Projectile.velocity.X * 0.05f;
			Projectile.frameCounter++;
			// change Projectile visual every 5 frames
			if (Projectile.frameCounter >= frameSpeed) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 6;
			}
		}
		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 116;
			Projectile.height = 100;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 0;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			// port all these to AI() (they are from the shooter.cs file in joostmod)
			// inertia = 20f;
			// chaseAccel = 40f;
			// chaseDist = 40f;
			// shoot = mod.ProjectileType("EnkiduWindFriendly");
			// shootSpeed = 20f;
			// shootCool = 90f;
			// shootNum = 3;
			// shootSpread = 45f;
            // predict = true;
		}
        public override bool MinionContactDamage()
        {
            return false;
        }
		public override bool? CanCutTiles() {
			return false;
		}
	}
}