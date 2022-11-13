using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Items
{
	public class JoostJuice : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Joost Juice");
			Tooltip.SetDefault("refweshing");
		}
        public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 20;
			Item.height = 26;
			Item.useTime = 17;
			Item.useAnimation = 17;
            Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(1, 0, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item3;
			Item.buffTime = 100000;
			Item.buffType = ModContent.BuffType<Buffs.JoostJuice>();
		}
        public override void AddRecipes()
		{
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ItemID.ChlorophyteOre);
			recipe.AddIngredient(ItemID.RegenerationPotion);
			recipe.AddIngredient(ItemID.SwiftnessPotion);
			recipe.AddIngredient(ItemID.IronskinPotion);
			recipe.AddIngredient(ItemID.HeartreachPotion);
			recipe.AddIngredient(ItemID.LifeforcePotion);
			recipe.AddIngredient(ItemID.EndurancePotion);
			recipe.AddIngredient(ItemID.RagePotion);
			recipe.AddIngredient(ItemID.WrathPotion);
			recipe.AddIngredient(ItemID.WarmthPotion);
			recipe.AddIngredient(ItemID.SummoningPotion);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
    }
}