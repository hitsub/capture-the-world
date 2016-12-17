using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalScript : MonoBehaviour {
	GameObject	Character;
	Camera cam;
	RectTransform rect;
	Text text;
	bool nearPlayer = false; //保険
	Vector2 localPos;
	RectTransform canvasRect;
	bool ready = false;
	int dis;
	float angle;
	// Use this for initialization
	void Start () {
		Character = GameObject.Find ("Character");
		cam = GameObject.Find ("FirstPersonCharacter").GetComponent<Camera> ();
		canvasRect = GameObject.Find ("Canvas").GetComponent<RectTransform> ();

		GameObject namePrefab = Resources.Load ("Prefabs/CrystalNamePrefab") as GameObject;
		GameObject name = Instantiate (namePrefab) as GameObject;
		name.transform.SetParent (GameObject.Find ("Canvas").transform);
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
			/*
			Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
			//print (name+" ("+(int)screenPos.x+","+(int)screenPos.y+")"+"  ("+Screen.width+","+Screen.height+")");
			RectTransformUtility.ScreenPointToLocalPointInRectangle (parentRect, screenPos, camera, out localPos);
			rect.position = localPos;
			print (RectTransformUtility.ScreenPointToLocalPointInRectangle (parentRect, screenPos, camera, out localPos));
			*/
			Vector2 ViewportPosition = cam.WorldToViewportPoint (this.gameObject.transform.position);
			Vector2 WorldObject_ScreenPosition = new Vector2 (
				                                     ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
				                                     ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
			                                     );
			rect.anchoredPosition = WorldObject_ScreenPosition;
			dis = (int)Vector3.Distance (Character.transform.position, transform.position);
			text.text = name + "\n" + dis + "m";
			if (dis < 30f)
				text.fontSize = 20 - (int)(10f * (((float)dis) / 30f));
			else
				text.fontSize = 10;

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
		Destroy (text.gameObject);
		Destroy (this.gameObject);
	}
}
