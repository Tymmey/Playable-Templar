﻿using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
	public class TemplarChargeRocket : BaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = TemplarChargeRocket.baseDuration / this.attackSpeedStat;
			UnityEngine.Object modelAnimator = base.GetModelAnimator();
			Transform modelTransform = base.GetModelTransform();
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/ToolbotGrenadeLauncherCrosshair");
			Util.PlayAttackSpeedSound(ChargeMegaFireball.attackString, base.gameObject, this.attackSpeedStat);
			bool flag = modelTransform;
			bool flag2 = flag;
			if (flag2)
			{
				ChildLocator component = modelTransform.GetComponent<ChildLocator>();
				bool flag3 = component;
				bool flag4 = flag3;
				if (flag4)
				{
					Transform transform = component.FindChild(MinigunState.muzzleName);
					bool flag5 = transform && ChargeMegaFireball.chargeEffectPrefab;
					bool flag6 = flag5;
					if (flag6)
					{
						this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>(ChargeMegaFireball.chargeEffectPrefab, transform.position, transform.rotation);
						this.chargeInstance.transform.parent = transform;
						ScaleParticleSystemDuration component2 = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
						bool flag7 = component2;
						bool flag8 = flag7;
						if (flag8)
						{
							component2.newDuration = this.duration;
						}
					}
				}
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			bool flag = this.chargeInstance;
			bool flag2 = flag;
			if (flag2)
			{
				EntityState.Destroy(this.chargeInstance);
			}
		}

		public override void Update()
		{
			base.Update();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(2f, false);
			bool flag = base.fixedAge >= this.duration && base.isAuthority && !this.skillButtonState.down;
			bool flag2 = flag;
			if (flag2)
			{
				TemplarFireRocket nextState = new TemplarFireRocket();
				this.outer.SetNextState(nextState);
			}
		}

		// (get) Token: 0x06000033 RID: 51 RVA: 0x00003E10 File Offset: 0x00002010
		protected ref InputBankTest.ButtonState skillButtonState
		{
			get
			{
				return ref base.inputBank.skill4;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		public static float baseDuration = 0.5f;

		private float duration;

		private GameObject chargeInstance;
	}
}
