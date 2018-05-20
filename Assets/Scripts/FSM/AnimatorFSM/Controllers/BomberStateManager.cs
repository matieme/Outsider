﻿using System;
using BattleSystem;
using Entities;
using UnityEngine;

namespace AnimatorFSM
{
    public class BomberStateManager : AbstractStateManager
    {
        public int StartingBullets = 4;
        public int CurrentBullets;
        public float EnemyCloserRange = 4f;
        
        private void Start()
        {
            CurrentBullets = StartingBullets;
        }

        private void Update()
        {
            FSM.SetBool("HasAmmo", CurrentBullets > 0);
            FSM.SetBool("EnemyCloser", !Entity.Target.IsDead && Vector3.Distance(Entity.transform.position, Entity.Target.transform.position) <= EnemyCloserRange);
            FSM.SetBool("TargetInAttackRange", !Entity.Target.IsDead && Vector3.Distance(Entity.transform.position, Entity.Target.transform.position) <= Entity.AttackRange);
        }
    }
}
