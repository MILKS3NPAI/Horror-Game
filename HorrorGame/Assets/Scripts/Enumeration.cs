using System;
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
}

public enum STIMULUS
{
	AUDIO = 1,VISUAL = 2,PHYSICAL = 4,SCRIPTED = 100
}