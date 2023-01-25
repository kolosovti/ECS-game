using System;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Game.Components;
using Game.Data;
using Game.Helpers;
using Game.Interfaces;
using Game.MonoBehaviours;
using UnityEngine;

namespace Game.Strategies
{
    public class WeaponSpawnStrategy : ISpawnStrategy, IDisposable
    {
        private readonly EcsPool<PlayerInputComponent> _playerInputPool;
        private readonly EcsPool<WeaponViewComponent> _weaponViewPool;
        private readonly EcsPool<RayWeaponComponent> _weaponPool;

        private readonly IPool<GameObject> _spawnPool;
        private readonly WeaponData _weaponData;
        private readonly EcsWorld _world;
        private readonly Transform _player;

        public WeaponSpawnStrategy(EcsWorld world, WeaponData weaponData, Transform player)
        {
            _world = world;
            _player = player;
            _weaponData = weaponData;

            _weaponPool = _world.GetPool<RayWeaponComponent>();
            _weaponViewPool = _world.GetPool<WeaponViewComponent>();
            _playerInputPool = _world.GetPool<PlayerInputComponent>();

            var factory = new GameObjectFactory(weaponData.Prefab);
            _spawnPool = new GameObjectPool(
                factory, weaponData.InitialPoolSize);
        }

        public void Spawn()
        {
            var obj = _spawnPool.GetObject();
            var weaponEntity = _world.NewEntity();

            obj.transform.position = _player.transform.position;
            obj.SetActive(true);

            _weaponPool.Add(weaponEntity);
            ref var weaponComponent = ref _weaponPool.Get(weaponEntity);

            _weaponViewPool.Add(weaponEntity);
            ref var weaponViewComponent = ref _weaponViewPool.Get(weaponEntity);
            weaponViewComponent.WeaponVisualImpact = obj.GetComponent<WeaponVisualImpact>();
            weaponViewComponent.WeaponVisualImpact.ImpactCompleteCallback += Despawn;

            var filter = _world.Filter<MovableComponent>().Inc<PlayerInputComponent>().End();
            foreach (var entity in filter)
            {
                var inputComponent = _playerInputPool.Get(entity);
                
                var direction = inputComponent.ViewDirectionInput;
                var position = (Vector2)_player.transform.position;

                weaponComponent.Damage = _weaponData.Damage;
                weaponComponent.Ray = new Ray(position, direction - position);

                var angle = Vector3.SignedAngle(direction - position, Vector3.right, Vector3.forward);

                if (angle < 0)
                {
                    angle = 360 - angle * -1;
                }

                obj.transform.eulerAngles = new Vector3(0, 0,
                    -angle);
            }
        }

        public void Despawn(GameObject o)
        {
            var filter = _world.Filter<WeaponViewComponent>().End();
            foreach (var entity in filter)
            {
                ref var weaponViewComponent = ref _weaponViewPool.Get(entity);
                if (weaponViewComponent.WeaponVisualImpact.gameObject == o)
                {
                    weaponViewComponent.WeaponVisualImpact.ImpactCompleteCallback -= Despawn;
                    _weaponViewPool.Del(entity);
                }
            }

            o.SetActive(false);
            _spawnPool.ReturnObject(o);
        }

        public void Dispose()
        {
            var filter = _world.Filter<WeaponViewComponent>().End();
            foreach (var entity in filter)
            {
                ref var weaponViewComponent = ref _weaponViewPool.Get(entity);
                weaponViewComponent.WeaponVisualImpact.ImpactCompleteCallback -= Despawn;
            }
        }
    }
}