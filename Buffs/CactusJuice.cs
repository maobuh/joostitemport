using Terraria;
using Terraria.ModLoader;

namespace joostitemport.Buffs
{
	public class CactusJuice : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("High on Cactus Juice");
			Description.SetDefault("Minus 5 defense, damage increased by 25%, wont stop moving");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 5;
			player.GetDamage<GenericDamageClass>() += 0.25f;
			player.slippy2 = true;
		}

	}
}
