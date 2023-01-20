using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<PlayerInputComponent>().End();
            var playerInputPool = systems.GetWorld().GetPool<PlayerInputComponent>();

            foreach (var entity in filter)
            {
                ref var playerInputComponent = ref playerInputPool.Get(entity);

                playerInputComponent.MoveInput = 
                    new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }
        }
    }
}