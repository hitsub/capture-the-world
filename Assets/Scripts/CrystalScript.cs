using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalScript : MonoBehaviour {

	//クリスタル名表示用
	[SerializeField]GameObject[] Character;
	[SerializeField]Camera[] camera;
	RectTransform[] rect = new RectTransform[2];
	[SerializeField]RectTransform[] canvasRect;
	Text[] textCrystalName = new Text[2];
	Text[] textDistance = new Text[2];
	bool[] nearPlayer = new bool[2]{false, false}; //保険
	[SerializeField]GameObject[] canvas;
	Vector2[] localPos = new Vector2[2];
	bool[] ready = new bool[2]{false, false};
	int[] distanceCameraCrystal = new int[2];
	float[] angleCameraCrystal = new float[2]; //キャラの向いている方向とクリスタルの方向の確度(180度違う場合にパネル表示されることの回避)
	Image[] imagePanel = new Image[2], imageGuage = new Image[2], imageGuageBG = new Image[2];
	float[] panelSize = new float[2];

	//クリスタルのステータス初期化
	public int energy = 0;
	const int energyMax = 200;
	public FlagType energyColor = FlagType.None;
	public FlagType flagColor = FlagType.None;
	public int flagNum = 0;

	//色変更用
	Renderer Body;
	ParticleSystem CoreGlow;
	ParticleSystem Tail;
	ParticleSystem Burner;
	GameObject[] ShockWave = new GameObject[2];

	//スコア送信
	float keepTime = 0;
	Manager manager;


	// Use this for initialization
	void Start () {
		ShockWave[0] = Resources.Load ("Prefabs/FireballBlue") as GameObject;
		ShockWave[1] = Resources.Load ("Prefabs/FireballGreen") as GameObject;
		GameObject namePrefab = Resources.Load ("Prefabs/CrystalNamePanel") as GameObject;
		manager = GameObject.Find ("Manager").GetComponent<Manager>();

		//名前表示用
		for (int i = 0; i < Character.Length; i++) {
			canvasRect[i] = canvas[i].GetComponent<RectTransform> ();
			GameObject name = Instantiate (namePrefab) as GameObject;
			name.transform.SetParent (canvas[i].transform);
			name.name = "C:" + this.gameObject.name;
			rect[i] = name.GetComponent<RectTransform> ();
			imagePanel[i] = name.GetComponent<Image> ();
			imageGuage[i] = name.transform.Find ("Guage").gameObject.GetComponent<Image> ();
			imageGuageBG[i] = name.transform.Find ("GuageBG").gameObject.GetComponent<Image> ();
			textCrystalName[i] = name.transform.Find ("Name").gameObject.GetComponent<Text> ();
			textDistance[i] = name.transform.Find ("Distance").gameObject.GetComponent<Text> ();
			textCrystalName[i].text = this.gameObject.name;
			imagePanel[i].enabled = false;
			ready[i] = true;
		}

		//色変更用
		Body = transform.Find ("BodyParent/Body").gameObject.GetComponent<Renderer>();
		CoreGlow = transform.Find ("Domain/CoreGlow").gameObject.GetComponent<ParticleSystem>();
		Tail = transform.Find ("Domain/Tail").gameObject.GetComponent<ParticleSystem>();
		Burner = transform.Find ("Burner").gameObject.GetComponent<ParticleSystem>();

	}
	// Update is called once per frame
	void Update () {
		// クリスタル名の表示
		for (int i = 0; i < Character.Length; i++) {
			angleCameraCrystal[i] = Vector3.Angle (Character[i].transform.forward, transform.position - Character[i].transform.position);
			if (imagePanel[i].enabled != nearPlayer[i]) //いちおうOnTirggerEnter/Exitすっぽぬけた時の保険
				Show (nearPlayer[i], i);
			if (imagePanel[i].enabled && Mathf.Abs (angleCameraCrystal[i]) < 90) {
				Vector2 ViewportPosition = camera[i].WorldToViewportPoint (new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1f, this.gameObject.transform.position.z));
				Vector2 WorldObject_ScreenPosition = new Vector2 (
					                                     ((ViewportPosition.x * canvasRect [i].sizeDelta.x) - (canvasRect [i].sizeDelta.x * 0.5f)),
					                                     ((ViewportPosition.y * canvasRect [i].sizeDelta.y) - (canvasRect [i].sizeDelta.y * 0.5f))
				                                     );
				rect[i].anchoredPosition = WorldObject_ScreenPosition;
				distanceCameraCrystal[i] = (int)Vector3.Distance (Character[i].transform.position, transform.position);
				textDistance[i].text = distanceCameraCrystal[i] + "m";
				panelSize[i] = 1f - (0.5f * (((float)distanceCameraCrystal[i]) / 60f));
				if (imageGuage[i].fillAmount != (float)energy / energyMax) {
					imageGuage[i].fillAmount = (imageGuage[i].fillAmount + (float)energy / energyMax) / 2;
				}
				rect[i].localScale = new Vector3 (panelSize[i], panelSize[i], panelSize[i]);
			}
		}

		if (flagColor != FlagType.None)
			keepTime += 1f * Time.deltaTime;
		if (keepTime >= CrystalInfo.scoreCrystalSpan) {
			manager.GetScore (1, flagColor);
			keepTime = 0;
		}
	}

	void OnTriggerEnter(Collider col){
		for (int i = 0; i < Character.Length; i++) {
			if (col.gameObject.name == "CharacterP"+(i+1).ToString() && ready[i]) {
				nearPlayer[i] = true;
				Show (true, i);
			}
		}
	}

	void OnTriggerStay(Collider col){
		for (int i = 0; i < Character.Length; i++) {
			if (col.gameObject.name == "CharacterP"+(i+1).ToString() && ready[i]) {
				nearPlayer[i] = true;
			}
		}
	}

	void OnTriggerExit(Collider col){
		for (int i = 0; i < Character.Length; i++) {
			if (col.gameObject.name == "CharacterP"+(i+1).ToString() && ready[i]) {
				nearPlayer[i] = false;
				Show (false, i);
			}
		}
	}

	void Show(bool flag, int i){
		imageGuage[i].enabled = flag;
		imageGuageBG[i].enabled = flag;
		imagePanel[i].enabled = flag;
		textCrystalName[i].enabled = flag;
		textDistance[i].enabled = flag;
	}

	void ChangeFlagColor(FlagType flag){
		//変更用の色の準備
		Color c = FlagColor.Color (flag);

		//UI
		for (int i = 0; i < Character.Length; i++) {
			imageGuage[i].color = c;
		}

		//クリスタル
		flagColor = flag;
		CoreGlow.startColor = c;
		Tail.startColor = c;
		Burner.startColor = new Color (c.r, c.g, c.b, 0.313f);
		Body.material.SetColor ("_CristalColor", c);
		if (flag == FlagType.Blue)
			Instantiate (ShockWave [0], transform.position, Quaternion.identity);
		if (flag == FlagType.Green)
			Instantiate (ShockWave [1], transform.position, Quaternion.identity);
		//スコア用タイマー
		keepTime = 0;
	}

	FlagType OtherSideFlag(FlagType flag){
		return (flag == FlagType.Blue) ? FlagType.Green : FlagType.Blue;
	}

	public InjectResult InjectEnergy(int amount, FlagType side){
		InjectResult result = new InjectResult();
		//自陣営
		if (flagColor == side || (flagColor == FlagType.None && (energyColor == FlagType.None || energyColor == side))) {
			for (int i = 0; i < Character.Length; i++) {
				imageGuage[i].color = FlagColor.Color (side);
			}
			if (energy >= energyMax)
				return result;
			energy += amount;
			if (energy >= energyMax) {
				energy = energyMax;
				ChangeFlagColor (side);
				result.Capture = true;
			}
			result.Result = true;
			return result;
		}
		//敵陣営
		if (flagColor != side || (flagColor == FlagType.None && energyColor == OtherSideFlag(side))) {
			energy -= amount;
			if (energy >= 0) {
				energy -= amount;
			}
			if (energy <= 0) {
				ChangeFlagColor (FlagType.None);
				result.Destroy = true;
				energy = 0;
			}
			result.Result = true;
			return result;
		}

		return result;
	}

	public void Kill(){
		//Destroy (text.gameObject);
		//Destroy (this.gameObject);
	}

}
