using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
	public bool createSound = false;
	private void FixedUpdate()
	{
		if (createSound)
		{
			createSound = false;
			Stimulus lStimulus = new Stimulus();
			lStimulus.priority = 100;
			lStimulus.range = 500;
			lStimulus.sourceLocation = new Vector2(transform.position.x, transform.position.y);
			lStimulus.stimulusType = StimulusType.AUDIO;
			Enemy lEnemy = FindObjectOfType<Enemy>();
			if (lEnemy == null)
			{
				Debug.LogError("Beacon has no target, no enemy in scene.", gameObject);
				gameObject.SetActive(false);
				return;
			}
			lEnemy.ReceiveStimulus(lStimulus);
		}
	}
}