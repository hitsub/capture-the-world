﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlagType{
	None, Blue, Green
}

//フラッグ色管理クラス
static public class FlagColor {
	static public Color32 White = new Color32 (255, 255, 255, 255);
	static public Color32 Blue = new Color32 (0, 128, 255, 255);
	static public Color32 Green = new Color32 (72, 255, 0, 0);
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

//プレイヤーデータクラス
public class Player{
	public Vector3 Position;
	public Vector3 Angle;
}