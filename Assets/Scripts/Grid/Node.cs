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
        public Node(Func<Node, Vector2> getPositionFunc)
        {
            this._getPositionFunc = getPositionFunc;
        }
        private Func<Node, Vector2> _getPositionFunc;
        //Node's position in the grid
        public int x { get; set; }
        public int y { get; set; }

        //Node's costs for pathfinding purposes
        public float hCost { get; set; }
        public float gCost { get; set; }
        public float fCost { get { return hCost + gCost; } }
        public float NodeCost { get; set; } = 1f;

        public Node ParentNode { get; set; }
        public bool IsWalkable { get; set; } = true;
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

        //public GameObject WorldObject { get; set; }

        //public NodeTypes Type { get; set; }
        //public enum NodeTypes
        //{
        //    Ground,
        //    Water
        //}

        public override string ToString()
        {
            return $"X: {x} Y: {y}";
        }
    }
}
