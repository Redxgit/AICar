using System.Collections;
using UnityEngine;

public class CreateSpriteFromPNG : MonoBehaviour {
	[SerializeField] private Texture2D tex;
	[SerializeField] private Texture img;

	[SerializeField] private Color targetCol;

	[SerializeField] private SpriteRenderer rend;

	[SerializeField] private string absPath;

	// Use this for initialization
	void Start() { }

	// Update is called once per frame
	void Update() { }

	private void Reset() {
		rend = GetComponent<SpriteRenderer>();
	}

	[ContextMenu("DoThings")]
	private void DoThings() {
		//Texture2D tex = (Texture2D) img;
		Texture2D tempTex = new Texture2D(tex.width, tex.height);
		for (int i = 0; i < tex.width; i++) {
			for (int j = 0; j < tex.height; j++) {
				Color col = tex.GetPixel(i, j);
				if (ColorDist(col, targetCol) < 0.1f) {
					tempTex.SetPixel(i, j, targetCol);
				}
			}
		}

		rend.sprite = Sprite.Create(tempTex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f),
			10.0f);
		//Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
		if (GetComponent<PolygonCollider2D>() == null)
			gameObject.AddComponent<PolygonCollider2D>();
	}

	[ContextMenu("DoMoreTHings")]
	private void DoMoreThings() {
		StartCoroutine(LoadSprite(absPath));
	}

	private float ColorDist(Color c1, Color c2) {
		float v =Mathf.Abs(c1.r - c2.r ) + Mathf.Abs( c1.g + c2.g) + Mathf.Abs( c1.b - c2.b);
		Debug.Log(v);
		return v;
	}

	public IEnumerator LoadSprite(string absoluteImagePath) {
		string finalPath;
		WWW localFile;
		Texture texture;
		Sprite sprite;

		finalPath = "file://" + absoluteImagePath;
		localFile = new WWW(finalPath);

		yield return localFile;

		texture = localFile.texture;
		//sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

		Texture2D tex = (Texture2D) texture;
		Texture2D tempTex = new Texture2D(tex.width, tex.height);
		for (int i = 0; i < tex.width; i++) {
			for (int j = 0; j < tex.height; j++) {
				Vector4 col = tex.GetPixel(i, j);
				if (Vector4.Distance(col, targetCol) < 0.1f) {
					tempTex.SetPixel(i, j, targetCol);
				}
			}
		}

		rend.sprite = Sprite.Create(tempTex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f),
			10.0f);
		//Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
		gameObject.AddComponent<PolygonCollider2D>();
	}
}