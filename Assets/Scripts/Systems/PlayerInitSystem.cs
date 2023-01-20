using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var playerEntity = ecsWorld.NewEntity();

            var movablePool = ecsWorld.GetPool<MovableComponent>();
            movablePool.Add(playerEntity);
            ref var movableComponent = ref movablePool.Get(playerEntity);

            var playerInputPool = ecsWorld.GetPool<PlayerInputComponent>();
            playerInputPool.Add(playerEntity);
            ref var playerInputComponent = ref playerInputPool.Get(playerEntity);

            var playerPool = ecsWorld.GetPool<PlayerComponent>();
            playerPool.Add(playerEntity);
            ref var playerComponent = ref playerPool.Get(playerEntity);

            var playerGO = GameObject.FindGameObjectWithTag("Player");
            movableComponent.EntitySpeed = 10f; // gameData.configuration.playerSpeed;
            movableComponent.Rigidbody = playerGO.GetComponent<Rigidbody2D>();
            playerComponent.PlayerTransform = playerGO.transform;
        }
    }
}