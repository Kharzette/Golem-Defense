using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public Combat		mCombat;
	public GameObject	mGrunt, mChief, mArcher, mShaman;

	float	mTimeToNextWave;

	Queue<int>		mDiff		=new Queue<int>();
	Queue<float>	mSpawnTime	=new Queue<float>();

	const float	SpawnRadius		=20f;
	const int	ChiefChance		=10;
	const int	ArcherChance	=40;
	const int	ShamanChance	=30;


	void Start()
	{
		mCombat.RegisterSpawner(this);
	}


	void Update()
	{
		mTimeToNextWave	-=Time.deltaTime;
		if(mTimeToNextWave < 0f)
		{
			if(mDiff.Count > 0)
			{
				Spawn(mDiff.Dequeue());

				if(mSpawnTime.Count > 0)
				{
					mTimeToNextWave	=mSpawnTime.Dequeue();
				}
			}
		}
	}


	internal void QueueWave(int difficulty, float seconds)
	{
		if(mSpawnTime.Count > 0)
		{
			mTimeToNextWave	=seconds;
		}
		else
		{
			mSpawnTime.Enqueue(seconds);
		}

		mDiff.Enqueue(difficulty);
	}


	void Spawn(int difficulty)
	{
		if(difficulty > 5)
		{
			int	chiefRoll	=Random.Range(1, 100);
			if(chiefRoll <= ChiefChance)
			{
				difficulty	-=4;

				SpawnChief();
			}
		}

		if(difficulty > 3)
		{
			int	shmRoll	=Random.Range(1, 100);
			if(shmRoll <= ShamanChance)
			{
				difficulty	-=3;

				SpawnShaman();
			}
		}

		if(difficulty > 2)
		{
			int	arRoll	=Random.Range(1, 100);
			if(arRoll <= ArcherChance)
			{
				difficulty	-=2;

				SpawnArcher();
			}
		}

		if(difficulty <= 0)
		{
			return;
		}

		//spawn grunts
		for(int i=0;i < difficulty;i++)
		{
			SpawnGrunt();
		}
	}


	Quaternion	SpawnRot()
	{
		Vector2	dir	=Random.insideUnitCircle;

		Vector3	spot	=Vector3.zero;

		spot.x	=dir.x;
		spot.z	=dir.y;

		return	Quaternion.LookRotation(spot);
	}


	Vector3	SpawnPos()
	{
		Vector2	circ	=Random.insideUnitCircle;
		Vector3	ret		=Vector3.zero;

		ret.x	=circ.x;
		ret.z	=circ.y;

		ret	*=SpawnRadius;

		ret	+=transform.position;

		return	ret;
	}


	void Register(Object obj)
	{
		GameObject	ga	=obj as GameObject;
		if(ga == null)
		{
			return;	//TODO: log something
		}

		EnemyAI	eai	=ga.GetComponent<EnemyAI>();
		if(eai == null)
		{
			return;	//TODO: log something
		}

		eai.SetCombat(mCombat);

	}


	void SpawnChief()
	{
		Object	obj	=Instantiate(mChief, SpawnPos(), SpawnRot());

		Register(obj);
	}

	void SpawnShaman()
	{
		Object	obj	=Instantiate(mShaman, SpawnPos(), SpawnRot());

		Register(obj);
	}

	void SpawnArcher()
	{
		Object	obj	=Instantiate(mArcher, SpawnPos(), SpawnRot());

		Register(obj);
	}

	void SpawnGrunt()
	{
		Object	obj	=Instantiate(mGrunt, SpawnPos(), SpawnRot());

		Register(obj);
	}
}
