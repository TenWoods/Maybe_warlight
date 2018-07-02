using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
	/*地图块上实际人数的UI显示*/
	public Text baseSoldierNum_UI;
	/*地图块上有效人数的UI显示*/
	public Text effectSoldierNum_UI;
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
	/*相邻的地图块*/
	[SerializeField]
	private Map[] nextMap;
	/*指向相邻地图块的箭头*/
	private GameObject[] arrows;

	private void Start() 
	{
		mapID_player = -1;
		playerID = 1;
		owner = null;
		arrows = new GameObject[nextMap.Length];
		GetNextBlock();
		InitMapUI();
	}

	/// <summary>
	/// 初始化地图块UI显示
	/// </summary>
	private void InitMapUI()
	{
		baseSoldierNum_UI.text = baseSoldierNum.ToString();
		effectSoldierNum_UI.text = effectSoldierNum.ToString();
	}

	/// <summary>
	/// 确认是否有操作权限
	/// </summary>
	/// <param name="operatorID">操作玩家ID</param>
	/// <returns>是否有权限</returns>
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

	/// <summary>
	/// 设置地图块所属
	/// </summary>
	/// <param name="owner">所属玩家</param>
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

	/// <summary>
	/// 增兵
	/// </summary>
	public void AddSoldier()
	{
		baseSoldierNum += 1;
		//更新UI显示
		baseSoldierNum_UI.text = baseSoldierNum.ToString();
	}

	/// <summary>
	/// 获取相邻的地图块
	/// </summary>
	private void GetNextBlock()
	{
		//TODO:或许用射线

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

	public Map[] NextMap 
	{
		get
		{
			return nextMap;
		}
	}

	public GameObject[] Arrows
	{
		get
		{
			return arrows;
		}
	}
}
