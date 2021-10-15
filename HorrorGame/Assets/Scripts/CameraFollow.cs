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
	[SerializeField]
	float cameraYSensitivity = 4f;
	bool movingY = false;

	public void SetTarget(Transform iTarget)
	{
		followTarget = iTarget;
		desiredPosition = new Vector3(iTarget.position.x, transform.position.y, iTarget.position.z);
	}

	private void Awake()
	{
		player = GameObject.FindObjectOfType<Player>();
		if (followTarget == null)
		{
			followTarget = player.transform;
		}
		desiredPosition.y = transform.position.y;
	}

	private void Update()
	{
		desiredPosition = new Vector3(followTarget.position.x, desiredPosition.y, followTarget.position.z);
		if (Mathf.Abs(desiredPosition.y - followTarget.position.y) > cameraYSensitivity)
		{
			desiredPosition.y = followTarget.position.y;
			transform.position = new Vector3(transform.position.x, desiredPosition.y + offset.y, transform.position.z);
		}
	}

	private void LateUpdate()
	{
		transform.position = Vector3.MoveTowards(transform.position, desiredPosition + offset, Mathf.Min(cameraSpeed + Vector3.Distance(transform.position, desiredPosition) * distanceWeight, cameraSpeedCap) * Time.deltaTime);
	}
}
