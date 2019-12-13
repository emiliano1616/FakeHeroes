using Assets.Scripts.Grid;
using Assets.Scripts.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public List<Obstacle> Obstacles = new List<Obstacle>();

        public void Init()
        {
            LoadObstacles();
        }

        public void LoadObstacles()
        {
            var allObstacles = FindObjectsOfType<Obstacle>();
            Obstacles.AddRange(allObstacles);
            var grid = GridBase.GetInstance;

            foreach (var obs in Obstacles)
            {

                var bx = obs.MainRenderer.gameObject.AddComponent<BoxCollider>();

                //var halfX = bx.size.x * .5f;
                //var halfY = bx.size.y * .5f;

                var halfX = obs.transform.localScale.x * .5f;
                var halfY = obs.transform.localScale.y * .5f;

                Vector2 center = obs.MainRenderer.bounds.center;
                Vector2 from = obs.MainRenderer.bounds.min;
                Vector2 to = obs.MainRenderer.bounds.max;

                int stepX = GetStep(from.x, to.x, grid.ScaleXY);
                int stepY = GetStep(from.y, to.y, grid.ScaleXY);

                for (int x = 0; x < stepX; x++)
                {
                    for (int y = 0; y < stepY; y++)
                    {
                        var tp = from;
                        tp.x += grid.ScaleXY * x;
                        tp.y += grid.ScaleXY * y;

                        var p = obs.MainRenderer.transform.InverseTransformPoint(tp) - bx.center;

                        if (p.x < halfX && p.y < halfY
                            && p.x > -halfX && p.y > -halfY)
                        {
                            var n = grid.GetNodeFromWorldPosition(tp);
                            n.IsWalkable = false;
                        }
                    }
                }
                //var p = x.position;
                //var n = grid.GetNodeFromWorldPosition(p);
                //n.IsWalkable = false;
            }
        }

        private int GetStep(float from, float to, float scale)
        {
            float diff = Mathf.Abs(from - to);
            int floor = Mathf.CeilToInt(diff / scale);

            return diff - floor > 0 ? 1 : floor;

        }

        public static LevelManager GetInstance;
        private void Awake()
        {
            GetInstance = this;
        }
    }
}
