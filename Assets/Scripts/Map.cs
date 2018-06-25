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

	private void Start() 
	{
		soldierNum = 0;
		mapID = -1;
		playerID = -1;
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
}
