using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour {
	private List<float> bestTimes;
	private List<RectTransform> rts;

	/*[SerializeField] private GameObject quadPrefab;

	[SerializeField] private RectTransform supIzq;
	[SerializeField] private RectTransform infDch;*/

	private const float maxScale = 18f;
	[SerializeField] private GameObject[] objsToScale;
	[SerializeField] private Vector2[] ranges;
	private int[] numberOfEachTime;

	//private float minX;
//
	//private float maxX;
//
	//private float minY;
//
	//private float maxY;
//
	// Use this for initialization

	public void ResetData() {
		Start();
	}
	
	void Start() {
		bestTimes = new List<float>();
		rts = new List<RectTransform>();
		numberOfEachTime = new int[objsToScale.Length];

		for (int i = 0; i < objsToScale.Length; i++) {
			objsToScale[i].transform.localScale = new Vector3(1,0,1);
		}
		//maxX = infDch.localPosition.x;
		//minY = infDch.localPosition.y;
		//minX = supIzq.localPosition.x;
		//maxY = supIzq.localPosition.y;
	}

	// Update is called once per frame
	void Update() { }

	public void AddTime(float newTime) {
		/*if (bestTimes.Count > 50) {
			bestTimes.RemoveAt(Random.Range(10, 40));
			bestTimes.Add(newTime);
		}
		else {
			bestTimes.Add(newTime);
			//RectTransform newrt = ;
			//rts.Add(Instantiate(quadPrefab, transform).GetComponent<RectTransform>());
		}*/

		for (int i = 0; i < ranges.Length; i++) {
			if (newTime > ranges[i].x && newTime < ranges[i].y) {
				numberOfEachTime[i]++;
				break;
			}
		}

		RecalculteGraph();
	}

	private void RecalculteGraph() {
		//float bestT = bestTimes[bestTimes.Count - 1];
		//float worstT = bestTimes[0];
		//int count = bestTimes.Count;

		int maxValue = 0;

		for (int i = 0; i < numberOfEachTime.Length; i++) {
			if (maxValue < numberOfEachTime[i]) {
				maxValue = numberOfEachTime[i];
			}
		}

		for (int i = 0; i < objsToScale.Length; i++) {
			objsToScale[i].transform.localScale = new Vector3(1, Mathf.Lerp(0, maxScale, numberOfEachTime[i] / (float)maxValue), 1);
		}

		/*for (int i = 0; i < bestTimes.Count; i++) {
			float percentage = bestTimes[i].Remap( bestT,worstT, 0, 1);
			Debug.Log(percentage);
			//Vector2 newPos = new Vector2(Mathf.Lerp(minX, maxX, (float) i / count), Mathf.Lerp(maxY, minY, percentage));

//			rts[i].localPosition = newPos;
		}*/
	}
}

public static class ExtensionMethods {
	public static float Remap(this float value, float high1, float high2, float low1, float low2) {
		return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
		//return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}