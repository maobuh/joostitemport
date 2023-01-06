using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Items;

namespace joostitemport.Projectiles
{
	public class DukeFishHook : FishingGuns
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BobberGolden);
            itemType = ModContent.ItemType<DukeFishingGun>();
            gravMod = 15f;
            lineColor = new Color(255, 249, 183, 100);
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DukeFishHook");
        }
	}
}