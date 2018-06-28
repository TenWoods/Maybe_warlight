using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Operation
{
	/*执行操作的玩家ID*/
	private int playerID;
	/*目前玩家处于的操作阶段*/
	private OperateState state;
	/*分配阶段的第一个地图块*/
	private GameObject firstMap;
	/*分配阶段的第二个地图块*/
	private GameObject secondMap;
	/*增兵阶段的目前统率值*/
	private int leaderPoint_current;

	/// <summary>
	/// 操作类的构造函数
	/// </summary>
	/// <param name="player">所属玩家</param>
	public Operation(Player player)
	{
		playerID = player.PlayerID;
		state = player.OpState;
		firstMap = null;
		secondMap = null;
	}

	/// <summary>
	/// 更新操作类的数据
	/// </summary>
	/// <param name="player"></param>
	public void UpdateData(Player player)
	{
		leaderPoint_current = player.LeaderPoint;
		//TODO:可能会有其他的更新
	}

	/// <summary>
	/// 玩家所能进行的所有操作
	/// </summary>
	/// <param name="player">玩家</param>
	/// <param name="state">玩家此时操作状态</param>
	public void Operate(Player player, OperateState state)
	{
		this.state = state;
		//TODO:玩家所能进行的所有操作
		ClickMapBlock();
	}

	/// <summary>
	/// 对鼠标点击事件处理
	/// </summary>
	private void ClickMapBlock()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("鼠标点击");
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo);
			if (hitInfo.collider != null)
			{
				Debug.Log("击中物体");
				//TODO:每个case需要做的事
				switch(state)
				{
					case OperateState.COMMAND_SOLDIER:
					{
						if (hitInfo.collider.tag == "Map")
						{
							Debug.Log("分配兵力");
							CommandOperate(hitInfo.collider.gameObject);	
						}
						break;
					}
					case OperateState.ADD_SOLDIER:
					{
						if (hitInfo.collider.tag == "Map")
						{
							Debug.Log("增兵");
							OperateMapSoldierNum(hitInfo.collider.gameObject);
						}
						break;
					}
					case OperateState.USE_CARDS:
					{
						if (hitInfo.collider.tag == "Card")
						{

						}
						break;
					}
					default: break;
				}
			}
		}
	}
	
	/// <summary>
	/// 分配兵力攻打操作
	/// </summary>
	/// <param name="mapBlock">地图块</param>
	private void CommandOperate(GameObject mapBlock)
	{
		if (firstMap == null)
		{
			bool hasAuthority = mapBlock.GetComponent<Map>().CheckAuthority(playerID);
			if (!hasAuthority)
			{
				return;
			}
			firstMap = mapBlock;
			Debug.Log("选择起始地图块");
			return;
		}
		secondMap = mapBlock;
		Map firstMap_property = firstMap.GetComponent<Map>();
		Map secondMap_property = secondMap.GetComponent<Map>();
		if (!CheckNeighbour(firstMap_property, secondMap_property))
		{
			Debug.Log("这两块不相邻!");
			return;
		}
		Debug.Log("选择第二块地图块");
		Debug.Log("画了个箭头");
		firstMap = null;
		secondMap = null;
		//TODO:画箭头，计算第一地图块上的有效人数

	}

	/// <summary>
	/// 检测是否相邻
	/// </summary>
	/// <param name="firstMap">第一块地图块</param>
	/// <param name="secondMap">第二块地图块</param>
	/// <returns>检测结果</returns>
	private bool CheckNeighbour(Map firstMap, Map secondMap)
	{
		foreach(Map m in firstMap.NextMap)
		{
			if (m.MapID_GameManager == secondMap.MapID_GameManager)
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 增加选定地图块的兵力
	/// </summary>
	/// <param name="mapBlock">地图块</param>
	private void OperateMapSoldierNum(GameObject mapBlock)
	{
		bool hasAuthority = mapBlock.GetComponent<Map>().CheckAuthority(playerID);
		int mapID = mapBlock.GetComponent<Map>().MapID_GameManager;
		if (!hasAuthority)
		{
			Debug.Log("没有权限");
			return;
		}
		if (leaderPoint_current > 0)
		{
			Debug.Log(mapID + "增兵");
			mapBlock.GetComponent<Map>().AddSoldier();
			leaderPoint_current -= 1;
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

	public OperateState State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
		}
	}
}
