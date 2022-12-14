using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using joostitemport.Items;

namespace joostitemport.Projectiles
{
	public class DukeFishHook2 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BobberGolden);
		}

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duke Fish hook");
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
 
                if (type == ModContent.ItemType<DukeFishRod>()) //add your Fishing Pole name here
                {
                    pPosX += 50 * Main.player[Projectile.owner].direction;
                    if (Main.player[Projectile.owner].direction < 0)
                    {
                        pPosX -= 13f;
                    }
                    pPosY -= 30f * gravDir;
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
                        Color color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Color(255, 249, 183, 100));
 
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