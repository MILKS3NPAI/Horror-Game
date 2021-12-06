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
	public static GameEngine mEngine { get; private set; }
	static string fileName = "MyLog.txt";
	[SerializeField] GameObject[] deathScreens = new GameObject[ConstantResources.ArraySize<DeathType>()];
	static bool gameRunning = true;

	public static void ReloadReferences()
	{
		_player = null;
		_enemy = null;
	}

	private void Awake()
	{
		mEngine = this;
		gameRunning = true;
		ConstantResources.Initialize();
	}

	public static void LogToFile(string iMessage)
	{
		StreamWriter lWriter = new StreamWriter(fileName, true);
		lWriter.WriteLine(iMessage);
		lWriter.Close();
	}

	public static void PlayDeathScreen(DeathType iType)
	{
		if (!gameRunning)
		{
			return;
		}
		gameRunning = false;
		sPlayer.Kill();
		if (mEngine.deathScreens[(int)iType] != null)
		{
			mEngine.deathScreens[(int)iType].SetActive(true);
		}
		else
		{
			foreach (GameObject lObject in mEngine.deathScreens) {
				if (lObject != null)
				{
					lObject.SetActive(true);
				}
			}
		}
	}
}
