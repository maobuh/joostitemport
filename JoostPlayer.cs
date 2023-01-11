using joostitemport.Items.Armor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using joostitemport.Projectiles;
using joostitemport.Projectiles.Minions;
using Terraria.GameInput;

namespace joostitemport
{
    public class JoostPlayer : ModPlayer
    {
        public bool useAmmo = true;
        public bool gRanged;
        public bool gMelee;
        public bool gSummon;
        public bool gMagic;
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
            if (Player.armor[0].netID != ModContent.ItemType<GenjiHelmSummon>()) {
                gSummon = false;
            }
            if (gSummon && Player.ownedProjectileCounts[ModContent.ProjectileType<EnkiduMinion>()] <= 0) {
                Projectile.NewProjectile(Player.GetSource_Misc("fuck you"), Player.Center, Vector2.Zero, ModContent.ProjectileType<EnkiduMinion>(), 0, 0, Player.whoAmI);
            }
        }
        public override void OnHitAnything(float x, float y, Entity victim)
        {
            // make new masamune if there isnt already one on screen
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
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (joostitemport.ArmorAbilityHotKey.JustPressed && gMagic && !Player.HasBuff(BuffID.ManaSickness) && Player.ownedProjectileCounts[ModContent.ProjectileType<BitterEndFriendly>()] + Player.ownedProjectileCounts[ModContent.ProjectileType<BitterEndFriendly2>()] <= 0 && Player.statMana >= Player.statManaMax2)
            {
                Projectile.NewProjectile(Player.GetSource_ItemUse(Player.armor[0]), Player.Center, Vector2.Zero, ModContent.ProjectileType<BitterEndFriendly>(), (int)Player.GetDamage<MagicDamageClass>().ApplyTo(2000), 20f, Player.whoAmI);
                Player.manaRegenDelay = 180 * (2 + Player.manaRegenDelayBonus);
                Player.statMana *= 0;
            }
        }
    }
}