using UnityEngine;

public class ShotAreaTrigger : MonoBehaviour {
	[HideInInspector] public bool isShot = false;
	[HideInInspector] public Collider col;
	void OnTriggerEnter(Collider c){
		if (c.gameObject.name == "PhysicsTrigger") {
			col = c;
			isShot = true;
		}
	}
	void OnTriggerExit(Collider c){
		if (c.gameObject.name == "PhysicsTrigger")
			isShot = false;
	}

}
