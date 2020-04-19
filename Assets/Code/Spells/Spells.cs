using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tracks active spells on the map
public class Spells : MonoBehaviour
{
	internal enum SpellTypes
	{
		MendEarth, VisageOfTerror, VampiricStrikes,
		WardOfVengeance, Crystalize, Rapidity,
		FireBall, ManaDrinker, WallOfFire,
		Curse, DeadlyCloud,
		ReGrowthAura, RageAura, None
	};

	internal enum AlterStat
	{
		Health, Mana, AC, Attack, DmgMin, DmgMax,
		AttackRange, GCD, MaxHealth, MaxMana
	};

	List<Spell>	mLiveSpells, mExpiringSpells;
	



	void Start()
	{
		mLiveSpells		=new List<Spell>();
		mExpiringSpells	=new List<Spell>();
	}


	void Update()
	{
		foreach(Spell sp in mLiveSpells)
		{
			sp.mDurationRemaining	-=Time.deltaTime;
			if(sp.mDurationRemaining <= 0f)
			{
				mExpiringSpells.Add(sp);
			}
		}

		foreach(Spell sp in mExpiringSpells)
		{
			Combatant	targ	=sp.mTarget;
			switch(sp.mStatPartAffected)
			{
				case	AlterStat.AC:
					targ.mAC	=targ.mBaseAC;
					break;
				case	AlterStat.Attack:
					targ.mAttack	=targ.mBaseAttack;
					break;
				case	AlterStat.AttackRange:
					targ.mAttackRange	=targ.mBaseAttackRange;
					break;
				case	AlterStat.DmgMax:
					targ.mDmgMax	=targ.mBaseDmgMax;
					break;
				case	AlterStat.DmgMin:
					targ.mDmgMin	=targ.mBaseDmgMin;
					break;
				case	AlterStat.GCD:
					targ.mGCD	=targ.mBaseGCD;
					break;
				default:
					print("Expiring buff on something not yet implemented!");
					break;

			}

			print("Spell expiring: " + sp);
			mLiveSpells.Remove(sp);
		}

		mExpiringSpells.Clear();
	}


	//return true if ok to cast
	bool CheckStacking(Spell spell)
	{
		if(spell.mDuration <= 0f)
		{
			return	true;	//no lasting effect
		}

		//check direct targeted
		foreach(Spell sp in mLiveSpells)
		{
			if(sp.mTarget != spell.mTarget)
			{
				continue;
			}

			if(sp.mStatPartAffected == spell.mStatPartAffected)
			{
				print("Stacking rejecting spell");
				return	false;
			}
		}

		//TODO: auras and areas and such

		return	true;
	}


	internal void CastSpell(Spell spell)
	{
		int	amount	=Random.Range(spell.mMinAmount, spell.mMaxAmount);

		Combatant	targ	=spell.mTarget;

		if(spell.mDuration > 0f)
		{
			if(CheckStacking(spell))
			{
				switch(spell.mStatPartAffected)
				{
					case	AlterStat.AC:
						targ.mAC	=targ.mBaseAC + amount;
						break;
					case	AlterStat.Attack:
						targ.mAttack	=targ.mBaseAttack + amount;
						break;
					case	AlterStat.AttackRange:
						targ.mAttackRange	=targ.mBaseAttackRange + amount;
						break;
					case	AlterStat.DmgMax:
						targ.mDmgMax	=targ.mBaseDmgMax + amount;
						break;
					case	AlterStat.DmgMin:
						targ.mDmgMin	=targ.mBaseDmgMin + amount;
						break;
					case	AlterStat.GCD:
						targ.mGCD	=targ.mBaseGCD + amount;
						break;
					default:
						print("Duration buff on something not yet implemented!");
						break;
				}

				spell.mDurationRemaining	=spell.mDuration;

				mLiveSpells.Add(spell);
			}
		}
		else
		{
			switch(spell.mStatPartAffected)
			{
				case	AlterStat.Health:	//heal rather than stat add
					targ.mHealth	+=amount;
					if(targ.mHealth > targ.mMaxHealth)
					{
						targ.mHealth	=targ.mMaxHealth;
					}
					break;
				case	AlterStat.Mana:	//heal rather than stat add
					targ.mMana	+=amount;
					if(targ.mMana > targ.mMaxMana)
					{
						targ.mMana	=targ.mMaxMana;
					}
					break;
				default:
					print("Stat altering spell with no duration!");
					break;
			}
		}
	}
}
