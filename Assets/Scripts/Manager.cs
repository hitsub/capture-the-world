using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniJSON;

public class Manager : MonoBehaviour {
	JsonNode json;
	int crystalNum;
	int catchedCrystalNum = 0;
	Vector3[] crystalPos = new Vector3[10];
	Text showNum;
	// Use this for initialization
	void Start () {

		//JSONの読み込み
		TextAsset text = Resources.Load ("map") as TextAsset;
		json = JsonNode.Parse (text.text);

		//クリスタル生成
		crystalNum = json ["crystal"].Count;
		GameObject parent = GameObject.Find ("Crystals");
		GameObject crystal = Resources.Load ("Prefabs/Crystal") as GameObject;
		for (int i = 0; i < crystalNum; i++) {
			GameObject tmp = Instantiate (crystal) as GameObject;
			tmp.transform.SetParent (parent.transform);
			tmp.transform.position = new Vector3 ((float)json ["crystal"] [i] ["pos"] [0].Get<double> (), (float)json ["crystal"] [i] ["pos"] [1].Get<double> (), (float)json ["crystal"] [i] ["pos"] [2].Get<double> ());
			tmp.name = json ["crystal"] [i] ["name"].Get<string> ();
			crystalPos [i] = tmp.transform.position;
		}

		//表示準備
		showNum = GameObject.Find("CrystalNum").GetComponent<Text>();
		showNum.text = "" + catchedCrystalNum + "/" + crystalNum;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CatchCrystal(){
		catchedCrystalNum++;
		showNum.text = "" + catchedCrystalNum + "/" + crystalNum;
	}
}