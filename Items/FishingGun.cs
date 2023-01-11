using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Projectiles;
using Terraria.DataStructures;

namespace joostitemport.Items
{
    public class FishingGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fishing Shotgun");
            Tooltip.SetDefault("Fires multiple fishing hooks");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 14;
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 0;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = 1;
            Item.value = 1000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Fishhook>();
            Item.shootSpeed = 11f;
            Item.fishingPole = 15;
        }

        // public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        // {
        //     if (JoostMod.instance.battleRodsLoaded)
        //     {
        //         mult *= BattleRodsFishingDamage / player.GetDamage(DamageClass.Ranged);
        //     }
        // }
        // public override void GetWeaponCrit(Player player, ref int crit)
        // {
        //     if (JoostMod.instance.battleRodsLoaded)
        //     {
        //         crit += BattleRodsCrit - player.GetCritChance(DamageClass.Ranged);
        //     }
        // }
        // public float BattleRodsFishingDamage
        // {
        //     get { Player player = Main.player[Main.myPlayer]; return player.GetModPlayer<UnuBattleRods.FishPlayer>().bobberDamage; }
        // }
        // public int BattleRodsCrit
        // {
        //     get { Player player = Main.player[Main.myPlayer]; return player.GetModPlayer<UnuBattleRods.FishPlayer>().bobberCrit; }
        // }
        // public override void ModifyTooltips(List<TooltipLine> list)
        // {
        //     if (JoostMod.instance.battleRodsLoaded)
        //     {
        //         Player player = Main.player[Main.myPlayer];
        //         int dmg = list.FindIndex(x => x.Name == "Damage");
        //         list.RemoveAt(dmg);
        //         list.Insert(dmg, new TooltipLine(mod, "Damage", player.GetWeaponDamage(Item) + " Fishing damage"));
        //     }
        // }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ItemID.Boomstick, 1);
            recipe.AddIngredient(ItemID.ReinforcedFishingPole, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const float spread = 45f * 0.0174f;
            float baseSpeed = velocity.Length();
            float startAngle = (float) Math.Atan2(velocity.X, velocity.Y) - (spread / 2);
            const float deltaAngle = spread / 4f;
            for (int i = 0; i < 4; i++)
            {
                float offsetAngle = startAngle + (deltaAngle * i);
                Vector2 direction = new((float)Math.Sin(offsetAngle), (float)Math.Cos(offsetAngle));
                Projectile.NewProjectile(source, position, baseSpeed * direction, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}

