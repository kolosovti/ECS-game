using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class EnemyMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<ChaseComponent> _chasePool;
        private EcsPool<MovableComponent> _movablePool;

        public void Init(IEcsSystems systems)
        {
            _movablePool = systems.GetWorld().GetPool<MovableComponent>();
            _chasePool = systems.GetWorld().GetPool<ChaseComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<MovableComponent>().Inc<ChaseComponent>().End();

            foreach (var entity in filter)
            {
                ref var movableComponent = ref _movablePool.Get(entity);
                ref var chaseComponent = ref _chasePool.Get(entity);

                if (chaseComponent.Target != null)
                {
                    movableComponent.Rigidbody.velocity =
                        (chaseComponent.Target.transform.position - movableComponent.Rigidbody.transform.position)
                        .normalized * movableComponent.EntitySpeed;
                }
            }
        }
    }
}