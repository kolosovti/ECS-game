using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class EnemyMoveSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<MovableComponent>().Inc<ChaseComponent>().End();
            var movablePool = systems.GetWorld().GetPool<MovableComponent>();
            var chasePool = systems.GetWorld().GetPool<ChaseComponent>();

            foreach (var entity in filter)
            {
                ref var movableComponent = ref movablePool.Get(entity);
                ref var chaseComponent = ref chasePool.Get(entity);

                if (chaseComponent.Target != null)
                {
                    movableComponent.Rigidbody.velocity = (chaseComponent.Target.transform.position - movableComponent.Rigidbody.transform.position).normalized * movableComponent.EntitySpeed;
                }
            }
        }
    }
}
