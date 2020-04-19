using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystalize
{
	internal Spell	mData;


	public Crystalize()
	{
		mData	=new Spell();

		mData.mCastTime		=5f;
		mData.mMinAmount	=3;
		mData.mMaxAmount	=5;
		mData.mDuration		=20f;

		mData.mStatPartAffected	=Spells.AlterStat.AC;
	}
}
