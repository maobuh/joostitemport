using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Projectiles;
using Terraria.DataStructures;

namespace joostitemport.Items
{
    public class ManipulationTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Manipulation");
            Tooltip.SetDefault("Allows you to pick up and move friendly NPCs\n" + "Right click while holding the NPC to rapidly damage the NPC");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.noMelee = true;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.reuseDelay = 5;
            Item.autoReuse = true;
            Item.channel = true;
            Item.useStyle = 4;
            Item.value = 10000;
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<Manipulation>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            damage = 25;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Book);
            recipe.AddIngredient(ItemID.FallenStar);
            recipe.AddIngredient(ItemID.GoldBar);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Book);
            recipe.AddIngredient(ItemID.FallenStar);
            recipe.AddIngredient(ItemID.PlatinumBar);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

    }
}

