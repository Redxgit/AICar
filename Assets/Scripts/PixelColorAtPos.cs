using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelColorAtPos : MonoBehaviour {
	public Color c;
	public Vector2 pos;
	[SerializeField] private SpriteRenderer rend;

	private Sprite sp;

	private Texture2D tex;
	private int heightFactor;
	private int widthFactor;

	private Vector2 conversionSize;

	// Use this for initialization
	void Start() {
		sp = rend.sprite;
		tex = sp.texture;
		heightFactor = (int) (tex.height / sp.pixelsPerUnit / 2);
		widthFactor = (int) (tex.width / sp.pixelsPerUnit / 2);
	}

	// Update is called once per frame
	void Update() {
		int xCoord = (int) (transform.position.x + widthFactor);
		int yCoord = (int) (transform.position.y + heightFactor);

		//Textura 1024 x 1024 a 10 pixels per unit

		xCoord = (int) (transform.position.x + widthFactor) * (int) sp.pixelsPerUnit;
		yCoord = (int) (transform.position.y + heightFactor) * (int) sp.pixelsPerUnit;

		pos = new Vector2(xCoord, yCoord);
		c = tex.GetPixel(xCoord, yCoord);
	}
}