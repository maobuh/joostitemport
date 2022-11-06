using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Items
{
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
            Item.useTime = 1;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useAnimation = 5;
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 0;
			Item.autoReuse = true;
			Item.useTurn = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            player.UpdateDead();
            if (!player.releaseRight || !player.releaseLeft)
            {
                player.bodyVelocity = Vector2.Multiply(player.bodyVelocity, 2000000);
            }
        }

        // public override bool AltFunctionUse(Player player)
        // {
        //     while (player.altFunctionUse == 2)
        //     {

        //     }
        //     return true;
        // }
    }
}