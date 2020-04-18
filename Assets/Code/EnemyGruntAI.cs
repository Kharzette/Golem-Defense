using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using estate = EnemyState.ThinkState;

public class EnemyGruntAI : MonoBehaviour
{
	EnemyAI		mAI;
	Combatant	mStats;
	Rigidbody	mRBody;
	bool		mbDeadImpulse;

	const float	AttackRange		=1.5f;	//melee
	const float	AttackCoolDown	=2f;
	const float	SmackDownForce	=10f;

	void Start()
	{
		mAI		=GetComponent<EnemyAI>();
		mStats	=GetComponent<Combatant>();
		mRBody	=GetComponent<Rigidbody>();

		mStats.mAC			=5;
		mStats.mAttack		=1;
		mStats.mDmgMin		=1;
		mStats.mDmgMax		=6;
		mStats.mAttackRange	=AttackRange;
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
			if(mStats.mGCD <= 0f)
			{
				//in range?
				if(mAI.InRange(AttackRange))
				{
					mAI.Attack();
					mStats.mGCD	=AttackCoolDown;
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
