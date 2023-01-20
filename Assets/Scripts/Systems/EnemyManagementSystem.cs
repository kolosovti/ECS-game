using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class EnemyManagementSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var enemyEntity = ecsWorld.NewEntity();

            var movablePool = ecsWorld.GetPool<MovableComponent>();
            movablePool.Add(enemyEntity);
            ref var movableComponent = ref movablePool.Get(enemyEntity);

            var chasePool = ecsWorld.GetPool<ChaseComponent>();
            chasePool.Add(enemyEntity);
            ref var chaseComponent = ref chasePool.Get(enemyEntity);

            var enemyGO = GameObject.FindGameObjectWithTag("Enemy");
            movableComponent.EntitySpeed = 7f; // gameData.configuration.playerSpeed;
            movableComponent.Rigidbody = enemyGO.GetComponent<Rigidbody2D>();
        }

        public void Run(IEcsSystems systems)
        {
        }
    }
}