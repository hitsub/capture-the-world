using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniJSON;

public class Manager : MonoBehaviour {
	JsonNode json;
	int catchedCrystalNum = 0;
	Dictionary<string,Vector3> crystalPos = new Dictionary<string, Vector3>();
	Text showNum;
	public GameObject crystalnameParentCanvas;
	public Vector3[] crystalPositions;
	// Use this for initialization
	void Start () {

		//JSONの読み込み
		TextAsset text = Resources.Load ("map") as TextAsset;
		json = JsonNode.Parse (text.text);

		//クリスタル生成
		GameObject parent = GameObject.Find ("Crystals");
		GameObject crystal = Resources.Load ("Prefabs/Crystal") as GameObject;
		crystalPositions = new Vector3[100];
		for (int i = 0; i < json ["crystal"].Count; i++) {
			//生成
			GameObject tmp = Instantiate (crystal) as GameObject;
			tmp.transform.SetParent (parent.transform);
			crystalPositions[i] = new Vector3 ((float)json ["crystal"] [i] ["pos"] [0].Get<double> (), (float)json ["crystal"] [i] ["pos"] [1].Get<double> (), (float)json ["crystal"] [i] ["pos"] [2].Get<double> ());
			tmp.transform.position = crystalPositions [i];
			tmp.name = json ["crystal"] [i] ["name"].Get<string> ();
			//情報取得
			crystalPos.Add (tmp.name,tmp.transform.position);
		}

		//表示準備
		showNum = GameObject.Find("CrystalNum").GetComponent<Text>();
		showNum.text = "" + catchedCrystalNum + "/" + crystalPos.Count;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CatchCrystal(){
		catchedCrystalNum++;
		showNum.text = "" + catchedCrystalNum + "/" + crystalPos.Count;
	}
}