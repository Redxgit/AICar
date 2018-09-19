using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class JSONAICarParameters {
	public int resolution;

	public int maxAngle;

	public float maxRayDist;

	public float distanceFactor;
	public float angleFactor;

	public float speed = 1f;
	public float turnSpeed = 1f;

	public float dampForward;
	public float dampRight;
	public float criticalDistance;
	public float fwdCriticalDamp;

	public string Description;
	public int ID;

	public JSONAICarParameters(int resolution, int maxAngle, float maxRayDist, float distanceFactor, float angleFactor,
		float dampForward, float dampRight, float criticalDistance, float fwdCriticalDamp, string description, int id) {
		this.resolution = resolution;
		this.maxAngle = maxAngle;
		this.maxRayDist = maxRayDist;
		this.distanceFactor = distanceFactor;
		this.angleFactor = angleFactor;
		this.dampForward = dampForward;
		this.dampRight = dampRight;
		this.criticalDistance = criticalDistance;
		this.fwdCriticalDamp = fwdCriticalDamp;
		Description = description;
		ID = id;
	}
}

public class GeneericUtilsEditor {
	/// <summary>
	/// Return list of all assets of the given type
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static List<T> FindAssetsByType<T>() where T : Object {
		List<T> assets = new List<T>();
		string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
		for (int i = 0; i < guids.Length; i++) {
			string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
			T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
			if (asset != null) {
				assets.Add(asset);
			}
		}

		return assets;
	}

	public static T CreateAsset<T>(string path, string name) where T : ScriptableObject {
		T asset = ScriptableObject.CreateInstance<T>();
		/*if (path == "") {
			path = AssetDatabase.GetAssetPath(Selection.activeObject);
		}
		else if (Path.GetExtension(path) != "") {
			path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
		}*/

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");

		//AssetDatabase.CreateAsset(asset, path + name);
		AssetDatabase.CreateAsset(asset, assetPathAndName);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		//EditorUtility.FocusProjectWindow();
		//Selection.activeObject = asset;
		return asset;
	}

	public static void LoadJSON() {
		string fileContentAsString;
		//TextAsset fileContent = Resources.Load("JSON/AICarParametersJSON.json") as TextAsset;
		TextAsset fileContent = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/JSON/AICarParameters.json");
		Debug.Log("wtf");
		if (fileContent == null)
			return;
		fileContentAsString = fileContent.text;

		JSONContainer containerJSON = JsonUtility.FromJson<JSONContainer>(fileContentAsString);

		List<AICarParameters> soParams = FindAssetsByType<AICarParameters>();
		
		Debug.Log("dataInJSON" + containerJSON.parameters[0].Description );
		
		Debug.Log("soParams" + soParams.Count );

		foreach (var VARIABLE in containerJSON.parameters) {
			bool found = false;
			for (int i = 0; i < soParams.Count; i++) {
				if (VARIABLE.ID == soParams[i].ID) {
					soParams[i].resolution = VARIABLE.resolution;
					soParams[i].maxAngle = VARIABLE.maxAngle;
					soParams[i].maxRayDist = VARIABLE.maxRayDist;
					soParams[i].distanceFactor = VARIABLE.distanceFactor;
					soParams[i].angleFactor = VARIABLE.angleFactor;
					soParams[i].dampForward = VARIABLE.dampForward;
					soParams[i].dampRight = VARIABLE.dampRight;
					soParams[i].criticalDistance = VARIABLE.criticalDistance;
					soParams[i].fwdCriticalDamp = VARIABLE.fwdCriticalDamp;
					soParams[i].Description = VARIABLE.Description;
					soParams[i].ID = VARIABLE.ID;
					found = true;
					break;
				}
			}

			if (!found) {
				string path = "Assets/Resources/AICarParameters/";
				AICarParameters so = CreateAsset<AICarParameters>(path, "AICarParameters" + VARIABLE.ID + ".asset");
				//IngameCollectibleSO so = AssetDatabase.LoadAssetAtPath<IngameCollectibleSO>(path + name);
				//IngameCollectibleSO so = Resources.Load(resourcesPath + name + ".asset") as IngameCollectibleSO;
				if (so != null) {
					so.resolution = VARIABLE.resolution;
					so.maxAngle = VARIABLE.maxAngle;
					so.maxRayDist = VARIABLE.maxRayDist;
					so.distanceFactor = VARIABLE.distanceFactor;
					so.angleFactor = VARIABLE.angleFactor;
					so.dampForward = VARIABLE.dampForward;
					so.dampRight = VARIABLE.dampRight;
					so.criticalDistance = VARIABLE.criticalDistance;
					so.fwdCriticalDamp = VARIABLE.fwdCriticalDamp;
					so.Description = VARIABLE.Description;
					so.ID = VARIABLE.ID;
					EditorUtility.SetDirty(so);
				}
			}
		}
	}
	
