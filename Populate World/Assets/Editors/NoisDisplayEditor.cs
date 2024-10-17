using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (NoiseDisplay))]
public class NoisDisplayEditor : Editor
{
    public override void OnInspectorGUI(){
        NoiseDisplay noiseDisplay = (NoiseDisplay)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Clear")){
            noiseDisplay.ClearTilemaps();
        }
    }
}
