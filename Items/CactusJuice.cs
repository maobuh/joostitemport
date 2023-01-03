using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Items
{
    public class CactusJuice : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cactus Juice");
            Tooltip.SetDefault("'It'll quench ya'\n" +
            "'Nothings quenchier'\n" +
            "'It's the quenchiest!");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 30;
            Item.consumable = true;
            Item.width = 20;
            Item.height = 26;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = 2;
            Item.knockBack = 5;
            Item.value = 750;
            Item.rare = 2;
            Item.UseSound = SoundID.Item3;
            Item.buffTime = 7200;
            Item.buffType = ModContent.BuffType<Buffs.CactusJuice>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ItemID.Cactus);
            recipe.AddIngredient(ItemID.Deathweed);
            recipe.AddIngredient(ItemID.Bottle, 5);
            recipe.ReplaceResult(this, 5);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }

    }
}

