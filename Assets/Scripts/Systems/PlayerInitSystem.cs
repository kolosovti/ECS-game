using Game.Components;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Game.MonoBehaviours;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var playerEntity = world.NewEntity();
            var playerData = systems.GetShared<MonoProvider>().PlayerData;

            var movablePool = world.GetPool<MovableComponent>();
            movablePool.Add(playerEntity);
            ref var movableComponent = ref movablePool.Get(playerEntity);

            var playerInputPool = world.GetPool<PlayerInputComponent>();
            playerInputPool.Add(playerEntity);

            var playerPool = world.GetPool<PlayerComponent>();
            playerPool.Add(playerEntity);
            ref var playerComponent = ref playerPool.Get(playerEntity);

            var healthPool = world.GetPool<HealthComponent>();
            healthPool.Add(playerEntity);
            ref var healthComponent = ref healthPool.Get(playerEntity);
            healthComponent.Health = playerData.Health;
            healthComponent.Armor = playerData.Armor;

            //TODO: переделать на спавн из ScriptableObject конфига, добавить пул объектов
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            movableComponent.EntitySpeed = playerData.PlayerSpeed;
            movableComponent.Rigidbody = playerGO.GetComponent<Rigidbody2D>();
            playerComponent.PlayerTransform = playerGO.transform;
        }
    }
}