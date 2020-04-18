using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
	public enum ThinkState
	{
		MarchingToFirstGoal,
		MarchingToSecondGoal,
		PissedOff,
		Dead
	}

	ThinkState	mState	=ThinkState.MarchingToFirstGoal;

	internal void SetState(ThinkState st)
	{
		mState	=st;
	}


	internal ThinkState GetState()
	{
		return	mState;
	}
}
