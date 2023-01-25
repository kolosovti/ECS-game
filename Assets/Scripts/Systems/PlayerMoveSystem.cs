using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<MovableComponent> _movablePool;
        private EcsPool<PlayerInputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {
            _movablePool = systems.GetWorld().GetPool<MovableComponent>();
            _inputPool = systems.GetWorld().GetPool<PlayerInputComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<MovableComponent>().Inc<PlayerInputComponent>().End();
            foreach (var entity in filter)
            {
                ref var movableComponent = ref _movablePool.Get(entity);
                ref var playerInputComponent = ref _inputPool.Get(entity);

                movableComponent.Rigidbody.velocity = playerInputComponent.MoveInput.normalized * movableComponent.EntitySpeed;
            }
        }
    }
}