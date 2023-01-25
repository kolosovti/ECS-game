using System;
using System.Collections;
using System.Collections.Generic;
using Game.Components;
using Game.Data;
using Game.Helpers;
using Game.Interfaces;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Strategies
{
    public class EnemySpawnStrategy : ISpawnStrategy
    {
        private readonly Camera _camera;
        private readonly EcsPool<ChaseComponent> _chasePool;
        private readonly EnemyData _enemyData;
        private readonly EcsPool<EnemyComponent> _enemyPool;
        private readonly EcsPool<HealthComponent> _healthPool;
        private readonly EcsPool<MovableComponent> _movablePool;

        private readonly IPool<GameObject> _spawnPool;
        private readonly EcsWorld _world;

        public EnemySpawnStrategy(EcsWorld world, EnemyData enemyData, Camera mainCamera)
        {
            _world = world;
            _camera = mainCamera;
            _enemyData = enemyData;

            _enemyPool = _world.GetPool<EnemyComponent>();
            _chasePool = _world.GetPool<ChaseComponent>();
            _healthPool = _world.GetPool<HealthComponent>();
            _movablePool = _world.GetPool<MovableComponent>();

            var factory = new GameObjectFactory(enemyData.Prefab);
            _spawnPool = new GameObjectPool(
                factory, enemyData.InitialPoolSize);
        }

        public void Spawn()
        {
            var gameObject = _spawnPool.GetObject();
            var entity = _world.NewEntity();
            gameObject.SetActive(true);

            _chasePool.Add(entity);

            _enemyPool.Add(entity);
            ref var enemyComponent = ref _enemyPool.Get(entity);
            enemyComponent.Damage = _enemyData.Damage;
            enemyComponent.EnemyTransform = gameObject.transform;
            enemyComponent.DeathCallback = () => Despawn(gameObject);

            _healthPool.Add(entity);
            ref var healthComponent = ref _healthPool.Get(entity);
            healthComponent.Health = _enemyData.Health;
            healthComponent.Armor = _enemyData.Armor;

            _movablePool.Add(entity);
            ref var movableComponent = ref _movablePool.Get(entity);
            movableComponent.EntitySpeed = _enemyData.EntitySpeed;
            movableComponent.Rigidbody = gameObject.GetComponent<Rigidbody2D>();
            movableComponent.Rigidbody.MovePosition(CameraExtensions.GetRandomPositionOnTheBounds(_camera));
        }

        public void Despawn(GameObject obj)
        {
            _spawnPool.ReturnObject(obj);
            obj.SetActive(false);

            var filter = _world.Filter<EnemyComponent>().Inc<DeathComponent>().End();
            foreach (var entity in filter)
            {
                var entityGameObject = _enemyPool.Get(entity).EnemyTransform.gameObject;
                if (entityGameObject == obj)
                {
                    _movablePool.Del(entity);
                    _healthPool.Del(entity);
                    _enemyPool.Del(entity);
                    _chasePool.Del(entity);
                }
            }
        }
    }
}