using Game.Data;
using Game.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Game.Helpers;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class MonoProvider : MonoBehaviour
    {
        public List<EnemyData> EnemiesData;
        public Camera MainCamera;
        public PlayerData PlayerData;
        public GameObject ReloadSceneView;
        public List<WeaponData> WeaponsData;
    }
}