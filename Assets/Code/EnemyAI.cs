using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using estate = EnemyState.ThinkState;

public class EnemyAI : MonoBehaviour
{
	internal EnemyState	mState	=new EnemyState();

	float		mScanInterval;
	Combatant	mTarget, mMyStats;

	internal Combat	mCombat;

	internal GameObject	mBridgeGoal, mFinalGoal;

	const float	MarchSpeed		=5f;
	const float	GoalRange		=5f;
	const float	ScanIntervalMin	=1f;
	const float	ScanIntervalMax	=3f;
	const float	ScanRange		=50f;


	void Awake()
	{
		mMyStats	=GetComponent<Combatant>();
	}


	void Update()
	{
		estate	st	=mState.GetState();

		if(st == estate.PissedOff || st == estate.Dead)
		{
			return;
		}

		mScanInterval	-=Time.deltaTime;
		if(mScanInterval < 0f)
		{
			mTarget	=mCombat.Scan(mMyStats, ScanRange);
			if(mTarget != null)
			{
				mState.SetState(estate.PissedOff);
			}
			else
			{
				mScanInterval	=Random.Range(ScanIntervalMin, ScanIntervalMax);
			}
		}

		if(st == estate.MarchingToFirstGoal)
		{
			GoTo(mBridgeGoal.transform.position);
		}
		else if(st == estate.MarchingToSecondGoal)
		{
			GoTo(mFinalGoal.transform.position);
		}
	}


	internal void GoToEnemy()
	{
		GoTo(mTarget.gameObject.transform.position);
	}


	void GoTo(Vector3 pos)
	{
		estate	st	=mState.GetState();
		Vector3	dir	=pos - transform.position;

		float	dist	=dir.magnitude;
		if(st == estate.MarchingToFirstGoal && dist < GoalRange)
		{
			mState.SetState(estate.MarchingToSecondGoal);
			return;
		}
		else if(st == estate.MarchingToSecondGoal && dist < GoalRange)
		{
			//lose game
			return;
		}

		dir.Normalize();

		dir	*=MarchSpeed * Time.deltaTime;

		transform.position	+=dir;
	}


	internal void SetCombat(Combat cmb)
	{
		mCombat	=cmb;

		mCombat.RegisterCombatant(mMyStats, false);
	}

	internal void SetBridgeGoal(GameObject go)
	{
		mBridgeGoal	=go;
	}

	internal void SetFinalGoal(GameObject go)
	{
		mFinalGoal	=go;
	}


	internal void Attack()
	{
		if(mTarget == null)
		{
			print("Attack target null!");
			return;
		}

		mCombat.Attack(mMyStats, mTarget);
	}


	internal bool InRange(float range)
	{
		if(mTarget == null)
		{
			print("InRange target null!");
			return	false;
		}

		float	dist	=Vector3.Distance(
			mTarget.gameObject.transform.position,
			gameObject.transform.position);

		return	(dist < range);
	}
}
