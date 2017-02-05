using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MiniJSON;

public class ScreenSelect : MonoBehaviour {

	[SerializeField] RectTransform p1Select;
	[SerializeField] RectTransform p2Select;
	[SerializeField] Image p1GuagePower;
	[SerializeField] Image p1GuageFirerate;
	[SerializeField] Image p1GuageSpeed;
	[SerializeField] Image p1GuageEnergy;
	[SerializeField] Image p1GuageRecovery;
	[SerializeField] Image p2GuagePower;
	[SerializeField] Image p2GuageFirerate;
	[SerializeField] Image p2GuageSpeed;
	[SerializeField] Image p2GuageEnergy;
	[SerializeField] Image p2GuageRecovery;

	[SerializeField] Text p1Stat;
	[SerializeField] Text p2Stat;

	int p1SelectPos = 0, p2SelectPos = 0;
	bool p1SelectFlag = true, p2SelectFlag = true;

	string[] className = { "Assalt", "Bomber", "Charger", "Recon" };
	string[] classStat = { "Damage", "FireRate", "RunSpeed", "Energy", "Recovery" };
	string[] classFunc = { "Power", "FireRate", "Speed", "Energy", "Recovery" };
	int[] classMax = { 200, 200, 10, 2500, 20 };

	JsonNode json;
	bool p2stickback = true;

	//Animation Const.
	const string animGuageEase = "easeOutCubic";
	const float animGuageTime = 0.3f;
	const string animPositionEase = "easeOutBack";
	const float animPositionTime = 0.3f;

	// Use this for initialization
	void Start () {
		//TextAsset text = Resources.Load ("class") as TextAsset;
		json = JsonNode.Parse (Resources.Load <TextAsset>("class").text);
		for (int i = 0; i < classStat.Length; i++) {
			//print (i + ":" + (float)json ["Assalt"] [classStat [i]].Get<long> () / classMax [i]+"\n"+json ["Assalt"] [classStat [i]].Get<long> ()+" / "+classMax [i]);
			iTween.ValueTo (gameObject, iTween.Hash ("from", 0f, "to", (float)json ["Assalt"] [classStat [i]].Get<long> () / classMax [i], "time", 1f, "easetype", animGuageEase, "onupdate", "SetP1Guage" + classFunc [i]));
		}
		for (int i = 0; i < classStat.Length; i++) {
			//print (i + ":" + (float)json ["Assalt"] [classStat [i]].Get<long> () / classMax [i]+"\n"+json ["Assalt"] [classStat [i]].Get<long> ()+" / "+classMax [i]);
			iTween.ValueTo (gameObject, iTween.Hash ("from", 0f, "to", (float)json ["Assalt"] [classStat [i]].Get<long> () / classMax [i], "time", 1f, "easetype", animGuageEase, "onupdate", "SetP2Guage" + classFunc [i]));
		}
	}
	
