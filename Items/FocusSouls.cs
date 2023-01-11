using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Items
{
    public class FocusSouls : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Souls");
            Tooltip.SetDefault("Fires multiple focused beams of souls");
        }
		public override void SetDefaults()
		{
			Item.damage = 180;
			Item.DamageType= DamageClass.Magic;
            Item.mana = 100;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.reuseDelay = 5;
			Item.useStyle = 4;
			Item.knockBack = 2;
			Item.value = 500000;
			Item.rare = 8;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.FocusSouls>();
			Item.shootSpeed = 5f;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] == 0;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Ectoplasm, 50);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}

