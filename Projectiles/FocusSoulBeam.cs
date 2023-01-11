using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;
using ReLogic.Content;

namespace joostitemport.Projectiles
{
	public class FocusSoulBeam : ModProjectile
	{
		private const float MAX_CHARGE = 30f;
		private const int MOVE_DISTANCE = 20;
		private const float MAX_DISTANCE = 2000f;
		// Distance that the laser should travel (would be equal to MAX_DISTANCE unless it would collide with a tile)
		public float Distance {
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		// Frames needed for the laser to charge
		public float Charge {
			get => Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}
		public bool IsAtMaxCharge => Charge == MAX_CHARGE;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Souls");
		}
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 120;
		}

        public override bool PreDraw(ref Color lightColor)
		{
			if (IsAtMaxCharge)
			{
				DrawLaser((Texture2D) ModContent.Request<Texture2D>("joostitemport/Projectiles/FocusSoulBeam", (AssetRequestMode)2),
					Projectile.Center, Projectile.velocity, 10,
					(float) Math.PI / -2, 1f, MOVE_DISTANCE);
            }
			return false;
		}

		/// <summary>
		/// Draws the sprite of the laser
		/// </summary>
		public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit,
								float step, float rotation = 0f, float scale = 1f, int transDist = 50)
		{
			float r = unit.ToRotation() + rotation;

			#region Draw laser tail
			Main.EntitySpriteDraw(texture, start + (unit * (transDist - step)) - Main.screenPosition,
				new Rectangle(0, 0, 16, 26), Color.White, r, new Vector2(16 / 2, 26 / 2), scale, 0, 0);
			#endregion

			#region Draw laser body
			for (float i = step; i <= Distance; i += step)
			{
				start += step * unit;
				Main.EntitySpriteDraw(texture, start - Main.screenPosition,
					new Rectangle(0, 26, 16, 26), Color.White, r,
					new Vector2(16 / 2, 26 / 2), scale, 0, 0);
			}
			#endregion

			#region Draw laser head
			start += step * unit;
			Main.EntitySpriteDraw(texture, start - Main.screenPosition,
				new Rectangle(0, 52, 16, 26), Color.White, r, new Vector2(16 / 2, 26 / 2), scale, 0, 0);
			#endregion
		}

		/// <summary>
		/// Change the way of collision check of the Projectile (collision with mobs not tiles)
		/// </summary>
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Charge < MAX_CHARGE) return false;
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + (Projectile.velocity * Distance), 10, ref point);
		}

        /// <summary>
        /// The AI of the Projectile
        /// </summary>
        public override void AI()
        {
            #region Charging process
            Vector2 pos = Projectile.Center - new Vector2(5, 5);
            double rot = Projectile.velocity.ToRotation();
            if (Charge == 0)
            {
                Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
                Projectile.spriteDirection = Projectile.direction;
                rot += 45 * (Math.PI / 180) * Projectile.spriteDirection;
            }
            if (Charge < MAX_CHARGE)
            {
                Charge++;
            }
            else
            {
                rot -= Projectile.spriteDirection * (Math.PI / 180);
            }
            #endregion
			CreateCircleDust(pos, rot);
            if (Charge < MAX_CHARGE) return;
            SetLaserPosition();
			CreateLaserTipDust();
			CastLights();
        }
		// create particle effects on the 4 circles
		private void CreateCircleDust(Vector2 pos, double rot) {
			Projectile.velocity = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));
            int chargeFact = (int)(Charge / 20);
            Vector2 dustVelocity = Vector2.UnitX * 18f;
            dustVelocity = dustVelocity.RotatedBy(Projectile.rotation - 1.57f, default);
            Vector2 spawnPos = Projectile.Center + dustVelocity;
            for (int k = 0; k < chargeFact + 1; k++)
            {
                Vector2 spawn = spawnPos + (((float)Main.rand.NextDouble() * 6.28f).ToRotationVector2() * (12f - (chargeFact * 2)));
                Dust dust = Main.dust[Dust.NewDust(pos, 10, 10, 92, Projectile.velocity.X / 2f,
                    Projectile.velocity.Y / 2f, 0, default, 1f)];
                dust.velocity = Vector2.Normalize(spawnPos - spawn) * 1.5f * (10f - (chargeFact * 2f)) / 10f;
                dust.noGravity = true;
                dust.scale = Main.rand.Next(10, 20) * 0.05f;
            }
		}
		// sets Distance to the distance between the player and where the laser would first collide with a tile
		private void SetLaserPosition() {
			// Set laser tail position and dusts
            for (Distance = MOVE_DISTANCE; Distance <= MAX_DISTANCE; Distance += 5f)
            {
                Vector2 start = Projectile.Center + (Projectile.velocity * Distance);
                if (!Collision.CanHitLine(Projectile.Center, 1, 1, start, 1, 1))
                {
					Distance -= 5f;
                    break;
                }
            }
		}
		// creates particle effects at the tip of the laser (including tile collision)
		private void CreateLaserTipDust() {
			Vector2 dustPos = Projectile.Center;
			for (float i = 10; i <= Distance; i += 10)
			{
				dustPos += 10 * Projectile.velocity;
			}
            for (int i = 0; i < 2; ++i)
            {
                float num1 = Projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f);
                float num2 = (float)((Main.rand.NextDouble() * 0.8f) + 1.0f);
                Vector2 dustVel = new((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, 92, dustVel.X, dustVel.Y)];
                dust.noGravity = true;
                dust.scale = 1.2f;
            }
		}
		// overlaps the laser with a line of light
		private void CastLights() {
			//Add lights
            DelegateMethods.v3_1 = new Vector3(0.1f, 0.8f, 1f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + (Projectile.velocity * (Distance - MOVE_DISTANCE)), 26, new Utils.TileActionAttempt(DelegateMethods.CastLight));
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = Projectile.velocity;
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + (unit * Distance), (Projectile.width + 16) * Projectile.scale, new Utils.TileActionAttempt(DelegateMethods.CutTiles));
		}
	}
}