	public static void LoadJSON(TextAsset fileContent) {
		string fileContentAsString;
		//TextAsset fileContent = Resources.Load("JSON/AICarParametersJSON.json") as TextAsset;
		Debug.Log("wtf");
		if (fileContent == null)
			return;
		fileContentAsString = fileContent.text;

		JSONContainer containerJSON = JsonUtility.FromJson<JSONContainer>(fileContentAsString);

		List<AICarParameters> soParams = FindAssetsByType<AICarParameters>();
		
		Debug.Log("dataInJSON" + containerJSON.parameters[0].Description );
		
		Debug.Log("soParams" + soParams.Count );

		foreach (var VARIABLE in containerJSON.parameters) {
			bool found = false;
			for (int i = 0; i < soParams.Count; i++) {
				if (VARIABLE.ID == soParams[i].ID) {
					soParams[i].resolution = VARIABLE.resolution;
					soParams[i].maxAngle = VARIABLE.maxAngle;
					soParams[i].maxRayDist = VARIABLE.maxRayDist;
					soParams[i].distanceFactor = VARIABLE.distanceFactor;
					soParams[i].angleFactor = VARIABLE.angleFactor;
					soParams[i].dampForward = VARIABLE.dampForward;
					soParams[i].dampRight = VARIABLE.dampRight;
					soParams[i].criticalDistance = VARIABLE.criticalDistance;
					soParams[i].fwdCriticalDamp = VARIABLE.fwdCriticalDamp;
					soParams[i].Description = VARIABLE.Description;
					soParams[i].ID = VARIABLE.ID;
					found = true;
					break;
				}
			}

			if (!found) {
				string path = "Assets/Resources/";
				AICarParameters so = CreateAsset<AICarParameters>(path, "AICarParameters" + VARIABLE.ID );
				//IngameCollectibleSO so = AssetDatabase.LoadAssetAtPath<IngameCollectibleSO>(path + name);
				//IngameCollectibleSO so = Resources.Load(resourcesPath + name + ".asset") as IngameCollectibleSO;
				if (so != null) {
					so.resolution = VARIABLE.resolution;
					so.maxAngle = VARIABLE.maxAngle;
					so.maxRayDist = VARIABLE.maxRayDist;
					so.distanceFactor = VARIABLE.distanceFactor;
					so.angleFactor = VARIABLE.angleFactor;
					so.dampForward = VARIABLE.dampForward;
					so.dampRight = VARIABLE.dampRight;
					so.criticalDistance = VARIABLE.criticalDistance;
					so.fwdCriticalDamp = VARIABLE.fwdCriticalDamp;
					so.Description = VARIABLE.Description;
					so.ID = VARIABLE.ID;
					EditorUtility.SetDirty(so);
				}
			}
		}
	}

	[System.Serializable]
	public class JSONContainer {
		public JSONAICarParameters[] parameters;
	}

	public static void CreateJSONFile() {
		List<AICarParameters> allParameters = FindAssetsByType<AICarParameters>();
		
		Debug.Log(allParameters.Count);
		
		JSONAICarParameters[] jsonParameters = new JSONAICarParameters[allParameters.Count];

		for (int i = 0; i < jsonParameters.Length; i++) {
			jsonParameters[i] = new JSONAICarParameters(
				allParameters[i].resolution,
				allParameters[i].maxAngle,
				allParameters[i].maxRayDist,
				allParameters[i].distanceFactor,
				allParameters[i].angleFactor,
				allParameters[i].dampForward,
				allParameters[i].dampRight,
				allParameters[i].criticalDistance,
				allParameters[i].fwdCriticalDamp,
				allParameters[i].Description,
				allParameters[i].ID
			);
		}

		JSONContainer cont = new JSONContainer {parameters = jsonParameters};

		Debug.Log(cont.parameters[0].Description);

		string dataAsJson = JsonUtility.ToJson(cont, true);
		File.WriteAllText(
			Application.dataPath + "/Resources/JSON/AICarParametersJSON" + ".json",
			dataAsJson);
		AssetDatabase.Refresh();
	}
}