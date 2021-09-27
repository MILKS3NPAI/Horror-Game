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
	Vector2 velocity = new Vector2();
	Rigidbody2D body;
	new protected BoxCollider2D collider;
	[SerializeField]
	bool groundDetected = false;
	protected List<RaycastHit2D> recentFloorHits = new List<RaycastHit2D>();
	protected int floorAction = 0;
	UnityAction[] physicsActions;
	UnityAction<Vector2>[] moveActions;
	public bool physicsEnabled = true;
	public ContactFilter2D mGroundFilter { get; protected set; }
	public Vector2 mPosition2D { get { return new Vector2(transform.position.x, transform.position.y); } }
	public Vector2 mPosition { get { return transform.position; } }
	bool mGrounded { get { return groundDetected && (velocity.y <= 0); } }
	public Vector2 mGroundDetectionOrigin { get { return mPosition2D + collider.offset + (Vector2.up * stepHeight); } }
	public Vector2 mGroundDetectionBoxDimensions { get { return collider.size * transform.localScale.y; } }
	public float mGroundDetectionDistance { get { return (groundDistance) * Mathf.Max(-velocity.y, minDist) * Time.fixedDeltaTime + stepHeight; } }
	public Vector2 mGroundDetectionPoint { get; protected set; }
	public Vector2 mGroundDetectionUnderfootPoint { get { return new Vector2((mPosition2D + collider.offset).x, mGroundDetectionPoint.y + groundDistance); } }
	public float mMoveSpeed { get { return moveSpeed; } }
	public bool mGroundDetected { get { return groundDetected; } }

	protected virtual void Awake()
	{
		Rigidbody2D lBody = GetComponent<Rigidbody2D>();
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
		mGroundFilter = ConstantResources.sGroundMask;
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

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(mGroundDetectionOrigin + Vector2.down * mGroundDetectionDistance, new Vector3(mGroundDetectionBoxDimensions.x,
				mGroundDetectionBoxDimensions.y, 0));
			Gizmos.DrawLine(mGroundDetectionOrigin, mGroundDetectionOrigin + Vector2.down * (mGroundDetectionDistance +
				(mGroundDetectionBoxDimensions.y * .5f)));
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
			Vector2.down, mGroundFilter, recentFloorHits, mGroundDetectionDistance + groundRayDistance) > 0)) && velocity.y <= 0);
		floorAction = (*(byte*)&lFloorDetection & 0x0001b);
	}

	void FloorHitAction()
	{
		foreach (RaycastHit2D lHit in recentFloorHits)
		{
			//if (lHit.point.y - stepHeight <= (mGroundDetectionOrigin.y - (mGroundDetectionBoxDimensions.y * .5f)))
			if (lHit.point.y <= (mGroundDetectionOrigin.y - (mGroundDetectionBoxDimensions.y * .5f)))
			{
				mGroundDetectionPoint = lHit.point;
				Vector2 lSlope = new Vector2(lHit.normal.y * entityMovement.x, lHit.normal.x * -entityMovement.x);
				entityMovement = (Vector2.ClampMagnitude(lSlope, 1.0f) * moveSpeed);
				body.MovePosition(mGroundDetectionUnderfootPoint + ((entityMovement + physicsMovement) * Time.fixedDeltaTime) +
					(Vector2.up * mGroundDetectionBoxDimensions.y * .5f));// + (totalMovement * Time.fixedDeltaTime));
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
		entityMovement = iMovement;
	}
}