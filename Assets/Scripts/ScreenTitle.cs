using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTitle : MonoBehaviour {
	[SerializeField] Fade fade = null;
	[SerializeField] RectTransform	rectLogo;
	[SerializeField] RectTransform	rectLogoMask;
	[SerializeField] Image	imagePushAnyKey;

	void Start () {
		//YPos
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", 0f, "to", 100f, "time", 0.5f, "delay" , 1.5f ,
			"easetype", "easeInOutSine", 
			"onupdate", "SetLogoPos"
		));
		//Angle
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", 0f, "to", 360f, "time", 1.5f, 
			"easetype", "easeInOutSine", 
			"onupdate", "SetLogoAngle"
		));
		//Scale
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", 0f, "to", 0.8f, "time", 1.5f, 
			"easetype", "easeInOutSine", 
			"onupdate", "SetLogoScale"
		));
		//MaskWidth
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", 0f, "to", 400f, "time", 0.5f, "delay" , 1.5f ,
			"easetype", "easeInOutSine", 
			"onupdate", "SetLogoMaskWifth"
		));
		//PushAnyKeyAlpha
		Invoke ("VoidiTweenPushAnyKey", 2f);

		fade.FadeIn (1, () => {
			fade.FadeOut (1.5f);
		});
	}

	void Update () {
		if (Input.anyKey) {
			fade.FadeIn (1, () =>
				{
					fade.FadeOut(1);
				});
		}
	}
	void VoidiTweenPushAnyKey(){
		float valueFrom = (imagePushAnyKey.color.a == 1f) ? 1f : 0f;
		float valueTo = (imagePushAnyKey.color.a == 1f) ? 0f : 1f;
		iTween.ValueTo (gameObject, iTween.Hash (
			"from", valueFrom, "to", valueTo , "time", 2f,
			"easetype", "easeInOutSine", 
			"onupdate", "SetPushAnyKeyAlpha",
			"oncomplete", "VoidiTweenPushAnyKey"
		));
	}

	void SetLogoScale(float value){
		rectLogo.localScale = new Vector2 (value, value);
	}
	void SetLogoAngle(float value){
		rectLogo.eulerAngles = new Vector3 (0, 0, value);
	}
	void SetLogoPos(float value){
		rectLogo.anchoredPosition = new Vector2 (0, value);
	}
	void SetLogoMaskWifth(float value){
		rectLogoMask.sizeDelta = new Vector2 (value, 120f);
	}
	void SetPushAnyKeyAlpha (float value){
		imagePushAnyKey.color = new Color (1f, 1f, 1f, value);
	}
}
