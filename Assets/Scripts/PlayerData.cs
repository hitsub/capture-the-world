using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	public int[] energy;
	public int[] WalkSpeed;
	public int[] RunSpeed;
	public int[] Mileage;
	public int[] RecoveryCooltime;
	public int[] Recovery;
	public int[] WalkRecoveryBonus;
	public int[] Damage;
	public int[] FireRate;

	// Use this for initialization
	void Start () {
		energy = new int[] { 1000, 1000 };
		WalkSpeed = new int[] { 4, 4 };
		RunSpeed = new int[] { 10, 10 };
		Mileage = new int[] { 50, 50 };
		RecoveryCooltime = new int[] { 1, 1 };
		Recovery = new int[] { 5, 5 };
		WalkRecoveryBonus = new int[] { 2, 2 };
		Damage = new int[]{ 10, 10 };
		FireRate = new int[] { 100, 100 };

		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
