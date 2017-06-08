using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTitle : MonoBehaviour {
	[SerializeField] Fade fade = null;

	void Start() {
		fade.FadeIn (0, () =>
			{
				fade.FadeOut(1.5f);
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

	void SetLogoScale(float value){
		
	}
}
