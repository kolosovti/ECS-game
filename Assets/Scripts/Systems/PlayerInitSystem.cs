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
            var world = systems.GetWorld();
            var playerEntity = world.NewEntity();

            var movablePool = world.GetPool<MovableComponent>();
            movablePool.Add(playerEntity);
            ref var movableComponent = ref movablePool.Get(playerEntity);

            var playerInputPool = world.GetPool<PlayerInputComponent>();
            playerInputPool.Add(playerEntity);
            ref var playerInputComponent = ref playerInputPool.Get(playerEntity);

            var playerPool = world.GetPool<PlayerComponent>();
            playerPool.Add(playerEntity);
            ref var playerComponent = ref playerPool.Get(playerEntity);

            //TODO: переделать на спавн из ScriptableObject конфига, добавить пул объектов
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            movableComponent.EntitySpeed = 10f; // gameData.configuration.playerSpeed;
            movableComponent.Rigidbody = playerGO.GetComponent<Rigidbody2D>();
            playerComponent.PlayerTransform = playerGO.transform;
        }
    }
}