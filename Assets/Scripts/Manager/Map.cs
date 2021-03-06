﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Terrain
{
	HIGHLAND,
	FLATLAND,
	FOREST,
	DESERT,
	VALLY
};

public class Buff
{
	public int lastTurn;
	public float attackBuff;
	public float defendBuff;

	public Buff(int lastTurn, float attackBuff, float defendBuff)
	{
		this.lastTurn = lastTurn;
		this.attackBuff = attackBuff;
		this.defendBuff = defendBuff;
	}
}

public class Command
{
	public Map map;
	public int soldierNum;

	public Command(Map m, int num)
	{
		map = m;
		soldierNum = num;
	}
}

public class Map : MonoBehaviour
{
	/*地图的地形属性*/
	[SerializeField]//调试用
	private Terrain terrain;
	/*地图在玩家处的索引，-1为错误*/
	[SerializeField]//调试用
	private int mapID_player;
	/*地图在MapManger处的编号*/
	[SerializeField]//调试用
	[Header("MapManger处的编号")]
	private int mapID_MapManager;
	/*所属MapManager的编号*/
	[SerializeField]//调试用
	[Header("MapManger编号")]
	private int mapManagerID;
	/*玩家编号，-1为不属于任何玩家*/
	[SerializeField]//调试用
	[Header("玩家编号")]
	private int playerID;
	/*地图块上实际人数的UI显示*/
	public Text baseSoldierNum_UI;
	/*地图块上有效人数的UI显示*/
	public Text effectSoldierNum_UI;
	/*地图所属玩家对象*/
	private Player owner;
	/*该格实际人数*/
	//人数在计算时向上取整
	[SerializeField]//调试用
	private int baseSoldierNum = 1;
	/*该格有效人数*/
	private float effectSoldierNum = 1;
	/*相邻的地图块*/
	//自动获取
	[SerializeField]
	private List<Map> nextMaps;
	/*指向相邻地图块的箭头*/
	[SerializeField]
	private List<GameObject> arrows_Green;
	/*指向相邻地图块的箭头*/
	[SerializeField]
	private List<GameObject> arrows_Red;
	/*地图块攻击力*/
	private float attackPower = 1.0f;
	/*地图块防御力*/
	private float defendPower = 1.0f;
	/*移动目的地图块*/
	public List<Map> MoveDirMap;
	/*移动士兵数量*/
	public List<int> MoveSoldierNum;
	/*地图上的buff*/
	public List<Buff> all_buff;
	/*旗子UI*/
	public Image flagUI;
	/*旗子图片*/
	public Sprite[] flag_Sprite;
	/*是否检查了buff*/
	public bool isChecked = false;

	/// <summary>
	/// 根据初始数据进行初始化
	/// </summary>
	public void Init_Start() 
	{
		MoveDirMap = new List<Map>();
		MoveSoldierNum = new List<int>();
		nextMaps = new List<Map>();
		GetNextBlock();
		arrows_Green = new List<GameObject>();
		arrows_Red = new List<GameObject>();
		all_buff = new List<Buff>();
		InitMapUI();
	}

	/// <summary>
	/// 确认是否有操作权限
	/// </summary>
	/// <param name="operatorID">操作玩家ID</param>
	/// <returns>是否有权限</returns>
	public bool CheckAuthority(int operatorID)
	{
		if (operatorID == playerID)
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
	/// 更新地图显示UI
	/// </summary>
	public void UpdateFlagUI()
	{
		if (playerID == -1)
		{
			flagUI.enabled = false;
		}
		else
		{
			flagUI.enabled = true;
			switch (playerID)
			{
				case 0:
				{
					flagUI.sprite = flag_Sprite[0];
					break;
				}
				case 1:
				{
					flagUI.sprite = flag_Sprite[1];
					break;
				}
			}
		}
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
	/// 更新UI显示
	/// </summary>
	public void UpdateMapUI()
	{
		baseSoldierNum_UI.text = baseSoldierNum.ToString();
	}

	/// <summary>
	/// 检查当前回合buff
	/// </summary>
	public void CheckBuff()
	{
		if (isChecked)
		{
			return;
		}
		foreach(Buff b in all_buff)
		{
			attackPower += b.attackBuff;
			defendPower += b.defendBuff;
		}
		isChecked = true;
	}

	/// <summary>
	/// 清除这回合buff
	/// </summary>
	public void CleanBuff()
	{
		isChecked = false;
		foreach(Buff b in all_buff)
		{
			attackPower -= b.attackBuff;
			defendPower -= b.defendBuff;
		}
		for (int i = all_buff.Count - 1; i >= 0; i--)
		{
			all_buff[i].lastTurn -= 1;
			if (all_buff[i].lastTurn <= 0)
			{
				all_buff.Remove(all_buff[i]);
			}
		}
	}

	/// <summary>
	/// 初始化地图块UI显示
	/// </summary>
	private void InitMapUI()
	{
		baseSoldierNum_UI.text = baseSoldierNum.ToString();
		effectSoldierNum_UI.text = effectSoldierNum.ToString();
		UpdateFlagUI();
	}

	/// <summary>
	/// 获取相邻的地图块(使用碰撞体)
	/// </summary>
	private void GetNextBlock()
	{
		GetComponent<EdgeCollider2D>().enabled = true;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!(other.tag == "Map"))
		{
			return;
		}
		Map mapData = other.gameObject.GetComponent<Map>();
		//提出重复的地图块
		if (nextMaps.Contains(mapData))
		{
			return;
		}
		nextMaps.Add(mapData);
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

	public int MapID_MapManager 
	{
		get
		{
			return mapID_MapManager;
		}
		set
		{
			mapID_MapManager = value;
		}
	}

	public int MapManagerID 
	{
		get
		{
			return mapManagerID;
		}
		set
		{
			mapManagerID = value;
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

	public Terrain Terrain 
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

	public List<Map> NextMaps
	{
		get
		{
			return nextMaps;
		}
	}

	public List<GameObject> Arrows_Green
	{
		get
		{
			return arrows_Green;
		}
	}

	public List<GameObject> Arrows_Red
	{
		get
		{
			return arrows_Red;
		}
	}

	public int BaseSoldierNum 
	{
		get
		{
			return baseSoldierNum;
		}
		set
		{
			baseSoldierNum = value;
		}
	}

	public float AttackPower
	{
		get 
		{
			return attackPower;
		}
		set
		{
			attackPower = value;
		}
	}

	public float DefendPower 
	{
		get
		{
			return defendPower;
		}
		set
		{
			defendPower = value;
		}
	}
}
