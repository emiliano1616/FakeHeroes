using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public float RotateSpeed = 0.2f;
        private void Update()
        {
            Movement();
            Rotation();
        }

        private void Movement()
        {
            Camera.main.transform.Translate(
                Input.GetAxisRaw("Horizontal") * 0.5f, Input.GetAxisRaw("Vertical") * .5f, 0);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                Camera.main.orthographicSize = Camera.main.orthographicSize - Camera.main.orthographicSize * scroll;
            }
        }

        private void Rotation()
        {
            bool rotateLeft = Input.GetKey(KeyCode.Q);
            bool rotateRigth = Input.GetKey(KeyCode.E);
            if(rotateLeft || rotateRigth)
            {
                Vector3 euler = transform.localEulerAngles;
                euler.z += RotateSpeed * (rotateLeft ? -1 : 1);
                transform.localEulerAngles = euler;
            }
        }

        public static CameraManager GetInstance;
        private void Awake()
        {
            GetInstance = this;
        }
    }
}
