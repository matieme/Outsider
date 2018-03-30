﻿using FSM;
using Managers;
using System;
using UnityEngine;

namespace Entities
{
    public class CharacterEntity : Entity
    {
        public CharacterFSM fsm;

		public event Action OnAttackRecovering = delegate { };
		public event Action OnAttackRecovered = delegate { };
        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnHeavyAttack = delegate { };

		public void AttackRecovered()
		{
			if (OnAttackRecovered != null)
			{
				OnAttackRecovered();
			}
		}

		public void AttackRecovering()
		{
			if (OnAttackRecovering != null)
			{
				OnAttackRecovering();
			}
		}

        private void Update()
        {
            if (InputManager.Instance.AxisMoving && OnMove != null)
            {
                OnMove();
            }
            if (InputManager.Instance.Attack && OnAttack != null)
            {
                OnAttack();
            }
            if (InputManager.Instance.SpecialAttack && OnHeavyAttack != null)
            {
                OnHeavyAttack();
            }
           
            fsm.Update();
        }

        private void Start()
        {
            fsm = new CharacterFSM(this);
        }
    }
}
