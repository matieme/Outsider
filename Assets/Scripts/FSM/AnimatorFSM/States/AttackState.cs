﻿using BattleSystem;
using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/Stalking State")]
	public class AttackState : BaseState
	{
		private BasicEnemy _entity;

		protected override void Setup()
		{
			_entity = GetComponentInParent<BasicEnemy>();
		}

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				_entity.Animator.SetTrigger(EntityAnimations.Attack);
				_entity.CurrentAction = GroupAction.Stalking;
			};
		}
	}
}
