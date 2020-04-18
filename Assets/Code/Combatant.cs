using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
	//stats
	internal int	mHealth, mMana;
	internal int	mAC;			//armor class
	internal int	mAttack;
	internal int	mDmgMin, mDmgMax;
	internal float	mAttackRange;

	internal float	mGCD;


	void Start()
	{
	}


	void Update()
	{
		mGCD	-=Time.deltaTime;
	}
}
