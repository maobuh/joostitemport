using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace joostitemport.Projectiles
{
	public abstract class FishingGuns : ModProjectile
	{
        // fishing pole class name
        protected int itemType;
        protected float gravMod;
        protected Color lineColor;
		public override void SetDefaults()
		{
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}
		public override bool? CanHitNPC(NPC target)
		{
            return !target.friendly;
		}
        public override bool CanHitPvp(Player target)
        {
            return true;
        }
        int hitMob = -1;
        bool pvp;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!Projectile.wet)
            {
                if (hitMob < 0)
                {
                    hitMob = target.whoAmI;
                    pvp = false;
                }
                if (!target.active || target.friendly || target.life <= 0)
                {
                    Projectile.ai[0] = 1;
                }
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (!Projectile.wet)
            {
                if (hitMob < 0)
                {
                    hitMob = target.whoAmI;
                    pvp = true;
                }
                if (!target.active || target.dead)
                {
                    Projectile.ai[0] = 1;
                }
            }
        }
        public override void AI()
        {
            // without this line, the line + bobber does not sync between players
            if (Projectile.owner != Main.myPlayer) return;
            Projectile.netUpdate = true;
            Projectile.netImportant = true;
            Player player = Main.player[Projectile.owner];
            if (hitMob > 0 && Projectile.Distance(player.Center) < 2000 && !player.controlUseItem)
            {
                Projectile.ai[0] = 0;
            }
            if (Projectile.ai[0] > 0)
            {
                hitMob = -1;
            }
            if (pvp && hitMob > 0 && hitMob < 255 && Projectile.ai[0] == 0)
            {
                Player P = Main.player[hitMob];
                Projectile.velocity = P.velocity;
                Projectile.position = P.Center - (Projectile.Size / 2);
                if (!P.active || P.dead)
                {
                    hitMob = -1;
                    pvp = false;
                    Projectile.ai[0] = 1;
                }
            }
            else if (hitMob >= 0 && Projectile.ai[0] == 0)
            {
                NPC npc = Main.npc[hitMob];
                Projectile.velocity = npc.velocity;
                Projectile.position = npc.Center - (Projectile.Size / 2);
                if (!npc.active || npc.friendly || npc.life <= 0)
                {
                    hitMob = -1;
                    Projectile.ai[0] = 1;
                }
            }
            else
            {
                for (int i = 0; i < Main.item.Length; i++)
                {
                    if (Main.item[i].active)
                    {
                        Item I = Main.item[i];
                        if (Projectile.Hitbox.Intersects(I.Hitbox))
                        {
                            I.velocity = Projectile.velocity;
                            I.position = Projectile.Center - (I.Size / 2);
                        }
                    }
                }
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hitMob);
            writer.Write(pvp);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hitMob = reader.ReadInt16();
            pvp = reader.ReadBoolean();
        }
        public override bool PreDrawExtras()      //this draws the fishing line correctly
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.bobber && Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].holdStyle > 0)
            {
                float pPosX = player.MountedCenter.X;
                float pPosY = player.MountedCenter.Y;
                pPosY += Main.player[Projectile.owner].gfxOffY;
                int type = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].type;
                float gravDir = Main.player[Projectile.owner].gravDir;
 
                if (type == itemType) //add your Fishing Pole name here
                {
                    pPosX += 50 * Main.player[Projectile.owner].direction;
                    if (Main.player[Projectile.owner].direction < 0)
                    {
                        pPosX -= 13f;
                    }
                    pPosY -= gravMod * gravDir;
                }
 
                if (gravDir == -1f)
                {
                    pPosY -= 12f;
                }
                Vector2 value = new(pPosX, pPosY);
                value = Main.player[Projectile.owner].RotatedRelativePoint(value + new Vector2(8f), true) - new Vector2(8f);
                float projPosX = Projectile.position.X + (Projectile.width * 0.5f) - value.X;
                float projPosY = Projectile.position.Y + (Projectile.height * 0.5f) - value.Y;
                bool flag2 = true;
                if (projPosX == 0f && projPosY == 0f)
                {
                    flag2 = false;
                }
                else
                {
                    float projPosXY = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    projPosXY = 12f / projPosXY;
                    projPosX *= projPosXY;
                    projPosY *= projPosXY;
                    value.X -= projPosX;
                    value.Y -= projPosY;
                    projPosX = Projectile.position.X + (Projectile.width * 0.5f) - value.X;
                    projPosY = Projectile.position.Y + (Projectile.height * 0.5f) - value.Y;
                }
                while (flag2)
                {
                    float num = 12f;
                    float num2 = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    float num3 = num2;
                    if (float.IsNaN(num2) || float.IsNaN(num3))
                    {
                        flag2 = false;
                    }
                    else
                    {
                        if (num2 < 20f)
                        {
                            num = num2 - 8f;
                            flag2 = false;
                        }
                        num2 = 12f / num2;
                        projPosX *= num2;
                        projPosY *= num2;
                        value.X += projPosX;
                        value.Y += projPosY;
                        projPosX = Projectile.position.X + (Projectile.width * 0.5f) - value.X;
                        projPosY = Projectile.position.Y + (Projectile.height * 0.1f) - value.Y;
                        if (num3 > 12f)
                        {
                            float num4 = 0.3f;
                            float num5 = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
                            if (num5 > 16f)
                            {
                                num5 = 16f;
                            }
                            num5 = 1f - (num5 / 16f);
                            num4 *= num5;
                            num5 = num3 / 80f;
                            if (num5 > 1f)
                            {
                                num5 = 1f;
                            }
                            num4 *= num5;
                            if (num4 < 0f)
                            {
                                num4 = 0f;
                            }
                            num5 = 1f - (Projectile.localAI[0] / 100f);
                            num4 *= num5;
                            if (projPosY > 0f)
                            {
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                            else
                            {
                                num5 = Math.Abs(Projectile.velocity.X) / 3f;
                                if (num5 > 1f)
                                {
                                    num5 = 1f;
                                }
                                num5 -= 0.5f;
                                num4 *= num5;
                                if (num4 > 0f)
                                {
                                    num4 *= 2f;
                                }
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                        }
                        float rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
                        Color color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), lineColor);    //this is the fishing line color in RGB, 200 is red, 12 is green, 50 blue
                        Main.EntitySpriteDraw((Texture2D) TextureAssets.FishingLine, new Vector2(value.X - Main.screenPosition.X + (TextureAssets.FishingLine.Width() * 0.5f),
                                                value.Y - Main.screenPosition.Y + (TextureAssets.FishingLine.Height() * 0.5f)),
                                                new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, TextureAssets.FishingLine.Width(),
                                                (int)num)), color2, rotation2, new Vector2(TextureAssets.FishingLine.Width() * 0.5f, 0f), 1f, SpriteEffects.None, 0);
                    }
                }
            }
            return false;
        }
	}
}