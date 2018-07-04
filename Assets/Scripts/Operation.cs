using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Operation
{
	/*执行操作的玩家ID*/
	private int playerID;
	/*目前玩家处于的操作阶段*/
	private OperateState state;
	/*分配阶段选中地图块*/
	private GameObject clickMap;
	/*卡牌使用阶段选择的卡牌*/
	private Card usedCard;
	/*增兵阶段的目前统率值*/
	private int leaderPoint_current;
	/*记录玩家行为步骤*/
	private PlayerStep save_Steps;

	/// <summary>
	/// 操作类的构造函数
	/// </summary>
	/// <param name="player">所属玩家</param>
	public Operation(Player player, PlayerStep steps)
	{
		playerID = player.PlayerID;
		state = player.OpState;
		clickMap = null;
		save_Steps = steps;
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
			Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hitInfo = Physics2D.Raycast(mousPos, Vector2.zero);
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
							Debug.Log("指挥");
							DrawArrows(hitInfo.collider.gameObject);	
						}
						if (hitInfo.collider.tag == "Arrow")
						{
							CommandSoilder(hitInfo.collider.gameObject, clickMap.GetComponent<Map>());
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
							Debug.Log("出牌");
							UseCard();
							//TODO:出牌操作;牌的效果、手牌List中相应牌去除
						}
						break;
					}
					default: break;
				}
			}
		}
	}
	
	/// <summary>
	/// 画箭头，显示箭头
	/// </summary>
	/// <param name="mapBlock">地图块</param>
	private void DrawArrows(GameObject mapBlock)
	{
		if(clickMap == null)
		{
			bool hasAuthority = mapBlock.GetComponent<Map>().CheckAuthority(playerID);
			if (!hasAuthority)
			{
				return;
			}
			clickMap = mapBlock;
			Debug.Log("选择地图块");
			Map mapBlockData = mapBlock.GetComponent<Map>();
			for (int i = 0; i < mapBlockData.NextMaps.Count; i++)
			{
				if (mapBlockData.Arrows[i] != null)
				{
					mapBlockData.Arrows[i].SetActive(true);
					continue;
				}
				Debug.Log("画了个箭头");
				//TODO:画箭头,并把箭头给地图块保存,索引代表指向哪块相邻块
			}
		}
		
	}

	/// <summary>
	/// 控制士兵移动方向
	/// </summary>
	/// <param name="arrow">点击的箭头</param>
	private void CommandSoilder(GameObject arrow, Map map)
	{
		save_Steps.SaveCommamdSteps(map);
		//TODO:记录此步操作
	}

	/// <summary>
	/// 检测是否相邻
	/// </summary>
	/// <param name="firstMap">第一块地图块</param>
	/// <param name="secondMap">第二块地图块</param>
	/// <returns>检测结果</returns>
	private bool CheckNeighbour(Map firstMap, Map secondMap)
	{
		foreach(Map m in firstMap.NextMaps)
		{
			if (m.MapID_MapManager == secondMap.MapID_MapManager && m.MapManagerID == secondMap.MapManagerID)
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
		if (!hasAuthority)
		{
			Debug.Log("没有权限");
			return;
		}
		if (leaderPoint_current > 0)
		{
			Debug.Log("增兵");
			save_Steps.SaveAddSteps(1, mapBlock.GetComponent<Map>());
			mapBlock.GetComponent<Map>().AddSoldier();
			leaderPoint_current -= 1;
		}
	}

	private void UseCard()
	{
		//TODO:使用卡牌
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
