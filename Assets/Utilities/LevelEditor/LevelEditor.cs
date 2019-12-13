using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Utilities.LevelEditor
{
    [ExecuteInEditMode]
    public class LevelEditor : Editor
    {
        public static LevelEditor GetInstance;

        private void Update()
        {
            if (GetInstance == null)
                GetInstance = this;
        }
    }
}
