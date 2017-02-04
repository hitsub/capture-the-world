using System.Collections;
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
	public int energy = 0;
	const int energyMax = 250;
	public FlagType energyColor = FlagType.None;
	public FlagType flag = FlagType.None;
	public int flagNum = 0;

	//色変更用
	Renderer Body;
	ParticleSystem CoreGlow;
	ParticleSystem Tail;
	ParticleSystem Burner;
	GameObject[] ShockWave = new GameObject[2];


	// Use this for initialization
	void Start () {
		GameObject canvas = GameObject.Find ("CrystalNameCanvas");
		Character = GameObject.Find ("Character");
		camera = GameObject.Find ("FirstPersonCharacter").GetComponent<Camera> ();
		canvasRect = canvas.GetComponent<RectTransform> ();

		ShockWave[0] = Resources.Load ("Prefabs/ShockWaveBlue") as GameObject;
		ShockWave[1] = Resources.Load ("Prefabs/ShockWaveGreen") as GameObject;
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
			imageGuage.fillAmount = (float)energy / energyMax;
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

		if (flag == FlagType.Blue)
			Instantiate (ShockWave [0], transform.position, Quaternion.identity);
		if (flag == FlagType.Green)
			Instantiate (ShockWave [1], transform.position, Quaternion.identity);
	}

	FlagType OtherSideFlag(FlagType flag){
		return (flag == FlagType.Blue) ? FlagType.Green : FlagType.Blue;
	}

	public InjectResult InjectEnergy(int amount, FlagType side){
		InjectResult result = new InjectResult();
		//自陣営
		if (flag == side || (flag == FlagType.None && (energyColor == FlagType.None || energyColor == side))) {
			imageGuage.color = FlagColor.Color (side);
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
		if (flag != side || (flag == FlagType.None && energyColor == OtherSideFlag(side))) {
			energy -= amount;
			if (energy >= 0) {
				energy = 0;
				ChangeFlagColor (FlagType.None);
				result.Destroy = true;
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
