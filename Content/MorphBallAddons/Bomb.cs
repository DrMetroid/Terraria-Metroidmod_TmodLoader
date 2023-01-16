﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.MorphBallAddons
{
	public class Bomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Bomb/BombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Bomb/BombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Bomb/BombProjectile";

		public override bool AddOnlyAddonItem => false;

		//public override bool CanGenerateOnChozoStatue(int x, int y) => true;

		//public override double GenerationChance(int x, int y) => 30;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Bombs");
			ModProjectile.DisplayName.SetDefault("Morph Ball Bomb");
			Tooltip.SetDefault("-Right Click to set off a bomb");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 10;
			item.value = Item.buyPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.Green;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(2)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(3)
				.AddIngredient(ItemID.Bomb, 15)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
