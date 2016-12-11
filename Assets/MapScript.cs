using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScript : MonoBehaviour {
	Image imageMap;
	Image imageMe;
	RectTransform rectMe;
	GameObject Character;

	Vector2 size = new Vector2 (1040f, 840f);
	Vector2 real = new Vector2 (-343.7f, -281f);

	Vector3 mepos, pos, angle;
	// Use this for initialization
	void Start () {
		imageMap = this.gameObject.GetComponent<Image>();
		imageMe = this.gameObject.transform.FindChild("Me").gameObject.GetComponent<Image>();
		rectMe = this.gameObject.transform.FindChild("Me").gameObject.GetComponent<RectTransform>();
		Character = GameObject.Find ("Character");
	}
	
	// Update is called once per frame
	void Update () {
		pos = Character.transform.position;
		mepos = new Vector3 (size.x * (pos.x / real.x) - size.x/2, size.y * (pos.z / real.y) - size.y/2, 0);
		rectMe.anchoredPosition = new Vector3(-mepos.x, -mepos.y,0);

		angle =  Vector3.Angle (new Vector3 (0, 0, 0f) - new Vector3 (0, 0, -10f), Character.transform.forward);
		rectMe.rotation = angle;
	}
}
