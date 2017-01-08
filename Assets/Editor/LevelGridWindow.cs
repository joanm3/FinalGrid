using UnityEngine;
using UnityEditor;
using System.Collections;
using EditorSupport;

public class LevelGridWindow : EditorWindow
{
    LevelGrid m_levelGrid = null;
    SnapToGrid m_snapToGrid = null;
    static bool _showControls = true;
    static bool _selectionWithoutSnap = false;
    static bool _noTransformSelected = false;

    public void Init()
    {
        m_levelGrid = LevelGrid.Ins;
    }

    private void OnEnable()
    {
        _selectionWithoutSnap = false;
        SceneView.onSceneGUIDelegate += EventHandler;
    }



    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= EventHandler;
    }

    private void EventHandler(SceneView sceneview)
    {
        _noTransformSelected = (Selection.activeTransform == null);

        if (Selection.activeTransform != null)
        {
            if (Selection.activeTransform.gameObject == LevelGrid.Ins && Selection.activeTransform.gameObject.GetComponent<SnapToGrid>() == null)
            {
                _selectionWithoutSnap = true;

            }
        }

        if (Selection.activeTransform != null)
        {
            m_snapToGrid = Selection.activeTransform.GetComponent<SnapToGrid>();
        }

        Repaint();


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
        EditorGUILayout.Space();

        MontStyle.fontSize = 20;
        MontStyle.alignment = TextAnchor.LowerLeft;
        MontStyle.font = (Font)Resources.Load("MontserratAlternates-ExtraBold");
        EditorGUILayout.LabelField("Grid", MontStyle);

        if (LevelGrid.Ins == null)
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("Add LevelGrid"))
            {
                LevelGridEditor.AddLevelGrid();
            }
            return;
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(20f));
        m_levelGrid.snapToGrid = EditorGUILayout.Toggle(m_levelGrid.snapToGrid);
        EditorGUILayout.LabelField("Snap to grid");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(20f));
        LevelGrid.Ins.hideUnityHandles = EditorGUILayout.Toggle(LevelGrid.Ins.hideUnityHandles);
        EditorGUILayout.LabelField("Hide Unity Handles");
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.PrefixLabel("Grid Pow:");
        m_levelGrid.gridSize = (LevelGrid.Pow2)EditorGUILayout.EnumPopup(m_levelGrid.gridSize);
        EditorGUILayout.PrefixLabel("Height Change Pow:");
        m_levelGrid.heightGridSize = (LevelGrid.Pow2)EditorGUILayout.EnumPopup(m_levelGrid.heightGridSize);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (!_noTransformSelected)
        {
            if (_selectionWithoutSnap)
            {

                EditorGUILayout.LabelField("Selected GameObject", MontStyle);
                if (GUILayout.Button("Snap GameObject to Grid"))
                {
                    Selection.activeTransform.gameObject.AddComponent<SnapToGrid>();
                    _selectionWithoutSnap = false;
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            if (m_snapToGrid != null)
            {
                EditorGUILayout.LabelField("Selected GameObject", MontStyle);

                EditorGUILayout.PrefixLabel("Pivot Point:");
                m_snapToGrid.pivot = (SnapToGrid.PivotPoint)EditorGUILayout.EnumPopup(m_snapToGrid.pivot);
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(20f));
                m_snapToGrid.removeBoxColliderInGame = EditorGUILayout.Toggle(m_snapToGrid.removeBoxColliderInGame);
                EditorGUILayout.LabelField("Remove Collider in game");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
        }

        GUIStyle _foldout = EditorStyles.foldout;
        _foldout.font = (Font)Resources.Load("MontserratAlternates-ExtraBold");
        _foldout.fontSize = 20;
        _showControls = EditorGUILayout.Foldout(_showControls, "Controls", _foldout);
        EditorGUILayout.Space();
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
