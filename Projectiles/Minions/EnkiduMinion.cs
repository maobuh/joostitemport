using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace joostitemport.Projectiles.Minions
{
	public class EnkiduMinion : ModProjectile
	{
		const float overlapVelocity = 0.04f;
		const int frameSpeed = 5;
		int shootCooldown;
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
			float distanceFromTarget = 1080f;
			Vector2 targetCenter = Projectile.position;
			bool foundTarget = false;
			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC) {
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f) {
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
			if (!foundTarget) {
				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++) {
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
						// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
						bool closeThroughWall = between < 100f;
						if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			if (foundTarget) {
				shootCooldown++;
				if (shootCooldown > 30) {
					// projectile will be in a random position within 4 tiles left or right of the player and 4 tiles up from the player
					Vector2 projPos = new(Projectile.Center.X + ((Main.rand.NextFloat() - 0.5f) * 128), Projectile.Center.Y - (Main.rand.NextFloat() * 64));
					// original damage and knockback calculation ported to 1.4 i think i think i dont know
					int damage = (int)(1500 * player.GetDamage(DamageClass.Generic).Multiplicative * (player.GetDamage(DamageClass.Generic).Additive + player.GetTotalDamage<SummonDamageClass>().Additive - 1f) * player.GetDamage(DamageClass.Summon).Multiplicative);
					float knockback = player.GetKnockback<SummonDamageClass>().ApplyTo(10f);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), projPos, Vector2.Zero, ModContent.ProjectileType<EnkiduWindFriendly>(), damage, knockback, player.whoAmI, 0); // 0 for bottom gust
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), projPos, Vector2.Zero, ModContent.ProjectileType<EnkiduWindFriendly>(), damage, knockback, player.whoAmI, 1); // 1 for top gust
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), projPos, Vector2.Zero, ModContent.ProjectileType<EnkiduWindFriendly>(), damage, knockback, player.whoAmI, 2); // 2 for middle gust
					shootCooldown = 0;
				}
			}
			// movement
			// divided by 16 because it is one tile
			// speed and intertia based on how many tiles away the minion is
			float speed = distanceToIdlePosition / 8;
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
			Projectile.damage = 1500;
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