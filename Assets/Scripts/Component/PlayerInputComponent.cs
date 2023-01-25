using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    public struct PlayerInputComponent
    {
        public Vector2 MoveInput;
        public Vector2 ViewDirectionInput;
        public bool AttackRequested;
    }
}