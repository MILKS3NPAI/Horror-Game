using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CollisionTrigger : MonoBehaviour
{
	[SerializeField] bool firesOnce;
	[SerializeField] ScriptedAction action;

	void OnTriggerEnter2D(Collider2D iCollider)
	{
		if (firesOnce)
		{
			gameObject.SetActive(false);
		}
		GameEngine.sEnemy.TakeScriptedAction(action);
	}
}