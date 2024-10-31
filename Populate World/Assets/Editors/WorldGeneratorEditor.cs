using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		WorldGenerator mapGen = (WorldGenerator)target;

		DrawDefaultInspector();
		// if (DrawDefaultInspector ()) {
		// 	if (mapGen.autoUpdate) {
		// 		mapGen.GenerateNoise ();
		// 	}
		// }

		if (GUILayout.Button("Generate Land"))
		{
			mapGen.GenerateNoise_Land();
		}
		if (GUILayout.Button("Generate Water"))
		{
			mapGen.GenerateNoise_Water();
		}
		if (GUILayout.Button("Generate LW"))
		{
			mapGen.GenerateNoise_LW();
		}
	}
}
