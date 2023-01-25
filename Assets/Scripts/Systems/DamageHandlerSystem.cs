using System.Collections;
using System.Collections.Generic;
using Game.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class DamageHandlerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<HealthComponent> _healthPool;
        private EcsPool<DamagedComponent> _damagedPool;

        public void Init(IEcsSystems systems)
        {
            _healthPool = systems.GetWorld().GetPool<HealthComponent>();
            _damagedPool = systems.GetWorld().GetPool<DamagedComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<DamagedComponent>().Inc<HealthComponent>().End();
            foreach (var entity in filter)
            {
                ref var damagedComponent = ref _damagedPool.Get(entity);
                if (damagedComponent.Damage != 0)
                {
                    ref var healthComponent = ref _healthPool.Get(entity);

                    var armor = healthComponent.Armor > 100 ? 100 : healthComponent.Armor;
                    armor = armor < 0 ? 0 : armor;
                    var damageMultiplier = (100 - armor) / 100;
                    healthComponent.Health -= damagedComponent.Damage * damageMultiplier;
                    if (healthComponent.Health < 0)
                    {
                        healthComponent.Health = 0;
                    }
                }

                _damagedPool.Del(entity);
            }
        }
    }
}