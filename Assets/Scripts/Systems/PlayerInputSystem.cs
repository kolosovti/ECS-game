using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Game.MonoBehaviours;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<PlayerInputComponent> _inputPool;
        private Camera _camera;

        public void Init(IEcsSystems systems)
        {
            _inputPool = systems.GetWorld().GetPool<PlayerInputComponent>();
            _camera = systems.GetShared<MonoProvider>().MainCamera;
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<PlayerInputComponent>().End();

            foreach (var entity in filter)
            {
                ref var playerInputComponent = ref _inputPool.Get(entity);

                playerInputComponent.MoveInput =
                    new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                playerInputComponent.ViewDirectionInput =
                    _camera.ScreenToWorldPoint(new Vector3(
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        10f));
                playerInputComponent.AttackRequested = Input.GetMouseButtonDown(0);
            }
        }
    }
}