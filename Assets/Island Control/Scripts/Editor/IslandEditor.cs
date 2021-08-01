using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UdonSharp;
using UdonSharpEditor;

[CustomEditor(typeof(Island))]
public class IslandEditor : Editor
{
    enum RenderType { Wireframe, Solid }

    public override void OnInspectorGUI()
    {
        Island island = (Island)target;

        GUILayout.Label("Island Control", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        GUILayout.Space(10f);
        //GUILayout.Label("Collider");
        float newFailsafe = EditorGUILayout.FloatField("Collider Size", island.failsafeColliderSize);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(island, "Modify Float Value");

            island.failsafeColliderSize = newFailsafe;
        }

        EditorGUI.BeginChangeCheck();

        GUIContent rtContent = new GUIContent("Collider Render Type", "Only shows in editor");
        RenderType rt = (RenderType)EditorGUILayout.EnumPopup(rtContent, (island.renderObaqueCube) ? RenderType.Solid : RenderType.Wireframe);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(island, "Modify Render Type");

            switch (rt)
            {
                case RenderType.Solid:
                    island.renderObaqueCube = true;
                    break;
                case RenderType.Wireframe:
                    island.renderObaqueCube = false;
                    break;
                default:
                    island.renderObaqueCube = false;
                    break;
            }
        }

        EditorGUI.BeginChangeCheck();

        GUILayout.Space(10f);
        Transform newSpawn = (Transform)EditorGUILayout.ObjectField("Teleport Position", island.spawnLocation, typeof(Transform), true);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(island, "Modify Transform Value");

            island.spawnLocation = newSpawn;
        }
    }
}
