using System.Collections.Generic;
using Game.Components;
using Game.Data;
using Game.Helpers;
using Game.Interfaces;
using Game.MonoBehaviours;
using Game.Strategies;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly List<ISpawnStrategy> _spawnStrategies = new List<ISpawnStrategy>();
        private EcsPool<EnemyComponent> _enemyPool;

        private int _spawnCount;

        public void Init(IEcsSystems systems)
        {
            _enemyPool = systems.GetWorld().GetPool<EnemyComponent>();
            var monoProvider = systems.GetShared<MonoProvider>();

            foreach (var enemyData in monoProvider.EnemiesData)
            {
                _spawnStrategies.Add(new EnemySpawnStrategy(systems.GetWorld(), enemyData, monoProvider.MainCamera));
            }
        }

        public void Run(IEcsSystems systems)
        {
            var deathFilter = systems.GetWorld().Filter<DeathComponent>().Inc<EnemyComponent>().End();
            foreach (var entity in deathFilter)
            {
                _enemyPool.Get(entity).DeathCallback?.Invoke();
            }

            var chaseFilter = systems.GetWorld().Filter<ChaseComponent>().End();
            //TODO: вынести константу в глобальные переменные
            while (chaseFilter.GetEntitiesCount() < 10)
            {
                _spawnCount++;
                GetNextStrategy().Spawn();
            }
        }

        private ISpawnStrategy GetNextStrategy()
        {
            return _spawnStrategies[_spawnCount % _spawnStrategies.Count];
        }
    }
}