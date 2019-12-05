using Assets.Scripts.Algorithms;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public UnitController Players;
        private Node PreviusMouseNode;
        private PathManager PathData;
        private GridBase Grid;


        public void Init()
        {
        }

        private void Start()
        {
            Grid = GridBase.GetInstance;
            Players.transform.position = Grid.GetWorldCoordinatesFromNode(5, 6);
            PathData = PathManager.GetInstance;
        }

        private void Update()
        {
            if (!Grid.IsInitialized) return;
            if (Players == null) return;

            var path = PathData.GetAvailablePath();
            if (path != null && path.Count > 0
                && Input.GetMouseButton(0))
            {
                Players.Move(path);
            }

            if (!Players.IsMoving)
                DrawPath();


        }
        private void DrawPath()
        {
            var CurrentMouseNode = FindNodeUnderMouse();
            if (CurrentMouseNode != null && PreviusMouseNode != CurrentMouseNode)
            {
                PreviusMouseNode = CurrentMouseNode;
                var playerPositionNode = Grid.GetNodeFromWorldPosition(Players.transform.position);

                var pf = new PathFinding(Grid.Grid, playerPositionNode, CurrentMouseNode);
                PathData.SetNewPath(pf.GetPath(), this.Players.ActionPoints);
            }
        }

        private Node FindNodeUnderMouse()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
                return Grid.GetNodeFromWorldPosition(hit.point);

            return null;
        }


        public static GameManager GetInstance;
        private void Awake()
        {

            GetInstance = this;
        }
    }
}
