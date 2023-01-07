using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Items;

namespace joostitemport.Projectiles
{
	public class LunarFishHook : FishingGuns
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BobberGolden);
            itemType = ModContent.ItemType<LunarFishingGun>();
            gravMod = 15f;
            lineColor = new Color(34, 221, 151, 100);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Fish Hook");
        }
	}
}