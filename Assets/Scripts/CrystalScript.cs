using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Flag{
	None, Blue, Green
}

public class CrystalScript : MonoBehaviour {
	GameObject	Character;
	Camera camera;
	RectTransform rect;
	RectTransform canvasRect;
	Text text;
	bool nearPlayer = false; //保険
	Vector2 localPos;
	bool ready = false;
	int dis;
	float angle;

	//クリスタルのステータス
	public float energy = 1f;
	public Flag flag = Flag.None;
	public int flagNum = 0;


	// Use this for initialization
	void Start () {
		GameObject canvas = GameObject.Find ("CrystalNameCanvas");
		Character = GameObject.Find ("Character");
		camera = GameObject.Find ("FirstPersonCharacter").GetComponent<Camera> ();
		canvasRect = canvas.GetComponent<RectTransform> ();

		GameObject namePrefab = Resources.Load ("Prefabs/CrystalNamePrefab") as GameObject;
		GameObject name = Instantiate (namePrefab) as GameObject;
		name.transform.SetParent (canvas.transform);
		name.name = "C:"+this.gameObject.name;
		rect = name.GetComponent<RectTransform> ();
		text = name.GetComponent<Text> ();
		text.text = this.gameObject.name;
		text.enabled = false;
		ready = true;
	}
	// Update is called once per frame
	void Update () {
		angle = Vector3.Angle (Character.transform.forward, transform.position - Character.transform.position);
		//いちおうOnTirggerEnter/Exitすっぽぬけた時の保険
		if (text.enabled != nearPlayer)
			text.enabled = nearPlayer;
		if (text.enabled && Mathf.Abs (angle) < 90) {
			
			Vector2 ViewportPosition = camera.WorldToViewportPoint (this.gameObject.transform.position);
			Vector2 WorldObject_ScreenPosition = new Vector2 (
				                                     ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
				                                     ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
			                                     );
			rect.anchoredPosition = WorldObject_ScreenPosition;
			dis = (int)Vector3.Distance (Character.transform.position, transform.position);
			text.text = name + "\n" + dis + "m";
			text.fontSize = 20 - (int)(10f * (((float)dis) / 31f));

		}
	}
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player" && ready) {
			nearPlayer = true;
			text.enabled = true;
		}
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Player" && ready) {
			nearPlayer = true;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player" && ready) {
			nearPlayer = false;
			text.enabled = false;
		}
	}

	public void Kill(){
		//Destroy (text.gameObject);
		//Destroy (this.gameObject);
	}
}
