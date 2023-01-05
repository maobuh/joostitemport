using Terraria;
using Terraria.ModLoader;

namespace joostitemport.Buffs
{
	public class EnkiduMinion : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enkidu");
			Description.SetDefault("Enkidu will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			JoostPlayer modPlayer = player.GetModPlayer<JoostPlayer>();
			if (!modPlayer.gSummon)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
