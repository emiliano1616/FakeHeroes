using Assets.Scripts.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PathManager : MonoBehaviour
    {
        public static PathManager GetInstance;
        private void Awake()
        {
            GetInstance = this;
        }

        [SerializeField]
        private Material AvailableMovementMaterial;
        [SerializeField]
        private Material MissingMovementMaterial;

        private float AvailableActionPoints { get; set; }
        private List<Node> Path { get; set; }
        private LineRenderer AvailableMovementLine { get; set; }
        private LineRenderer MissingMovementLine { get; set; }
        private bool IsNewPath = false;

        public List<Node> GetFullPath()
        {
            return this.Path;
        }

        public List<Node> GetAvailablePath()
        {
            if (this.Path == null) return null;
            return this.Path.Where(t => t.APAvailable).ToList();
        }

        public void SetNewPath(List<Node> path, float actionPoints)
        {
            this.IsNewPath = true;
            this.Path = path;
            this.AvailableActionPoints = actionPoints;
            this.DrawPath();
        }

        private void DrawPath()
        {
            if (IsNewPath && Path != null && Path.Count > 0)
            {
                var nodeCostAcum = 0d;
                for (int i = 0; i < Path.Count; i++)
                {
                    nodeCostAcum += Path[i].NodeCost;
                    Path[i].APAvailable = AvailableActionPoints + 1 >= nodeCostAcum;
                }

                var aPos = Path.Where(t => t.APAvailable).Select(t => t.Position3).ToArray();
                var mPos = Path.Where(t => !t.APAvailable).Select(t => t.Position3).ToList();


                AvailableMovementLine.positionCount = aPos.Length;
                AvailableMovementLine.SetPositions(aPos);
                if (mPos.Count() > 0)
                {
                    EnableMissingLine();
                    if (aPos.Count() > 0)
                        mPos.Insert(0, aPos.Last());
                    MissingMovementLine.positionCount = mPos.Count;
                    MissingMovementLine.SetPositions(mPos.ToArray());
                }
                else
                {
                    DisableMissingLine();
                }

            }
        }

        private void EnableMissingLine()
        {
            this.MissingMovementLine.enabled = true;
        }

        private void DisableMissingLine()
        {
            this.MissingMovementLine.enabled = false;
        }

        public void Move()
        {

            var positions = Path.Where(t => t.APAvailable).Select(t => t.Position3).ToArray();
            if (Path.Any())
                Path.RemoveAt(0);
            AvailableMovementLine.SetPositions(positions);


        }


        private void Start()
        {
            var go = new GameObject();
            go.name = "Line Path Available";
            this.AvailableMovementLine = go.AddComponent<LineRenderer>();
            this.AvailableMovementLine.startWidth = 0.2f;
            this.AvailableMovementLine.endWidth = 0.2f;
            this.AvailableMovementLine.material = AvailableMovementMaterial;

            var go2 = new GameObject();
            go2.name = "Line Path Missing";
            this.MissingMovementLine = go2.AddComponent<LineRenderer>();
            this.MissingMovementLine.startWidth = 0.2f;
            this.MissingMovementLine.endWidth = 0.2f;
            this.MissingMovementLine.material = MissingMovementMaterial;

        }
    }
}
