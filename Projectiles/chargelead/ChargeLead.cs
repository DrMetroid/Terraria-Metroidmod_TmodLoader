using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.Projectiles;

namespace MetroidMod.Projectiles.chargelead
{
	public class ChargeLead : MProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Charge Attack";
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.timeLeft = 8800;
			projectile.ownerHitCheck = true;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
		}

		public string Shot = "PowerBeamShot",
				ChargeShot = "PowerBeamChargeShot",
				ShotSound = "PowerBeamSound",
				ChargeShotSound = "PowerBeamChargeSound",
				ChargeUpSound = "ChargeStartup_Power",
				ChargeTex = "ChargeLead";
		public int ShotAmt = 1,
				ChargeShotAmt = 1,
				DustType = 64,
				waveDir = -1;
		public Color DustColor = default(Color),
				LightColor = MetroidMod.powColor;
		public bool IsChargeV2 = false;

		bool soundPlayed = false;
		SoundEffectInstance soundInstance;
		public override void AI()
		{
			Projectile P = projectile;
			Player O = Main.player[P.owner];
			
			MPlayer mp = O.GetModPlayer<MPlayer>(mod);
			
			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			if (O.gravDir == -1f)
			{
				MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			
			P.scale = mp.statCharge / MPlayer.maxCharge;
			float targetrotation = (float)Math.Atan2((MY-oPos.Y),(MX-oPos.X));
			P.rotation += 0.5f * P.direction;
			O.itemTime = 2;
			O.itemAnimation = 2;
			Item I = O.inventory[O.selectedItem];
			int range = I.width+4;
			int width = (I.width/2)-(P.width/2);
			int height = (I.height/2)-(P.height/2);
			
			float dmgMult = (1f+((float)mp.statCharge*0.02f));
			int damage = (int)((float)I.damage*O.rangedDamage);
			
			Vector2 iPos = O.itemLocation;

			P.friendly = false;
			P.damage = 0;
			/*if(mp.somersault)
			{
				P.alpha = 255;
				if(mp.statCharge >= MPlayer.maxCharge && mp.SMoveEffect <= 0)
				{
					P.friendly = true;
					P.damage = damage*3*ChargeShotAmt;
					mp.overheatDelay = (I.useTime*2);
				}
				P.position.X = oPos.X-P.width/2;
				P.position.Y = oPos.Y-P.height/2;
				if(O.controlLeft)
				{
					O.direction = -1;
				}
				if(O.controlRight)
				{
					O.direction = 1;
				}
			}
			else
			{*/
				P.position = new Vector2(iPos.X+(float)Math.Cos(targetrotation)*range+width,iPos.Y+(float)Math.Sin(targetrotation)*range+height);
				P.alpha = 0;
				if(P.velocity.X < 0)
				{
					P.direction = -1;
				}
				else
				{
					P.direction = 1;
				}
				P.spriteDirection = P.direction;
				O.direction = P.direction;
			/*}
			P.position.X += (float)(P.width / 2);
			P.position.Y += (float)(P.height / 2);
			P.width = mp.somersault?50:16;
			P.height = mp.somersault?60:16;
			P.position.X -= (float)(P.width / 2);
			P.position.Y -= (float)(P.height / 2);*/
			
			O.heldProj = P.whoAmI;
			O.itemRotation = (float)Math.Atan2((MY-oPos.Y)*O.direction,(MX-oPos.X)*O.direction) - O.fullRotation;

			P.position -= P.velocity;
			P.timeLeft = 60;
			if(O.whoAmI == Main.myPlayer)
			{
				if(mp.statCharge == 10)
				{
					soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/"+ChargeUpSound));
				}
				else if(mp.statCharge >= MPlayer.maxCharge && !soundPlayed)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/ChargeMax"));
					if(soundInstance != null)
					{
						soundInstance.Stop(true);
					}
					soundPlayed = true;
				}
			}
			if(mp.statCharge >= MPlayer.maxCharge)// && !mp.somersault)
			{
				int dust = Dust.NewDust(P.position+P.velocity, P.width, P.height, DustType, 0, 0, 100, DustColor, 2.0f);
				Main.dust[dust].noGravity = true;
			}
			Lighting.AddLight(P.Center, (LightColor.R/255f)*P.scale,(LightColor.G/255f)*P.scale,(LightColor.B/255f)*P.scale);
			if((O.channel || O.controlUseItem) && /* !mp.ballstate && !mp.shineActive &&*/ !O.dead && !O.noItems)
			{
				if (P.owner == Main.myPlayer)
				{
					P.velocity = targetrotation.ToRotationVector2()*O.inventory[O.selectedItem].shootSpeed;
				}
			}
			else
			{
				if(O.whoAmI == Main.myPlayer)
				{
					if(soundInstance != null)
					{
						soundInstance.Stop(true);
					}
					soundPlayed = false;
				}
				P.Kill();
			}
		}
		/*public override bool PreKill(int timeLeft)
		{
			Projectile P = projectile;
			Player O = Main.player[P.owner];
			MPlayer mp = O.GetModPlayer<MPlayer>(mod);

			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			if (O.gravDir == -1f)
			{
				MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			Item I = O.inventory[O.selectedItem];
			float dmgMult = (1f+((float)mp.statCharge*0.02f));
			int damage = I.damage;

			if(mp.statCharge >= (MPlayer.maxCharge*0.5))
			{
				if(IsChargeV2 && ChargeShotAmt <= 1)
				{
					double sideangle = Math.Atan2(P.velocity.Y, P.velocity.X) + (Math.PI/2);

					int chargeProj1 = Projectile.NewProjectile(oPos.X,oPos.Y,P.velocity.X,P.velocity.Y,mod.ProjectileType(ChargeShot),(int)((float)damage*dmgMult),I.knockBack,P.owner);
					int chargeProj2 = Projectile.NewProjectile(oPos.X,oPos.Y,P.velocity.X,P.velocity.Y,mod.ProjectileType(ChargeShot),(int)((float)damage*dmgMult),I.knockBack,P.owner);
					Projectile proj = Main.projectile[chargeProj1];
					float offset = ((float)Main.projectileTexture[proj.type].Width/2f)*proj.scale - 0.5f;
					proj.position.X -= (float)Math.Cos(sideangle) * offset;
					proj.position.Y -= (float)Math.Sin(sideangle) * offset;
					proj.ai[0] = 1f;
					MProjectile mProj = (MProjectile)proj.modProjectile;
					mProj.canDiffuse = (mp.statCharge >= (MPlayer.maxCharge*0.9));
					proj = Main.projectile[chargeProj2];
					proj.position.X += (float)Math.Cos(sideangle) * offset;
					proj.position.Y += (float)Math.Sin(sideangle) * offset;
					proj.ai[0] = -1f;
					mProj = (MProjectile)proj.modProjectile;
					mProj.canDiffuse = (mp.statCharge >= (MPlayer.maxCharge*0.9));
				}
				else
				{
					for(int i = 0; i < ChargeShotAmt; i++)
					{
						int chargeProj = Projectile.NewProjectile(oPos.X,oPos.Y,P.velocity.X,P.velocity.Y,mod.ProjectileType(ChargeShot),(int)((float)damage*dmgMult),I.knockBack,P.owner);
						MProjectile mProj = (MProjectile)Main.projectile[chargeProj].modProjectile;
						mProj.waveStyle = i;
						mProj.waveDir = waveDir;
						mProj.canDiffuse = (mp.statCharge >= (MPlayer.maxCharge*0.9));
					}
				}
				
				Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/"+ChargeShotSound));
				
				O.itemTime = (I.useTime*3);
				O.itemAnimation = (I.useAnimation*3);
			}
			else if(mp.statCharge >= 30)
			{
				if(IsChargeV2 && ShotAmt <= 1)
				{
					double sideangle = Math.Atan2(P.velocity.Y, P.velocity.X) + (Math.PI/2);

					int shotProj1 = Projectile.NewProjectile(oPos.X,oPos.Y,P.velocity.X,P.velocity.Y,mod.ProjectileType(Shot),damage,I.knockBack,P.owner);
					int shotProj2 = Projectile.NewProjectile(oPos.X,oPos.Y,P.velocity.X,P.velocity.Y,mod.ProjectileType(Shot),damage,I.knockBack,P.owner);
					Projectile proj = Main.projectile[shotProj1];
					float offset = ((float)Main.projectileTexture[proj.type].Width/2f)*proj.scale - 0.5f;
					proj.position.X -= (float)Math.Cos(sideangle) * offset;
					proj.position.Y -= (float)Math.Sin(sideangle) * offset;
					proj.ai[0] = 1f;
					proj = Main.projectile[shotProj2];
					proj.position.X += (float)Math.Cos(sideangle) * offset;
					proj.position.Y += (float)Math.Sin(sideangle) * offset;
					proj.ai[0] = -1f;
				}
				else
				{
					for(int i = 0; i < ShotAmt; i++)
					{
						int shotProj = Projectile.NewProjectile(oPos.X,oPos.Y,P.velocity.X,P.velocity.Y,mod.ProjectileType(Shot),damage,I.knockBack,P.owner);
						MProjectile mProj = (MProjectile)Main.projectile[shotProj].modProjectile;
						mProj.waveStyle = i;
						mProj.waveDir = waveDir;
					}
				}
				
				Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/"+ShotSound));
				
				O.itemTime = I.useTime;
				O.itemAnimation = I.useAnimation;
			}
			return true;
		}*/
		public override void Kill(int timeLeft)
		{
			MPlayer mp = Main.player[projectile.owner].GetModPlayer<MPlayer>(mod);
			mp.statCharge = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModLoader.GetMod(UIParameters.MODNAME).GetTexture("Projectiles/chargelead/"+ChargeTex);
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width/2, (float)tex.Height/2), projectile.scale, spriteEffects, 0f);
			return false;
		}
	}
}