using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LayerChanger))]
public class LevelBlockEditorUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LayerChanger scriptReference = (LayerChanger) target;

        if(GUILayout.Button("Change Layer"))
        {
            scriptReference.ChangeLayer();
        }
    }
}
