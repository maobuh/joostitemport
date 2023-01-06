using System;
using System.Configuration;
using joostitemport.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

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
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DukeFishRod>(), 4));
            }
        }
    }
}