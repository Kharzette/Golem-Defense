using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
	List<Combatant>		mFighters	=new List<Combatant>();
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


	internal void RegisterCombatant(Combatant c)
	{
		if(mFighters.Contains(c))
		{
			return;
		}
		mFighters.Add(c);
	}
	

	void Update()
	{
	}


	internal void DebugSpawn()
	{
		foreach(EnemySpawner es in mSpawners)
		{
			es.QueueWave(Random.Range(3, 10), 1f);
		}
	}
}
