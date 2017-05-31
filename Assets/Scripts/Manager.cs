using System.Collections;
using System.Collections.Generic;
using System;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEngine.UI;
using MiniJSON;
using UnityStandardAssets.Characters.FirstPerson;

public class CrystalStatus{
	public FlagType Color = FlagType.None;
	public int Energy = 0;
}

public class Manager : MonoBehaviour {

	JsonNode json;
	public CrystalScript[] CrystalScripts = new CrystalScript[CrystalInfo.Num];
	FlagType[] FlagColorTmp = new FlagType[CrystalInfo.Num];
	public Vector3[] crystalPositions;
	public CrystalStatus[] crystalStatus = new CrystalStatus[CrystalInfo.Num];
	Image[] UICrystals = new Image[CrystalInfo.Num];

	//タイマー管理
	public Text Timer;
	float time = 120;

	//スコア管理系
	public Text[] scoreText = new Text[2];
	public Image[] scoreGuage = new Image[2];
	int[] score = new int[2]{ 0, 0 };
	const int scoreMax = 200;
	//float[] scoreCrystalTime = new float[10]{0};
	public bool isEndGame = false;
	public bool isStart = false;
	float isStartTimer = 3f;

	[SerializeField]Text[] textEnergy;
	[SerializeField]Image[] imageEnergy;
	[SerializeField] public FirstPersonController[] fpsController;
	Text ReadyTimer;
	// Use this for initialization
	void Start () {
		ReadyTimer = GameObject.Find ("ReadyPanel").transform.Find ("Timer").gameObject.GetComponent<Text> ();

		//JSONの読み込み
		TextAsset text = Resources.Load ("map") as TextAsset;
		json = JsonNode.Parse (text.text);
		//クリスタル/UIクリスタル生成
		GameObject parent = GameObject.Find ("Crystals");
		//GameObject crystal = Resources.Load ("Prefabs/Crystal") as GameObject;
		GameObject crystal = GameObject.Find ("Crystal") as GameObject;
		GameObject UIparent = GameObject.Find ("UICrystals");
		GameObject UIcrystal = Resources.Load ("Prefabs/UICrystal") as GameObject;

		crystalPositions = new Vector3[100];
		for (int i = 0; i < CrystalInfo.Num; i++) {
			//クリスタル生成
			GameObject tmp = Instantiate <GameObject>(crystal);
			tmp.transform.SetParent (parent.transform);
			crystalPositions[i] = new Vector3 ((float)json ["crystal"] [i] ["pos"] [0].Get<double> (), (float)json ["crystal"] [i] ["pos"] [1].Get<double> (), (float)json ["crystal"] [i] ["pos"] [2].Get<double> ());
			tmp.transform.position = crystalPositions [i];
			tmp.name = json ["crystal"] [i] ["name"].Get<string> ();
			CrystalScripts [i] = tmp.GetComponent<CrystalScript> ();

			//UIクリスタル生成
			GameObject uitmp= Instantiate <GameObject> (UIcrystal);
			uitmp.transform.SetParent (UIparent.transform);
			uitmp.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-84 + 24 * i, 0);
			uitmp.GetComponent<RectTransform> ().localScale = new Vector2 (0.3f, 0.3f);
			uitmp.transform.Find ("Text").gameObject.GetComponent<Text> ().text = GetColumnName (i);
			UICrystals [i] = uitmp.GetComponent<Image> ();

		}
			
		//自陣営の色適用
		for (int i = 0; i < imageEnergy.Length; i++) {
			imageEnergy [i].color = FlagColor.Color (fpsController [i].flagColor);
		}

		//UIクリスタルのフラグ初期化
		for (int i = 0; i < 6; i++) {
			FlagColorTmp [i] = CrystalScripts [i].flagColor;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//開始前
		if (!isStart) {
			isStartTimer -= 1f * Time.deltaTime;
			if (ReadyTimer)
				ReadyTimer.text = isStartTimer.ToString("F1");
			if (isStartTimer <= 0) {
				isStart = true;
				Destroy (GameObject.Find ("ReadyPanel"));
			}
			return;
		}
		//エネルギー表示
		for (int i = 0; i < imageEnergy.Length; i++) {
			textEnergy[i].text = fpsController [i].Energy.ToString ("F0");
			imageEnergy[i].fillAmount = fpsController[i].Energy / 1000;
		}
		//タイマー処理
		if (time >= 0) {
			time -= 1f * Time.deltaTime;
			Timer.text = "" + (int)time / 60 + ":" + (int)time % 60;
		}
		//UIクリスタル管理
		for (int i = 0; i < CrystalInfo.Num; i++) {
			if (FlagColorTmp [i] != CrystalScripts [i].flagColor) {
				UICrystals [i].color = FlagColor.Color (CrystalScripts [i].flagColor);
			}
			FlagColorTmp [i] = CrystalScripts [i].flagColor;
		}

		//ゲームの終了判定
		if ((score [0] >= scoreMax || score [1] >= scoreMax || time <= 0) && (!isEndGame)) {
			isEndGame = true;
			GameObject resPanel = Instantiate (Resources.Load<GameObject> ("Prefabs/ResultPanel"), GameObject.Find("UICanvas").transform);
			resPanel.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);
			resPanel.transform.Find ("Score").gameObject.GetComponent<Text> ().text = score [0].ToString () + " - " + score [1].ToString ();
			resPanel.transform.Find ("ResultText").gameObject.GetComponent<Text> ().text = (score[0]>score[1])?"Blue Won!":(score[0]==score[1])?"DRAW":"Green Won!";
		}
	}

	string GetColumnName(int index) {
		string str = "";
		do {
			str = Convert.ToChar(index % 26 + 0x41) + str;
		} while ((index = index / 26 - 1) != -1);

		return str;
	}

	public void CatchCrystal(){
		//catchedCrystalNum++;
		//showNum.text = "" + catchedCrystalNum + "/" + crystalPos.Count;
	}

	public void GetScore(int s, FlagType side){
		if (side == FlagType.Blue) {
			score [0] += s;
			scoreGuage [0].fillAmount = (float)score [0] / scoreMax;
			scoreText [0].text = score [0].ToString();
		} else {
			score [1] += s;
			scoreGuage [1].fillAmount = (float)score [1] / scoreMax;
			scoreText [1].text = score [1].ToString();
		}
	}

}