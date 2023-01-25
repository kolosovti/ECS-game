using Game.Components;
using Game.MonoBehaviours;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class CameraFollowSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Camera _camera;
        private EcsPool<PlayerComponent> _playerPool;

        public void Init(IEcsSystems systems)
        {
            _camera = systems.GetShared<MonoProvider>().MainCamera;
            _playerPool = systems.GetWorld().GetPool<PlayerComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            if (_camera != null)
            {
                ref var player = ref _playerPool.Get(0);
                _camera.transform.position = new Vector3(player.PlayerTransform.position.x, player.PlayerTransform.position.y, _camera.transform.position.z);
            }
        }
    }
}