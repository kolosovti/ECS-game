using System;
using Game.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Helpers
{
    public class GameObjectFactory : IFactory<GameObject>
    {
        private readonly GameObject _prefab;

        public GameObjectFactory(GameObject prefab)
        {
            _prefab = prefab;
        }

        public GameObject Create(params object[] args)
        {
            var o = Object.Instantiate(_prefab);
            o.SetActive(false);
            return o;
        }
    }
}