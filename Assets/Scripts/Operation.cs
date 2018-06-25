using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operation
{
	private int playerID;

	private void Operate()
	{
		//TODO:玩家所能进行的所有操作
		if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log("press A");
		}
	}

	public int PlayerID
	{
		get
		{
			return playerID;
		}
		set
		{
			playerID = value;
		}
	}
}
