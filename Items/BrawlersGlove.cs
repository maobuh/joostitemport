using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Items
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class BrawlersGlove : ModItem
    {
        // timer for enforcing useSpeed on right click
        private int timer = 30;
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
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.noMelee = true;
			Item.noUseGraphic = true;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.knockBack = 5;
			Item.useTurn = true;
            Item.shootSpeed = 10;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.BrawlersGlove>();
        }

        public override void HoldItem(Player player)
        {
            Vector2 mouseCoord = new(Main.mouseX + Main.screenPosition.X - player.Center.X, Main.mouseY + Main.screenPosition.Y - player.Center.Y);
            // while the player is right clicking
            if (player.controlUseTile && timer >= 30)
            {
                // turn player around to where mouse is like when you normally use an item
                if (mouseCoord.X > 0)
                {
                    player.direction = 1;
                }
                else
                {
                    player.direction = -1;
                }
                // spawn brawlers glove projectile but stationary and on the player
                Projectile.NewProjectile(player.GetSource_OpenItem(ModContent.ItemType<BrawlersGlove>()), player.Center, Vector2.Zero, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
                // reset timer
                timer = 0;
                // give player a little nudge in the direction of the mouse :) scaled by the players knockback stat times 3 because it feels right?
                player.velocity = Vector2.Multiply(Vector2.Normalize(mouseCoord), player.GetTotalKnockback<MeleeDamageClass>().ApplyTo(Item.knockBack) * 3);
            }
            else if (timer < 30)
            {
                // increment timer if ur not allowed to use right click yet but only up to 30 to avoid overflow
                timer++;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return !player.controlUseTile;
        }
    }
}