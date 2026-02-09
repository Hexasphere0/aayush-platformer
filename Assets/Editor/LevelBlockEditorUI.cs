using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelBlockEditor))]
public class LevelBlockEditorUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelBlockEditor scriptReference = (LevelBlockEditor) target;

        if(GUILayout.Button("Change Layer"))
        {
            scriptReference.ChangeLayer();
        }
    }
}
