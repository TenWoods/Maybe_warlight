using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	/*玩家所拥有的地图格*/
	private Map[] maps;
	/*玩家在GameManager处的索引*/
	private int playerID = 0;
	private bool operateEnd = true;
	private Operation selfOperate;

	private void Start() 
	{
		maps = new Map[100];
		selfOperate = new Operation();
	}

	private void Update() 
	{
		if (!operateEnd)
		{
			selfOperate.Operate(this);
		}
	}

	public bool OperateEnd 
	{
		set 
		{
			operateEnd = value;
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
