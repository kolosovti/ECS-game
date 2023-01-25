using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IPool<T>
    {
        T GetObject(params object[] args);
        void ReturnObject(T obj);
    }
}