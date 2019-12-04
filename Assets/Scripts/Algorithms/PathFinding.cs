using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Algorithms
{
    public class PathFinding
    {
        private bool[,] _closedList;
        private List<Node> _openList;
        public Node[,] Grid { get; }
        public Node Source { get; }
        public Node Destination { get; }
        private List<Node> _path;
        private bool _pathRequested;
        public PathFinding(Node[,] grid, Node source, Node destination)
        {
            this.Grid = grid;
            this.Source = source;
            this.Destination = destination;
            this._closedList = new bool[this.Grid.GetLength(0), this.Grid.GetLength(1)];
            this._path = new List<Node>();
            this._openList = new List<Node>();

            for (int x = 0; x < this.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < this.Grid.GetLength(1); y++)
                {
                    this._closedList[x, y] = false;
                    var n = this.Grid[x, y];
                    n.hCost = double.MaxValue;
                    n.gCost = double.MaxValue;
                    n.ParentNode = null;
                }
            }
        }
        private Node GetNode(int x, int y)
        {
            if (x >= 0 && y >= 0
                && Grid.GetLength(0) > x && Grid.GetLength(1) > y)
                return Grid[x, y];
            return null;
        }
        private bool IsDestination(Node n)
        {
            return this.Destination.x == n.x && this.Destination.y == n.y;
        }

        private double CalculateH(Node current)
        {
            return Math.Max(Math.Abs(current.x - Destination.x), Math.Abs(current.y - Destination.y));
        }

        public List<Node> GetPath()
        {
            if (_pathRequested)
                return _path;
            _pathRequested = true;

            if (!this.Destination.IsWalkable || !this.Source.IsWalkable)
            {
                Debug.Log("Source or the destination is blocked");
                return null;
            }

            if (this.IsDestination(this.Source))
            {
                Debug.Log("We are already at the destination");
                return null;
            }

            Source.gCost = 0;
            Source.hCost = 0;
            Source.ParentNode = Source;

            _openList.Add(Source);
            bool destinationFound = false;

            while (_openList.Any() && !destinationFound)
            {
                var currentNode = _openList.First();
                _openList.Remove(currentNode);
                _closedList[currentNode.x, currentNode.y] = true;

                destinationFound = EvaluateNeighbor(currentNode, this.GetNode(currentNode.x - 1, currentNode.y)) ??
                    EvaluateNeighbor(currentNode, this.GetNode(currentNode.x + 1, currentNode.y)) ??
                    EvaluateNeighbor(currentNode, this.GetNode(currentNode.x, currentNode.y + 1)) ??
                    EvaluateNeighbor(currentNode, this.GetNode(currentNode.x, currentNode.y - 1)) ??
                    EvaluateNeighbor(currentNode, this.GetNode(currentNode.x - 1, currentNode.y + 1)) ??
                    EvaluateNeighbor(currentNode, this.GetNode(currentNode.x - 1, currentNode.y - 1)) ??
                    EvaluateNeighbor(currentNode, this.GetNode(currentNode.x + 1, currentNode.y + 1)) ??
                    EvaluateNeighbor(currentNode, this.GetNode(currentNode.x + 1, currentNode.y - 1)) ??
                    false;
            }

            if (destinationFound)
                return _path;

            return null;

        }

        private bool? EvaluateNeighbor(Node currentNode, Node nextNode)
        {
            if (nextNode == null)
                return null;

            if (IsDestination(nextNode))
            {
                nextNode.ParentNode = currentNode;
                //Debug.Log("Destination found");
                TracePath();
                return true;
            }

            if (_closedList[nextNode.x, nextNode.y] || !nextNode.IsWalkable)
                return null;
            var gNew = currentNode.gCost + nextNode.NodeCost;
            var hNew = CalculateH(nextNode);
            var fNew = gNew + hNew;

            if (nextNode.fCost == double.MaxValue ||
                nextNode.fCost > fNew)
            {
                _openList.Add(nextNode);

                nextNode.gCost = gNew;
                nextNode.hCost = hNew;
                nextNode.ParentNode = currentNode;
            }
            return null;
        }

        private void TracePath()
        {
            var tempNode = Destination;
            var stack = new Stack<Node>();

            while (tempNode.ParentNode != tempNode)
            {
                stack.Push(tempNode);
                tempNode = tempNode.ParentNode;
            }
            stack.Push(tempNode);

            while (stack.Any())
            {
                _path.Add(stack.Pop());
            }
        }
    }
}
