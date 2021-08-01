using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Teleport))]
public class TeleportEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Teleport teleport = (Teleport)target;

        EditorGUI.BeginChangeCheck();

        Island newIsland = (Island)EditorGUILayout.ObjectField("Island", teleport.island, typeof(Island), true);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(teleport, "Modify Island Value");

            teleport.island = newIsland;
        }
    }
}