	// Update is called once per frame
	void Update () {
		//カメラの回転
		transform.Rotate (0, -5f * Time.deltaTime, 0);
		//キー入力
		if (p1SelectFlag) {
			if (Input.GetKeyDown (KeyCode.UpArrow) && (p1SelectPos != 0)) {
				p1SelectPos--;
				iTween.ValueTo (gameObject, iTween.Hash (
					"from", 100f - 50f * (p1SelectPos + 1), 
					"to", 100f - 50f * (p1SelectPos), 
					"time", animPositionTime, 
					"easetype", animPositionEase, 
					"onupdate", "SetP1SelectPos"
				));
				for (int i = 0; i < classStat.Length; i++) {
					iTween.ValueTo (gameObject, iTween.Hash (
						"from", (float)json [className [p1SelectPos + 1]] [classStat [i]].Get<long> () / classMax [i], 
						"to", (float)json [className [p1SelectPos]] [classStat [i]].Get<long> () / classMax [i], 
						"time", animGuageTime, 
						"easetype", animGuageEase, 
						"onupdate", "SetP1Guage" + classFunc [i]
					));
				}
			}
			if (Input.GetKeyDown (KeyCode.DownArrow) && (p1SelectPos != 3)) {
				p1SelectPos++;
				iTween.ValueTo (gameObject, iTween.Hash (
					"from", 100f - 50f * (p1SelectPos - 1), 
					"to", 100f - 50f * (p1SelectPos), 
					"time", animPositionTime, 
					"easetype", animPositionEase, 
					"onupdate", "SetP1SelectPos"
				));
				for (int i = 0; i < classStat.Length; i++) {
					iTween.ValueTo (gameObject, iTween.Hash (
						"from", (float)json [className [p1SelectPos - 1]] [classStat [i]].Get<long> () / classMax [i], 
						"to", (float)json [className [p1SelectPos]] [classStat [i]].Get<long> () / classMax [i], 
						"time", animGuageTime, 
						"easetype", animGuageEase, 
						"onupdate", "SetP1Guage" + classFunc [i]
					));
				}
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				p1SelectFlag = false;
				p1Stat.text = "Waiting";
			}
		} else {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				p1SelectFlag = true;
				p1Stat.text = "Selecting";
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				SetStatus ();
				SceneManager.LoadScene ("Split");
			}
		}
		if (p2SelectFlag) {
			if (Input.GetAxis ("Axis8") <= 0.3f && Input.GetAxis ("Axis8") >= -0.3f && !p2stickback)
				p2stickback = true;
			if (Input.GetAxis("Axis8")>=0.9f && p2stickback && (p2SelectPos != 0)) {
				p2SelectPos--;
				p2stickback = false;
				iTween.ValueTo (gameObject, iTween.Hash (
					"from", 100f - 50f * (p2SelectPos + 1), 
					"to", 100f - 50f * (p2SelectPos), 
					"time", animPositionTime, 
					"easetype", animPositionEase, 
					"onupdate", "SetP2SelectPos"
				));
				for (int i = 0; i < classStat.Length; i++) {
					iTween.ValueTo (gameObject, iTween.Hash (
						"from", (float)json [className [p2SelectPos + 1]] [classStat [i]].Get<long> () / classMax [i], 
						"to", (float)json [className [p2SelectPos]] [classStat [i]].Get<long> () / classMax [i], 
						"time", animGuageTime, 
						"easetype", animGuageEase, 
						"onupdate", "SetP2Guage" + classFunc [i]
					));
				}
			}
			if (Input.GetAxis("Axis8")<=-0.9f && p2stickback && (p2SelectPos != 3)) {
				p2SelectPos++;
				p2stickback = false;
				//p2Select.anchoredPosition = new Vector2 (130, 100f - 50f * (p2SelectPos));

				iTween.ValueTo (gameObject, iTween.Hash (
					"from", 100f - 50f * (p2SelectPos - 1), 
					"to", 100f - 50f * (p2SelectPos), 
					"time", animPositionTime, 
					"easetype", animPositionEase, 
					"onupdate", "SetP2SelectPos"
				));
				for (int i = 0; i < classStat.Length; i++) {
					iTween.ValueTo (gameObject, iTween.Hash (
						"from", (float)json [className [p2SelectPos - 1]] [classStat [i]].Get<long> () / classMax [i], 
						"to", (float)json [className [p2SelectPos]] [classStat [i]].Get<long> () / classMax [i], 
						"time", animGuageTime, 
						"easetype", animGuageEase, 
						"onupdate", "SetP2Guage" + classFunc [i]
					));
				}
			}
			if (Input.GetKeyDown(KeyCode.JoystickButton1)) {
				p2SelectFlag = false;
				p2Stat.text = "Waiting";
			}
		} else {
			if (Input.GetKeyDown (KeyCode.JoystickButton2)) {
				p2SelectFlag = true;
				p2Stat.text = "Selecting";
			}
		}
		if (!p1SelectFlag && !p2SelectFlag) {
			SetStatus ();
			SceneManager.LoadScene ("Split");
		}
	}

	void SetStatus(){
		PlayerData pData = GameObject.Find ("PlayerData").GetComponent<PlayerData> ();
		pData.energy = new int[] {
			(int)json [className [p1SelectPos]] ["Energy"].Get<long> (),
			(int)json [className [p2SelectPos]] ["Energy"].Get<long> ()
		};
		pData.WalkSpeed = new int[] {
			(int)json [className [p1SelectPos]] ["WalkSpeed"].Get<long> (),
			(int)json [className [p2SelectPos]] ["WalkSpeed"].Get<long> ()
		};
		pData.RunSpeed = new int[] {
			(int)json [className [p1SelectPos]] ["RunSpeed"].Get<long> (),
			(int)json [className [p2SelectPos]] ["RunSpeed"].Get<long> ()
		};
		pData.Mileage = new int[] {
			(int)json [className [p1SelectPos]] ["Mileage"].Get<long> (),
			(int)json [className [p2SelectPos]] ["Mileage"].Get<long> ()
		};
		pData.RecoveryCooltime = new int[] {
			(int)json [className [p1SelectPos]] ["RecoveryCooltime"].Get<long> (),
			(int)json [className [p2SelectPos]] ["RecoveryCooltime"].Get<long> ()
		};
		pData.Recovery = new int[] {
			(int)json [className [p1SelectPos]] ["Recovery"].Get<long> (),
			(int)json [className [p2SelectPos]] ["Recovery"].Get<long> ()
		};
		pData.WalkRecoveryBonus = new int[] {
			(int)json [className [p1SelectPos]] ["WalkRecoveryBonus"].Get<long> (),
			(int)json [className [p2SelectPos]] ["WalkRecoveryBonus"].Get<long> ()
		};
		pData.Damage = new int[] {
			(int)json [className [p1SelectPos]] ["Damage"].Get<long> (),
			(int)json [className [p2SelectPos]] ["Damage"].Get<long> ()
		};
		pData.FireRate = new int[] {
			(int)json [className [p1SelectPos]] ["FireRate"].Get<long> (),
			(int)json [className [p2SelectPos]] ["FireRate"].Get<long> ()
		};
	}

	//P1
	void SetP1SelectPos (float y){
		p1Select.anchoredPosition = new Vector2 (-130, y);
	}
	void SetP1GuagePower (float value){
		p1GuagePower.fillAmount = value;
	}
	void SetP1GuageFireRate (float value){
		p1GuageFirerate.fillAmount = value;
	}
	void SetP1GuageSpeed (float value){
		p1GuageSpeed.fillAmount = value;
	}
	void SetP1GuageEnergy (float value){
		p1GuageEnergy.fillAmount = value;
	}
	void SetP1GuageRecovery (float value){
		p1GuageRecovery.fillAmount = value;
	}
	//P2
	void SetP2SelectPos (float y){
		p2Select.anchoredPosition = new Vector2 (130, y);
	}
	void SetP2GuagePower (float value){
		p2GuagePower.fillAmount = value;
	}
	void SetP2GuageFireRate (float value){
		p2GuageFirerate.fillAmount = value;
	}
	void SetP2GuageSpeed (float value){
		p2GuageSpeed.fillAmount = value;
	}
	void SetP2GuageEnergy (float value){
		p2GuageEnergy.fillAmount = value;
	}
	void SetP2GuageRecovery (float value){
		p2GuageRecovery.fillAmount = value;
	}
}
