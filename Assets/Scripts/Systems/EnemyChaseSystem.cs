using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class EnemyChaseSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var playerFilter = systems.GetWorld().Filter<PlayerComponent>().End();
            var chaserFilter = systems.GetWorld().Filter<ChaseComponent>().Inc<MovableComponent>().End();
            var playerPool = systems.GetWorld().GetPool<PlayerComponent>();
            var chaserPool = systems.GetWorld().GetPool<ChaseComponent>();
            var movablePool = systems.GetWorld().GetPool<MovableComponent>();

            foreach (var entity in playerFilter)
            {
                ref var playerEntity = ref playerPool.Get(entity);

                foreach (var chase in chaserFilter)
                {
                    ref var chaserEntity = ref chaserPool.Get(chase);

                    if (chaserEntity.Target == null)
                    {
                        chaserEntity.Target = playerEntity.PlayerTransform;
                        continue;
                    }

                    ref var chaserMoveEntity = ref movablePool.Get(chase);

                    var oldDist = Vector3.Distance(chaserEntity.Target.position,
                        chaserMoveEntity.Rigidbody.transform.position);
                    var newDist = Vector3.Distance(playerEntity.PlayerTransform.position,
                        chaserMoveEntity.Rigidbody.transform.position);
                    if (newDist < oldDist)
                    {
                        chaserEntity.Target = playerEntity.PlayerTransform;
                    }
                }
            }
        }
    }
}