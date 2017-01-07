using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorSupport;

[CustomEditor(typeof(LevelGrid))]
public class LevelGridEditor : Editor
{
    LevelGrid _myTarget;

    private void OnEnable()
    {
        _myTarget = target as LevelGrid;
        _myTarget.boxCollider = _myTarget.GetComponent<BoxCollider>();
        SceneView.onSceneGUIDelegate += EventHandler;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= EventHandler;
    }

    private void EventHandler(SceneView sceneview)
    {
        if (!_myTarget)
            _myTarget = target as LevelGrid;

        _myTarget.transform.position = Vector3.zero;

        float cols = _myTarget.sizeColums;
        float rows = _myTarget.sizeRows;

        //properly place the collider
        _myTarget.UpdateBoxCollider(_myTarget.boxCollider, cols, rows, _myTarget.height);


        LevelGrid.Ins.UpdateInputGridHeight();
        //ToolsSupport.UnityHandlesHidden = _myTarget.hideUnityHandles; 
    }



    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Open Grid Window", GUILayout.Width(255)))
        {
            OpenLevelGridWindow();
        }
    }

    [MenuItem("Level Grid/Show Level Grid Window")]
    static public void OpenLevelGridWindow()
    {
        LevelGridWindow window = (LevelGridWindow)EditorWindow.GetWindow(typeof(LevelGridWindow));
        window.Init();
    }

    [MenuItem("Level Grid/Add LevelGrid")]
    static public void AddLevelGrid()
    {
        if (LevelGrid.Ins == null)
        {
            GameObject go = Instantiate(Resources.Load("LevelGrid", typeof(GameObject))) as GameObject;
            go.transform.position = Vector3.zero;
            LevelGrid.Ins = go.GetComponent<LevelGrid>();
        } else
        {
            Debug.LogError("Already a LevelGrid Singleton in this scene");
        }
    }



}
