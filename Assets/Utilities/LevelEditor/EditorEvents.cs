using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Utilities.LevelEditor
{
    [CustomEditor(typeof(LevelEditor))]
    public class EditorEvents : Editor
    {
        private LevelEditor level;
        private void OnSceneGUI()
        {
            if (level == null)
                level = LevelEditor.GetInstance;

            var e = Event.current;

            switch (e.type)
            {
                case EventType.MouseDown:
                    break;
            }
        }
    }
}
