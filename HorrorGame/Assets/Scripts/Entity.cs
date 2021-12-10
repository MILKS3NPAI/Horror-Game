using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
	//The velocity given when a character jumps.
	[SerializeField]
	float jumpStrength = 10f;
	//How fast the character moves through the world.
	[SerializeField]
	float moveSpeed = 5f;
	[SerializeField]
	float groundDistance = .2f;
	[SerializeField]
	float maxSlope = .4f;
	[SerializeField]
	float gravity = 9.86f;
	[SerializeField]
	float minDist = 9.86f;
	[SerializeField]
	protected Vector2 previousPhysicsMovement = new Vector2();
	[SerializeField]
	protected Vector2 previousEntityMovement = new Vector2();
	protected Vector2 physicsMovement = new Vector2();
	protected Vector2 entityMovement = new Vector2();
	public Vector2 mMovement { get { return physicsMovement + entityMovement; } }
	public Vector2 mPreviousMovement { get { return previousPhysicsMovement + previousEntityMovement; } }
	[SerializeField]
	float groundRayDistance = 1.25f;
	[SerializeField]
	float stepHeight = .5f;
	[SerializeField]
	protected Vector2 velocity = new Vector2(0f, -1f);
	Rigidbody2D body;
	new protected BoxCollider2D collider;
	public BoxCollider2D mCollider { get { if (collider == null) collider = GetComponent<BoxCollider2D>(); return collider; } }
	[SerializeField]
	bool groundDetected = false;
	protected List<RaycastHit2D> recentFloorHits = new List<RaycastHit2D>();
	protected int floorAction = 0;
	UnityAction[] physicsActions;
	UnityAction<Vector2>[] moveActions;
	public bool physicsEnabled = true;
	[SerializeField]
	ContactFilter2D _groundFilter;
	public ContactFilter2D mGroundFilter { get { return _groundFilter; } protected set { _groundFilter = value; } }
	public Vector2 mPosition2D { get { return new Vector2(transform.position.x, transform.position.y); } set { transform.position = new Vector3(value.x, value.y, transform.position.z); } }
	public Vector2 mPosition { get { return transform.position; } set { transform.position = value; } }
	bool mGrounded { get { return groundDetected && (velocity.y <= 0); } }
	public Vector2 mGroundDetectionOrigin { get { return (mPosition2D + collider.offset) + ((Vector2.down * collider.size.y * .5f * transform.localScale.y) + (Vector2.up * stepHeight)); } }
	public Vector2 mGroundDetectionBoxDimensions { get { return new Vector2(collider.size.x, collider.size.y * .5f) * transform.localScale.y; } }
	public float mGroundDetectionDistance { get { return (Mathf.Max(-velocity.y, minDist) * groundDistance * Time.fixedDeltaTime) + stepHeight + groundRayDistance; } }
	public Vector2 mGroundDetectionPoint { get; protected set; }
	public Vector2 mGroundDetectionUnderfootPoint { get { return new Vector2((mPosition2D + collider.offset).x, mGroundDetectionPoint.y + (groundDistance - (velocity.y * Time.fixedDeltaTime))); } }
	public float mMoveSpeed { get { return moveSpeed; } }
	public bool mGroundDetected { get { return groundDetected; } }
	public bool mMovementDisabled { get; set; }
	public bool mVisible { get { return sprite.enabled; } set { sprite.enabled = value; mMovementDisabled = !sprite.enabled; collider.isTrigger = !sprite.enabled; } }
	public Vector2 mPathTarget { get; protected set; }
	public float mUseRadius { get { return useRadius; } }
	public SpriteRenderer mSprite { get { return sprite; } protected set { sprite = value; } }
	public DoorController mClosestRelevantDoor
	{
		get
		{
			if (closestRelevantDoor != null && Mathf.Abs(closestRelevantDoor.transform.position.y - mPosition.y) <= verticalTolerance) { GameEngine.LogToFile("Found at " + closestRelevantDoor.transform.position); return closestRelevantDoor; }
			if (DoorController.sAllDoors.Count == 0) return null;
			closestRelevantDoor = null;
			foreach (DoorController lController in DoorController.sAllDoors)
			{
				if ((!lController.GoesToward(mPathTarget.y - mPosition.y)) || Mathf.Abs(lController.transform.position.y - mPosition.y) > verticalTolerance)
				{
					continue;
				}
				if (closestRelevantDoor == null)
				{
					closestRelevantDoor = lController;
				}
				//if (Mathf.Abs(lController.transform.position.y - mPosition.y) <= verticalTolerance && lController.GoesToward(mPathTarget.y - mPosition.y) && Mathf.Abs(lController.transform.position.x - mPosition.x) < Mathf.Abs(closestRelevantDoor.transform.position.x - mPosition.x))
				//if (Mathf.Abs(lController.transform.position.x - mPosition.x) < Mathf.Abs(closestRelevantDoor.transform.position.x - mPosition.x))
				if (Vector2.Distance(lController.transform.position, mPosition2D) < Vector2.Distance(closestRelevantDoor.transform.position, mPosition2D))
				{
					closestRelevantDoor = lController;
				}
			}
			return closestRelevantDoor;
		}
	}
	protected SpriteRenderer sprite;
	DoorController closestRelevantDoor;
	static protected float verticalTolerance = 8f;
	[SerializeField] protected float useRadius = 3f;

	protected virtual void Awake()
	{
		Rigidbody2D lBody = GetComponent<Rigidbody2D>();
		closestRelevantDoor = null;
		if (lBody != null)
		{
			body = lBody;
		}
		if (body == null)
		{
			Debug.LogError("Rigidbody not able to be acquired and not set. Was it not enabled in the scene?", gameObject);
		}
		collider = GetComponent<BoxCollider2D>();
		physicsActions = new UnityAction[] { FloorMissAction, FloorHitAction };
		moveActions = new UnityAction<Vector2>[] { MoveThroughAir, MoveAlongFloor };
		sprite = GetComponent<SpriteRenderer>();
		if (sprite == null)
		{
			sprite = GetComponentInChildren<SpriteRenderer>();
			if (sprite == null)
			{
				Debug.LogError("Could not locate sprite renderer for this game object. Is it missing?", gameObject);
			}
		}
	}

	public void ZeroMovement()
	{
		entityMovement = Vector2.zero;
		physicsMovement = Vector2.zero;
		previousEntityMovement = Vector2.zero;
		previousPhysicsMovement = Vector2.zero;
	}

	protected virtual void Start()
	{
		return;
	}

	public void Jump()
	{
		if (mGrounded)
		{
			velocity.y = jumpStrength;
			//Time.timeScale -= .1f;
		}
	}

	protected virtual void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(mGroundDetectionOrigin + Vector2.down * mGroundDetectionDistance, new Vector3(mGroundDetectionBoxDimensions.x,
				mGroundDetectionBoxDimensions.y, 0));
			Gizmos.DrawLine(mGroundDetectionOrigin, mGroundDetectionOrigin + Vector2.down * (mGroundDetectionDistance +
				(mGroundDetectionBoxDimensions.y * .5f)));
			Gizmos.DrawLine(mGroundDetectionOrigin, mGroundDetectionOrigin + Vector2.right);
			if (recentFloorHits.Count > 0)
			{
				Gizmos.DrawWireSphere(recentFloorHits[0].point, .4f);
			}
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + new Vector3(previousPhysicsMovement.x + previousEntityMovement.x,
				previousEntityMovement.y + previousPhysicsMovement.y, 0) * 4f * Time.fixedDeltaTime);
			//Gizmos.DrawLine(mPosition2D + (Vector2.down * collider.size.y / 2), mPosition2D + Vector2.down * groundDistance * Mathf.Max(-velocity.y, minDist) * Time.fixedDeltaTime);
		}
	}

	protected virtual void Update()
	{
		return;
	}

	protected virtual void OnCollisionEnter(Collision iOther)
	{
		Debug.Log("Collision: " + iOther);
	}

	protected unsafe virtual void DetectFloor()
	{
		//bool lFloorDetection = groundDetected = (Physics2D.Raycast(mGroundDetectionOrigin, Vector2.down, mGroundFilter, recentFloorHits, mGroundDetectionDistance + (mGroundDetectionBoxDimensions.y * .5f)) > 0 || Physics2D.BoxCast(mGroundDetectionOrigin, mGroundDetectionBoxDimensions, 0f, Vector2.down, mGroundFilter, recentFloorHits, mGroundDetectionDistance + stepHeight) > 0) && velocity.y <= 0;
		//bool lFloorDetection = groundDetected = Physics2D.BoxCast(mGroundDetectionOrigin, mGroundDetectionBoxDimensions, 0f, Vector2.down,
		//	mGroundFilter, recentFloorHits, mGroundDetectionDistance) > 0 && velocity.y <= 0;
		bool lFloorDetection = groundDetected = physicsEnabled && ((Physics2D.BoxCast(mGroundDetectionOrigin, mGroundDetectionBoxDimensions, 0f, Vector2.down,
			mGroundFilter, recentFloorHits, mGroundDetectionDistance) > 0 || (velocity.y == 0 && Physics2D.Raycast(mGroundDetectionOrigin,
			Vector2.down, mGroundFilter, recentFloorHits, mGroundDetectionDistance) > 0)) && velocity.y <= 0);
		floorAction = (*(byte*)&lFloorDetection & 0x0001b);
	}

	void FloorHitAction()
	{
		foreach (RaycastHit2D lHit in recentFloorHits)
		{
			//if (lHit.point.y - stepHeight <= (mGroundDetectionOrigin.y - (mGroundDetectionBoxDimensions.y * .5f)))
			if (lHit.point.y <= (mGroundDetectionOrigin.y))
			{
				mGroundDetectionPoint = lHit.point;
				Vector2 lSlope = new Vector2(lHit.normal.y * entityMovement.x, lHit.normal.x * -entityMovement.x);
				entityMovement = (Vector2.ClampMagnitude(lSlope, 1.0f) * moveSpeed);
				body.MovePosition(mGroundDetectionUnderfootPoint + ((entityMovement + physicsMovement) * Time.fixedDeltaTime) +
					(Vector2.up * mGroundDetectionBoxDimensions.y));// + (totalMovement * Time.fixedDeltaTime));
																	//body.MovePosition(mGroundDetectionUnderfootPoint + ((playerMovement + physicsMovement) * Time.fixedDeltaTime) + (Vector2.up * mGroundDetectionBoxDimensions.y * .5f));// + (totalMovement * Time.fixedDeltaTime));
				if (Vector2.Dot(lHit.normal, Vector2.up) > maxSlope)
				{
					velocity = Vector2.zero;
					groundDetected = true;
					return;
				}
			}
		}
		body.MovePosition(body.position + ((entityMovement + physicsMovement) * Time.fixedDeltaTime));
	}

	void FloorMissAction()
	{
		velocity.y -= gravity * Time.fixedDeltaTime;
		physicsMovement += velocity;
		body.MovePosition(body.position + ((entityMovement + physicsMovement) * Time.fixedDeltaTime));
	}

	protected virtual void RunPhysicsStep()
	{
		if (!physicsEnabled)
		{
			velocity = Vector2.zero;
			body.MovePosition(body.position + ((entityMovement) * Time.fixedDeltaTime));
			return;
		}
		groundDetected = false;
		DetectFloor();
		physicsActions[floorAction].Invoke();
	}

	protected virtual void FixedUpdate()
	{
		RunPhysicsStep();
		previousPhysicsMovement = physicsMovement;
		previousEntityMovement = entityMovement;
		physicsMovement = Vector2.zero;
		entityMovement = Vector2.zero;
	}

	void MoveAlongFloor(Vector2 iMovement)
	{
		foreach (RaycastHit2D lHit in recentFloorHits)
		{
			if (lHit.point.y - groundRayDistance <= (mGroundDetectionOrigin.y - mGroundDetectionBoxDimensions.y * .5f) && Vector2.Dot(lHit.normal, Vector2.up) > maxSlope)
			{
				//Vector2 lSlope = new Vector2(lHit.normal.y * iMovement.x, lHit.normal.x * -iMovement.x);
				//playerMovement = (Vector2.ClampMagnitude(lSlope, 1.0f) * moveSpeed);
				entityMovement = (Vector2.ClampMagnitude(iMovement, 1.0f) * moveSpeed);// * Time.fixedDeltaTime);
				return;
			}
			else
			{
				entityMovement = (Vector2.ClampMagnitude(iMovement, 1.0f) * moveSpeed * Time.fixedDeltaTime);
			}
		}
	}

	void MoveThroughAir(Vector2 iMovement)
	{
		entityMovement = (Vector2.ClampMagnitude(iMovement, 1.0f) * moveSpeed);
	}

	public void MoveRelative(Vector2 iMovement)
	{
		if (mMovementDisabled)
		{
			return;
		}
		Vector2 lMovement = new Vector2(iMovement.x * moveSpeed, 0);
		if (iMovement.y < 0)
		{
			lMovement *= -iMovement.y;
		}
		else if (iMovement.y > 0)
		{
			Jump();
		}
		moveActions[floorAction].Invoke(lMovement);
	}

	public void MoveAbsolute(Vector2 iMovement)
	{
		if (mMovementDisabled)
		{
			return;
		}
		entityMovement = iMovement;
	}

	public void PathTo(Vector2 iLocation)
	{
		mPathTarget = iLocation;
		if (mClosestRelevantDoor == null || Mathf.Abs(iLocation.y - mPosition.y) <= verticalTolerance)
		{
			MoveRelative(new Vector2(iLocation.x - mPosition.x, 0));
			//Debug.Log("Door: " + mClosestRelevantDoor + ", Mathf.abs: " + Mathf.Abs(iLocation.y - mPosition.y) + ", vert: " + verticalTolerance + ", Movement: " + new Vector2(iLocation.x - mPosition.x, 0), mClosestRelevantDoor);
		}
		else if (closestRelevantDoor != null)
		{
			//Debug.Log("Closest door not null");
			MoveRelative(new Vector2(closestRelevantDoor.transform.position.x - mPosition.x, 0));
			if (Mathf.Abs(closestRelevantDoor.transform.position.x - mPosition.x) <= useRadius && Mathf.Abs(closestRelevantDoor.transform.position.y - mPosition.y) <= verticalTolerance)
			{
				DoorTrigger lDoor = closestRelevantDoor.GetComponent<DoorTrigger>();
				if (lDoor == null)
				{
					lDoor = closestRelevantDoor.GetComponentInChildren<DoorTrigger>();
				}
				if (lDoor != null)
				{
					lDoor.Use(this);
					Debug.Log("Used door: " + lDoor, lDoor);
					closestRelevantDoor = null;
					return;
				}
				else
				{
					Debug.LogError("For some reason, the door here does not have a door trigger.", closestRelevantDoor);
				}
			}
		}
	}
}