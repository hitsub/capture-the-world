using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniJSON;

public class MapScript : MonoBehaviour {
	Image imageMap;
	Image imageMe;
	RectTransform rectMe;
	GameObject Character;

	Vector2 size = new Vector2 (1040f, 840f);
	Vector2 real = new Vector2 (-343.7f, -281f);

	Vector3 mepos, pos;
	float angle;

	bool crystal = false;
	// Use this for initialization
	void Start () {
		imageMap = this.gameObject.GetComponent<Image>();
		imageMe = this.gameObject.transform.FindChild("Me").gameObject.GetComponent<Image>();
		rectMe = this.gameObject.transform.FindChild("Me").gameObject.GetComponent<RectTransform>();
		Character = GameObject.Find ("Character");


		TextAsset text = Resources.Load ("map") as TextAsset;
		JsonNode json = JsonNode.Parse (text.text);

		GameObject	mapCrystal = Resources.Load ("Prefabs/mapCrystal") as GameObject;
		Transform	parent = GameObject.Find ("Map").transform;
		for (int i = 0; i < json ["crystal"].Count; i++) {
			GameObject tmp = Instantiate (mapCrystal) as GameObject;
			tmp.transform.SetParent (parent);
			Vector3 crystalPos = new Vector3 ((float)json ["crystal"] [i] ["pos"] [0].Get<double> (), (float)json ["crystal"] [i] ["pos"] [1].Get<double> (), (float)json ["crystal"] [i] ["pos"] [2].Get<double> ());;
			Vector3 crystalMapPos = new Vector3 (size.x * (crystalPos.x / real.x) - size.x/2, size.y * (crystalPos.z / real.y) - size.y/2, 0);
			tmp.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (-crystalMapPos.x, -crystalMapPos.y, 0);
			tmp.transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
		}

	}
	
	// Update is called once per frame
	void Update () {
		//自機位置
		pos = Character.transform.position;
		mepos = new Vector3 (size.x * (pos.x / real.x) - size.x/2, size.y * (pos.z / real.y) - size.y/2, 0);
		rectMe.anchoredPosition = new Vector3(-mepos.x, -mepos.y,0);
		//自機角度
		Vector3 charaAngle = Character.transform.eulerAngles;
		rectMe.eulerAngles = new Vector3 (0f, 0f, -charaAngle.y);
	}

}
