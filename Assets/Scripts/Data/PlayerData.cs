using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenuAttribute(fileName = "PlayerData", menuName = "Game Data/Player Data")]
    public class PlayerData : ScriptableObject
    {
        public GameObject Prefab;
        public float PlayerSpeed = 5f;
        public float Health = 100f;
        public float Damage = 20f;
        public float Armor = 50f;
    }
}