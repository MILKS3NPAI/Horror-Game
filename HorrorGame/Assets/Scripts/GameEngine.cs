using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
	private static Player _player;
	public static Player sPlayer { get { if (_player == null) _player = FindObjectOfType<Player>(); return _player; } }
	private void Awake()
	{
		ConstantResources.Initialize();
	}
}
