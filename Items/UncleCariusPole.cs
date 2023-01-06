using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Utilities;
using Microsoft.Xna.Framework.Graphics;
using joostitemport.Projectiles;

namespace joostitemport.Items
{
    public class UncleCariusPole : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Uncle Carius's Fishing Pole");
            Tooltip.SetDefault("'This is the pole of ol' Uncle Carius\n" +
            "Does more damage and fires more hooks as you kill bosses throughout the game\n" +
            "Fishing power is equivelent to damage\n" +
            "Right click to throw some fish, velocity increases with boss progression");
        }
        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.ranged = true;
            Item.width = 46;
            Item.height = 40;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = 1;
            Item.value = 300000;
            Item.rare = 9;
            Item.knockBack = 6;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<UncleCariusHook>();
            Item.shootSpeed = 17f;
            //Item.fishingPole = 100; 
            Item.GetGlobalItem<JoostGlobalItem>().glowmaskTex = (Texture2D) ModContent.Request<Texture2D>("JoostMod/Items/UncleCariusPole_String");
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.GetGlobalItem<JoostGlobalItem>().glowmaskColor = new Color(90, 255, (int)(51 + (Main.DiscoG * 0.75f)));
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.GetGlobalItem<JoostGlobalItem>().glowmaskColor = new Color(90, 255, (int)(51 + (Main.DiscoG * 0.75f)));
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ItemID.FiberglassFishingPole);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier statmod)
        {
            // if (joostitemport.instance.battleRodsLoaded)
            // {
            //     statmod.Base *= JoostGlobalItem.LegendaryDamage() * 0.08f * BattleRodsFishingDamage / player.rangedDamage;
            // }
            // else
            // {
                statmod.Base *= JoostGlobalItem.LegendaryDamage() * 0.08f;
            // }
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }
        // public override void GetWeaponDamage(Player player, ref int damage)
        // {
        //     /*if (JoostMod.instance.battleRodsLoaded)
        //     {
        //         damage = (int)Math.Round((double)damage * JoostGlobalItem.LegendaryDamage() * 0.08f * BattleRodsFishingDamage / player.rangedDamage);
        //     }
        //     else
        //     {
        //         damage = (int)Math.Round((double)damage * JoostGlobalItem.LegendaryDamage() * 0.08f);
        //     }*/
        //     Item.fishingPole = damage;
        // }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            if (joostitemport.instance.battleRodsLoaded)
            {
                crit += BattleRodsCrit - player.GetCritChance(DamageClass.Ranged);
            }
        }
        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(0, 255, (int)(51 + (Main.DiscoG * 0.75f)));
                }
            }
            if (JoostMod.instance.battleRodsLoaded)
            {
                Player player = Main.player[Main.myPlayer];
                int dmg = list.FindIndex(x => x.Name == "Damage");
                list.RemoveAt(dmg);
                list.Insert(dmg, new TooltipLine(mod, "Damage", player.GetWeaponDamage(Item) + " Fishing damage"));
            }
        }
        public float BattleRodsFishingDamage
        {
            get { Player player = Main.player[Main.myPlayer]; return player.GetModPlayer<UnuBattleRods.FishPlayer>().bobberDamage; }
        }
        public int BattleRodsCrit
        {
            get { Player player = Main.player[Main.myPlayer]; return player.GetModPlayer<UnuBattleRods.FishPlayer>().bobberCrit; }
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.reuseDelay = 40;
            }
            else
            {
                Item.reuseDelay = 0;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int num = 3 + //3
                (NPC.downedSlimeKing ? 1 : 0) + //4
                (NPC.downedBoss1 ? 1 : 0) + //5
                (NPC.downedBoss2 ? 1 : 0) + //6
                (NPC.downedQueenBee ? 1 : 0) + //7
                (NPC.downedBoss3 ? 1 : 0) + // 8
                (JoostWorld.downedCactusWorm ? 1 : 0) +//9
                (Main.hardMode ? 1 : 0) + //10
                (NPC.downedMechBoss1 ? 1 : 0) + //11
                (NPC.downedMechBoss2 ? 1 : 0) + //12
                (NPC.downedMechBoss3 ? 1 : 0) + //13
                (NPC.downedPlantBoss ? 1 : 0) + //14
                (NPC.downedGolemBoss ? 1 : 0) + //15
                (NPC.downedFishron ? 1 : 0) + //16
                (NPC.downedAncientCultist ? 1 : 0) + //17
                (NPC.downedTowerNebula ? 1 : 0) + //18
                (NPC.downedTowerSolar ? 1 : 0) + //19
                (NPC.downedTowerVortex ? 1 : 0) + //20
                (NPC.downedTowerStardust ? 1 : 0) + //21
                (NPC.downedMoonlord ? 4 : 0) + //25
                (JoostWorld.downedJumboCactuar ? 5 : 0) + //30
                (JoostWorld.downedSAX ? 5 : 0) + //35
                (JoostWorld.downedGilgamesh ? 5 : 0); //40
            if (player.altFunctionUse == 2)
            {
                int numberProjectiles = 4 + Main.rand.Next(3);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12));
                    float scale = 1f - (Main.rand.NextFloat() * .3f);
                    perturbedSpeed = perturbedSpeed * scale * 0.04f * num;
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("UncleCariusFish"), (int)(damage * 1.5f), knockBack, player.whoAmI);
                }
            }
            else
            {
                float spread = (float)num * 2 * 0.0174f;
                float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                double startAngle = Math.Atan2(speedX, speedY) - spread / 2;
                double deltaAngle = spread / num;
                double offsetAngle;
                int i;
                for (i = 0; i < num; i++)
                {
                    offsetAngle = startAngle + deltaAngle * i;
                    Projectile.NewProjectile(position.X, position.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), type, damage, 0, player.whoAmI);
                }
            }
            return false;
        }
    }
}

