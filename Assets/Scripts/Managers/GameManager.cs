using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public Transform CurrentUnit;
        public bool IsPlayerMoving;

        public void Init()
        {
            var worldPosition = GridBase.GetInstance.GetWorldCoordinatesFromNode(5, 6);
            CurrentUnit.transform.position = worldPosition;


        }


        public static GameManager GetInstance;
        private void Awake()
        {
            GetInstance = this;
        }
    }
}
