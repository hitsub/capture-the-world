using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEngine.UI;
using MiniJSON;
using UnityStandardAssets.Characters.FirstPerson;

public class Manager : MonoBehaviour {
	JsonNode json;
	Dictionary<string,Vector3> crystalPos = new Dictionary<string, Vector3>();
	public Vector3[] crystalPositions;

	[SerializeField]Text textEnergy;
	[SerializeField]Image imageEnergy;
	[SerializeField]FirstPersonController[] fpsController;

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
			GameObject tmp = Instantiate <GameObject>(crystal);
			tmp.transform.SetParent (parent.transform);
			crystalPositions[i] = new Vector3 ((float)json ["crystal"] [i] ["pos"] [0].Get<double> (), (float)json ["crystal"] [i] ["pos"] [1].Get<double> (), (float)json ["crystal"] [i] ["pos"] [2].Get<double> ());
			tmp.transform.position = crystalPositions [i];
			tmp.name = json ["crystal"] [i] ["name"].Get<string> ();
			//情報取得
			crystalPos.Add (tmp.name,tmp.transform.position);
		}
			
		//自陣営の色適用
		GameObject.Find("EnergyGuage").GetComponent<Image>().color = FlagColor.Color(FlagType.Blue);

	}
	
	// Update is called once per frame
	void Update () {
		textEnergy.text = fpsController[0].Energy.ToString ("F0");
		imageEnergy.fillAmount = fpsController[0].Energy / 1000;
	}

	public void CatchCrystal(){
		//catchedCrystalNum++;
		//showNum.text = "" + catchedCrystalNum + "/" + crystalPos.Count;
	}
}