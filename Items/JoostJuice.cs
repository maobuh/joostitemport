using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.Items
{
	public class JoostJuice : ModItem
	{
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Joost Juice"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("refweshing");
		}
        public override void SetDefaults()
        {
            Item.potion = true;
            Item.useTime = 17;
        }
    }
}