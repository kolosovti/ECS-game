using System.Collections;
using System.Collections.Generic;
using Game.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class DeathHandlerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<DeathComponent> _deathPool;
        private EcsPool<HealthComponent> _healthPool;

        public void Init(IEcsSystems systems)
        {
            _deathPool = systems.GetWorld().GetPool<DeathComponent>();
            _healthPool = systems.GetWorld().GetPool<HealthComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<HealthComponent>().End();
            foreach (var entity in filter)
            {
                ref var healthComponent = ref _healthPool.Get(entity);
                if (healthComponent.Health <= 0)
                {
                    _deathPool.Add(entity);
                    _healthPool.Del(entity);
                }

                if (systems.GetWorld().GetPool<PlayerComponent>().Has(entity))
                {
                    Debug.Log("Player hp: " + _healthPool.Get(entity).Health);
                }
            }
        }
    }
}