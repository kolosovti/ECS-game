using Game.Systems;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EcsStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private IEcsSystems _initSystems;
        private IEcsSystems _updateSystems;

        private void Start()
        {
            _world = new EcsWorld();

            _initSystems = new EcsSystems(_world);
            _initSystems
                .Add(new PlayerInitSystem())
                .Init();

            _updateSystems = new EcsSystems(_world);
            _updateSystems
                .Add(new PlayerInputSystem())
                .Add(new PlayerMoveSystem())
                .Add(new EnemyManagementSystem())
                .Add(new EnemyChaseSystem())
                .Add(new EnemyMoveSystem())
                .Init();
        }

        private void Update()
        {
            _updateSystems?.Run();
        }

        private void OnDestroy()
        {
            if (_updateSystems != null)
            {
                _updateSystems.Destroy();
                _updateSystems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}