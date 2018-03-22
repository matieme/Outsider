﻿using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class EntityStats
    {
        #region Properties
        public Stat Health { get { return health; } }
        public Stat MoveSpeed { get { return movementSpeed; } }
        public Stat Damage { get { return damage; } }
        #endregion

        #region Local Vars
        [SerializeField] private Stat health;

        [SerializeField] private Stat movementSpeed;

        [SerializeField] private Stat damage;
        #endregion
    }
}
