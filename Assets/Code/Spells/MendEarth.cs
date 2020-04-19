using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MendEarth
{
	internal Spell	mData;


	public MendEarth()
	{
		mData	=new Spell();

		mData.mCastTime		=3f;
		mData.mMinAmount	=10;
		mData.mMaxAmount	=50;

		mData.mStatPartAffected	=Spells.AlterStat.Health;
	}
}
