using Game.Systems;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private MonoProvider _monoProvider;

        private EcsWorld _world;
        private IEcsSystems _initSystems;
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;

        private void Start()
        {
            Time.timeScale = 1;

            _world = new EcsWorld();

            _initSystems = new EcsSystems(_world, _monoProvider);
            _initSystems
                .Add(new PlayerInitSystem())
                .Init();

            _updateSystems = new EcsSystems(_world, _monoProvider);
            _updateSystems
                .Add(new PlayerInputSystem())
                .Add(new CameraFollowSystem())
                .Add(new PlayerAttackSystem())
                .Add(new EnemyAttackSystem())
                .Add(new WeaponDamageSystem())
                .Add(new DamageHandlerSystem())
                .Add(new DeathHandlerSystem())
                .Add(new EnemySpawnSystem())
                .Add(new EnemyChaseSystem())
                .Add(new PlayerDeathSystem())
                .Init();

            _fixedUpdateSystems = new EcsSystems(_world, _monoProvider);
            _fixedUpdateSystems
                .Add(new PlayerMoveSystem())
                .Add(new EnemyMoveSystem())
                .Init();
        }

        private void Update()
        {
            _updateSystems?.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
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