using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//shared spell data
internal class Spell
{
	internal float		mDuration, mTicRate, mCastTime;
	internal int		mMinAmount, mMaxAmount;
	internal float		mAreaRange;
	internal Vector3	mTargetPos;
	internal Combatant	mTarget;

	internal Spells.AlterStat	mStatPartAffected;

	internal float	mDurationRemaining;	//for live spells
}
