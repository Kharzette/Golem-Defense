using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using estate = EnemyState.ThinkState;

public class EnemyAI : MonoBehaviour
{
	EnemyState	mState	=new EnemyState();

	internal Combat	mCombat;

	internal GameObject	mBridgeGoal, mFinalGoal;

	const float	MarchSpeed	=5f;
	const float	GoalRange	=5f;


	void Start()
	{
	}


	void Update()
	{
		estate	st	=mState.GetState();

		if(st == estate.PissedOff || st == estate.Dead)
		{
			return;
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


	void GoTo(Vector3 pos)
	{
		Vector3	dir	=pos - transform.position;

		float	dist	=dir.magnitude;
		if(dist < GoalRange)
		{
			mState.SetState(estate.MarchingToSecondGoal);
			return;
		}

		dir.Normalize();

		dir	*=MarchSpeed * Time.deltaTime;

		transform.position	+=dir;
	}


	internal void SetCombat(Combat cmb)
	{
		mCombat	=cmb;
	}

	internal void SetBridgeGoal(GameObject go)
	{
		mBridgeGoal	=go;
	}

	internal void SetFinalGoal(GameObject go)
	{
		mFinalGoal	=go;
	}
}
