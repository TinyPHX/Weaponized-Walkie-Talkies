using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// TODO: add documentation for PrefabLinkEditor
/// </summary>
[CustomEditor(typeof(PrefabLink))]
[ExecuteInEditMode]
public class PrefabLinkEditor : Editor
{ 
    string message = "";
    MessageType messageType = MessageType.None;
    float messageDuration = 1;
    float messageStartTime = 0;

    public override void OnInspectorGUI()
    {
        PrefabLink prefabLink = (PrefabLink)target;
        
        DrawDefaultInspector();

        bool revertAttempt = false;
        
        if (GUILayout.Button("Revert")) {
            revertAttempt = true;

            Object prefab = PrefabUtility.GetPrefabParent(prefabLink.gameObject);
            
            bool revertSuccessful = prefabLink.Revert(prefab as GameObject);

            if (!revertSuccessful)
            {
                message = "Reverting was uncessful!";
                messageType = MessageType.Warning;
                messageStartTime = Time.time;
            }
            else
            {
                message = "Reverting succesful.";
                messageType = MessageType.Info;
                messageStartTime = Time.time;
            }
        }

        if (message != "" && Time.time - messageStartTime < messageDuration)
        {
            EditorGUILayout.HelpBox(message, messageType);
        }

        
        //if (revertAttempt) {
        //    GUIUtility.ExitGUI();
        //}
    }
}