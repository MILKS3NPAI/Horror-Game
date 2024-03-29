﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Entity
{
	float alertnessLevel = 0f;
	Vector2 lastKnownLocation;
	List<Stimulus> stimuli = new List<Stimulus>();
	int maxStimuli = 5;
	UnityAction[] stateEnters = new UnityAction[ConstantResources.ArraySize<AIState>()];
	UnityAction[] stateUpdates = new UnityAction[ConstantResources.ArraySize<AIState>()];
	UnityAction[] stateFixeds = new UnityAction[ConstantResources.ArraySize<AIState>()];
	UnityAction[] stateExits = new UnityAction[ConstantResources.ArraySize<AIState>()];
	[SerializeField]
	private AIState _aiState;
	public AIState mPreviousAIState { get; private set; }
	public AIState mAIState { get { return _aiState; } set { if (value == _aiState) return; mPreviousAIState = _aiState; stateExits[(int)_aiState].Invoke(); _aiState = value; stateEnters[(int)_aiState].Invoke(); stateTime = 0f; } }
	public Transform mPatrolRoute { get { return patrolRoutes[currentRoute]; } }
	public Transform[] patrolRoutes = new Transform[0];
	[SerializeField] float patrolDistanceMinimum = 10f;
	int currentRoute = 0;
	int patrolDir = 1;
	int patrolTarget = 0;
	Vector2 moveTarget = new Vector2();
	public ComplexTraversal traversal;
	[SerializeField] float playerDetectionRadius = 5f;
	[SerializeField] float warningSoundRadius = 7f;
	[SerializeField] float warningSoundPan = .8f;
	[SerializeField] float searchRadius = 5f;
	[SerializeField] float searchDuration = 10f;
	float searchTimer = 10f;
	[SerializeField] bool lethal = true;
	[SerializeField] float lethalRange = 2f;
	int searchDir = 1;
	public bool mCanSeePlayer { get { return (!GameEngine.sPlayer.mHidden && mAIState != AIState.INACTIVE) && Mathf.Abs(GameEngine.sPlayer.transform.position.y - transform.position.y) < playerDetectionRadius; } }
	public bool mSameFloorAsPlayer { get { return Mathf.Abs(GameEngine.sPlayer.mPosition.y - mPosition.y) <= verticalTolerance; } }
	DoorTrigger suspectedDoor;
	[SerializeField] Transform[] hidePoints = new Transform[0];
	Transform nearestHidePoint;
	[SerializeField] float stateTime = 0f;
	[SerializeField] float inactiveTime = 30f;
	FlashlightController flashlight;
	const string warningSoundString = "WarningSound";
	Sound warningSound;
	Player player;
	AudioSource warningSoundSource;
	ScriptedAction currentScriptedAction = ScriptedAction.RUN_AND_HIDE;
	bool playerSeenHiding = false;

	protected override void Awake()
	{
		base.Awake();
		for (int i = 0; i < stateEnters.Length; i++)
		{
			stateEnters[i] = DoNothing;
			stateUpdates[i] = DoNothing;
			stateFixeds[i] = DoNothing;
			stateExits[i] = DoNothing;
		}
		stateFixeds[(int)AIState.PATROL] = PatrolFixed;
		stateFixeds[(int)AIState.CHASE] = ChaseFixed;
		stateFixeds[(int)AIState.COMPLEX_TRAVERSAL] = TraverseFixed;
		stateFixeds[(int)AIState.SEARCH] = SearchFixed;
		stateFixeds[(int)AIState.INACTIVE] = InactiveFixed;
		stateEnters[(int)AIState.CHASE] = ChaseEnter;
		stateEnters[(int)AIState.SEARCH] = SearchEnter;
		stateEnters[(int)AIState.INACTIVE] = InactiveEnter;
		stateExits[(int)AIState.CHASE] = ChaseExit;
		stateExits[(int)AIState.INACTIVE] = InactiveExit;
		stateEnters[(int)AIState.SCRIPTED] = ScriptedEnter;
		EnemyScriptedActions.Initialize();
		ConstantResources.Initialize();
		mGroundFilter = ConstantResources.sEnemyGroundMask;
		flashlight = FindObjectOfType<FlashlightController>();
		player = GameEngine.sPlayer;
	}

	protected override void Start()
	{
		base.Start();
		warningSound = AudioManager.GetSound(warningSoundString);
		if (warningSound == null)
		{
			Debug.LogError("No warning sound set.");
		}
		else
		{
			warningSoundSource = warningSound.source;
			//warningSoundSource.volume = 0;
			if (warningSoundSource != null)
			{
				warningSoundSource.volume = 0f;
				warningSoundSource.Play();
			}
		}
		ResetPatrol();
	}

	void DoNothing()
	{
		return;
	}

	protected override void Update()
	{
		stateUpdates[(int)mAIState].Invoke();
		stateTime += Time.deltaTime;
	}
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Application.isPlaying)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(lastKnownLocation, .25f);
		}
	}
	protected override void FixedUpdate()
	{
		stateFixeds[(int)mAIState].Invoke();
		if (lethal && !GameEngine.sPlayer.mHidden && Mathf.Abs(GameEngine.sPlayer.mPosition.x - mPosition.x) <= lethalRange && Mathf.Abs(GameEngine.sPlayer.mPosition.y - mPosition.y) <= lethalRange)
		{
			GameEngine.sPlayer.Kill();
			GameEngine.PlayDeathScreen(DeathType.CHASE_CONTACT);
		}
		sprite.flipX = previousEntityMovement.x < 0;
		base.FixedUpdate();
	}

	public void ReceiveStimulus(Stimulus iStimulus)
	{
		iStimulus.receiverLocation = mPosition2D;
		if (CanDetectStimulus(iStimulus))
		{
			stimuli.Add(iStimulus);
			stimuli.Sort();
			if (stimuli.Count > maxStimuli)
			{
				stimuli.RemoveAt(maxStimuli);
			}
			alertnessLevel += iStimulus.mValue;
		}
	}

	public bool CanDetectStimulus(Stimulus iStimulus)
	{
		return iStimulus.range < Vector2.Distance(iStimulus.sourceLocation, iStimulus.receiverLocation);
	}

	void ChaseFixed()
	{
		MoveRelative(new Vector2(lastKnownLocation.x - mPosition2D.x, 0));
		if (Mathf.Abs(lastKnownLocation.x - mPosition.x) <= mMoveSpeed * Time.fixedDeltaTime)
		{
			if (!mCanSeePlayer)
			{
				mAIState = AIState.SEARCH;
			}
			else
			{
				mAIState = AIState.PATROL;
			}
		}
		else if (Mathf.Abs(GameEngine.sPlayer.mPosition.x - mPosition.x) <= playerDetectionRadius)
		{
			if (mCanSeePlayer)
			{
				lastKnownLocation = GameEngine.sPlayer.mPosition2D;
			}
			else
			{
				mAIState = AIState.SEARCH;
			}
		}
		PlayWarning();
		flashlight.NarrowCone(Mathf.Max(playerDetectionRadius - Vector2.Distance(mPosition2D, GameEngine.sPlayer.mPosition2D), 1));
	}

	void PatrolFixed()
	{
		moveTarget = new Vector2(mPatrolRoute.GetChild(patrolTarget).position.x, mPatrolRoute.GetChild(patrolTarget).position.y);
		if (Mathf.Abs(moveTarget.x - mPosition.x) <= mMoveSpeed * Time.fixedDeltaTime && Mathf.Abs(moveTarget.y - mPosition.y) <= verticalTolerance)
		{
			patrolTarget += patrolDir;
			if (patrolTarget >= mPatrolRoute.childCount - 1 || patrolTarget == 0)
			{
				patrolDir *= -1;
			}
		}
		PathTo(moveTarget);
		if (Mathf.Abs(GameEngine.sPlayer.mPosition.x - mPosition.x) <= playerDetectionRadius && mCanSeePlayer)
		{
			mAIState = AIState.CHASE;
			lastKnownLocation = GameEngine.sPlayer.mPosition2D;
		}
		PlayWarning(mCanSeePlayer);
	}

	void TraverseFixed()
	{
		traversal.Traverse(this);
	}

	void SearchEnter()
	{
		Debug.Log("Search enter");
		Collider2D[] lResults = new Collider2D[2];
		suspectedDoor = null;
		if (Physics2D.OverlapBox(lastKnownLocation, mMoveSpeed * Time.fixedDeltaTime * Vector2.one, 0f, ConstantResources.sUseableMask, lResults) > 0)
		{
			foreach (Collider2D lCollider in lResults)
			{
				if (lCollider == null)
				{
					continue;
				}
				suspectedDoor = lCollider.GetComponent<DoorTrigger>();
			}
		}
		searchTimer = searchDuration;
		searchDir = previousEntityMovement.x > 0 ? 1 : -1;
		if (player.mHidden)
		{
			Collider2D[] lUseables = new Collider2D[1];
			if (Physics2D.OverlapCircle(mPosition2D, playerDetectionRadius, ConstantResources.sUseableMask, lUseables) > 0)
			{
				foreach (Collider2D lCollider in lUseables)
				{
					HidingSpot lSpot = lCollider.GetComponent<HidingSpot>();
					if (lSpot != null)
					{
						if (lSpot == null)
						{
							continue;
						}
						if (lSpot.mVisibleDistance >= Mathf.Abs(lastKnownLocation.x - lSpot.transform.position.x) && Random.Range(0, 100) <= (lSpot.mChanceToSee + (lSpot.mVisibleDistance / Mathf.Max(Mathf.Abs(mPosition2D.x - lSpot.transform.position.x), .00001f))))
						{
							Debug.Log("Player was caught hiding.");
							playerSeenHiding = true;
							break;
						}
					}
				}
			}
		}
	}

	void SearchFixed()
	{
		float lMoveTarget = lastKnownLocation.x + (searchDir * (playerDetectionRadius * .75f));
		if (Mathf.Abs(lMoveTarget - mPosition.x) <= mMoveSpeed * Time.fixedDeltaTime)
		{
			searchDir *= -1;
		}
		if (Mathf.Abs(GameEngine.sPlayer.mPosition.x - mPosition.x) <= playerDetectionRadius)
		{
			if (mCanSeePlayer)
			{
				lastKnownLocation = GameEngine.sPlayer.mPosition2D;
				mAIState = AIState.CHASE;
				return;
			}
			else if (suspectedDoor != null)
			{
				if (Mathf.Abs(mPosition.x - suspectedDoor.transform.position.x) < useRadius)
				{
					suspectedDoor.Use(this);
				}
				else
				{
					lMoveTarget = suspectedDoor.transform.position.x;
				}
			}
		}
		MoveRelative(new Vector2(lMoveTarget - mPosition.x, 0));
		searchTimer -= Time.fixedDeltaTime;
		if (searchTimer <= 0)
		{
			searchTimer = searchDuration;
			if (playerSeenHiding)
			{
				GameEngine.PlayDeathScreen(DeathType.HIDING_SPOT_FIND);
			}
			else
			{
				mAIState = AIState.INACTIVE;
			}
		}
		PlayWarning();
	}

	void InactiveEnter()
	{
		nearestHidePoint = null;
		foreach (Transform lPoint in hidePoints)
		{
			if (Mathf.Abs(lPoint.position.y - mPosition.y) < 5)
			{
				nearestHidePoint = lPoint;
				break;
			}
		}
		PlayWarning(false);
		flashlight.RestoreCone();
	}

	void InactiveFixed()
	{
		if (nearestHidePoint == null)
		{
			MoveRelative(new Vector2(mPosition2D.x - player.mPosition2D.x, 0));
			if (Mathf.Abs(GameEngine.sPlayer.mPosition.x - mPosition.x) > Camera.main.orthographicSize * 2f)
			{
				mVisible = false;
			}
		}
		else
		{
			MoveRelative(new Vector2(nearestHidePoint.position.x, mPosition.y) - mPosition2D);
			if (Mathf.Abs(nearestHidePoint.position.x - mPosition.x) <= mMoveSpeed * Time.fixedDeltaTime)
			{
				mVisible = false;
			}
		}
		if (stateTime > inactiveTime)
		{
			mVisible = true;
			transform.position = patrolRoutes[0].position;
			mAIState = AIState.PATROL;
		}
	}
	void ChaseEnter()
	{
	}

	void ChaseExit()
	{
		flashlight.RestoreCone();
	}

	void InactiveExit()
	{
		ResetPatrol();
	}

	void ScriptedEnter()
	{
		EnemyScriptedActions.enterActions[(int)currentScriptedAction].Invoke(this);
	}

	void PlayWarning(bool iPlay = true)
	{
		if (warningSound == null || warningSoundSource == null)
		{
			warningSound = AudioManager.GetSound(warningSoundString);
			if (warningSound == null)
			{
				return;
			}
			warningSoundSource = warningSound.source;
			warningSoundSource.Play();
			if (warningSound == null || warningSoundSource == null)
			{
				return;
			}
		}
		if (iPlay && Mathf.Abs(GameEngine.sPlayer.mPosition.x - mPosition.x) <= warningSoundRadius)
		{
			warningSoundSource.volume = 1f / Mathf.Max((float)Mathf.Abs(player.mPosition.x - mPosition.x), .001f);
			warningSoundSource.panStereo = ((player.mPosition.x < mPosition.x) ? warningSoundPan : -warningSoundPan) * Mathf.Min(Vector2.Distance(mPosition2D, player.mPosition), 1f);
		}
		else
		{
			warningSoundSource.volume = 0f;
		}
	}

	public void TakeScriptedAction(ScriptedAction iAction)
	{
		currentScriptedAction = iAction;
		mAIState = AIState.SCRIPTED;
	}

	public void ResetPatrol()
	{
		for (int i = patrolTarget; i < mPatrolRoute.childCount; i++)
		{
			float lDistance = Mathf.Abs(mPatrolRoute.GetChild(i).position.x - player.mPosition.x);
			if (lDistance > patrolDistanceMinimum)
			{
				patrolTarget = i;
				break;
			}
		}
		for (int i = patrolTarget = 0; i < mPatrolRoute.childCount; i++)
		{
			float lDistance = Mathf.Abs(mPatrolRoute.GetChild(i).position.x - player.mPosition.x);
			if (lDistance > patrolDistanceMinimum)
			{
				patrolTarget = i;
				break;
			}
		}

		if (patrolTarget == 0)
		{
			patrolDir = 1;
		}
		else if (patrolTarget == mPatrolRoute.childCount - 1)
		{
			patrolDir = -1;
		}
		mPosition = mPatrolRoute.GetChild(patrolTarget).position;
	}

	public void SetPatrolRoute(int iRoute, int iPoint = 0)
	{
		currentRoute = iRoute;
		patrolTarget = Mathf.Min(iPoint, patrolRoutes[currentRoute].childCount - 1);
	}
}