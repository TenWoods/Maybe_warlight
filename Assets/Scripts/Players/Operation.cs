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
	/*选中单个目标*/
	private GameObject singleObject = null;
	/*选中多个目标*/
	private GameObject[] multiObject = null;
	/*增兵阶段的目前士兵数*/
	private int soldierNum_current;
	/*卡牌使用阶段统帅值*/
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
		soldierNum_current = player.SoldierNum;
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
					//增兵
					case OperateState.ADD_SOLDIER:
					{
						if (hitInfo.collider.tag == "Map")
						{
							AddMapSoldierNum(hitInfo.collider.gameObject);
						}
						break;
					}
					//指挥士兵
					case OperateState.COMMAND_SOLDIER:
					{
						if (hitInfo.collider.tag == "Map")
						{
							commandUI.SetActive(false);
							DrawArrows(hitInfo.collider.gameObject);	
						}
						if (hitInfo.collider.tag == "Arrow")
						{
							CommandSoilderUI(hitInfo.collider.gameObject, clickMap.GetComponent<Map>());
						}
						break;
					}
					//使用卡牌
					case OperateState.USE_CARDS:
					{
						if (hitInfo.collider.tag == "Card")
						{
							ChooseCard(hitInfo.collider.gameObject);
							break;
						}
						if (clickCard == null)
						{
							break;
						}
						ChooseTarget(hitInfo.collider.gameObject);
						break;
					}
					default: break;
				}
			}
			else
			{
				if (state == OperateState.USE_CARDS)
				{
					if (clickCard != null)
					{
						clickCard.transform.localScale /= 2;    //还原卡牌大小
						clickCard.GetComponent<SpriteRenderer>().sortingOrder = 1;  //还原卡片层级
						clickCard = null;    //清空选中卡牌
					}
				}
			}
		}
	}

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
			Debug.Log("没有权限");
			return;
		}
		if (soldierNum_current > 0)
		{
			Debug.Log("增兵");
			save_Steps.SaveAddSteps(1, mapBlock.GetComponent<Map>());
			mapBlock.GetComponent<Map>().AddSoldier();
			soldierNum_current -= 1;
		}
	}

	#endregion
	
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
			Debug.Log("没有权限");
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
		//显示箭头
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
		commandUI.GetComponent<CommandUIUpdate>().SetCommandUI(startMap.BaseSoldierNum, startMap, startMap.NextMaps[index], save_Steps, arrow);
	}

	/// <summary>
	/// 隐藏显示的箭头
	/// </summary>
	public void DisabledArrows()
	{
		int i;
		Map arrowMap = clickMap.GetComponent<Map>();
		for (i = 0; i < arrowMap.Arrows.Count; i++)
		{
			if (save_Steps.Arrows.Contains(arrowMap.Arrows[i]))
			{
				continue;
			}
			arrowMap.Arrows[i].SetActive(false);
		}
	}

	#endregion

	#region 使用卡牌阶段

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
			clickCard.transform.localScale /= 2;    //还原卡牌大小
			clickCard.GetComponent<SpriteRenderer>().sortingOrder = 1;  //还原卡片层级
		} 
		clickCard = card;
		Debug.Log(clickCard.name);
		opKind = clickCard.GetComponent<Card>().OpKind;
		clickCard.transform.localScale *= 2;
		clickCard.GetComponent<SpriteRenderer>().sortingOrder = 2; //将卡片置顶
	}

	/// <summary>
	/// 选择卡牌作用目标
	/// </summary>
	private void ChooseTarget(GameObject target)
	{
		Card card = clickCard.GetComponent<Card>();
		switch (opKind)
		{
			case CardOpKind.SingleMap : 
			case CardOpKind.MapArea :
			{
				singleObject = target;
				clickCard.transform.localScale /= 2;   //还原卡牌大小
				clickCard.GetComponent<SpriteRenderer>().sortingOrder = 1;  //还原卡片层级
				if(leaderPoint_current - card.LeaderPoint <= 0)
				{
					Debug.Log("统帅值不足");
					return;
				}
				if (card.CardEffect(playerID, singleObject))
				{
					card.SetCardMoveDir(singleObject.transform.position);
					card.HasUsed = true;
					singleObject = null;
					GameManager.Instance.Players[playerID].CardObjects.Remove(clickCard.GetComponent<Card>());
				}
				break;
			}
			case CardOpKind.MultiMap : 
			{
				if (multiObject == null)
				{
					Debug.Log("选择第一个");
					multiObject = new GameObject[2];
					multiObject[0] = target;
					return;
				}
				Debug.Log("选择第二个");
				multiObject[1] = target;
				clickCard.transform.localScale /= 2;   //还原卡牌大小
				clickCard.GetComponent<SpriteRenderer>().sortingOrder = 1;  //还原卡片层级
				if(leaderPoint_current - card.LeaderPoint <= 0)
				{
					Debug.Log("统帅值不足");
					return;
				}
				if (card.CardEffect(playerID, multiObject))
				{
					card.SetCardMoveDir((multiObject[0].transform.position + multiObject[1].transform.position) / 2);
					card.HasUsed = true;
					multiObject = null;
					GameManager.Instance.Players[playerID].CardObjects.Remove(clickCard.GetComponent<Card>());
				}				
				break;
			}
		}
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
