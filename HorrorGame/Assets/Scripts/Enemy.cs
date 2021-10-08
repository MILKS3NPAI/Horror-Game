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

	protected override void FixedUpdate()
	{
		stateFixeds[(int)mAIState].Invoke();
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
		MoveRelative(new Vector2(stimuli[0].sourceLocation.x - mPosition2D.x, 0));
	}

	void PatrolFixed()
	{
		moveTarget = new Vector2(patrolPoints[patrolTarget].position.x, patrolPoints[patrolTarget].position.y);
		if (Mathf.Abs(moveTarget.x - mPosition2D.x) < mMoveSpeed * Time.fixedDeltaTime){
			patrolTarget += patrolDir;
			if (patrolTarget >= patrolPoints.Length - 1 || patrolTarget == 0)
			{
				patrolDir *= -1;
			}
		}
		MoveRelative(new Vector2(patrolPoints[patrolTarget].position.x - mPosition.x, 0));
	}

	void TraverseFixed()
	{
		traversal.Traverse(this);
	}

}