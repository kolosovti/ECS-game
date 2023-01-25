using Game.Interfaces;
using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Helpers
{
    public class GameObjectPool : IPool<GameObject>
    {
        private readonly IFactory<GameObject> _factory;
        private readonly List<GameObject> _pool;

        public GameObjectPool(
            IFactory<GameObject> factory,
            int initialSize = 0)
        {
            initialSize = initialSize < 0 ? 0 : initialSize;

            _factory = factory;
            _pool = new List<GameObject>(initialSize);

            for (var i = 0; i < initialSize; i++)
            {
                var obj = _factory.Create();
                _pool.Add(obj);
            }
        }

        public GameObject GetObject(params object[] args)
        {
            if (_pool.Count > 0)
            {
                var obj = _pool[0];
                _pool.RemoveAt(0);
                return obj;
            }
            else
            {
                var obj = _factory.Create();
                return obj;
            }
        }

        public void ReturnObject(GameObject obj)
        {
            _pool.Add(obj);
        }
    }
}