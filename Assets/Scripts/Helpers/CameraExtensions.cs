using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Helpers
{
    public static class CameraExtensions
    {
        public static Bounds GetCameraBounds(Camera camera)
        {
            var screenAspect = Screen.width / (float)Screen.height;
            var cameraHeight = camera.orthographicSize * 2;
            var bounds = new Bounds(
                Vector3.zero,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }

        public static Vector2 GetRandomPositionOnTheBounds(Camera camera)
        {
            var cameraBounds = GetCameraBounds(camera);
            var rand = new System.Random();

            var randomizeX = rand.Next() > int.MaxValue / 2;
            if (randomizeX)
            {
                var min = cameraBounds.min.x;
                var max = cameraBounds.max.x;

                var y = rand.Next() > int.MaxValue / 2 ? cameraBounds.min.y : cameraBounds.max.y;

                return (Vector2)camera.transform.position +
                       new Vector2((float)rand.NextDouble() * (max - min) + min, y);
            }
            else
            {
                var min = cameraBounds.min.y;
                var max = cameraBounds.max.y;

                var x = rand.Next() > int.MaxValue / 2 ? cameraBounds.min.x : cameraBounds.max.x;

                return (Vector2)camera.transform.position +
                       new Vector2(x, (float)rand.NextDouble() * (max - min) + min);
            }
        }
    }
}