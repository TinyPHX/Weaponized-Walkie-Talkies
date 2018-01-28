using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor class for the PlayerCharacterController
/// </summary>
[CustomEditor(typeof(Effect))]
[ExecuteInEditMode]
public class EffectEditor : Editor
{
    int tab;

    public override void OnInspectorGUI()
    {
        Effect effect = (Effect)target;
        
        DrawDefaultInspector();
        
        if (GUILayout.Button("Play")) { effect.Play(); }
        
        //tab = GUILayout.Toolbar (tab, new string[] {"Object", "Bake", "Layers"});
        
        ////GARBAGE - THIS IS HOW YOU HAVE TO GET PRIVATE VARIBLES IN THE EDITOR
        //serializedObject.FindProperty("scale").floatValue = EditorGUILayout.FloatField("scale", serializedObject.FindProperty("scale").floatValue);
    }

    [MenuItem("Tools/Tiny Tools/Add Effect")]
    private static void AddEffect()
    {
        GameObject activeGameObject = (GameObject)Selection.activeObject;

        activeGameObject.AddComponent<Effect>();
    }

    [MenuItem("Assets/ProcessTexture", true)]
    private static bool NewMenuOptionValidation()
    {
        // This returns true when the selected object is a Texture2D (the menu item will be disabled otherwise).
        return Selection.activeObject.GetType() == typeof(GameObject);
    }
}
