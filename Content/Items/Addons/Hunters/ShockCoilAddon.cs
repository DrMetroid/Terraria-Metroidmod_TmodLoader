using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class ShockCoilAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ShockCoil");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				"Fires a short range electric charge that slowly heals\n" +
                "Circuit fails on contact with tiles\n" +
                string.Format("[c/78BE78:+10% damage]\n") +
				string.Format("[c/78BE78:+50% speed]\n") +
                string.Format("[c/BE7878:Incompatible with ice, wave, or plasma effects]\n") +
                string.Format("[c/BE7878:Incompatible Calamitymod (for some reason)]\n"));

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 2500;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.ShockCoilTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonDmg = .1f;
			mItem.addonSpeed = .5f;
		}
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<MissileAddons.BeamCombos.WavebusterAddon>(1)
                .AddIngredient<Miscellaneous.EnergyShard>(30)
                .AddIngredient(ItemID.FallenStar, 30)
                .AddIngredient(ItemID.Sapphire, 30)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}