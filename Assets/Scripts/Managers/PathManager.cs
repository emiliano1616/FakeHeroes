using Assets.Scripts.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PathManager 
    {
        public PathManager() { }
        public PathManager(int availableActionPoints, Material availableMovementMaterial, Material missingMovementMaterial)
        {
            var go = new GameObject();
            go.name = "Line Path Available";
            this.AvailableMovementLine = go.AddComponent<LineRenderer>();
            this.AvailableMovementLine.startWidth = 0.2f;
            this.AvailableMovementLine.endWidth = 0.2f;
            this.AvailableMovementLine.material = availableMovementMaterial;

            var go2 = new GameObject();
            go2.name = "Line Path Missing";
            this.MissingMovementLine = go2.AddComponent<LineRenderer>();
            this.MissingMovementLine.startWidth = 0.2f;
            this.MissingMovementLine.endWidth = 0.2f;
            this.MissingMovementLine.material = missingMovementMaterial;

            this.AvailableActionPoints = availableActionPoints;

        }
        public int AvailableActionPoints { get; set; }
        private List<Node> Path { get; set; }
        public LineRenderer AvailableMovementLine { get; set; }
        public LineRenderer MissingMovementLine { get; set; }
        private bool IsNewPath = false;

        public void SetNewPath(List<Node> path,int actionPoints)
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
                var areMissing = false;
                for (int i = 0; i < Path.Count; i++)
                {
                    nodeCostAcum += Path[i].NodeCost;
                    var position = Path[i].Position;
                    if (AvailableActionPoints >= nodeCostAcum)
                    {
                        DrawLine(AvailableMovementLine, i, position);
                    }
                    else
                    {
                        if (!areMissing)
                        {
                            EnableMissingLine();
                            areMissing = true;
                            i--;
                            position = Path[i].Position;
                        }

                        DrawLine(MissingMovementLine, i - (AvailableMovementLine.positionCount - 1), position);
                    }
                }
                if (!areMissing)
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

        private void DrawLine(LineRenderer line, int index, Vector2 position)
        {
            line.positionCount = index + 1;
            line.SetPosition(index, position);
        }
    }
}
