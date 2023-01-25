using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Game Data/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public GameObject Prefab;
        public float EntitySpeed = 1f;
        public float Health = 100f;
        public float Damage = 20f;
        public float Armor = 50f;
        public int InitialPoolSize = 0;
    }
}