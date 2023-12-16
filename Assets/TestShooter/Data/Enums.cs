using System.Collections.Generic;
using UnityEngine;

namespace TestShooter.Data
{
    public static class Enums
    {
        public enum MovementState
        {
            Idle,
            Move,
            Rotate,
            Jump,
            Shoot
        }

        public static readonly Dictionary<MovementState, Color> MovementStatesDictionary = new Dictionary<MovementState, Color>()
        {
            {MovementState.Idle, Color.black},
            {MovementState.Jump, Color.green},
            {MovementState.Move, Color.blue},
            {MovementState.Rotate, Color.cyan},
            {MovementState.Shoot, Color.red},
        };
    }
}