using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using joostitemport.Items;

namespace joostitemport.Projectiles
{
	public class MechanicalFishHook : FishingGuns
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BobberMechanics);
            itemType = ModContent.ItemType<MechanicalFishingGun>();
            gravMod = 13f;
            lineColor = new Color(250, 100, 80, 100);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechanical Fish Hook");
        }
	}
}