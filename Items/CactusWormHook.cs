using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Projectiles;

namespace joostitemport.Items
{
    public class CactusWormHook : ModItem
    {
        public override void SetDefaults()
        {
            //clone and modify the ones we want to copy
            Item.CloneDefaults(ItemID.AmethystHook);
            Item.shootSpeed = 20f; // how quickly the hook is shot.
            Item.shoot = ModContent.ProjectileType<CactusHook>();
            Item.rare = 2;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus Worm Hook");
			Tooltip.SetDefault("Hold the grapple button to have the hook go through tiles\n" + 
                "Let go of the grapple button to grip tile\n" +
                "Pulls you through tiles");
		}

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Cactus, 100);
			recipe.AddIngredient(ItemID.Stinger, 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
    }
}