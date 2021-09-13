﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ConstantResources
{
	public static ContactFilter2D sGroundMask = new ContactFilter2D();
	public static void Initialize()
	{
		sGroundMask.layerMask = LayerMask.GetMask("Terrain");
		sGroundMask.useLayerMask = true;
	}
	public static Array EnumArray<T>()
	{
		return Enum.GetValues(typeof(T));
	}
	public static string[] EnumNames<T>()
	{
		return Enum.GetNames(typeof(T));
	}
	public static string FormattedName<T>(T lValue, bool lCapitalizeAll = true)
	{
		StringBuilder lInitial = new StringBuilder(Enum.GetName(typeof(T), lValue).ToLower());
		lInitial = lInitial.Replace('_', ' ');
		lInitial[0] = Char.ToUpper(lInitial[0]);
		if (lCapitalizeAll)
		{
			for (int i = 1; i < lInitial.Length; i++)
			{
				if (lInitial[i - 1] == ' ')
				{
					lInitial[i] = Char.ToUpper(lInitial[i]);
				}
			}
		}
		return lInitial.ToString();
	}
	public static int ArraySize<T>()
	{
		return Enum.GetValues(typeof(T)).Length;
	}
}

public enum StimulusType
{
	AUDIO = 1,VISUAL = 2,PHYSICAL = 4,SCRIPTED = 100
}

public enum AIState
{
	INACTIVE, IDLE, PATROL, CHASE, SEARCH, SCRIPTED
}

public enum StateEvent {
	ENTER,UPDATE,FIXED,EXIT
}