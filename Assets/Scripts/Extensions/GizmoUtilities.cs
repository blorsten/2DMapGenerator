using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Extensions
{
    public enum ArrowDirection
    {
        Up,
        Down,
        Left,
        Right
    }


    public static class GizmoUtilities
    {
        /// <summary>
        /// Call this to draw a gizmo arrow
        /// </summary>
        /// <param name="position">The origin position</param>
        /// <param name="direction">The direction of the arrow</param>
        /// <param name="size">The size of the arrow</param>
        public static void DrawArrow(Vector3 position, ArrowDirection direction,float size = .5f)
        {
            Vector3 shaftPosition = Vector3.zero;
            Vector3 leftPosition = Vector3.zero;
            Vector3 rightPosition = Vector3.zero;

            switch (direction)
            {
                case ArrowDirection.Up:
                    shaftPosition = new Vector3(position.x, position.y - size, position.z);
                    leftPosition = new Vector3(position.x - size / 2, position.y - size / 2);
                    rightPosition = new Vector3(position.x + size / 2, position.y - size / 2);
                    break;
                case ArrowDirection.Down:
                    shaftPosition = new Vector3(position.x, position.y + size, position.z);
                    leftPosition = new Vector3(position.x - size / 2, position.y + size / 2);
                    rightPosition = new Vector3(position.x + size / 2, position.y + size / 2);
                    break;
                case ArrowDirection.Left:
                    shaftPosition = new Vector3(position.x + size, position.y, position.z);
                    leftPosition = new Vector3(position.x + size / 2, position.y - size / 2);
                    rightPosition = new Vector3(position.x + size / 2, position.y + size / 2);
                    break;
                case ArrowDirection.Right:
                    shaftPosition = new Vector3(position.x - size, position.y, position.z);
                    leftPosition = new Vector3(position.x - size / 2, position.y + size / 2);
                    rightPosition = new Vector3(position.x - size / 2, position.y - size / 2);
                    break;
            }

            UnityEngine.Gizmos.DrawLine(position,shaftPosition);
            UnityEngine.Gizmos.DrawLine(position,leftPosition);
            UnityEngine.Gizmos.DrawLine(position,rightPosition);
        }

    }
}

