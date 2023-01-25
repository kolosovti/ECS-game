using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Game.Components;
using Game.Interfaces;
using Game.MonoBehaviours;
using Game.Strategies;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace Game.Systems
{
    public class EnemyAttackSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsPool<CollisionProviderComponent> _collisionProviderPool;
        private EcsPool<DamagedComponent> _damagedPool;
        private EcsPool<DeathComponent> _deathPool;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<PlayerComponent> _playerPool;
        private EcsWorld _world;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _deathPool = _world.GetPool<DeathComponent>();
            _enemyPool = _world.GetPool<EnemyComponent>();
            _playerPool = _world.GetPool<PlayerComponent>();
            _damagedPool = _world.GetPool<DamagedComponent>();
            _collisionProviderPool = _world.GetPool<CollisionProviderComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<EnemyComponent>().End();
            foreach (var entity in filter)
            {
                if (!_collisionProviderPool.Has(entity))
                {
                    _collisionProviderPool.Add(entity);
                    ref var collisionProviderComponent = ref _collisionProviderPool.Get(entity);
                    collisionProviderComponent.CollisionDetector =
                        _enemyPool.Get(entity).EnemyTransform.gameObject.GetComponent<CollisionDetector>();
                    collisionProviderComponent.CollisionDetector.CollisionEnterCallback += OnCollisionEnter;
                }
            }
        }

        private void OnCollisionEnter(Collision2D collision, Transform originalObject)
        {
            //TODO: вынести константу в глобальные переменные
            if (collision.gameObject.CompareTag("Player"))
            {
                var filter = _world.Filter<EnemyComponent>().Inc<CollisionProviderComponent>().End();
                foreach (var entity in filter)
                {
                    ref var enemyComponent = ref _enemyPool.Get(entity);
                    if (enemyComponent.EnemyTransform == originalObject)
                    {
                        _deathPool.Add(entity);

                        var playerFilter = _world.Filter<PlayerComponent>().End();
                        foreach (var playerEntity in playerFilter)
                        {
                            if (_playerPool.Get(playerEntity).PlayerTransform == collision.transform)
                            {
                                if (!_damagedPool.Has(playerEntity))
                                {
                                    _damagedPool.Add(playerEntity);
                                }

                                ref var damagedComponent = ref _damagedPool.Get(playerEntity);
                                damagedComponent.Damage += enemyComponent.Damage;
                            }
                        }

                        DeleteCollisionProvider(entity);
                    }
                }
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<EnemyComponent>().Inc<CollisionProviderComponent>().End();
            foreach (var entity in filter)
            {
                DeleteCollisionProvider(entity);
            }
        }

        private void DeleteCollisionProvider(int entity)
        {
            ref var collisionProviderComponent = ref _collisionProviderPool.Get(entity);
            collisionProviderComponent.CollisionDetector.CollisionEnterCallback -= OnCollisionEnter;
            _collisionProviderPool.Del(entity);
        }
    }
}