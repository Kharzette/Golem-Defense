using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Golem	mPet;
	public Combat	mCombat;
	public Spells	mSpells;

	Rigidbody	mRigidBody;
	Combatant	mStats;
	Camera		mMyCam;
	float		mCamDist;

	//spellery
	Spells.SpellTypes	mCastingType;
	Spell				mCastingSpell;
	float				mCastTimeRemaining;

	const float	MoveSpeed	=700f;
	const float	ZoomSpeed	=100f;
	const float	TurnSpeed	=70f;
	const float	JumpSpeed	=50f;
	const float	MinZoom		=5f;
	const float	MaxZoom		=25;


	void Start()
	{
		mRigidBody	=GetComponent<Rigidbody>();
		mStats	=GetComponent<Combatant>();

		GameObject	go	=GameObject.Find("Main Camera");

		mMyCam	=go.GetComponent<Camera>();

		mCombat.RegisterCombatant(mStats, true);

		mCamDist	=mMyCam.transform.localPosition.magnitude;

		mCastingType	=Spells.SpellTypes.None;
	}
	

	void Update()
	{
		UpdateSpells();

		float	side	=Input.GetAxis("Horizontal");
		float	forw	=Input.GetAxis("Vertical");
		float	mx		=Input.GetAxis("Mouse X");
		float	my		=Input.GetAxis("Mouse Y");
		float	m2		=Input.GetAxis("Fire2");
		float	jmp		=Input.GetAxis("Jump");
		float	sprint	=1f + Input.GetAxis("Sprint");
		float	spawn	=Input.GetAxis("DebugSpawn");
		float	wheel	=Input.GetAxis("Mouse ScrollWheel");

		bool	bSnapCameraLocal	=false;
		if(spawn > 0f)
		{
			mCombat.DebugSpawn();
		}

		if(mCastingType != Spells.SpellTypes.None)
		{
			sprint	=0.5f;	//slow when casting
		}

		if(wheel != 0f)
		{
			mCamDist	-=ZoomSpeed * wheel * Time.deltaTime;
			mCamDist	=Mathf.Clamp(mCamDist, MinZoom, MaxZoom);

			bSnapCameraLocal	=true;
		}

		//rotate camera if right mouse held
		if(m2 > 0f)
		{
			Cursor.lockState	=CursorLockMode.Locked;

			Vector3	ang	=mMyCam.transform.eulerAngles;

			ang.y	+=(mx * TurnSpeed * Time.deltaTime);

			//do camera look up down
			ang.x	-=(my * TurnSpeed * Time.deltaTime);

			ang.x	=Mathf.Clamp(ang.x, 0.01f, 65f);

			mMyCam.transform.eulerAngles	=ang;

			bSnapCameraLocal	=true;
		}
		else
		{
			Cursor.lockState	=CursorLockMode.None;
		}

		Vector3	move	=mMyCam.transform.forward * forw;

		//flatten
		move.y	=0f;

		move	+=side * mMyCam.transform.right;

		if(move.magnitude > 0f)
		{
			//grab a copy of the world camera direction
			Quaternion	camCopy	=mMyCam.transform.rotation;

			move.Normalize();

			mRigidBody.AddForce(move * Time.deltaTime * (MoveSpeed * sprint));

//			transform.position	+=move * Time.deltaTime * (MoveSpeed * sprint);

			//rotate player to face camera direction if moving
			Vector3	dir	=mMyCam.transform.position - transform.position;

			//flatten
			dir.y	=0f;
			dir.Normalize();

			//when moving, rotate the player to face
			transform.rotation	=Quaternion.LookRotation(-dir);

			//adjust camera position for new rotation
			mMyCam.transform.rotation	=camCopy;

			//this can cause roll sometimes to build up slowly
			Vector3		eul	=mMyCam.transform.localEulerAngles;

			//clear the roll
			eul.z	=0;

			mMyCam.transform.localEulerAngles	=eul;

			bSnapCameraLocal	=true;
		}

		if(jmp > 0)
		{
			mRigidBody.AddRelativeForce(Vector3.up
				* Time.deltaTime * JumpSpeed, ForceMode.Impulse);
		}

		if(bSnapCameraLocal)
		{
			Vector3	dir	=mMyCam.transform.localRotation * -Vector3.forward;

			mMyCam.transform.localPosition	=dir * mCamDist;
		}

//		print("Sprint: " + sprint);
	}


	void UpdateSpells()
	{
		if(mCastingType != Spells.SpellTypes.None)
		{
			mCastTimeRemaining	-=Time.deltaTime;
			if(mCastTimeRemaining <= 0f)
			{
				CastFinished();
			}
			return;
		}

		if(mStats.mCurGCD > 0f)
		{
			return;
		}

		float	s1		=Input.GetAxis("Spell01");
		float	s2		=Input.GetAxis("Spell02");
		float	s3		=Input.GetAxis("Spell03");
		float	s4		=Input.GetAxis("Spell04");
		float	s5		=Input.GetAxis("Spell05");
		float	s6		=Input.GetAxis("Spell06");

		if(s1 > 0f)
		{
			MendEarth	me		=new MendEarth();
			mCastingSpell		=me.mData;
			mCastingType		=Spells.SpellTypes.MendEarth;
			mCastTimeRemaining	=me.mData.mCastTime;

			mCastingSpell.mTarget	=mPet.mStats;
			return;
		}

		if(s2 > 0f)
		{
			Crystalize	cr		=new Crystalize();
			mCastingSpell		=cr.mData;
			mCastingType		=Spells.SpellTypes.Crystalize;
			mCastTimeRemaining	=cr.mData.mCastTime;

			mCastingSpell.mTarget	=mPet.mStats;
			return;
		}
	}


	void CastFinished()
	{
		mSpells.CastSpell(mCastingSpell);

		//will need this later for particles etc
		switch(mCastingType)
		{
			case	Spells.SpellTypes.Crystalize:
				break;
			case	Spells.SpellTypes.Curse:
				break;
			case	Spells.SpellTypes.DeadlyCloud:
				break;
			case	Spells.SpellTypes.FireBall:
				break;
			case	Spells.SpellTypes.ManaDrinker:
				break;
			case	Spells.SpellTypes.MendEarth:
				break;
			case	Spells.SpellTypes.RageAura:
				break;
			case	Spells.SpellTypes.Rapidity:
				break;
			case	Spells.SpellTypes.ReGrowthAura:
				break;
			case	Spells.SpellTypes.VampiricStrikes:
				break;
			case	Spells.SpellTypes.VisageOfTerror:
				break;
			case	Spells.SpellTypes.WallOfFire:
				break;
			case	Spells.SpellTypes.WardOfVengeance:
				break;
			default:
				print("Casting empty spell!?");
				break;
		}

		//clear spell stuff
		mStats.mCurGCD	=mStats.mGCD;
		mCastingSpell	=null;
		mCastingType	=Spells.SpellTypes.None;
	}
}
