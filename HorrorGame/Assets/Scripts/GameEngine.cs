using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameEngine : MonoBehaviour
{
	private static Player _player;
	private static Enemy _enemy;
	public static Player sPlayer { get { if (_player == null) _player = FindObjectOfType<Player>(); return _player; } }
	public static Enemy sEnemy { get { if (_enemy == null) _enemy = FindObjectOfType<Enemy>(); return _enemy; } }
	private void Awake()
	{
		ConstantResources.Initialize();
	}
	static string fileName = "MyLog.txt";
	public static void LogToFile(string iMessage)
	{
		StreamWriter lWriter = new StreamWriter(fileName, true);
		lWriter.WriteLine(iMessage);
		lWriter.Close();
	}
}
