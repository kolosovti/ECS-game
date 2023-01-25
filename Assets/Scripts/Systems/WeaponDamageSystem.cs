using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.Components;
using Game.MonoBehaviours;
using Leopotam.EcsLite;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Game.Systems
{
    public class WeaponDamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<DamagedComponent> _damagedPool;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<RayWeaponComponent> _weaponPool;

        public void Init(IEcsSystems systems)
        {
            _damagedPool = systems.GetWorld().GetPool<DamagedComponent>();
            _weaponPool = systems.GetWorld().GetPool<RayWeaponComponent>();
            _enemyPool = systems.GetWorld().GetPool<EnemyComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<RayWeaponComponent>().End();
            foreach (var entity in filter)
            {
                ref var weaponComponent = ref _weaponPool.Get(entity);
                var ray = weaponComponent.Ray;

                //TODO: вынести слой Enemy и дистанцию в глобальные переменные
                var hit = Physics2D.Raycast(ray.origin, ray.direction, 30f,
                    LayerMask.GetMask("Enemy"));

                //TODO: вынести тег "Enemy" в глобальные переменные
                if (hit.collider != null && hit.collider.tag == "Enemy")
                {
                    var enemyFilter = systems.GetWorld().Filter<EnemyComponent>().Inc<HealthComponent>().End();
                    foreach (var enemyEntity in enemyFilter)
                    {
                        if (hit.collider.transform == _enemyPool.Get(enemyEntity).EnemyTransform)
                        {
                            _damagedPool.Add(enemyEntity);
                            ref var damagedComponent = ref _damagedPool.Get(enemyEntity);
                            damagedComponent.Damage = weaponComponent.Damage;
                            break;
                        }
                    }
                }

                _weaponPool.Del(entity);
            }
        }
    }
}