using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
	//this one will show up in inspector
	public int	mHealth, mMana;

	//stats
	internal int	mMaxHealth, mMaxMana;
	internal int	mAC;			//armor class
	internal int	mAttack;
	internal int	mDmgMin, mDmgMax;
	internal float	mAttackRange;
	internal float	mGCD;

	EnemyAI	mAI;


	void Start()
	{
		mAI	=GetComponent<EnemyAI>();

		mMaxHealth	=mHealth	=100;
	}


	void Update()
	{
		mGCD	-=Time.deltaTime;
	}


	internal int	DamageRoll()
	{
		return	Random.Range(mDmgMin, mDmgMax);
	}
}
