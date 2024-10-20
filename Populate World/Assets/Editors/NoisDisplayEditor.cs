using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (NoiseDisplay))]
public class NoisDisplayEditor : Editor
{
    public override void OnInspectorGUI(){
        NoiseDisplay noiseDisplay = (NoiseDisplay)target;
        WorldGenerator world = FindAnyObjectByType<WorldGenerator>();

        DrawDefaultInspector();

        if(GUILayout.Button("Clear")){
            noiseDisplay.ClearTilemaps();
        }

        if(GUILayout.Button("Load Chunk")){
            noiseDisplay.LoadChunks();
        }

        if(GUILayout.Button("Draw 1")){
            noiseDisplay.DrawTiles(world.generatedGrid);
        }
        if(GUILayout.Button("Draw 2")){
            noiseDisplay.DrawTiles_2(world.generatedGrid);
        }
        if(GUILayout.Button("Draw 3")){
            noiseDisplay.DrawTiles_3(world.generatedGrid);
        }
        if(GUILayout.Button("Draw 4")){
            noiseDisplay.DrawTiles_4(world.generatedGrid);
        }
    }
}
