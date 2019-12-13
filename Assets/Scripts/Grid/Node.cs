using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class Node
    {
        public Node(Func<Node, Vector2> getPositionFunc,
            Action<Node,bool> setWalkableFunc)
        {
            this._getPositionFunc = getPositionFunc;
            this._setWalkableFunc = setWalkableFunc;
        }
        private Func<Node, Vector2> _getPositionFunc;
        private Action<Node, bool> _setWalkableFunc;

        //Node's position in the grid
        public int x { get; set; }
        public int y { get; set; }

        //Node's costs for pathfinding purposes
        public float hCost { get; set; }
        public float gCost { get; set; }
        public float fCost { get { return hCost + gCost; } }
        public float NodeCost { get; set; } = 1f;

        public Node ParentNode { get; set; }
        private bool _isWalkable;
        public bool IsWalkable
        {
            get
            {
                return _isWalkable;
            }
            set
            {
                this._setWalkableFunc?.Invoke(this, value);

                _isWalkable = value;
            }
        }
        public bool APAvailable { get; set; } = true;
        public Vector2 Position
        {
            get
            {
                if (this._getPositionFunc == null)
                    throw new Exception("GetPositionFunc cannot be null");
                return this._getPositionFunc(this);
            }
        }

        public Vector3 Position3
        {
            get
            {
                if (this._getPositionFunc == null)
                    throw new Exception("GetPositionFunc cannot be null");
                return this._getPositionFunc(this);
            }
        }

        public GameObject WorldObject { get; set; }

        //public GameObject WorldObject { get; set; }

        //public NodeTypes Type { get; set; }
        //public enum NodeTypes
        //{
        //    Ground,
        //    Water
        //}

        public SaveableNode ToSaveable
        {
            get
            {
                return new SaveableNode()
                {
                    IsWalkable = this.IsWalkable,
                    X = this.x,
                    Y = this.y
                };
            }
        }

        public override string ToString()
        {
            return $"X: {x} Y: {y}";
        }
    }
}
