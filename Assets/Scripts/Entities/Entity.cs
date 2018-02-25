﻿using BattleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Stats;
using FSM;
using System;

namespace Entities
{
    public class Entity : MonoBehaviour, ITargettable, IDamageable
    {
        #region Properties
        public Animator Animator { get { return _animator; } }
        public bool IsDead { get { return (_stats.Health.Actual <= _stats.Health.Min) && !_godMode; } }
        public EntityStats Stats { get { return _stats; } }
        #endregion


        #region Events
        public event Action OnThink = delegate { };
        #endregion


        #region Local Vars
        [SerializeField]
        private EntityStats _stats;

        [SerializeField]
        private bool _godMode = false;

        [SerializeField]
        private CharacterFSM _fsm;

        private Animator _animator;
        #endregion 


        #region Interfaces
        public virtual void Hit(int damage)
        {
            Stats.Health.Actual -= damage;
        }

        public virtual Transform Target()
        {
            return transform;
        }
        #endregion

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _fsm = new CharacterFSM(this);
        }

        private void Update()
        {
            if (OnThink != null)
            {
                OnThink();
            }
            _fsm.Update();
        }
    }
}