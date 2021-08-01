using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UdonSharp;
using UdonSharpEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(IslandManager))]
public class IslandManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        IslandManager manager = (IslandManager) target;

        if (GUILayout.Button("Find and apply all islands"))
        {
            Debug.Log("[<color=blue>Island Manager</color>] Finding all islands and applying them...");
            manager.UpdateProxy();

            var allRootObjects = new List<GameObject>(1000);
            SceneManager.GetActiveScene().GetRootGameObjects(allRootObjects);

            List<Island> newIslands = new List<Island>();
            foreach (var obj in allRootObjects)
            {
                var udonBehaviours = obj.GetUdonSharpComponentsInChildren<Island>();
                foreach (var ub in udonBehaviours)
                {
                    newIslands.Add(ub);
                }
            }

            Debug.Log("[<color=blue>Island Manager</color>] Found " + newIslands.Count + " Islands!");

            manager.islands = new Island[0];

            manager.islands = newIslands.ToArray();

            manager.ApplyProxyModifications();
        }
    }
}
