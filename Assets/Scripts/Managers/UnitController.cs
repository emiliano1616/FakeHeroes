using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UnitController : MonoBehaviour
    {
        private List<Node> Path;
        private int pathIndex;
        private float MoveT;
        private float CurrentSpeed;
        private Vector2 StartPosition;
        private Vector2 EndPosition;
        private bool LerpInitiated;
        private Node PreviusNode;
        private int MovingSpeed = 6;

        public bool IsMoving;
        public float ActionPoints;

        private PathManager PathData;

        private void Awake()
        {

        }

        public Node Node
        {
            get
            {
                return GridBase.GetInstance.GetNodeFromWorldPosition(transform.position);
            }
        }

        public void Start()
        {
            PathData = PathManager.GetInstance;
        }

        private void Update()
        {
            if (this.IsMoving)
                MovingLogic();
        }

        private void MovingLogic()
        {
            if (!LerpInitiated)
                InitLerp();

            MoveT += Time.deltaTime * CurrentSpeed;

            if (MoveT > 1)
            {
                MoveT = 1;
                LerpInitiated = false;
                if (pathIndex < Path.Count - 1)
                {
                    pathIndex++;
                }
                else
                {
                    IsMoving = false;
                    PathData.Move();
                }
            }
            transform.position = Vector2.Lerp(StartPosition, EndPosition, MoveT);

        }

        private void InitLerp()
        {
            ActionPoints -= Path[pathIndex].NodeCost;
            PathData.Move();

            if (pathIndex == Path.Count)
            {
                this.IsMoving = false;
                return;
            }

            Node.IsWalkable = true;
            MoveT = 0;
            StartPosition = this.transform.position;
            EndPosition = Path[pathIndex].Position;
            float distance = Vector2.Distance(StartPosition, EndPosition);
            CurrentSpeed = this.MovingSpeed / distance;

            LerpInitiated = true;


        }

        public void Move(List<Node> path)
        {
            this.pathIndex = 0;
            this.Path = path;
            this.IsMoving = true;

        }
    }
}
