using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
	private void Awake()
	{
		ConstantResources.Initialize();
	}
}
