using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerMoveSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<MovableComponent>().Inc<PlayerInputComponent>().End();
            var movablePool = systems.GetWorld().GetPool<MovableComponent>();
            var playerInputPool = systems.GetWorld().GetPool<PlayerInputComponent>();

            foreach (var entity in filter)
            {
                ref var movableComponent = ref movablePool.Get(entity);
                ref var playerInputComponent = ref playerInputPool.Get(entity);

                movableComponent.Rigidbody.velocity = playerInputComponent.MoveInput.normalized * movableComponent.EntitySpeed;
            }
        }
    }
}