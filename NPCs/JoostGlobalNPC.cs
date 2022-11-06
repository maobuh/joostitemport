using System;
using System.Configuration;
using joostitemport.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace joostitemport.NPCs
{
    public class JoostGlobalNPC : GlobalNPC
    {
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            // add brawler's glove to travelling merchant shop
            shop[nextSlot] = ModContent.ItemType<BrawlersGlove>();
            nextSlot++;
        }
    }
}