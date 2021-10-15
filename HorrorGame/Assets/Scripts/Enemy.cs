using System.Collections;
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
	public AIState mAIState { get { return _aiState; } set { if (value == _aiState) return; mPreviousAIState = _aiState; stateExits[(int)_aiState].Invoke(); _aiState = value; stateEnters[(int)_aiState].Invoke(); } }
	public Transform[] patrolPoints = new Transform[0];
	int patrolDir = 1;
	int patrolTarget = 0;
	Vector2 moveTarget = new Vector2();
	public ComplexTraversal traversal;
	[SerializeField] float playerDetectionRadius = 5f;
	int searchStage = 0;
	[SerializeField] float searchRadius = 5f;
	[SerializeField] float searchDuration = 10f;
	float searchTimer = 10f;
	[SerializeField] bool lethal = true;
	[SerializeField] float lethalRange = 2f;
	int searchDir = 1;
	public bool mCanSeePlayer { get { return (!GameEngine.sPlayer.mHidden) && Mathf.Abs(GameEngine.sPlayer.transform.position.y - transform.position.y) < playerDetectionRadius; } }


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
		stateEnters[(int)AIState.SEARCH] = SearchEnter;
		ConstantResources.Initialize();
		mGroundFilter = ConstantResources.sEnemyGroundMask;
	}

	void DoNothing()
	{
		return;
	}

	protected override void Update()
	{
		stateUpdates[(int)mAIState].Invoke();
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
		if (!GameEngine.sPlayer.mHidden && Mathf.Abs(GameEngine.sPlayer.mPosition.x - mPosition.x) <= lethalRange)
		{
			GameEngine.sPlayer.Kill();
		}
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
	}

	void PatrolFixed()
	{
		moveTarget = new Vector2(patrolPoints[patrolTarget].position.x, patrolPoints[patrolTarget].position.y);
		if (Mathf.Abs(moveTarget.x - mPosition.x) <= mMoveSpeed * Time.fixedDeltaTime)
		{
			patrolTarget += patrolDir;
			if (patrolTarget >= patrolPoints.Length - 1 || patrolTarget == 0)
			{
				patrolDir *= -1;
			}
		}
		MoveRelative(new Vector2(patrolPoints[patrolTarget].position.x - mPosition.x, 0));
		if (Mathf.Abs(GameEngine.sPlayer.mPosition.x - mPosition.x) <= playerDetectionRadius && mCanSeePlayer)
		{
			mAIState = AIState.CHASE;
			lastKnownLocation = GameEngine.sPlayer.mPosition2D;
		}
	}

	void TraverseFixed()
	{
		traversal.Traverse(this);
	}

	void SearchEnter()
	{
		Collider2D[] lResults = new Collider2D[2];
		if (Physics2D.OverlapBox(lastKnownLocation, Vector2.one * mMoveSpeed * Time.fixedDeltaTime, 0f, ConstantResources.sUseableMask, lResults) > 0)
		{
			foreach (Collider2D lCollider in lResults)
			{
				if (lCollider == null)
				{
					return;
				}
				DoorTrigger lTrigger = lCollider.GetComponent<DoorTrigger>();
				if (lTrigger != null)
				{
					lTrigger.Use(this);
				}
			}
		}
		searchTimer = searchDuration;
		searchStage = 0;
		searchDir = previousEntityMovement.x > 0 ? 1 : -1;
	}

	void SearchFixed()
	{
		float lMoveTarget = lastKnownLocation.x + (searchDir * (playerDetectionRadius * .75f));
		if (Mathf.Abs(lMoveTarget - mPosition.x) <= mMoveSpeed * Time.fixedDeltaTime)
		{
			searchDir *= -1;
		}
		if (Mathf.Abs(GameEngine.sPlayer.mPosition.x - mPosition.x) <= playerDetectionRadius && mCanSeePlayer)
		{
			lastKnownLocation = GameEngine.sPlayer.mPosition2D;
			mAIState = AIState.CHASE;
			return;
		}
		MoveRelative(new Vector2(lMoveTarget - mPosition.x, 0));
		searchTimer -= Time.fixedDeltaTime;
		if (searchTimer <= 0)
		{
			mAIState = AIState.PATROL;
		}
	}
}