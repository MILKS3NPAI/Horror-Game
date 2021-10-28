using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class EnemyTrigger : Useable
{
	[SerializeField] ScriptedAction action;
	public override void Use(Entity iEntity)
	{
		GameEngine.sEnemy.TakeScriptedAction(action);
        AudioManager.PlaySound("Hell");
	}
}