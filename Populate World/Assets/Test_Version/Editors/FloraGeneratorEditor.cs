using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

// [CustomEditor(typeof(FloraGenerator))]
public class FloraGeneratorEditor : Editor
{
    /* public override void OnInspectorGUI()
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

        if (GUILayout.Button("Generate Forest"))
        {
            floraGenerator.GenerateForestNoise();
        }
        if (GUILayout.Button("Draw Forest"))
        {
            floraGenerator.DrawForest();
        }
        if (GUILayout.Button("Draw Noise"))
        {
            floraGenerator.DrawNoiseMap();
        }
        if (GUILayout.Button("Draw Value"))
        {
            floraGenerator.DrawValues();
        }
    } */
}
