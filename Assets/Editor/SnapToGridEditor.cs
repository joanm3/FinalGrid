﻿using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(SnapToGrid))]
public class SnapToGridEditor : Editor
{
    SnapToGrid m_myTarget;
    float distance;
    Vector3 gridPos = new Vector3();
    GameObject onMouseOverGameObject;
    bool m_instantiated = false;
    static private bool m_controlPressed = false;
    static private bool m_rotationKeyPressed = false;
    static private bool m_shiftPressed = false;
    static private bool m_leftMousePressed = false;
    //private bool m_showGridKeyPressed = false; 
    static bool isThisObject = false;
    static bool isMouseDown = false;
    private bool objectDragged = false;
    //GameObject m_instantiatedGameObject = new GameObject(); 

    private void OnEnable()
    {
        m_instantiated = false;
        SceneView.onSceneGUIDelegate += EventHandler;
    }

    private void OnDisable()
    {
        m_instantiated = false;
        SceneView.onSceneGUIDelegate -= EventHandler;
    }

    private void EventHandler(SceneView sceneview)
    {
        if (m_myTarget == null)
            m_myTarget = target as SnapToGrid;


        if (m_myTarget.childToApplyBoxCollider != null)
        {
            BoxCollider _boxCollider = m_myTarget.GetComponent<BoxCollider>();
            _boxCollider.size = m_myTarget.childToApplyBoxCollider.bounds.size;
            _boxCollider.center = m_myTarget.childToApplyBoxCollider.bounds.center;

        }


        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(worldRay, 1000);

        for (int i = 0; i < hits.Length; i++)
        {

            //Debug.Log("hit:" + hits[i].collider.gameObject.name);
            if (hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Grid"))
            {
                gridPos = hits[i].point;
            }
            else if (hits[i].transform.GetComponent<SnapToGrid>() != null)
            {
                //Debug.Log("Object name: " + hits[i].collider.name);
                onMouseOverGameObject = hits[i].transform.gameObject;
            }
        }

        if (hits.Length < 1)
            onMouseOverGameObject = null;

        //Debug.Log(onMouseOverGameObject);

        UpdateKeyEvents();

        //mouse position in the grid
        float col = (float)gridPos.x / ((float)LevelGrid.Ins.gridSize * LevelGrid.Ins.scaleFactor);
        float row = (float)gridPos.z / ((float)LevelGrid.Ins.gridSize * LevelGrid.Ins.scaleFactor);

        LevelGrid.Ins.UpdateInputGridHeight();

        //Debug.Log(onMouseOverGameObject == m_myTarget.gameObject);
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {

            isMouseDown = true;
            if (onMouseOverGameObject == null)
                return;

            if (onMouseOverGameObject == m_myTarget.gameObject)
            {
                isThisObject = true;
            }
            else
            {
                isThisObject = false;
                if (onMouseOverGameObject != null)
                {
                    Selection.activeGameObject = onMouseOverGameObject;
                }
                else
                {
                    //SnapToGrid((int)col, (int)row, LevelGrid.Ins.height);
                    Selection.activeGameObject = null;
                }
            }

        }

        if (isMouseDown && m_rotationKeyPressed)
        {
            Debug.Log("entered here");
            LevelGrid.Ins.selectedGameObject.transform.eulerAngles += new Vector3(0, 90f, 0);
            m_rotationKeyPressed = false;
        }


        //mouse click and dragandrop
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            if (Selection.activeGameObject == null)
                return;
            if (m_rotationKeyPressed)
            {
                LevelGrid.Ins.selectedGameObject.transform.eulerAngles += new Vector3(0, 90f, 0);
                m_rotationKeyPressed = false;
            }
            SnapToGrid((int)col, (int)row, LevelGrid.Ins.height);
            objectDragged = true;
        }
        ////Debug.Log(m_shiftPressed);
        //Debug.Log("Event.current.keyCode: " + Event.current.keyCode);

        //if mouse released when control pressed, make a copy / otherwise, destroy old object. 
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            isMouseDown = false;
            if (LevelGrid.Ins.selectedGameObject)
            {
                //make copy if control is pressed
                if (!m_controlPressed)
                {
                    Undo.IncrementCurrentGroup();
                    if (m_instantiated)
                        Undo.DestroyObjectImmediate(m_myTarget.gameObject);
                }
                if (objectDragged)
                {
                    Selection.activeGameObject = LevelGrid.Ins.selectedGameObject;
                    objectDragged = false;
                }
                m_instantiated = false;
            }
        }

        if ((Event.current.type == EventType.keyUp) && (Event.current.keyCode == KeyCode.O))
        {
            LevelGrid.Ins.showGrid = !LevelGrid.Ins.showGrid;
        }
    }

    private void UpdateKeyEvents()
    {
        if ((Event.current.type == EventType.keyDown) && (Event.current.keyCode == KeyCode.A || Event.current.keyCode == KeyCode.S))
            m_rotationKeyPressed = true;

        if ((Event.current.type == EventType.keyUp) && (Event.current.keyCode == KeyCode.A || Event.current.keyCode == KeyCode.S))
            m_rotationKeyPressed = false;

        //check if control is pressed. 
        if ((Event.current.type == EventType.keyDown) && (Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl))
            m_controlPressed = true;

        if ((Event.current.type == EventType.keyUp) && (Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl))
            m_controlPressed = false;

        //if ((Event.current.type == EventType.keyDown) && (Event.current.keyCode == KeyCode.I ))
        //    m_showGridKeyPressed = true;

        //if ((Event.current.type == EventType.keyUp) && (Event.current.keyCode == KeyCode.I ))
        //    m_showGridKeyPressed = false;


    }

    private void SnapToGrid(int col, int row, float height)
    {
        if (m_myTarget == null)
            m_myTarget = target as SnapToGrid;

        if (!LevelGrid.Ins.snapToGrid)
            return;

        // Check out of bounds and if we have a piece selected
        //if (!LevelGrid.Ins.IsInsideGridBounds(col, row))
        //    return;

        GameObject obj = m_myTarget.gameObject;
        if (!m_instantiated)
        {

            if (PrefabUtility.GetPrefabParent(Selection.activeObject) != null)
            {
                obj = PrefabUtility.InstantiatePrefab(PrefabUtility.GetPrefabParent(Selection.activeObject) as GameObject) as GameObject;
                obj.transform.rotation = m_myTarget.gameObject.transform.rotation;

            }
            else
            {
                //Debug.Log("prefab parent not found");
                obj = Instantiate(m_myTarget.gameObject);
            }

            obj.name = m_myTarget.gameObject.name;
            if (LevelGrid.Ins.parentGameObject != null) obj.transform.parent = LevelGrid.Ins.parentGameObject;

            LevelGrid.Ins.selectedGameObject = obj;

            m_instantiated = true;
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
        }
        LevelGrid.Ins.selectedGameObject.transform.position = LevelGrid.Ins.GridToWorldCoordinates(col, row, height);
        Undo.IncrementCurrentGroup();
    }
}
