using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NavpointEditor : EditorWindow
{

    [MenuItem("Tools/Nav Points")]
    public static void Open()
    {
        GetWindow<NavpointEditor>();
    }

    public Transform navpointRoot;
    private Vector3 colliderScale;

    private void OnGUI()
    {
        if (navpointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected. Please assign root transform. ", MessageType.Warning);
        }
        
         EditorGUILayout.BeginVertical("box");
         DrawButtons();
         EditorGUILayout.EndVertical();
    }

    void DrawButtons()
    {   
         if (GUILayout.Button("Create Navigation Point"))
         {
             CreateWaypoint();
         }
    }

    void CreateWaypoint()
    {
        navpointRoot = Selection.activeGameObject.GetComponent<Transform>();

        GameObject waypointObject = new GameObject("NavPoint" + navpointRoot.childCount, typeof(NavigationPoint));
        waypointObject.transform.SetParent(navpointRoot, false);

        if(navpointRoot.childCount > 1)
        {
            waypointObject.transform.position = navpointRoot.GetChild(navpointRoot.childCount - 1).transform.position;
        }

        BoxCollider col = waypointObject.GetComponent<BoxCollider>();
        col.isTrigger = true;
        colliderScale = new Vector3(20f, 20f, 20f);
        col.size = colliderScale;
    }
}
