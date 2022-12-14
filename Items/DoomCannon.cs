using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Items
{
	public class DoomCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doom Cannon");
			Tooltip.SetDefault("Charge up a deadly skull shot\n" + "Max charge tunnels through and destroys tiles\n" + "Right click to cancel the shot");
		}
		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 76;
			Item.height = 46;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.knockBack = 8f;
			Item.value = 70000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item73;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.DoomCannon>();
			Item.shootSpeed = 1f;
		}
        public override bool CanUseItem(Player player)
        {
            if (player.controlUseTile)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
        public override void ModifyTooltips(List<TooltipLine> list)
		{
			foreach (TooltipLine line2 in list)
			{
				if (line2.Mod == "Terraria" && line2.Name == "ItemName")
				{
					line2.OverrideColor = new Color(230, 204, 128);
				}
			}
		}
	}
}

