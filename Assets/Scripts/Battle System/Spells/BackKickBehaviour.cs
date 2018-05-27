﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Managers;
using UnityEngine;
using Util;

namespace BattleSystem.Spells
{
    [RequireComponent(typeof(SpellBehaviour))]
    public class BackKickBehaviour : MonoBehaviour
    {
        public float Displacement = 1f;
        public float LifeRecover = 2f;
        public GameObject Effect;
            
        private SpellBehaviour _behaviour;
        private CharacterEntity _character;
        private LineOfAim _lineOfAim;
        
        private void Start()
        {
            _behaviour = GetComponent<SpellBehaviour>();
            _character = GameManager.Instance.Character;
            _lineOfAim = GetComponentInChildren<LineOfAim>();
            
            Cast();
        }
        private void DmgCast(Transform target)
        {
            var part = Instantiate(_behaviour.Definition.HitEffect,  target.position + Vector3.up, Quaternion.identity, target);
            Destroy(part, 1.5f);
        }
        
        private void Cast()
        {
            var enemies = _lineOfAim.GetEnemiesInSight().ToList();
            Instantiate(Effect, transform.position, Quaternion.identity, transform);
            
            foreach (var enemy in enemies)
            {
                GameManager.Instance.Combo++;
                _character.Heal(LifeRecover);
                DmgCast(enemy.transform);
                
                enemy.TakeDamage(new Damage
                {
                    amount = _behaviour.Definition.Damage,
                    type = _behaviour.Definition.DamageType,
                    origin = transform,
                    originator = _character,
                    Displacement = Displacement
                });
            }
        }
    }
}
