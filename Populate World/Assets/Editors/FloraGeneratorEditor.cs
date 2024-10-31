using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloraGenerator))]
public class FloraGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FloraGenerator floraGenerator = (FloraGenerator)target;
        WorldGenerator world = FindAnyObjectByType<WorldGenerator>();

        DrawDefaultInspector();

        if (GUILayout.Button("Clear Flora"))
        {
            floraGenerator.ClearFloraMap();
        }

        if (GUILayout.Button("Generate Flora"))
        {
            floraGenerator.GenerateFlora_1(world.generatedGrid_LW);
        }

        if (GUILayout.Button("Draw Flora"))
        {
            floraGenerator.DrawFlora();
        }
    }
}
