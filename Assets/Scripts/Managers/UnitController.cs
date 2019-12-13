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
        public UnitController()
        : base()
        {

        }
        public int x, y;
        private List<Node> Path;
        //private int pathIndex;
        private float MoveT;
        private float CurrentSpeed;
        private Vector2 StartPosition;
        private Vector2 EndPosition;
        private bool LerpInitiated;
        private int MovingSpeed = 6;

        public bool IsMoving;
        public float ActionPoints;

        private PathManager PathData;
        private Animator animator;
        private Node CurrentNode { get; set; }
        private Node TargetNode { get; set; }

        public SaveableHero ToSaveable
        {
            get
            {
                return new SaveableHero()
                {
                    HeroPlayer = 1, //TODO
                    HeroIndex = 0,
                    X = CurrentNode.x,
                    Y = CurrentNode.y,
                    ActionPoints = ActionPoints
                };
            }
        }
        //public Node Node
        //{
        //    get
        //    {
        //        return GridBase.GetInstance.GetNodeFromWorldPosition(transform.position);
        //    }
        //}


        public void Init()
        {
            transform.position = GridBase.GetInstance.GetWorldCoordinatesFromNode(x, y);
            this.CurrentNode = GridBase.GetInstance.GetNode(x, y);
            this.CurrentNode.IsWalkable = false;
            this.animator = this.GetComponent<Animator>();
        }

        public void Start()
        {
            PathData = PathManager.GetInstance;
        }

        private bool lastMoving = false;
        private void Update()
        {
            if (this.IsMoving)
                MovingLogic();

            if (this.IsMoving != lastMoving)
            {
                lastMoving = IsMoving;
                animator.SetBool("moving", this.IsMoving);
            }
        }

        private void MovingLogic()
        {
            if (!LerpInitiated)
                InitLerp();

            MoveT += Time.deltaTime * CurrentSpeed;

            //Means that I moved a full square
            if (MoveT > 1)
            {
                MoveT = 1;
                LerpInitiated = false;
                if (!Path.Any())
                {
                    IsMoving = false;
                    CurrentNode.IsWalkable = false;
                }
                else
                {
                    //Debug.Log(CurrentNode);
                    ActionPoints -= CurrentNode == TargetNode ? 0 : CurrentNode.NodeCost;
                    PathData.Move();
                }

            }
            transform.position = Vector2.Lerp(StartPosition, EndPosition, MoveT);

        }

        private void InitLerp()
        {
            if (!Path.Any())
            {
                this.IsMoving = false;
                CurrentNode.IsWalkable = false;
                return;
            }
            this.CurrentNode = Path.First();
            Path.Remove(this.CurrentNode);

            MoveT = 0;
            StartPosition = this.transform.position;
            EndPosition = this.CurrentNode.Position;
            float distance = Vector2.Distance(StartPosition, EndPosition);
            CurrentSpeed = this.MovingSpeed / distance;

            LerpInitiated = true;
        }

        public void Move(List<Node> path)
        {
            if (path == null || !path.Any()) return;
            this.CurrentNode.IsWalkable = true;
            //this.pathIndex = 0;
            this.Path = path;
            this.TargetNode = this.Path.Last();
            this.IsMoving = true;

        }
    }
}
