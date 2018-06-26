using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	/*地图在玩家处的索引，-1为错误*/
	private int mapID_player;
	/*地图在GameManger处的编号*/
	private int mapID_gameManager;
	/*玩家编号，-1为不属于任何玩家*/
	private int playerID;
	/*地图所属玩家对象*/
	private Player owner;
	/*地图的地形属性*/
	private int terrain;
	//人数在计算时向上取整
	/*该格实际人数*/
	private float baseSoldierNum = 1;
	/*该格有效人数*/
	private float effectSoldierNum = 1;

	private void Start() 
	{
		mapID_player = -1;
		playerID = -1;
		owner = null;
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

	/*设置地图块所属*/
	public void SetMapOwner(Player owner)
	{
		if (playerID != -1)
		{
			this.owner.Maps.Remove(this);
		}
		this.owner = owner;
		this.owner.Maps.Add(this);
		playerID = this.owner.PlayerID;
		mapID_player = this.owner.Maps.IndexOf(this);
	}

	public int MapID_Player
	{
		get
		{
			return mapID_player;
		}
		set
		{
			mapID_player = value;
		}
	}

	public int MapID_GameManager 
	{
		get
		{
			return mapID_gameManager;
		}
		set
		{
			mapID_gameManager = value;
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
