using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	/*玩家所拥有的地图格*/
	private List<Map> maps;
	/*玩家在GameManager处的索引*/
	private int playerID = 0;
	/*玩家当前操作结束*/
	private bool operateEnd = true;
	/*玩家操作类*/
	private Operation selfOperate;

	private void Start() 
	{
		maps = new List<Map>();
		selfOperate = new Operation();
	}

	private void Update() 
	{
		if (!operateEnd)
		{
			selfOperate.Operate(this);
		}
	}

	public List<Map> Maps 
	{
		get
		{
			return maps;
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
