using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using joostitemport.Projectiles;

namespace joostitemport.Items
{
    public class GoldenFishingGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Fishing Shotgun");
            Tooltip.SetDefault("Fires multiple fishing hooks");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 18;
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 0;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = 1;
            Item.value = 100000;
            Item.rare = 4;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = false;
            //Item.shoot = 364;
            Item.shoot = ModContent.ProjectileType<GoldenFishHook>();
            Item.shootSpeed = 17f;
            Item.fishingPole = 50;
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

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ItemID.GoldenFishingRod);
            recipe.AddTile(TileID.WorkBenches);

            Recipe recipe2 = recipe.Clone();
            Recipe recipe3 = recipe.Clone();

            recipe.AddIngredient(null, "MechanicalFishingGun");
            recipe.Register();

            recipe2.AddIngredient(null, "FishingGun");
            recipe2.Register();

            recipe3.AddIngredient(ItemID.Boomstick);
            recipe3.Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const float spread = 55f * 0.0174f;
            float baseSpeed = velocity.Length();
            float startAngle = (float) Math.Atan2(velocity.X, velocity.Y) - (spread / 2);
            const float deltaAngle = spread / 10f;
            for (int i = 0; i < 10; i++)
            {
                float offsetAngle = startAngle + (deltaAngle * i);
                Vector2 direction = new((float)Math.Sin(offsetAngle), (float)Math.Cos(offsetAngle));
                Projectile.NewProjectile(source, position, baseSpeed * direction, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
