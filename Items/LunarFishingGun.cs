using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Projectiles;
using Terraria.DataStructures;

namespace joostitemport.Items
{
    public class LunarFishingGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunar Fishing Shotgun");
            Tooltip.SetDefault("Fires many fishing hooks\n" + "Can fish up Lunar Fragments");
        }
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 20;
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 0;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = 1;
            Item.value = 2000000;
            Item.rare = 10;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<LunarFishHook>();
            Item.shootSpeed = 19f;
            Item.fishingPole = 100;
        }

        // public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        // {
        //     if (JoostMod.instance.battleRodsLoaded)
        //     {
        //         mult *= BattleRodsFishingDamage / player.rangedDamage;
        //     }
        // }
        // public override void GetWeaponCrit(Player player, ref int crit)
        // {
        //     if (JoostMod.instance.battleRodsLoaded)
        //     {
        //         crit += BattleRodsCrit - player.rangedCrit;
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

        public override void HoldItem(Player player)
        {
            player.GetModPlayer<JoostPlayer>().lunarRod = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(null, "LunarRod", 1);
            recipe.AddTile(TileID.WorkBenches);

            Recipe recipe2 = recipe.Clone();
            Recipe recipe3 = recipe.Clone();
            Recipe recipe4 = recipe.Clone();
            Recipe recipe5 = recipe.Clone();
            Recipe recipe6 = recipe.Clone();

            // recipe.AddIngredient(null, "SuperFishingGun", 1);
            // recipe.Register();

            recipe2.AddIngredient(null, "DukeFishingGun", 1);
            recipe2.Register();

            recipe3.AddIngredient(null, "GoldenFishingGun", 1);
            recipe3.Register();

            recipe4.AddIngredient(null, "MechanicalFishingGun", 1);
            recipe4.Register();

            recipe5.AddIngredient(null, "FishingGun", 1);
            recipe5.Register();

            recipe6.AddIngredient(ItemID.Boomstick, 1);
            recipe6.Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const float spread = 70f * 0.0174f;
            float baseSpeed = velocity.Length();
            float startAngle = (float) Math.Atan2(velocity.X, velocity.Y) - (spread / 2);
            const float deltaAngle = spread / 20f;
            for (int i = 0; i < 20; i++)
            {
                float offsetAngle = startAngle + (deltaAngle * i);
                Vector2 direction = new((float)Math.Sin(offsetAngle), (float)Math.Cos(offsetAngle));
                Projectile.NewProjectile(source, position, baseSpeed * direction, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}

