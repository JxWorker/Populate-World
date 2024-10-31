using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoiseDisplay))]
public class NoisDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseDisplay noiseDisplay = (NoiseDisplay)target;
        WorldGenerator world = FindAnyObjectByType<WorldGenerator>();

        DrawDefaultInspector();

        if (GUILayout.Button("Clear"))
        {
            noiseDisplay.ClearTilemaps();
        }

        if (GUILayout.Button("Draw Noise"))
        {
            noiseDisplay.DrawNoiseMap(world.generatedGrid_Land);
        }


        //Wolrd Parameter
        //Amplitude 0.4
        //Frequency 0.02
        //Octaves 3
        //Lacunarity 2
        //Persistence 0.5
        if (GUILayout.Button("Draw 1"))
        {
            noiseDisplay.DrawTiles(world.generatedGrid_Land);
        }
        if (GUILayout.Button("Draw 2 - Double Dirt"))
        {
            noiseDisplay.DrawTiles_2(world.generatedGrid_Land);
        }
        if (GUILayout.Button("Draw 3 - Double Grass n Chunkload try"))
        {
            noiseDisplay.DrawTiles_3(world.generatedGrid_Land);
        }
        if (GUILayout.Button("Draw 4 - Chunkload"))
        {
            noiseDisplay.DrawTiles_4(world.generatedGrid_Land);
        }
        if (GUILayout.Button("Load Chunk"))
        {
            noiseDisplay.LoadChunks();
        }
        if (GUILayout.Button("Draw 5 - Triple Grass w/2 Layer"))
        {
            noiseDisplay.DrawTiles_5(world.generatedGrid_Land);
        }

        //Water Parameter
        //Amplitude 0.43
        //Frequency 0.005
        //Octaves 3
        //Lacunarity 2
        //Persistence 0.5
        if (GUILayout.Button("Draw Water 1"))
        {
            noiseDisplay.DrawTiles_Water_1(world.generatedGrid_Water);
        }
        if (GUILayout.Button("Draw Water 2"))
        {
            noiseDisplay.DrawTiles_Water_2(world.generatedGrid_Water);
        }

        if (GUILayout.Button("Draw LW 1"))
        {
            noiseDisplay.DrawTiles_LW_1(world.generatedGrid_LW);
        }
    }
}
