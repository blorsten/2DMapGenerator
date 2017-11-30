using MapGeneration;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Chunk))]
public class ChunkEditor : Editor
{
    private Chunk _chunk;

    private void OnEnable()
    {
        _chunk = target as Chunk;
    }

    public override void OnInspectorGUI()
    {
        if (_chunk.RecipeReference)
            if (GUILayout.Button("Find Recipe"))
                EditorGUIUtility.PingObject(_chunk.RecipeReference.gameObject);

        base.OnInspectorGUI();
    }
}
