using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Items;

namespace joostitemport.Projectiles
{
	public class Fishhook : FishingGuns
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BobberReinforced);
            itemType = ModContent.ItemType<FishingGun>();
            gravMod = 12f;
            lineColor = new Color(200, 200, 200, 100);
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fish Hook");
		}
	}
}