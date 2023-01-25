using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interfaces
{
    public interface ISpawnStrategy
    {
        void Spawn();
        void Despawn(GameObject obj);
    }
}