using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.Judicator
{
	public class JudicatorShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Judicator Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 10;
			Projectile.height = 22;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 90;

        }

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);

            if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
            if (Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Vortex"))
            {
                mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
            }
        }
		

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorImpactSound, Projectile.position);

				if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}

				if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}

			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (var i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorImpactSound, Projectile.position);
			}
			mProjectile.DustyDeath(Projectile, 135);
			SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorImpactSound, Projectile.position);
		}

        public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
    public class SpazerJudicatorShot : JudicatorShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Spazer Judicator Shot";

            mProjectile.amplitude = 10f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
            mProjectile.delay = 4;
        }
    }
    public class VortexJudicatorShot : JudicatorShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Vortex Judicator Shot";

            mProjectile.amplitude = 10f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
            mProjectile.delay = 4;
        }
    }
}