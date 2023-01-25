using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Game Data/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public GameObject Prefab;
        public float Damage = 20f;
        public int InitialPoolSize = 30;
    }
}