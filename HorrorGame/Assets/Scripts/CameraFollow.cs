using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform followTarget { get; protected set; }
	public bool movementOffset { get; set; }
	public Vector3 offset;
	public float cameraSpeed;
	public float cameraSpeedCap;
	Player player;
	Vector3 desiredPosition;
	public float distanceWeight = .25f;

	public void SetTarget(Transform iTarget)
	{
		followTarget = iTarget;
		desiredPosition = iTarget.position;
	}

	private void Awake()
	{
		player = GameObject.FindObjectOfType<Player>();
		desiredPosition = transform.position;
		if (followTarget == null)
		{
			followTarget = player.transform;
		}
	}

	private void Update()
	{
		desiredPosition = followTarget.position;
	}

	private void LateUpdate()
	{
		transform.position = Vector3.MoveTowards(transform.position, desiredPosition + offset, Mathf.Min(cameraSpeed + Vector3.Distance(transform.position, desiredPosition) * distanceWeight, cameraSpeedCap) * Time.deltaTime);
	}
}
