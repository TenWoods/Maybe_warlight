using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Operation
{
	/*执行操作的玩家ID*/
	private int playerID;
	/*目前玩家处于的操作阶段*/
	private OperateState state;
	/*分配阶、出牌段选中地图块*/
	private GameObject clickMap;
	/*卡牌使用阶段选择的卡牌*/
	private GameObject clickCard;
	/*选中卡牌的操作类型*/
	private CardOpKind opKind;
	/*增兵阶段的目前统率值*/
	private int leaderPoint_current;
	/*记录玩家行为步骤*/
	private PlayerStep save_Steps;
	/*箭头预制体*/
	private GameObject arrow;
	/*获取单步派兵数量(由UI控制)*/
	private int soldierNum;
	/*指挥所用UI*/
	private GameObject commandUI;

	/// <summary>
	/// 操作类的构造函数
	/// </summary>
	/// <param name="player">所属玩家</param>
	public Operation(Player player, PlayerStep steps)
	{
		arrow = player.arrow_Prefab;
		playerID = player.PlayerID;
		state = player.OpState;
		commandUI = player.commandUI;
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
		Click();
	}

	/// <summary>
	/// 对鼠标点击事件处理
	/// </summary>
	private void Click()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hitInfo = Physics2D.Raycast(mousPos, Vector2.zero);
			if (hitInfo.collider != null)
			{
				//TODO:每个case需要做的事
				switch(state)
				{
					case OperateState.COMMAND_SOLDIER:
					{
						if (hitInfo.collider.tag == "Map")
						{
							Debug.Log("选中");
							commandUI.SetActive(false);
							DrawArrows(hitInfo.collider.gameObject);	
						}
						if (hitInfo.collider.tag == "Arrow")
						{
							Debug.Log("指挥");
							//TODO:改为显示UI
							CommandSoilderUI(hitInfo.collider.gameObject, clickMap.GetComponent<Map>());
						}
						break;
					}
					case OperateState.ADD_SOLDIER:
					{
						if (hitInfo.collider.tag == "Map")
						{
							Debug.Log("增兵");
							AddMapSoldierNum(hitInfo.collider.gameObject);
						}
						break;
					}
					case OperateState.USE_CARDS:
					{
						if (hitInfo.collider.tag == "Card")
						{
							Debug.Log("出牌");
							ChooseCard(hitInfo.collider.gameObject);
						}
						if (clickCard == null)
						{
							break;
						}
						
						break;
					}
					default: break;
				}
			}
		}
	}
	
	#region 指挥阶段

	/// <summary>
	/// 画箭头，显示箭头
	/// </summary>
	/// <param name="mapBlock">地图块</param>
	private void DrawArrows(GameObject mapBlock)
	{
		bool hasAuthority = mapBlock.GetComponent<Map>().CheckAuthority(playerID);
		if (!hasAuthority)
		{
			//没有权限
			return;
		}
		if (clickMap != null)
		{
			for (int i = 0; i < clickMap.GetComponent<Map>().Arrows.Count; i++)
			{
				clickMap.GetComponent<Map>().Arrows[i].SetActive(false);
			}
		}
		clickMap = mapBlock;
		Debug.Log("选择地图块");
		Map mapBlockData = clickMap.GetComponent<Map>();
		//生成箭头
		if (mapBlockData.Arrows.Count == 0)
		{
			Vector3 nextPos;  //相邻方块位置
			Vector3 dir;   //箭头方向
			Vector3 pos;   //箭头的位置
			float angle;
			for (int i = 0; i < mapBlockData.NextMaps.Count; i++)
			{
				Debug.Log("画了个箭头");
				//TODO:箭头大小缩放
				nextPos = mapBlockData.NextMaps[i].transform.position;
				dir = nextPos - mapBlock.transform.position;
				angle = Vector3.Angle(Vector3.right, dir);
				if (Vector3.Cross(Vector3.right, dir).z < 0)  //修正旋转的方向 
				{
					angle *= -1;
				} 
				pos = (nextPos + mapBlock.transform.position) / 2;
				mapBlockData.Arrows.Add(GameObject.Instantiate(arrow, pos, Quaternion.Euler(0, 0, angle)));
			}
		}
		//画箭头
		for (int i = 0; i < mapBlockData.Arrows.Count; i++)
		{
			mapBlockData.Arrows[i].SetActive(true);
		}
	}

	/// <summary>
	/// 显示指挥UI
	/// </summary>
	/// <param name="arrow">点击的箭头</param>
	private void CommandSoilderUI(GameObject arrow, Map startMap)
	{
		int index = startMap.Arrows.IndexOf(arrow);
		commandUI.SetActive(true);
		//把步骤储存交给UI控制类
		commandUI.GetComponent<CommandUIUpdate>().SetCommandUI(startMap.BaseSoldierNum, startMap, startMap.NextMaps[index], save_Steps);
	}

	/// <summary>
	/// 隐藏显示的箭头
	/// </summary>
	public void DisabledArrows()
	{
		for (int i = 0; i < clickMap.GetComponent<Map>().Arrows.Count; i++)
		{
			clickMap.GetComponent<Map>().Arrows[i].SetActive(false);
		}
	}

	#endregion

	#region 增兵阶段

	/// <summary>
	/// 增加选定地图块的兵力
	/// </summary>
	/// <param name="mapBlock">地图块</param>
	private void AddMapSoldierNum(GameObject mapBlock)
	{
		bool hasAuthority = mapBlock.GetComponent<Map>().CheckAuthority(playerID);
		if (!hasAuthority)
		{
			//没有权限
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

	#endregion

	#region 使用卡牌阶段

	/// <summary>
	/// 选择卡牌作用目标
	/// </summary>
	private void ChooseTarget()
	{
		switch (opKind)
		{
			case CardOpKind.SingleMap : break;
			case CardOpKind.MapArea : break;
			case CardOpKind.EffectPlayer : break;
		}
	}

	/// <summary>
	/// 选择使用的卡牌
	/// </summary>
	/// <param name="card">选中的卡牌</param>
	private void ChooseCard(GameObject card)
	{
		if (clickCard == card)
		{
			return;
		}
		if (clickCard != null)
		{
			Debug.Log("Small1");
			clickCard.transform.localScale /= 2;
			clickCard.GetComponent<SpriteRenderer>().sortingOrder = 1;  //还原卡片层级
		} 
		clickCard = card;
		opKind = clickCard.GetComponent<Card>().OpKind;
		clickCard.transform.localScale *= 2;
		clickCard.GetComponent<SpriteRenderer>().sortingOrder = 2; //将卡片置顶
		//TODO:使用卡牌
	}

	/// <summary>
	/// 选择卡牌效果作用地图块
	/// </summary>
	/// <param name="targetMap"></param>
	private void ChooseTargetMap(GameObject targetMap)
	{
		if (clickCard == null)
		{
			return;
		}
		clickMap = targetMap;
		//TODO:调用卡牌类里的卡牌移动方法
		clickCard.transform.localScale /= 2;
		clickCard.GetComponent<SpriteRenderer>().sortingOrder = 1;
	}

	#endregion

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
