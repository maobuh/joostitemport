using joostitemport.Items.Armor;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace joostitemport
{
    public class JoostPlayer : ModPlayer
    {
        public bool useAmmo = true;
        public bool lunarRod;
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            useAmmo = Player.armor[0].netID != ModContent.ItemType<GenjiHelmRanged>();
            Main.NewText(useAmmo);
            return useAmmo;
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (lunarRod && Main.rand.Next(4) == 0)
            {
                itemDrop = 3456 + Main.rand.Next(4);
            }
        }
    }
}