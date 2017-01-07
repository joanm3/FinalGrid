using UnityEngine;
using UnityEditor;
using System.Collections;

public class LevelGridWindow : EditorWindow
{
    LevelGrid m_levelGrid = null;

    static bool _showControls = true;

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

        GUIStyle MontStyle = new GUIStyle(); 

        MontStyle.alignment = TextAnchor.MiddleCenter;
        MontStyle.font = (Font)Resources.Load("HomeRem");
        MontStyle.fontSize = 40;
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Grid Editor", MontStyle);
        //m_levelGrid.hideUnityHandles = EditorGUILayout.Toggle("Hide Unity Handles:", m_levelGrid.hideUnityHandles);
        //EditorGUILayout.PrefixLabel("Snap to Grid:");
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(20f)); 
        m_levelGrid.snapToGrid = EditorGUILayout.Toggle( m_levelGrid.snapToGrid);
        EditorGUILayout.LabelField("Snap to grid");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PrefixLabel("Grid Pow:");
        m_levelGrid.gridSize = (LevelGrid.Pow2)EditorGUILayout.EnumPopup(m_levelGrid.gridSize);
        EditorGUILayout.PrefixLabel("Height Change Pow:");
        m_levelGrid.heightGridSize = (LevelGrid.Pow2)EditorGUILayout.EnumPopup(m_levelGrid.gridSize);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        _showControls = EditorGUILayout.Foldout(_showControls, "Controls");
        if (_showControls)
        {
            EditorGUILayout.HelpBox("Ctrl + move: copy", MessageType.None);
            EditorGUILayout.HelpBox("Y: move height up", MessageType.None);
            EditorGUILayout.HelpBox("U: move height down", MessageType.None);
            EditorGUILayout.HelpBox("I: reset height to zero", MessageType.None);
            EditorGUILayout.HelpBox("O: show / hide grid", MessageType.None);
            EditorGUILayout.HelpBox("A: rotate object 90º", MessageType.None);
        }
        EditorGUILayout.HelpBox("Use Rectbox for better Usage. Rectbox shortcut: T", MessageType.Info);
        EditorGUILayout.HelpBox("Level Grid has to be in Layer with name 'Grid'", MessageType.Info);


        //m_levelGrid.Update(); 
    }

}
