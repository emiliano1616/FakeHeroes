using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class GridBase : MonoBehaviour
    {
        private readonly int _minSize = 16;
        private GameObject _debugNodeObj;
        private GridLevel _level;

        #region Public Variables
        public int SizeX = 32;
        public int SizeY = 32;
        public float ScaleXY = 1;
        public Node[,] Grid;

        public bool DebugNode = true;
        public Material DebugMaterial;
        public Material ObstacleMaterial;


        #endregion

        #region Unity Events
        private void Awake()
        {
            GetInstance = this;
        }
        private void Start()
        {
            InitPhase();
        }

        private void Update()
        {
            Camera.main.transform.Translate(
                Input.GetAxisRaw("Horizontal") * 0.75f, Input.GetAxisRaw("Vertical") * .75f, 0);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                Camera.main.orthographicSize = Camera.main.orthographicSize + Camera.main.orthographicSize * scroll;

            }

        }

        internal Node GetNodeFromWorldPosition(Vector2 point)
        {
            int x = Mathf.RoundToInt(point.x / ScaleXY);
            int y = Mathf.RoundToInt(point.y / ScaleXY);

            return GetNode(x, y);
        }

        #endregion
        private void CreateGrid()
        {
            this.Grid = new Node[SizeX, SizeY];

            _level = new GridLevel();
            _level.NodeParent = new GameObject();
            _level.NodeParent.name = "Level 0";

            CreateCollision();

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    var n = new Node(this.GetWorldCoordinatesFromNode);
                    n.x = x;
                    n.y = y;
                    n.IsWalkable = true;

                    if (DebugNode)
                    {

                        if ((x == 10 && y != 10)
                            ||
                            x == 12 && y != 2 && y != 18
                            ||
                            x == 14 && y != 4 && y != 30 && y != 15 


                            )
                        {
                            n.IsWalkable = false;
                        }

                        var targetPosition = GetWorldCoordinatesFromNode(x, y);
                        GameObject go = Instantiate(_debugNodeObj,
                            targetPosition, Quaternion.identity) as GameObject;

                        if (!n.IsWalkable)
                        {
                            go.GetComponentInChildren<MeshRenderer>().material = ObstacleMaterial;
                        }

                        //go.GetComponentInChildren<MeshRenderer>.sorting
                        go.transform.parent = _level.NodeParent.transform;
                        go.SetActive(true);

                    }

                    Grid[x, y] = n;

                }
            }
        }

        private void CreateCollision()
        {
            var go = new GameObject();
            var boxCollider = go.AddComponent<BoxCollider2D>();

            boxCollider.size = new Vector2(SizeX * ScaleXY + (ScaleXY * 2), SizeY * ScaleXY + (ScaleXY * 2));

            //middle of the grid
            boxCollider.transform.position = new Vector2((SizeX * ScaleXY) * 0.5f - (ScaleXY * 0.5f),
                (SizeY * ScaleXY) * 0.5f - (ScaleXY * 0.5f));

            _level.CollisionObject = go;
            _level.CollisionObject.name = "Level 0 Collision";
        }
        public static GridBase GetInstance { get; set; }
        public bool IsInitialized { get; internal set; } = false;

        public Node GetNode(int x, int y)
        {
            x = Mathf.Clamp(x, 0, SizeX - 1);
            y = Mathf.Clamp(y, 0, SizeY - 1);

            return Grid[x, y];
        }
        public Vector2 GetWorldCoordinatesFromNode(int x, int y)
        {
            Vector2 r = Vector2.zero;
            r.x = x * ScaleXY;
            r.y = y * ScaleXY;
            return r;
        }

        public Vector2 GetWorldCoordinatesFromNode(Node n)
        {
            Vector2 r = Vector2.zero;
            r.x = n.x * ScaleXY;
            r.y = n.y * ScaleXY;
            return r;
        }

        public void InitPhase()
        {
            if (DebugNode)
                _debugNodeObj = WorldNode();
            Check();
            CreateGrid();

            GameManager.GetInstance.Init();
            IsInitialized = true;

        }

        private GameObject WorldNode()
        {
            var go = new GameObject();
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(quad.GetComponent<Collider>());
            quad.transform.parent = go.transform;
            quad.transform.localPosition = Vector2.zero;
            quad.transform.localScale = Vector3.one * 0.95f;

            quad.GetComponent<MeshRenderer>().material = DebugMaterial;
            go.SetActive(false);
            return go;
        }

        void Check()
        {
            if (SizeX <= _minSize)
            {
                Debug.Log($"Size x is {SizeX}, assigning {_minSize}");
                SizeX = _minSize;
            }

            if (SizeY <= _minSize)
            {
                Debug.Log($"Size y is {SizeY}, assigning {_minSize}");
                SizeY = _minSize;
            }

            if (ScaleXY == 0)
            {
                Debug.Log($"Scaleis {ScaleXY}, assigning 1");
                ScaleXY = 1;
            }
        }
    }
}
