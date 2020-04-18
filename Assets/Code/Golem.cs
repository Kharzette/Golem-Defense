using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
	public Player	mMaster;
	public Combat	mCombat;

	Rigidbody	mRigidBody;
	Combatant	mMyStats;

	const float	MoveSpeed	=10f;
	const float	FollowRange	=10f;
	const float	AggroRange	=100f;


	void Start()
	{
		mRigidBody	=GetComponent<Rigidbody>();
		mMyStats	=GetComponent<Combatant>();

		mCombat.RegisterCombatant(mMyStats);
	}
	

	void Update()
	{
		Vector3	mastPos	=mMaster.transform.position;

		float	dist	=Vector3.Distance(transform.position, mastPos);

		if(dist > FollowRange)
		{
			Vector3	move	=mastPos - transform.position;

			move.Normalize();

			transform.rotation.SetLookRotation(move);

			move	*=MoveSpeed * Time.deltaTime;

			transform.position	+=move;



		}

		

	}	
}
