using joostitemport.Items.Armor;
using Terraria;
using Terraria.ModLoader;

namespace joostitemport
{
    public class JoostPlayer : ModPlayer
    {
        public bool useAmmo = true;

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            useAmmo = Player.armor[0].netID != ModContent.ItemType<GenjiHelmRanged>();
            Main.NewText(useAmmo);
            return useAmmo;
        }
    }
}