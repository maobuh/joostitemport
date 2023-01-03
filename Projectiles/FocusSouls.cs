using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ReLogic.Content;

namespace joostitemport.Projectiles
{
	public class FocusSouls : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Souls");
        }
		public override void SetDefaults()
		{
			Projectile.width = 104;
			Projectile.height = 104;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 114;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void AI()
        {
	        Player player = Main.player[Projectile.owner];
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.velocity = Vector2.Zero;
            Projectile.localAI[0]++;
            if (!player.noItems && !player.CCed)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    //Projectile.ai[0]++;
                    float scaleFactor6 = 1f;
                    if (player.inventory[player.selectedItem].shoot == Projectile.type)
                    {
                        scaleFactor6 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
                    }
                    Vector2 vector13 = Main.MouseWorld - Projectile.Center;
                    vector13.Normalize();
                    if (vector13.HasNaNs())
                    {
                        vector13 = Vector2.UnitX * player.direction;
                    }
                    vector13 *= scaleFactor6;
                    if (vector13.X != Projectile.velocity.X || vector13.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = vector13;
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.localAI[0] == 15)
            {
                SoundEngine.PlaySound(SoundID.Item132, Projectile.position);
            }
            if (Projectile.localAI[0] < 30)
            {
                Projectile.alpha -= 6;
            }
            if (Projectile.localAI[0] > 30 && Projectile.localAI[0] < 33 && player.channel && !player.noItems && !player.CCed)
            {
                Projectile.localAI[0] = 31;
            }
            if (Projectile.localAI[0] == 33)
            {
                SoundEngine.PlaySound(SoundID.Item132, Projectile.position);
                double rot = Projectile.rotation + (-35f * (Math.PI / 180));
                Vector2 pos1 = Projectile.Center + new Vector2((float)Math.Cos(rot) * 24, (float)Math.Sin(rot) * 24);
                rot = Projectile.rotation + (35f * (Math.PI / 180));
                Vector2 pos2 = Projectile.Center + new Vector2((float)Math.Cos(rot) * 26, (float)Math.Sin(rot) * 26);
                rot = Projectile.rotation + (145f * (Math.PI / 180));
                Vector2 pos3 = Projectile.Center + new Vector2((float)Math.Cos(rot) * 26, (float)Math.Sin(rot) * 26);
                rot = Projectile.rotation + (-145f * (Math.PI / 180));
                Vector2 pos4 = Projectile.Center + new Vector2((float)Math.Cos(rot) * 24, (float)Math.Sin(rot) * 24);

                Vector2 mousePos = Main.MouseWorld;
                if (Main.myPlayer == Projectile.owner)
                {
                    mousePos = Main.MouseWorld;
                }

                Vector2 dir1 = mousePos - pos1;
                dir1.Normalize();
                Vector2 dir2 = mousePos - pos2;
                dir2.Normalize();
                Vector2 dir3 = mousePos - pos3;
                dir3.Normalize();
                Vector2 dir4 = mousePos - pos4;
                dir4.Normalize();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos1.X, pos1.Y, dir1.X, dir1.Y,
                                            ModContent.ProjectileType<FocusSoulBeam>(), Projectile.damage,
                                            Projectile.knockBack, Projectile.owner, 0f, Main.rand.Next(-30, 2));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos2.X, pos2.Y, dir2.X, dir2.Y,
                                            ModContent.ProjectileType<FocusSoulBeam>(), Projectile.damage,
                                            Projectile.knockBack, Projectile.owner, 0f, Main.rand.Next(-30, 2));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos3.X, pos3.Y, dir3.X, dir3.Y,
                                            ModContent.ProjectileType<FocusSoulBeam>(), Projectile.damage,
                                            Projectile.knockBack, Projectile.owner, 0f, Main.rand.Next(-30, 2));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos4.X, pos4.Y, dir4.X, dir4.Y,
                                            ModContent.ProjectileType<FocusSoulBeam>(), Projectile.damage,
                                            Projectile.knockBack, Projectile.owner, 0f, Main.rand.Next(-30, 2));
                Projectile.netUpdate = true;
            }
            if (Projectile.localAI[0] == 60)
            {
                SoundEngine.PlaySound(SoundID.Item132, Projectile.position);
            }
            if (Projectile.timeLeft <= 10)
            {
                Projectile.alpha += 16;
            }
            if (Projectile.localAI[0] < 33)
            {
                Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
                Projectile.spriteDirection = Projectile.direction;
                Projectile.position = center - Projectile.velocity - (Projectile.Size / 2f) + new Vector2(0, -50 * player.gravDir);
                Projectile.localAI[1] = Projectile.velocity.ToRotation() + 1.57f;
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemTime = 10;
                player.itemAnimation = 10;
                Projectile.timeLeft = 130;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
            }
            Projectile.rotation = Projectile.localAI[1];
        }
        public override bool CanHitPvp(Player target)
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = (Texture2D) ModContent.Request<Texture2D>("joostitemport/Projectiles/FocusSouls", (AssetRequestMode)2);
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Color color = Color.White * (1f - (Projectile.alpha / 255f));
            //color.A = (byte)Projectile.alpha;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), color, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale, effects, 0);
            return false;
        }
    }
}
