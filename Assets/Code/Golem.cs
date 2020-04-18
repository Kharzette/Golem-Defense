using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
	enum GolemState
	{
		Idle,
		Following,
		PissedOff,
		Dead
	};

	GolemState	mState;
	float		mScanInterval;
	Combatant	mTarget;

	public Player	mMaster;
	public Combat	mCombat;

	Rigidbody	mRigidBody;
	Combatant	mStats;

	const float	MoveSpeed		=10f;
	const float	FollowRange		=10f;
	const float	AggroRange		=100f;
	const float	AttackRange		=2f;
	const float	AttackInterval	=1f;


	void Start()
	{
		mRigidBody	=GetComponent<Rigidbody>();
		mStats		=GetComponent<Combatant>();

		mCombat.RegisterCombatant(mStats, true);

		mStats.mAC			=12;
		mStats.mAttack		=5;
		mStats.mDmgMin		=4;
		mStats.mDmgMax		=16;
		mStats.mMaxHealth	=mStats.mHealth	=2000;
		mStats.mAttackRange	=AttackRange;
	}
	

	void Update()
	{
		if(mState == GolemState.Dead)
		{
			return;
		}

		if(mState == GolemState.Idle)
		{
			mScanInterval	-=Time.deltaTime;
			if(mScanInterval < 0)
			{
				mTarget	=mCombat.Scan(mStats, AggroRange);
				if(mTarget == null)
				{
					mScanInterval	=Random.Range(1f, 3f);
				}
				else
				{
					mState	=GolemState.PissedOff;
				}
			}
			else
			{
				Vector3	mastPos	=mMaster.transform.position;

				float	dist	=Vector3.Distance(transform.position, mastPos);

				if(dist > FollowRange)
				{
					mState	=GolemState.Following;
				}
			}
		}
		else if(mState == GolemState.Following)
		{
			Vector3	mastPos	=mMaster.transform.position;

			float	dist	=Vector3.Distance(transform.position, mastPos);

			if(dist > FollowRange)
			{
				Vector3	move	=mastPos - transform.position;

				move.Normalize();

				transform.rotation.SetLookRotation(move);

				move	*=MoveSpeed * Time.deltaTime;

				transform.position	+=move;
			}
			else
			{
				mState	=GolemState.Idle;
			}
		}
		else if(mState == GolemState.PissedOff)
		{
			if(mTarget == null)
			{
				mState	=GolemState.Idle;
				return;
			}

			Vector3	targPos	=mTarget.gameObject.transform.position;
			float	dist	=Vector3.Distance(transform.position, targPos);

			if(dist > AttackRange)
			{
				Vector3	move	=targPos - transform.position;

				move.Normalize();

				transform.rotation.SetLookRotation(move);

				move	*=MoveSpeed * Time.deltaTime;

				transform.position	+=move;
			}
			else
			{
				if(mStats.mGCD < 0)
				{
					mCombat.Attack(mStats, mTarget);

					mStats.mGCD	=AttackInterval;

					if(mTarget.mHealth <= 0)
					{
						mTarget	=mCombat.Scan(mStats, AggroRange);
					}
				}
			}
		}
	}	
}
