using Terraria;
using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace joostitemport.Projectiles
{
	public class EnkiduWindFriendly : ModProjectile
	{
		// larger number makes the outer projectiles go further away from the centre, no idea how much tho
		const float spread = 5;
		const float speed = 10;
		bool foundTarget;
		double timeToTarget;
		float distanceFromTarget = 1080f;
		const int chargeTime = 90;
		const int middle = 0;
		const int finalAlpha = 100;
		Vector2 targetCenter;
		Vector2 originalVelocity;
		Vector2 directionToTarget;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enkidu's Wind");
			Main.projFrames[Projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.penetrate = 32767;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 500;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.ai[1] = -1;
		}
		public override void AI()
		{
			// increment how long the projectile has been alive for
			Projectile.ai[1]++;
			// animation
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 4)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 6;
			}
			// wait for a few frames in place
			// Projectile.ai[1] counts how many frames the projectile has been alive for
			if (Projectile.ai[1] < chargeTime) {
				if (Projectile.ai[1] % 2 == 0) {
					Projectile.alpha -= (255 - finalAlpha) / chargeTime;
				}
				return;
			}
			// find target
			// find closest npc that can be targeted
			targetCenter = Projectile.position;
			if (!foundTarget) {
				for (int i = 0; i < Main.maxNPCs; i++) {
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						if ((closest && inRange) || !foundTarget) {
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
							// update the velocity to be in a straight line to where the target was once and then leave it
							Projectile.velocity = Projectile.DirectionTo(targetCenter) * speed;
							originalVelocity = Projectile.velocity;
							timeToTarget = Vector2.Distance(targetCenter, Projectile.Center) / Projectile.velocity.Length();
							directionToTarget = Vector2.Subtract(npc.Center, Projectile.Center);
						}
					}
				}
			}
			// shoot towards them
			if (foundTarget && Projectile.ai[1] - chargeTime < timeToTarget) {
				// Projectile.ai[0] determines if this specific projectile is the straight line one or the curved ones
				if (Projectile.ai[0] != middle) {
					// find unit vector perpendicular to the direction from the projectile to the target npc
					Vector2 perpendicularVelocity = new(-directionToTarget.Y, directionToTarget.X);
					perpendicularVelocity.Normalize();
					// multiply it by cos() * spread * Projectile.ai[0]
					perpendicularVelocity = perpendicularVelocity * (float)Math.Cos((Projectile.ai[1] - chargeTime) / timeToTarget * Math.PI) * spread * Projectile.ai[0];
					// add it to original velocity
					Projectile.velocity = originalVelocity + perpendicularVelocity;
				}
			}
			// reached target location - starts going in a straight line now
			Projectile.netUpdate = true;
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.EntitySpriteDraw((Texture2D) ModContent.Request<Texture2D>("joostitemport/Projectiles/EnkiduWindFriendly"), Projectile.Center, new Rectangle(0, 0, 28, 42),
				Color.White, Projectile.rotation, new Vector2(28 / 2, 42 / 2), 1, 0, 0);
			return true;
		}
	}
}