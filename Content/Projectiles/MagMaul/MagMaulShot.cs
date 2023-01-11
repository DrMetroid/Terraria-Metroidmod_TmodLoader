using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.MagMaul
{
	public class MagMaulShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MagMaul Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 14;
            Projectile.tileCollide = true;
        }

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);

            if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 286, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
            if (Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Vortex"))
            {
                mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
            }
        }
		
		public override void Kill(int timeLeft)
		{
			int dustType = 286;
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width += 50;
			Projectile.height += 50;
			Projectile.scale = 2f;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			mProjectile.Diffuse(Projectile, 286);
			SoundEngine.PlaySound(Sounds.Items.Weapons.MagMaulExplode, Projectile.position);
			Projectile.Damage();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
    public class VortexMagMaulShot : MagMaulShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Vortex MagMaul Shot";

            mProjectile.amplitude = 15f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
            mProjectile.delay = 4;
        }
    }
    public class SpazerMagMaulShot : MagMaulShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Spazer MagMaul Shot";

            mProjectile.amplitude = 15f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
            mProjectile.delay = 4;
        }
    }
}
