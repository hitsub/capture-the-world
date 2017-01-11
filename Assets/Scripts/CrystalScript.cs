﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalScript : MonoBehaviour {

	//クリスタル名表示用
	GameObject	Character;
	Camera camera;
	RectTransform rect;
	RectTransform canvasRect;
	Text textCrystalName;
	Text textDistance;
	bool nearPlayer = false; //保険
	Vector2 localPos;
	bool ready = false;
	int distanceCameraCrystal;
	float angleCameraCrystal; //キャラの向いている方向とクリスタルの方向の確度(180度違う場合にパネル表示されることの回避)
	Image imagePanel, imageGuage, imageGuageBG;
	float panelSize;

	//クリスタルのステータス初期化
	public float energy = 1f;
	public FlagType flag = FlagType.None;
	public int flagNum = 0;

	//色変更用
	Renderer Body;
	ParticleSystem CoreGlow;
	ParticleSystem Tail;
	ParticleSystem Burner;


	// Use this for initialization
	void Start () {
		GameObject canvas = GameObject.Find ("CrystalNameCanvas");
		Character = GameObject.Find ("Character");
		camera = GameObject.Find ("FirstPersonCharacter").GetComponent<Camera> ();
		canvasRect = canvas.GetComponent<RectTransform> ();

		GameObject namePrefab = Resources.Load ("Prefabs/CrystalNamePanel") as GameObject;
		GameObject name = Instantiate (namePrefab) as GameObject;
		name.transform.SetParent (canvas.transform);
		name.name = "C:"+this.gameObject.name;
		rect = name.GetComponent<RectTransform> ();
		imagePanel = name.GetComponent<Image> ();
		imageGuage = name.transform.FindChild ("Guage").gameObject.GetComponent<Image> ();
		imageGuageBG = name.transform.FindChild ("GuageBG").gameObject.GetComponent<Image> ();
		textCrystalName = name.transform.FindChild("Name").gameObject.GetComponent<Text> ();
		textDistance = name.transform.FindChild("Distance").gameObject.GetComponent<Text> ();
		textCrystalName.text = this.gameObject.name;
		imagePanel.enabled = false;
		ready = true;

		Body = transform.FindChild ("BodyParent/Body").gameObject.GetComponent<Renderer>();
		CoreGlow = transform.FindChild ("Domain/CoreGlow").gameObject.GetComponent<ParticleSystem>();
		Tail = transform.FindChild ("Domain/Tail").gameObject.GetComponent<ParticleSystem>();
		Burner = transform.FindChild ("Burner").gameObject.GetComponent<ParticleSystem>();

	}
	// Update is called once per frame
	void Update () {
		// クリスタル名の表示
		angleCameraCrystal = Vector3.Angle (Character.transform.forward, transform.position - Character.transform.position);
		if (imagePanel.enabled != nearPlayer) //いちおうOnTirggerEnter/Exitすっぽぬけた時の保険
			Show(nearPlayer);
		if (imagePanel.enabled && Mathf.Abs (angleCameraCrystal) < 90) {
			
			Vector2 ViewportPosition = camera.WorldToViewportPoint (new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1f, this.gameObject.transform.position.z));
			Vector2 WorldObject_ScreenPosition = new Vector2 (
				                                     ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
				                                     ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
			                                     );
			rect.anchoredPosition = WorldObject_ScreenPosition;
			distanceCameraCrystal = (int)Vector3.Distance (Character.transform.position, transform.position);
			textDistance.text = distanceCameraCrystal + "m";
			panelSize = 1f - (0.5f * (((float)distanceCameraCrystal) / 60f));
			rect.localScale = new Vector3 (panelSize, panelSize, panelSize);
		}

		//ステータス反映

	}
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player" && ready) {
			nearPlayer = true;
			Show (true);
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
			Show (false);
		}
	}

	void Show(bool flag){
		imageGuage.enabled = flag;
		imageGuageBG.enabled = flag;
		imagePanel.enabled = flag;
		textCrystalName.enabled = flag;
		textDistance.enabled = flag;
	}

	void ChangeFlagColor(FlagType flag){
		Color c = FlagColor.Color (flag);
		imageGuage.color = c;
		CoreGlow.startColor = c;
		Tail.startColor = c;
		Burner.startColor = new Color (c.r, c.g, c.b, 0.313f);
		Body.material.SetColor ("_CristalColor", c);
	}

	public void Kill(){
		//Destroy (text.gameObject);
		//Destroy (this.gameObject);
	}

}