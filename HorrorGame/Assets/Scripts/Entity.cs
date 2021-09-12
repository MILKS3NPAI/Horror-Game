using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
	[SerializeField]
	float jumpStrength = 10f;
	[SerializeField]
	float moveSpeed = 5f;
	[SerializeField]
	float groundDistance = .2f;
	[SerializeField]
	float maxSlope = 60f;
	[SerializeField]
	float gravity = 9.86f;
	float minDist = 9.86f;
	[SerializeField]
	protected Vector2 totalMovement = new Vector2();
	float slopeRayLength = 10f;
	float slopeRange = 50f;
	Vector2 velocity = new Vector2();
	protected Rigidbody2D body;
	new protected BoxCollider2D collider;
	[SerializeField]
	bool groundDetected = false;
	ContactFilter2D groundFilter = new ContactFilter2D();
	public Vector2 mPosition2D { get { return new Vector2(transform.position.x, transform.position.y); } }
	public Vector2 mPosition { get { return transform.position; } }
	bool mGrounded { get { return groundDetected && (body.velocity.y <= 0); } }

	private void Awake()
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
		groundFilter.layerMask = LayerMask.GetMask("Terrain");
		groundFilter.useLayerMask = true;
	}

	public void Jump()
	{
		if (mGrounded)
		{
			velocity.y = jumpStrength;
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.DrawCube(mPosition + Vector2.down * groundDistance * -velocity.y * Time.fixedDeltaTime, collider.size);
		}
	}

	protected virtual void RunPhysicsStep()
	{
		List<RaycastHit2D> lHits = new List<RaycastHit2D>();
		groundDetected = false;
		//Debug.Log("Boxcast " + mPosition2D + ". Distance: " + groundDistance * -velocity.y * Time.fixedDeltaTime);
		if (Physics2D.BoxCast(mPosition2D, collider.size, 0f, Vector2.down, groundFilter, lHits, groundDistance * Mathf.Max(-velocity.y, minDist) * Time.fixedDeltaTime) > 0)
		{
			foreach (RaycastHit2D lHit in lHits)
			{
				if (lHit.point.y <= mPosition2D.y && velocity.y <= 0)
				{
					Debug.Log("Normal: " + lHit.normal);
					velocity.y = 0f;
				}
			}
			groundDetected = true;
		}
		if (mGrounded && velocity.y <= 0)
		{
			body.MovePosition(lHits[0].point);
		}
		else
		{
			velocity.y -= gravity * Time.fixedDeltaTime;
		}
		totalMovement += velocity;
	}

	protected virtual void FixedUpdate()
	{
		totalMovement = Vector2.zero;
		RunPhysicsStep();
		body.MovePosition(body.position + totalMovement);
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
		List<RaycastHit2D> lHits = new List<RaycastHit2D>();
		if (mGrounded)
		{
			totalMovement += (Vector2.ClampMagnitude(lMovement, 1.0f) * moveSpeed);
			if (Physics2D.Raycast(transform.position, -transform.up, groundFilter, lHits, (collider.size.y/2) * slopeRayLength) > 0)
			{
				foreach(RaycastHit2D lHit in lHits) {
					if (lHit.normal != Vector2.up)
					{
						totalMovement += (-Vector2.up * (collider.size.y / 2) * slopeRange * Time.fixedDeltaTime);
					}
				}
			}
		}
		else
		{
			totalMovement += (Vector2.ClampMagnitude(lMovement, 1.0f) * moveSpeed);
		}
	}
}