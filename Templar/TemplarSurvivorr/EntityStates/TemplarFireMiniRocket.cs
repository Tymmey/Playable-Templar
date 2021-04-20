﻿using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace Templar
{
	public class TemplarFireMiniRocket : BaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			base.StartAimMode(2f, false);
			this.jelly = false;
			TemplarFireMiniRocket.rocketPrefab = Templar.templarRocket;
			TemplarFireMiniRocket.effectPrefab = FireMegaFireball.muzzleflashEffectPrefab;
			TemplarFireMiniRocket.rocketPrefab = Templar.templarRocket;
			base.AddRecoil(-1f * TemplarFireMiniRocket.recoilAmplitude, -2f * TemplarFireMiniRocket.recoilAmplitude, -0.5f * TemplarFireMiniRocket.recoilAmplitude, 0.5f * TemplarFireMiniRocket.recoilAmplitude);
			this.duration = TemplarFireMiniRocket.baseDuration / this.attackSpeedStat;
			this.fireDuration = TemplarFireMiniRocket.baseFireDuration / this.attackSpeedStat;
			Util.PlaySound(FireMegaFireball.attackString, base.gameObject);
		}

		public override void OnExit()
		{
			base.OnExit();
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			string muzzleName = MinigunState.muzzleName;
			bool isAuthority = base.isAuthority;
			bool flag = isAuthority;
			if (flag)
			{
				int num = Mathf.FloorToInt(base.fixedAge / this.fireDuration * (float)TemplarFireMiniRocket.projectileCount);
				bool flag2 = this.projectilesFired <= num && this.projectilesFired < TemplarFireMiniRocket.projectileCount;
				bool flag3 = flag2;
				if (flag3)
				{
					EffectManager.SimpleMuzzleFlash(TemplarFireMiniRocket.effectPrefab, base.gameObject, muzzleName, false);
					Ray aimRay = base.GetAimRay();
					float speedOverride = FireMegaFireball.projectileSpeed * 2f;
					float bonusYaw = (float)Mathf.FloorToInt((float)this.projectilesFired - (float)(TemplarFireMiniRocket.projectileCount - 1) / 2f) / (float)(TemplarFireMiniRocket.projectileCount - 1) * TemplarFireMiniRocket.totalYawSpread;
					bonusYaw = 0f;
					Vector3 forward = Util.ApplySpread(aimRay.direction, 0f, 0f, 1f, 1f, bonusYaw, 0f);
					ProjectileManager.instance.FireProjectile(TemplarFireMiniRocket.rocketPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(forward), base.gameObject, this.damageStat * TemplarFireMiniRocket.damageCoefficient, TemplarFireMiniRocket.force, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, speedOverride);
					this.projectilesFired++;
				}
			}
			bool flag4 = base.fixedAge >= this.duration && base.isAuthority;
			bool flag5 = flag4;
			if (flag5)
			{
				this.outer.SetNextStateToMain();
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		public static GameObject rocketPrefab;

		public static GameObject effectPrefab;

		public static int projectileCount = 1;

		public static float totalYawSpread = 1f;

		public static float baseDuration = 0.25f;

		public static float baseFireDuration = 0.25f;

		public static float damageCoefficient = Templar.miniBazookaDamageCoefficient.Value;

		public static float force = 25f;

		public static float recoilAmplitude = 7.5f;

		private float duration;

		private float fireDuration;

		private int projectilesFired;

		private bool jelly;
	}
}
