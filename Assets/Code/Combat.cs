using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
	List<Combatant>		mEnemies	=new List<Combatant>();
	List<Combatant>		mFriendlies	=new List<Combatant>();
	List<EnemySpawner>	mSpawners	=new List<EnemySpawner>();


	void Start()
	{
	}


	internal void RegisterSpawner(EnemySpawner es)
	{
		if(mSpawners.Contains(es))
		{
			return;
		}

		mSpawners.Add(es);
	}


	internal void RegisterCombatant(Combatant c, bool bFriendly)
	{
		if(c == null)
		{
			print("Attempt to register null combatant!");
			return;
		}

		if(bFriendly)
		{
			if(mFriendlies.Contains(c))
			{
				print("Double register on friendly combatant");
				return;
			}
			mFriendlies.Add(c);
		}
		else
		{
			if(mEnemies.Contains(c))
			{
				print("Double register on enemy combatant");
				return;
			}
			mEnemies.Add(c);
		}
	}
	

	void Update()
	{
	}


	internal Combatant Scan(Combatant scanner, float sightRange)
	{
		Vector3	scanPos	=scanner.gameObject.transform.position;

		if(mEnemies.Contains(scanner))
		{
			for(int i=0;i < mFriendlies.Count;i++)
			{
				//TODO: ray cast
				float	dist	=Vector3.Distance(scanPos, mFriendlies[i].gameObject.transform.position);
				if(dist < sightRange)
				{
					return	mFriendlies[i];
				}
			}
		}
		else if(mFriendlies.Contains(scanner))
		{
			for(int i=0;i < mEnemies.Count;i++)
			{
				//TODO: ray cast
				float	dist	=Vector3.Distance(scanPos, mEnemies[i].gameObject.transform.position);
				if(dist < sightRange)
				{
					return	mEnemies[i];
				}
			}
		}
		else
		{
			print("Combatant not registered!");
		}
		
		return	null;
	}


	internal void Attack(Combatant attacker, Combatant defender)
	{
		int	roll	=Random.Range(1, 20);

		print("Attack roll: " + roll);

		if(roll == 20)
		{
			//crit!
			int	dmg	=attacker.DamageRoll() * 2;

			print("" + attacker.gameObject + " attacks " + defender.gameObject + " and hits for " + dmg + " points of damage!");

			defender.mHealth	-=dmg;
		}
		else
		{
			roll	+=attacker.mAttack;

			if(roll >= defender.mAC)
			{
				int	dmg	=attacker.DamageRoll();

				print("" + attacker.gameObject + " attacks " + defender.gameObject + " and hits for " + dmg + " points of damage!");

				defender.mHealth	-=dmg;
			}
		}
	}


	internal void DebugSpawn()
	{
		foreach(EnemySpawner es in mSpawners)
		{
			es.QueueWave(Random.Range(3, 10), 1f);
		}
	}
}
