using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
	float alertnessLevel = 0f;
	Vector2 lastKnownLocation;
	List<Stimulus> stimuli = new List<Stimulus>();
	int maxStimuli = 5;
	protected  override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	public void ReceiveStimulus(Stimulus iStimulus)
	{
		stimuli.Add(iStimulus);
		stimuli.Sort();
		if (stimuli.Count > maxStimuli)
		{
			stimuli.RemoveAt(maxStimuli);
		}
	}
}