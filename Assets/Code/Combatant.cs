using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
	//these will show up in inspector
	public int		mHealth, mMana;
	public int		mAC, mAttack, mDmgMin, mDmgMax;
	public float	mAttackRange;

	//stats
	internal int	mMaxHealth, mMaxMana;
	internal int	mBaseAC;			//armor class
	internal int	mBaseAttack;
	internal int	mBaseDmgMin, mBaseDmgMax;
	internal float	mBaseAttackRange;
	internal float	mBaseGCD, mGCD, mCurGCD;

	EnemyAI	mAI;


	void Start()
	{
		mAI	=GetComponent<EnemyAI>();

		mMaxHealth	=mHealth	=100;
	}


	void Update()
	{
		mCurGCD	-=Time.deltaTime;
	}


	internal int	DamageRoll()
	{
		return	Random.Range(mDmgMin, mDmgMax);
	}
}
