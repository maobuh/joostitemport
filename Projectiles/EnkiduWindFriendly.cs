using Terraria;
using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace joostitemport.Projectiles
{
	public class EnkiduWindFriendly : ModProjectile
	{
		// 15 degrees in radians
		const float spread = 0.261799f;
		const float speed = 10;
		bool foundTarget;
		double timeToTarget;
		float distanceFromTarget = 1080f;
		float distanceTravelled;
		double height;
		Vector2 targetCenter;
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
			Projectile.timeLeft = 600;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.ai[1] = -1;
		}
		public override void SendExtraAI (BinaryWriter writer) {
			
		}
		public override void ReceiveExtraAI (BinaryReader reader) {
			
		}
		public override bool PreAI() {
			if (Projectile.ai[0] == 2 && foundTarget) {
					distanceTravelled = distanceFromTarget - Vector2.Distance(targetCenter, Projectile.Center);
			}
			return true;
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
			if (Projectile.ai[1] < 42) {
				if (Projectile.ai[1] % 2 == 0) {
					Projectile.alpha -= 5;
				}
				return;
			}
			// find target
			// find closest npc that can be targeted
			Vector2 targetCenter = Projectile.position;
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
							timeToTarget = Vector2.Distance(targetCenter, Projectile.Center) / Projectile.velocity.Length();
							// height used to calculate magnitude
							height = Math.Tan(spread) * distanceFromTarget / 2;
						}
					}
				}
			}
			// shoot towards them
			if (foundTarget) { //  && Projectile.ai[1] < 42
				// Projectile.ai[0] determines if this specific projectile is the straight line one or the curved ones
				if (Projectile.ai[0] != 2) {
					double currentAngle = Math.Atan(Projectile.DirectionTo(targetCenter).Y / Projectile.DirectionTo(targetCenter).X) + spread - (2 * spread * ((Projectile.ai[1] - 42) / timeToTarget));
					Vector2 velocityDirection = new((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
					double magnitude = height * Math.PI / distanceFromTarget * Math.Cos(Math.PI / distanceFromTarget * distanceTravelled);
					Projectile.velocity = Vector2.Multiply(velocityDirection, (float)magnitude);
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