using System.Collections;
using System.Collections.Generic;
using Game.Components;
using Game.MonoBehaviours;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerDeathSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<PlayerComponent> _playerPool;

        public void Init(IEcsSystems systems)
        {
            _playerPool = systems.GetWorld().GetPool<PlayerComponent>();
        }

        //TODO: переделать на поддержку нескольких игроков одновременно
        //архитектура предусматривает наличие нескольких игроков на экране
        public void Run(IEcsSystems systems)
        {
            var deathFilter = systems.GetWorld().Filter<DeathComponent>().Inc<PlayerComponent>().End();
            foreach (var entity in deathFilter)
            {
                if (_playerPool.Has(entity))
                {
                    Time.timeScale = 0;
                    _playerPool.Get(entity).PlayerTransform.gameObject.SetActive(false);
                    systems.GetShared<MonoProvider>().ReloadSceneView.SetActive(true);
                }
            }
        }
    }
}