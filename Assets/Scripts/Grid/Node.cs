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
        //Node's position in the grid
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        //Node's costs for pathfinding purposes
        public float hCost { get; set; }
        public float gCost { get; set; }

        public float fCost { get { return hCost + gCost; } }

        public Node ParentNode { get; set; }
        public bool isWalkable { get; set; } = true;

        public GameObject worldObject { get; set; }

        public NodeTypes Type { get; set; }
        public enum NodeTypes
        {
            Ground,
            Water
        }
    }
}
