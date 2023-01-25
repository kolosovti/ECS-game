using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MonoBehaviours
{
    public class WeaponVisualImpact : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private readonly float ShowTime = 0.5f;

        public Action<GameObject> ImpactCompleteCallback;

        private float _timer;

        private void OnEnable()
        {
            _timer = ShowTime;
        }

        private void Update()
        {
            _spriteRenderer.color = new Color(
                _spriteRenderer.color.r,
                _spriteRenderer.color.g,
                _spriteRenderer.color.b,
                _timer);

            _timer -= Time.deltaTime;

            if (_timer < 0)
            {
                ImpactCompleteCallback?.Invoke(gameObject);
            }
        }
    }
}