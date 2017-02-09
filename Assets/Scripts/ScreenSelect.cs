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

	[SerializeField] Dropdown[] dropdown;

	int p1SelectPos = 0, p2SelectPos = 0;
	bool p1SelectFlag = true, p2SelectFlag = true;

	string[] className = { "Assalt", "Bomber", "Charger", "Recon" };
	string[] classStat = { "Damage", "FireRate", "RunSpeed", "Energy", "Recovery" };
	string[] classFunc = { "Power", "FireRate", "Speed", "Energy", "Recovery" };
	int[] classMax = { 200, 200, 10, 2500, 20 };

	JsonNode json;
	bool p1stickback = true, p2stickback = true;

	//Animation Const.
	const string animGuageEase = "easeOutCubic";
	const float animGuageTime = 0.3f;
	const string animPositionEase = "easeOutBack";
	const float animPositionTime = 0.3f;

	PlayerData pData;

	// Use this for initialization
	void Start () {

		pData = GameObject.Find ("PlayerData").GetComponent<PlayerData> ();
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
			if (GlobalInput.Arrow("up", ValueToInputType(dropdown[0].value)) >= -0.3f && GlobalInput.Arrow("down", ValueToInputType(dropdown[0].value)) <= 0.3f && !p1stickback)
				p1stickback = true;
			if (GlobalInput.Arrow("up", ValueToInputType(dropdown[0].value)) <= -0.8f && p1stickback && (p1SelectPos != 0)) {
				p1SelectPos--;
				p1stickback = false;
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
			if (GlobalInput.Arrow("down", ValueToInputType(dropdown[0].value)) >= 0.9f && p1stickback && (p1SelectPos != 3)) {
				p1SelectPos++;
				p1stickback = false;
				//p1Select.anchoredPosition = new Vector2 (130, 100f - 50f * (p2SelectPos));

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
			if (GlobalInput.Convert("decide", ValueToInputType(dropdown[0].value), InputKind.Down)) {
				p1SelectFlag = false;
				p1Stat.text = "Waiting";
			}
		} else {
			if (GlobalInput.Convert("cancel", ValueToInputType(dropdown[0].value), InputKind.Down)) {
				p1SelectFlag = true;
				p1Stat.text = "Selecting";
			}
		}
		if (p2SelectFlag) {
			if (GlobalInput.Arrow("up", ValueToInputType(dropdown[1].value)) >= -0.3f && GlobalInput.Arrow("down", ValueToInputType(dropdown[1].value)) <= 0.3f && !p2stickback)
				p2stickback = true;
			if (GlobalInput.Arrow("up", ValueToInputType(dropdown[1].value)) <= -0.8f && p2stickback && (p2SelectPos != 0)) {
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
			if (GlobalInput.Arrow("down", ValueToInputType(dropdown[1].value)) >= 0.9f && p2stickback && (p2SelectPos != 3)) {
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
			if (GlobalInput.Convert("decide", ValueToInputType(dropdown[1].value), InputKind.Down)) {
				p2SelectFlag = false;
				p2Stat.text = "Waiting";
			}
		} else {
			if (GlobalInput.Convert("cancel", ValueToInputType(dropdown[1].value), InputKind.Down)) {
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

		for (int i = 0; i < dropdown.Length; i++) {
			if (dropdown[i].value == 0)
				pData.inputType [i] = InputType.Keyboard;
			if (dropdown[i].value == 1)
				pData.inputType [i] = InputType.DualShock4OnWindows;
			if (dropdown[i].value == 2)
				pData.inputType [i] = InputType.DualShock4OnMac;
			if (dropdown[i].value == 3)
				pData.inputType [i] = InputType.DualShock3OnWindows;
			if (dropdown[i].value == 4)
				pData.inputType [i] = InputType.DualShock3OnMac;
			if (dropdown[i].value == 5)
				pData.inputType [i] = InputType.DualShock2viaELECOM;
			if (dropdown[i].value == 6)
				pData.inputType [i] = InputType.XBOX360;
		}

	}

	InputType ValueToInputType(int value){
		if (value == 0)
			return InputType.Keyboard;
		if (value == 1)
			return InputType.DualShock4OnWindows;
		if (value == 2)
			return InputType.DualShock4OnMac;
		if (value == 3)
			return InputType.DualShock3OnWindows;
		if (value == 4)
			return InputType.DualShock3OnMac;
		if (value == 5)
			return InputType.DualShock2viaELECOM;
		if (value == 6)
			return  InputType.XBOX360;
		return InputType.Keyboard;
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
