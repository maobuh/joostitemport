using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ReLogic.Content;

namespace joostitemport.Projectiles
{
    public class CactusHook : ModProjectile
    {
        private bool isHooked;
        private bool canGrab = true;
        private bool retreat;
        private readonly float pullSpeed = 20f;
        private readonly float retreatSpeed = 40;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.light = 10f;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(isHooked);
            writer.Write(canGrab);
            writer.Write(retreat);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            isHooked = reader.ReadBoolean();
            canGrab = reader.ReadBoolean();
            retreat = reader.ReadBoolean();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cactus Worm Hook");
        }

        public override bool? SingleGrappleHook(Player player)
        {
            return true;
        }

        public override float GrappleRange()
        {
            return 1000f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead || (Vector2.Distance(player.Center, Projectile.Center) > GrappleRange() && !isHooked))
            {
                retreat = true;
                isHooked = false;
                canGrab = false;
            }
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 directionVector = new(Projectile.position.X + (float)(Projectile.width * 0.5f), Projectile.position.Y + (float)(Projectile.height * 0.5f));
            float directionX = mountedCenter.X - directionVector.X;
            float directionY = mountedCenter.Y - directionVector.Y;
            Projectile.rotation = (float)Math.Atan2((double)directionY, (double)directionX) - (float)(Math.PI / 2);
            Projectile.direction = (Projectile.rotation < 0 && Projectile.rotation > -1 * Math.PI) ? -1 : 1;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.soundDelay--;
            if (retreat)
            {
                Projectile.velocity = Projectile.DirectionTo(player.Center) * retreatSpeed;
                canGrab = player.releaseHook;
                if (Vector2.Distance(player.Center, Projectile.Center) < 16 && !isHooked)
                {
                    Projectile.Kill();
                }
            }
            int startPosX = (int)(Projectile.position.X / 16f) - 1;                         // originally named num124
            int endPosX = (int)((Projectile.position.X + Projectile.width) / 16f) + 2;      // originally named num125
            int startPosY = (int)(Projectile.position.Y / 16f) - 1;                         // originally named num126
            int endPosY = (int)((Projectile.position.Y + Projectile.height) / 16f) + 2;     // originally named num127
            if (startPosX < 0)
            {
                startPosX = 0;
            }
            if (endPosX > Main.maxTilesX)
            {
                endPosX = Main.maxTilesX;
            }
            if (startPosY < 0)
            {
                startPosY = 0;
            }
            if (endPosY > Main.maxTilesY)
            {
                endPosY = Main.maxTilesY;
            }
            bool flag3 = true;  // checks if the player has reached the hook
            // i think this is player go in
            if (isHooked)
            {
                player.rocketTime = player.rocketTimeMax;
                player.rocketDelay = 0;
                player.rocketFrame = false;
                player.canRocket = false;
                player.rocketRelease = false;
                player.fallStart = (int)(player.Center.Y / 16f);    // so the player doesnt take fall damage 
                player.wingTime = 0;
                Projectile.ai[0] = 2f;
                Projectile.velocity = default;
                Projectile.timeLeft = 2;

                // prevent player from needing to use a double jump item to get out of hook
                // not sure if theres a better way to do this
                player.canJumpAgain_Blizzard = false;
                player.canJumpAgain_Cloud = false;
                player.canJumpAgain_Fart = false;
                player.canJumpAgain_Sail = false;
                player.canJumpAgain_Sandstorm = false;

                for (int iX = startPosX; iX < endPosX; iX++)
                {
                    for (int iY = startPosY; iY < endPosY; iY++)
                    {
                        Vector2 currPos;    // originally vector9
                        currPos.X = iX * 16;
                        currPos.Y = iY * 16;
                        float projCentreX = Projectile.position.X + (Projectile.width / 2);
                        float projCentreY = Projectile.position.Y + (Projectile.height / 2);
                        if (projCentreX > currPos.X && projCentreX < currPos.X + 16f
                            && projCentreY > currPos.Y && projCentreY < currPos.Y + 16f
                            && Main.tile[iX, iY].HasUnactuatedTile && (Main.tileSolid[Main.tile[iX, iY].TileType]
                            || Main.tile[iX, iY].TileType == TileID.MinecartTrack))
                        {
                            flag3 = false;
                        }
                    }
                }
                if (flag3)
                {
                    isHooked = false;
                }
                retreat = false;
                player.velocity = player.DirectionTo(Projectile.Center) * pullSpeed;
                if (player.itemAnimation == 0)
                {
                    if (player.velocity.X > 0)
                    {
                        player.direction = 1;
                    }
                    if (player.velocity.X < 0)
                    {
                        player.direction = -1;
                    }
                }
                if (Vector2.Distance(player.Center, Projectile.Center) > 16)
                {
                    // this is the line that changes the path of the pull
                    player.position += player.velocity;
                    if (Projectile.soundDelay <= 0 && Collision.SolidCollision(player.position, player.width, player.height))
                    {
                        Projectile.soundDelay = 20;
                        SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
                    }
                }
                else
                {
                    player.position.X = Projectile.Center.X - (player.width / 2);
                    player.position.Y = Projectile.Center.Y - (player.height / 2);
                    player.velocity = Vector2.Zero;
                }
                if (!player.releaseJump) {
                    isHooked = false;

                    // reset players double jumps
                    // player.RefreshDoubleJumps() is private :(
                    player.canJumpAgain_Blizzard = true;
                    player.canJumpAgain_Cloud = true;
                    player.canJumpAgain_Fart = true;
                    player.canJumpAgain_Sail = true;
                    player.canJumpAgain_Sandstorm = true;
                }
            }
            // i think this is hook go out
            else
            {
                Projectile.ai[0] = 0f;
                for (int iX = startPosX; iX < endPosX; iX++)
                {
                    int iY = startPosY;
                    while (iY < endPosY)
                    {
                        Vector2 currPos;        // originally vector8
                        currPos.X = iX * 16;
                        currPos.Y = iY * 16;
                        if (Projectile.position.X + Projectile.width > currPos.X && Projectile.position.X < currPos.X + 16f
                            && Projectile.position.Y + Projectile.height > currPos.Y && Projectile.position.Y < currPos.Y + 16f
                            && Main.tile[iX, iY].HasUnactuatedTile && (Main.tileSolid[Main.tile[iX, iY].TileType] || Main.tile[iX, iY].TileType == TileID.MinecartTrack))
                        {
                            if (canGrab && !player.controlHook)
                            {
                                Projectile.velocity.X = 0f;
                                Projectile.velocity.Y = 0f;
                                SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
                                isHooked = true;
                                Projectile.position.X = (iX * 16) + 8 - (Projectile.width / 2);
                                Projectile.position.Y = (iY * 16) + 8 - (Projectile.height / 2);
                                Projectile.netUpdate = true;
                            }
                            else
                            {
                                if (Projectile.soundDelay <= 0)
                                {
                                    Projectile.soundDelay = 20;
                                    SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
                                }
                            }
                            break;
                        }
                        else
                        {
                            iY++;
                        }
                    }
                    if (isHooked)
                    {
                        break;
                    }
                }
            }

            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 projCentre = new(Projectile.position.X + (float)(Projectile.width * 0.5f), Projectile.position.Y + (float)(Projectile.height * 0.5f)); // originally vector14
            float distX = mountedCenter.X - projCentre.X;   // originally num84
            float distY = mountedCenter.Y - projCentre.Y;   // originally num85
            float rotation13 = (float)Math.Atan2((double)distY, (double)distX) - 1.57f;
            while (true)
            {
                float distance = (float)Math.Sqrt((double)((distX * distX) + (distY * distY))); // originally num86
                if (distance < 30f || float.IsNaN(distance))
                {
                    break;
                }
                else
                {
                    distance = 24f / distance;
                    distX *= distance;
                    distY *= distance;
                    projCentre.X += distX;
                    projCentre.Y += distY;
                    distX = mountedCenter.X - projCentre.X;
                    distY = mountedCenter.Y - projCentre.Y;
                    Color color15 = Lighting.GetColor((int)projCentre.X / 16, (int)(projCentre.Y / 16f));
                    const SpriteEffects effects = SpriteEffects.None;
                    Main.spriteBatch.Draw((Texture2D) ModContent.Request<Texture2D>("joostitemport/Projectiles/CactusHookChain", (AssetRequestMode)2),
                                            new Vector2(projCentre.X - Main.screenPosition.X, projCentre.Y - Main.screenPosition.Y),
                                            new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 24, 16)),
                                            color15, rotation13, new Vector2(12, 24),
                                            1f, effects, 0f);
                }
            }
            return true;
        }
    }
}