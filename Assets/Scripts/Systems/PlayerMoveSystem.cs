using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<PlayerInputComponent> _inputPool;
        private EcsPool<MovableComponent> _movablePool;
        private EcsPool<PlayerComponent> _playerPool;

        public void Init(IEcsSystems systems)
        {
            _playerPool = systems.GetWorld().GetPool<PlayerComponent>();
            _movablePool = systems.GetWorld().GetPool<MovableComponent>();
            _inputPool = systems.GetWorld().GetPool<PlayerInputComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<MovableComponent>().Inc<PlayerInputComponent>().End();
            foreach (var entity in filter)
            {
                ref var playerComponent = ref _playerPool.Get(entity);
                ref var movableComponent = ref _movablePool.Get(entity);
                ref var playerInputComponent = ref _inputPool.Get(entity);

                var direction = playerInputComponent.ViewDirectionInput;
                var position = (Vector2)playerComponent.PlayerTransform.position;

                var angle = Vector3.SignedAngle(
                    direction - position, Vector3.right, Vector3.forward);
                angle = angle < 0 ? 360 - angle * -1 : angle;

                movableComponent.Rigidbody.velocity =
                    playerInputComponent.MoveInput.normalized * movableComponent.EntitySpeed;
                movableComponent.Rigidbody.MoveRotation(-angle);
            }
        }
    }
}