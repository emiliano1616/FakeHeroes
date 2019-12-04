using Assets.Scripts.Algorithms;
using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public Transform Player;
        public bool IsPlayerMoving;
        public Material AvailableMovementMaterial;
        public Material MissingMovementMaterial;
        public int ActionPoints = 10;
        //private Node CurrentMouseNode;
        private Node PreviusMouseNode;
        private PathManager _pathData;



        public void Init()
        {
            _pathData = new PathManager(ActionPoints, AvailableMovementMaterial, MissingMovementMaterial);
            Player.transform.position = GridBase.GetInstance.GetWorldCoordinatesFromNode(5, 6); 
        }

        private void Update()
        {
            if (!GridBase.GetInstance.IsInitialized) return;
            if (Player == null) return;

            DrawPath();
        }
        private void DrawPath()
        {
            var CurrentMouseNode = FindNodeUnderMouse();
            if (CurrentMouseNode != null && PreviusMouseNode != CurrentMouseNode)
            {
                PreviusMouseNode = CurrentMouseNode;
                var playerPositionNode = GridBase.GetInstance.GetNodeFromWorldPosition(Player.transform.position);

                var pf = new PathFinding(GridBase.GetInstance.Grid, playerPositionNode, CurrentMouseNode);
                _pathData.SetNewPath(pf.GetPath(), this.ActionPoints);
            }
        }

        private Node FindNodeUnderMouse()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
                return  GridBase.GetInstance.GetNodeFromWorldPosition(hit.point);

            return null;
        }

        public static GameManager GetInstance;
        private void Awake()
        {
            GetInstance = this;
        }
    }
}
