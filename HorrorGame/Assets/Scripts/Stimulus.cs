using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Stimulus : IComparable<Stimulus>
{
	public int priority;
	public float range;
	public StimulusType stimulusType;
	public Vector2 sourceLocation;
	public Vector2 receiverLocation;
	public float mDistance { get { float lDist = Vector2.Distance(sourceLocation, receiverLocation); if (lDist > range) return 0; return 1f/lDist; } }
	public float mValue { get { return (float)stimulusType * mDistance * (float)priority; } }

	public int CompareTo(Stimulus iOther)
	{
		return mValue.CompareTo(iOther);
	}
}