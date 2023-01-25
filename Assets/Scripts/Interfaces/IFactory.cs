using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IFactory<T>
    {
        T Create(params object[] args);
    }
}