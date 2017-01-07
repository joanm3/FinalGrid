using UnityEngine;
using UnityEditor; 
using System.Collections;

public class LevelGridWindow : EditorWindow
{
    LevelGrid m_levelGrid = null; 

    public void Init()
    {
        m_levelGrid = LevelGrid.Ins; 
    }

    void OnGUI()
    {
        if (m_levelGrid == null)
        {
            Init(); 
        }
        //m_levelGrid.hideUnityHandles = EditorGUILayout.Toggle("Hide Unity Handles:", m_levelGrid.hideUnityHandles); 
        EditorGUILayout.PrefixLabel("Grid Pow:"); 
        m_levelGrid.gridSize = (LevelGrid.Pow2)EditorGUILayout.EnumPopup(m_levelGrid.gridSize);
        EditorGUILayout.PrefixLabel("Height Level:");
        m_levelGrid.height = EditorGUILayout.FloatField(m_levelGrid.heightIndex);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Controls:"); 
        EditorGUILayout.HelpBox("Ctrl + move: copy", MessageType.None);
        EditorGUILayout.HelpBox("Y: move height up", MessageType.None);
        EditorGUILayout.HelpBox("U: move height down", MessageType.None);
        EditorGUILayout.HelpBox("A: rotate object 90º", MessageType.None);
        EditorGUILayout.HelpBox("Use Rectbox for better Usage. Rectbox shortcut: T", MessageType.Info); 


        //m_levelGrid.Update(); 
    }

}
