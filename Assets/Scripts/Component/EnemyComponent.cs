using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    public struct EnemyComponent
    {
        public Transform EnemyTransform;
        public Action DeathCallback;
        public float Damage;
    }
}