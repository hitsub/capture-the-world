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
	public FlagType[] Team;

	// Use this for initialization
	void Awake () {
		energy = new int[] { 1000, 1000 };
		WalkSpeed = new int[] { 4, 4 };
		RunSpeed = new int[] { 10, 10 };
		Mileage = new int[] { 50, 50 };
		RecoveryCooltime = new int[] { 1, 1 };
		Recovery = new int[] { 25, 25 };
		WalkRecoveryBonus = new int[] { 2, 2 };
		Damage = new int[]{ 20, 20 };
		FireRate = new int[] { 200, 200 };

		Team = new FlagType[]{ FlagType.Blue, FlagType.Green };

		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
