using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using estate = EnemyState.ThinkState;

public class EnemyArcherAI : MonoBehaviour
{
	EnemyAI		mAI;
	Combatant	mStats;
	Rigidbody	mRBody;
	bool		mbDeadImpulse;

	const float	AttackRange		=20f;	//archery
	const float	AttackCoolDown	=4f;
	const float	SmackDownForce	=12f;

	void Start()
	{
		mAI		=GetComponent<EnemyAI>();
		mStats	=GetComponent<Combatant>();
		mRBody	=GetComponent<Rigidbody>();

		mStats.mAC			=mStats.mBaseAC				=2;
		mStats.mAttack		=mStats.mBaseAttack			=2;
		mStats.mDmgMin		=mStats.mBaseDmgMin			=1;
		mStats.mDmgMax		=mStats.mBaseDmgMax			=6;
		mStats.mAttackRange	=mStats.mBaseAttackRange	=AttackRange;

		mStats.mMaxHealth	=mStats.mHealth	=100;
	}


	void Update()
	{
		if(mAI.mState.GetState() == estate.Dead)
		{
			if(!mbDeadImpulse)
			{
				mRBody.constraints	=RigidbodyConstraints.None;

				Vector3	smack	=Random.onUnitSphere;
				smack	+=Vector3.up * 2f;

				mRBody.AddForce(smack * SmackDownForce, ForceMode.Impulse);
				mbDeadImpulse	=true;
			}
			return;
		}

		if(mAI.mState.GetState() == estate.PissedOff)
		{
			//ready to attack?
			if(mStats.mCurGCD <= 0f)
			{
				//in range?
				if(mAI.InRange(AttackRange))
				{
					mAI.Attack();
					mStats.mCurGCD	=mStats.mGCD;
				}
				else
				{
					//chase
					mAI.GoToEnemy();
				}
			}
			else
			{
				//in range?
				if(!mAI.InRange(AttackRange))
				{
					//chase
					mAI.GoToEnemy();
				}
			}
		}
	}
}
