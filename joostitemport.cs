using Terraria.ModLoader;

namespace joostitemport
{
	public class joostitemport : Mod
	{
		internal static ModKeybind ArmorAbilityHotKey;
		public override void Load()
		{
			ArmorAbilityHotKey = KeybindLoader.RegisterKeybind(ModContent.GetInstance<joostitemport>(), "ArmorAbilityHotKey", "Z");
		}
	}
}