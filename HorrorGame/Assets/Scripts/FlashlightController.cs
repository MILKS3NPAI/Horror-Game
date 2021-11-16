using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlashlightController : MonoBehaviour
{
	float innerRadius;
	float outerRadius;
	float innerAngle;
	float outerAngle;
	Light2D flashlight;
	float flickerTimer = 0f;
	float timeOut = 0f;
	[SerializeField]float maxTimeOut = 1f;
	[SerializeField]float maxTimeBetweenFlickers = 4f;

	private void Start()
	{
		flashlight = GetComponent<Light2D>();
		innerRadius = flashlight.pointLightInnerRadius;
		outerRadius = flashlight.pointLightOuterRadius;
		innerAngle = flashlight.pointLightInnerAngle;
		outerAngle = flashlight.pointLightOuterAngle;
	}
	public void NarrowCone(float iAmount)
	{
		//flashlight.pointLightInnerRadius = innerRadius * (1 / iAmount);
		//flashlight.pointLightOuterRadius = outerRadius * (1 / iAmount);
		flashlight.pointLightInnerAngle = innerAngle * (1 / iAmount);
		flashlight.pointLightOuterAngle = outerAngle * (1 / iAmount);
		if (iAmount > 1)
		{
			timeOut -= Time.fixedDeltaTime;
			if (flickerTimer <= 0)
			{
				flickerTimer = Random.Range(0f, maxTimeBetweenFlickers);
				timeOut = Random.Range(0f, maxTimeOut);
				flashlight.enabled = false;
			}
			if (timeOut <= 0)
			{
				flickerTimer -= Time.fixedDeltaTime;
				flashlight.enabled = true;
			}
		}
		else
		{
			flashlight.enabled = true;
		}
	}
	public void RestoreCone()
	{
		//flashlight.pointLightInnerRadius = innerRadius;
		//flashlight.pointLightOuterRadius = outerRadius;
		flashlight.pointLightInnerAngle = innerAngle;
		flashlight.pointLightOuterAngle = outerAngle;
		flashlight.enabled = true;
	}
}
