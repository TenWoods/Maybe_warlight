using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
	/*地图的地形属性*/
	private int terrain;
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
	private float baseSoldierNum = 1;
	/*该格有效人数*/
	private float effectSoldierNum = 1;
	/*相邻的地图块*/
	//自动获取
	[SerializeField]
	private List<Map> nextMaps = new List<Map>();
	/*指向相邻地图块的箭头*/
	[SerializeField]
	private List<GameObject> arrows;

	/// <summary>
	/// 根据初始数据进行初始化
	/// </summary>
	public void Init_Start() 
	{
		GetNextBlock();
		arrows = new List<GameObject>();
		InitMapUI();
		//StopGetNextBlock();
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
	/// 初始化地图块UI显示
	/// </summary>
	private void InitMapUI()
	{
		baseSoldierNum_UI.text = baseSoldierNum.ToString();
		effectSoldierNum_UI.text = effectSoldierNum.ToString();
	}

	/// <summary>
	/// 获取相邻的地图块(使用碰撞体)
	/// </summary>
	private void GetNextBlock()
	{
		Debug.Log("Turn On");
		GetComponent<EdgeCollider2D>().enabled = true;
	}

	/// <summary>
	/// 关闭相邻测试
	/// </summary>
	private void StopGetNextBlock()
	{
		Debug.Log("Turn Off");
		GetComponent<EdgeCollider2D>().enabled = false;
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

	public List<Map> NextMaps
	{
		get
		{
			return nextMaps;
		}
	}

	public List<GameObject> Arrows
	{
		get
		{
			return arrows;
		}
	}
}
