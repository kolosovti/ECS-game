using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Game.Components;
using Game.Interfaces;
using Game.MonoBehaviours;
using Game.Strategies;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly List<ISpawnStrategy> _spawnStrategies = new List<ISpawnStrategy>();
        private EcsPool<PlayerInputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {
            _inputPool = systems.GetWorld().GetPool<PlayerInputComponent>();

            var monoProvider = systems.GetShared<MonoProvider>();
            var playerPool = systems.GetWorld().GetPool<PlayerComponent>();

            foreach (var weaponData in monoProvider.WeaponsData)
            {
                var filter = systems.GetWorld().Filter<PlayerComponent>().End();
                foreach (var entity in filter)
                {
                    _spawnStrategies.Add(new WeaponSpawnStrategy(systems.GetWorld(), weaponData,
                        playerPool.Get(entity).PlayerTransform));
                }
            }
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<PlayerComponent>().End();
            foreach (var entity in filter)
            {
                if (_inputPool.Get(entity).AttackRequested)
                {
                    //TODO: добавить выбор стратегии (оружия которым стреляем)
                    _spawnStrategies[0].Spawn();
                }

                break;
            }
        }
    }
}