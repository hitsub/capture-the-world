using UnityEngine;
using System.Collections.Generic;
//陣営種類
public enum FlagType{
	None, Blue, Green
}
	
//基本データクラス
static public class CrystalInfo{
	static public int Num = 6;
	static public float scoreCrystalSpan = 3;
}


//フラッグ色管理クラス
static public class FlagColor {
	static public Color32 White = new Color32 (255, 255, 255, 255);
	static public Color32 Blue = new Color32 (0, 128, 255, 255);
	static public Color32 Green = new Color32 (72, 255, 0, 255);
	static public Color Color(FlagType flag){
		Color32 result = new Color32();
		switch (flag){
		case FlagType.None:
			result = FlagColor.White;
			break;
		case FlagType.Blue:
			result = FlagColor.Blue;
			break;
		case FlagType.Green:
			result = FlagColor.Green;
			break;
		}
		return result;
	}
}

public class InjectResult {
	public bool Result = false, Destroy = false, Capture = false;
}

//プレイヤーデータクラス
public class Player{
	public Vector3 Position;
	public Vector3 Angle;
}

//クリスタルステータスクラス
public class Crystal{
	FlagType crystalColor = FlagType.None;
	FlagType guageColor = FlagType.None;
	int Captures = 0;
	int[] Energy = new int[8];
}

