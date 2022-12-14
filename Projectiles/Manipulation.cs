using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Projectiles
{
    public class Manipulation : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Manipulation");
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
            return false;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return target.friendly && Main.myPlayer == Projectile.owner && Main.mouseRight; //&& target.type != mod.NPCType("FireBall"); <- we havent added the npc
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (target.lifeMax / 20) + (target.defense / 2);
            crit = true;
        }
        public override void AI()
        {
            int enpc = (int)Projectile.ai[0] - 1;
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.position = Main.MouseWorld;
                Projectile.netUpdate = true;
            }
            if (enpc >= 0)
            {
                NPC target = Main.npc[enpc];
                if (target.friendly && target.active) //&& target.type != ModContent.NPCType<FireBall>())
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
                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];
                    if (target.friendly && target.active && Projectile.Hitbox.Intersects(target.Hitbox)) // && target.type != mod.NPCType("FireBall"))
                    {
                        enpc = i;
                        Projectile.ai[0] = i + 1;
                    }
                }
            }
            bool channeling = player.channel && !player.noItems && !player.CCed;
            if (!channeling)
            {
                if (enpc >= 0)
                {
                    NPC target = Main.npc[enpc];
                    if (target.friendly && target.active) // && target.type != mod.NPCType("FireBall"))
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
                Projectile.Kill();
            }
        }
    }
}
