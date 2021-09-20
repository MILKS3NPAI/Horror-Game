using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ComplexTraversal : MonoBehaviour
{
	public Transform[] traversalPoints = new Transform[0];
	public Vector2 movementCondition = Vector2.right;
	public float movementLeeway = .8f;
	public float movementRate = 1f;
	public bool playerAccessible = false;
	public bool temporarilyDisablesCollision = true;
	int currentTarget;
	Vector2 currentTargetPosition;
	AIState previousState;

	private void OnTriggerEnter2D(Collider2D iOther)
	{
		try
		{
			Entity lEntity = iOther.GetComponent<Entity>();
			Debug.Log("Trigger enter: " + lEntity);
			BeginTraversal(lEntity);
		}
		catch (Exception)
		{
			return;
		}
	}

	public void BeginTraversal(Entity iEntity)
	{
		if (traversalPoints.Length == 0)
		{
			return;
		}
		if (iEntity is Player)
		{
			if (!playerAccessible)
			{
				return;
			}
		}
		currentTarget = 0;
		if (iEntity is Enemy)
		{
			Enemy lEnemy = (Enemy)iEntity;
			previousState = lEnemy.mAIState;
			lEnemy.mAIState = AIState.COMPLEX_TRAVERSAL;
			Debug.Log("Previous state: " + previousState);
			lEnemy.physicsEnabled = false;
			currentTargetPosition = new Vector2(traversalPoints[0].position.x, traversalPoints[0].position.y);
			lEnemy.traversal = this;
		}
		iEntity.GetComponent<Collider2D>().isTrigger = temporarilyDisablesCollision;
	}

	public void Traverse(Entity iEntity)
	{
		if (Vector2.Distance(currentTargetPosition, iEntity.mPosition2D) < iEntity.mMoveSpeed * Time.fixedDeltaTime)
		{
			iEntity.MoveAbsolute((new Vector2(traversalPoints[currentTarget].position.x, traversalPoints[currentTarget].position.y) - iEntity.mPosition2D));
			currentTarget++;
			if (currentTarget >= traversalPoints.Length)
			{
				TraverseEnd(iEntity);
				return;
			}
			currentTargetPosition = new Vector2(traversalPoints[currentTarget].position.x, traversalPoints[currentTarget].position.y);
		}
		else
		{
			iEntity.MoveAbsolute(Vector2.ClampMagnitude((currentTargetPosition - iEntity.mPosition2D), 1.0f) * iEntity.mMoveSpeed * movementRate);
		}
	}

	public void TraverseEnd(Entity iEntity)
	{
		if (iEntity is Player)
		{
			if (!playerAccessible)
			{
				return;
			}
		}
		if (iEntity is Enemy)
		{
			Enemy lEnemy = (Enemy)iEntity;
			lEnemy.mAIState = previousState;
			Debug.Log("Setting " + lEnemy + " to state " + previousState);
			lEnemy.physicsEnabled = true;
		}
		if (temporarilyDisablesCollision)
		{
			iEntity.GetComponent<Collider2D>().isTrigger = false;
		}
	}
}