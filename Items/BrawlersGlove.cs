using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Projectiles;

namespace joostitemport.Items
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class BrawlersGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Brawler's Glove");
			Tooltip.SetDefault("Punches enemies with your bare hands\n" +
                "Hold right click to grab an enemy\n" +
                "Left click while grabbing to pummel\n" +
                "Release right click to throw");
        }

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.noMelee = true;
			Item.noUseGraphic = true;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.knockBack = 0;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.BrawlersGlove>();
            Item.shootSpeed = 10;
        }

        // makes it so player can use the item with right click
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}