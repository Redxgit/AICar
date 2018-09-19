using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContainerData : ScriptableObject {
	public TextAsset jsonFile;
}

public class JSONThingsEditorWindow : EditorWindow {
	private static ContainerData JSONFile;
	private static SerializedProperty jsonData;
	private static SerializedObject serializedObject;

	[MenuItem("Window/CARAIParams")]
	public static void ShowWindow() {
		GetWindow<JSONThingsEditorWindow>("Redx rocks");
		InitThings();
	}

	private static void InitThings() {
		JSONFile = CreateInstance<ContainerData>();
	}

	private void OnGUI() {
		GUILayout.Label("Load And Create JSONs for AICarParams", EditorStyles.boldLabel);

		if (jsonData != null)
			EditorGUILayout.PropertyField(jsonData, true);
		else {
			serializedObject = new SerializedObject(JSONFile);
			jsonData = serializedObject.FindProperty("jsonFile");
			EditorGUILayout.PropertyField(jsonData, true);
		}
		
		serializedObject.ApplyModifiedProperties();
		if (GUILayout.Button("Create JSON!")) {
			CreateJSON();
		}

		if (GUILayout.Button("Load JSON!")) {
			LoadJSON();
		}
	}

	private void LoadJSON() {
		//GeneericUtilsEditor.LoadJSON();
		GeneericUtilsEditor.LoadJSON(JSONFile.jsonFile);
	}

	private void CreateJSON() {
		GeneericUtilsEditor.CreateJSONFile();
	}
}