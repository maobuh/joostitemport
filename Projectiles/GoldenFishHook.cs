using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Items;

namespace joostitemport.Projectiles
{
	public class GoldenFishHook : FishingGuns
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BobberGolden);
            itemType = ModContent.ItemType<GoldenFishingGun>();
            gravMod = 15f;
            lineColor = new Color(100, 250, 250, 100);
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Fish Hook");
        }
	}
}