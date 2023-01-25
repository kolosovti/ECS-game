using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class CollisionDetector : MonoBehaviour
    {
        public Action<Collision2D, Transform> CollisionEnterCallback;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionEnterCallback?.Invoke(collision, transform);
        }
    }
}