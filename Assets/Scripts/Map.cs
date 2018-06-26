using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	/*这一格所拥有士兵数s*/
	private float soldierNum;
	/*地图在玩家处的索引，-1为错误*/
	private int mapID;
	/*玩家编号，-1为不属于任何玩家*/
	private int playerID;
	/*地图的地形属性*/
	private int terrain;

	private void Start() 
	{
		soldierNum = 0;
		mapID = -1;
		playerID = -1;
	}

	/*确认是否对此地图块有操作权限*/
	public bool CheckAuthority(int operatorID)
	{
		if (operatorID == playerID || playerID == -1)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public int MapID
	{
		get
		{
			return mapID;
		}
		set
		{
			mapID = value;
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

	public int Terrain 
	{
		get
		{
			return terrain;
		}
		set
		{
			terrain = value;
		}
	}
}
