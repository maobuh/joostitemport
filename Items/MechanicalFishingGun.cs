using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Projectiles;
using Terraria.DataStructures;

namespace joostitemport.Items
{
    public class MechanicalFishingGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Fishing Shotgun");
            Tooltip.SetDefault("Fires multiple fishing hooks");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 16;
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 0;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = 1;
            Item.value = 10000;
            Item.rare = 3;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = false;
            //Item.shoot = 365;
            Item.shoot = ModContent.ProjectileType<MechanicalFishHook>();
            Item.shootSpeed = 15f;
            Item.fishingPole = 30;
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
            recipe.AddIngredient(ItemID.MechanicsRod, 1);
            recipe.AddTile(TileID.WorkBenches);

            Recipe recipe2 = recipe.Clone();

            recipe.AddIngredient(null, "FishingGun", 1);
            recipe.Register();

            recipe2.AddIngredient(ItemID.Boomstick, 1);
            recipe2.Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const float spread = 45f * 0.0174f;
            float baseSpeed = velocity.Length();
            float startAngle = (float) Math.Atan2(velocity.X, velocity.Y) - (spread / 2);
            const float deltaAngle = spread / 7f;
            for (int i = 0; i < 7; i++)
            {
                float offsetAngle = startAngle + (deltaAngle * i);
                Vector2 direction = new((float)Math.Sin(offsetAngle), (float)Math.Cos(offsetAngle));
                Projectile.NewProjectile(source, position, baseSpeed * direction, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}

