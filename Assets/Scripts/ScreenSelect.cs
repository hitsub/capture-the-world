using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	string[] className = { "Assalt", "Bomber", "Charger", "Recon" };
	string[] classStat = { "Damage", "FireRate", "RunSpeed", "Energy", "Recovery" };
	string[] classFunc = { "Power", "FireRate", "Speed", "Energy", "Recovery" };
	int[] classMax = { 300, 150, 15, 2500, 10 };

	JsonNode json;


	// Use this for initialization
	void Start () {
		//TextAsset text = Resources.Load ("class") as TextAsset;
		json = JsonNode.Parse (Resources.Load <TextAsset>("class").text);
		for (int i = 0; i < classStat.Length; i++) {
			print (i + ":" + (float)json ["Assalt"] [classStat [i]].Get<long> () / classMax [i]+"\n"+json ["Assalt"] [classStat [i]].Get<long> ()+" / "+classMax [i]);
			iTween.ValueTo (gameObject, iTween.Hash ("from", 0f, "to", (float)json ["Assalt"] [classStat [i]].Get<long>() / classMax [i], "time",0.1f, "onupdate","SetP1Guage"+classFunc[i]));
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, -5f * Time.deltaTime, 0);
		if (Input.GetKeyDown (KeyCode.UpArrow) && (p1SelectPos != 0)) {
			p1SelectPos--;
			iTween.ValueTo (gameObject, iTween.Hash ("from",100f-50f*(p1SelectPos+1),"to",100f-50f*(p1SelectPos),"time",0.1f,"onupdate","SetP1SelectPos"));
			for (int i = 0; i < classStat.Length; i++) {
				iTween.ValueTo (gameObject, iTween.Hash ("from", (float)json [className[p1SelectPos+1]] [classStat [i]].Get<long>() / classMax [i], "to", (float)json [className[p1SelectPos]] [classStat [i]].Get<long>() / classMax [i], "time",0.1f, "onupdate","SetP1Guage"+classFunc[i]));
			}
		}
		if (Input.GetKeyDown (KeyCode.DownArrow) && (p1SelectPos != 3)) {
			p1SelectPos++;
			iTween.ValueTo (gameObject, iTween.Hash ("from",100f-50f*(p1SelectPos-1),"to",100f-50f*(p1SelectPos),"time",0.1f,"onupdate","SetP1SelectPos"));
			for (int i = 0; i < classStat.Length; i++) {
				//print (i + " from:" + (float)json [className [p1SelectPos - 1]] [classStat [i]].Get<long> () / classMax [i]+" to:"+(float)json [className[p1SelectPos]] [classStat [i]].Get<long>() / classMax [i]);
				iTween.ValueTo (gameObject, iTween.Hash ("from", (float)json [className[p1SelectPos-1]] [classStat [i]].Get<long>() / classMax [i], "to", (float)json [className[p1SelectPos]] [classStat [i]].Get<long>() / classMax [i], "time",0.1f, "onupdate","SetP1Guage"+classFunc[i]));
			}
		}
	}

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
}
