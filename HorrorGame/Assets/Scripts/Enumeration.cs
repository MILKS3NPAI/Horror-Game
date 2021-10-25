using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ConstantResources
{
	public static ContactFilter2D sGroundMask = new ContactFilter2D();
	public static ContactFilter2D sPlayerGroundMask = new ContactFilter2D();
	public static ContactFilter2D sEnemyGroundMask = new ContactFilter2D();
	public static ContactFilter2D sUseableMask = new ContactFilter2D();
	static bool initialized = false;
	public static void Initialize()
	{
		if (initialized)
		{
			return;
		}
		initialized = true;
		sGroundMask.layerMask = LayerMask.GetMask("Terrain");
		sPlayerGroundMask.layerMask = LayerMask.GetMask("Terrain", "Enemy");
		sEnemyGroundMask.layerMask = LayerMask.GetMask("Terrain", "Player");
		sUseableMask.layerMask = LayerMask.GetMask("Useable");
		sGroundMask.useLayerMask = true;
		sPlayerGroundMask.useLayerMask = true;
		sEnemyGroundMask.useLayerMask = true;
		sUseableMask.useLayerMask = true;
		sUseableMask.useTriggers = true;
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
	AUDIO = 1, VISUAL = 2, PHYSICAL = 4, SCRIPTED = 100
}

public enum AIState
{
	INACTIVE, IDLE, PATROL, CHASE, COMPLEX_TRAVERSAL, SEARCH, SCRIPTED
}

public enum StateEvent
{
	ENTER, UPDATE, FIXED, EXIT
}

public enum ScriptedAction
{
	RESET_PATROL, RUN_AND_HIDE
}