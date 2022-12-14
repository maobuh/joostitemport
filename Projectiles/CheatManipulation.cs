using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Projectiles
{
    public class CheatManipulation : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Greater Manipulation");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 4;
            Projectile.netImportant = true;
        }
        public override bool CanHitPvp(Player target)
        {
            return Main.myPlayer == Projectile.owner && Main.mouseRight;
        }
        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            damage = (target.statLifeMax / 20) + (target.statDefense / 2);
            crit = true;
            target.noKnockback = true;
            target.immuneTime = 0;
        }
        public override bool CanHitPlayer(Player target)
        {
            return Main.myPlayer == Projectile.owner && Main.mouseRight;
        }
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = (target.statLifeMax / 20) + (target.statDefense / 2);
            crit = true;
            target.noKnockback = true;
            target.immuneTime = 0;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Main.myPlayer == Projectile.owner && Main.mouseRight;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (target.lifeMax / 20) + (target.defense / 2);
            crit = true;
        }
        public override void AI()
        {
            int enpc = (int)Projectile.ai[0] - 1;
            int pvp = (int)Projectile.ai[1] - 1;
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.position = Main.MouseWorld;
                Projectile.netUpdate = true;
            }
            for (int i = 0; i < Main.item.Length; i++)
            {
                if (Main.item[i].active)
                {
                    Item I = Main.item[i];
                    if (Projectile.Hitbox.Intersects(I.Hitbox) || Projectile.oldPosition == I.Center - I.velocity)
                    {
                        I.velocity = Vector2.Zero;
                        I.position = Projectile.position - I.Size / 2;
                    }
                }
            }
            if (enpc >= 0)
            {
                // moves the target
                NPC target = Main.npc[enpc];
                if (target.active && target.type != NPCID.TargetDummy)
                {
                    target.position = Projectile.position - new Vector2(target.width / 2, target.height / 2);
                    target.netUpdate = true;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            else
            {
                if (pvp >= 0)
                {
                    Player pTarget = Main.player[pvp];
                    if (pTarget.active && !pTarget.dead)
                    {
                        pTarget.position = Projectile.position - new Vector2(pTarget.width / 2, pTarget.height / 2);
                    }
                    else
                    {
                        Projectile.Kill();
                    }
                }
                else
                {
                    for (int i = 0; i < 255; i++)
                    {
                        Player pTarget = Main.player[i];
                        if (pTarget.active && !pTarget.dead && Projectile.Hitbox.Intersects(pTarget.Hitbox))
                        {
                            pvp = i;
                            Projectile.ai[1] = i + 1;
                        }
                    }
                    if (pvp < 0)
                    {
                        // finds which target to move
                        for (int i = 0; i < 200; i++)
                        {
                            NPC target = Main.npc[i];
                            if (target.active && Projectile.Hitbox.Intersects(target.Hitbox) && target.type != NPCID.TargetDummy)
                            {
                                enpc = i;
                                Projectile.ai[0] = i + 1;
                            }
                        }
                    }
                }
            }
            bool channeling = player.channel && !player.noItems && !player.CCed;
            if (!channeling)
            {
                if (enpc >= 0)
                {
                    NPC target = Main.npc[enpc];
                    if (target.active && target.type != NPCID.TargetDummy)
                    {
                        Vector2 vel = Projectile.position - Projectile.oldPosition;
                        if (Projectile.Distance(Projectile.oldPosition) > 25)
                        {
                            vel.Normalize();
                            target.velocity = vel * 25;
                            target.netUpdate = true;
                        }
                        else
                        {
                            target.velocity = vel;
                            target.netUpdate = true;
                        }
                    }
                }
                else if (pvp >= 0)
                {
                    Player pTarget = Main.player[pvp];
                    if (pTarget.active && !pTarget.dead)
                    {
                        Vector2 vel = Projectile.position - Projectile.oldPosition;
                        if (Projectile.Distance(Projectile.oldPosition) > 25)
                        {
                            vel.Normalize();
                            pTarget.velocity = vel * 25;
                        }
                        else
                        {
                            pTarget.velocity = vel;
                        }
                    }
                }
                Projectile.Kill();
            }
        }
    }
}
