using joostitemport.Items.Armor;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using joostitemport.Projectiles;

namespace joostitemport
{
    public class JoostPlayer : ModPlayer
    {
        public bool useAmmo = true;
        public bool gRanged;
        public bool gMelee;
        public int masamuneDelay;
        public override void PostUpdateEquips()
        {
            if (gRanged) {
                Player.GetDamage<RangedDamageClass>() *= 1 + (Player.statDefense * 0.005f);
                Player.statDefense = 0;
            }
            if (Player.armor[0].netID != ModContent.ItemType<GenjiHelmMelee>()) {
                gMelee = false;
            }
        }
        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if (Player.armor[0].netID == ModContent.ItemType<GenjiHelmMelee>() && gMelee && Player.HeldItem.DamageType == DamageClass.Melee && Player.ownedProjectileCounts[ModContent.ProjectileType<Masamune>()] < 1) {
                Projectile.NewProjectile(Player.GetSource_OnHit(victim), Player.Center, Vector2.Zero, ModContent.ProjectileType<Masamune>(), (int)(500 * Player.GetDamage(DamageClass.Generic).Multiplicative * (Player.GetDamage(DamageClass.Generic).Additive + Player.GetTotalDamage<MeleeDamageClass>().Additive - 1f) * Player.GetDamage(DamageClass.Melee).Multiplicative), 5f, Player.whoAmI);
                Projectile.NewProjectile(Player.GetSource_OnHit(victim), Player.Center, Vector2.Zero, ModContent.ProjectileType<Masamune>(), 0, 0, Player.whoAmI);
            }
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            useAmmo = Player.armor[0].netID != ModContent.ItemType<GenjiHelmRanged>();
            return useAmmo;
        }
    }
}