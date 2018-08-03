using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Operation : MonoBehaviour
{
	/*执行操作的玩家ID*/
	private int playerID;
	/*目前玩家处于的操作阶段*/
	private OperateState state;
	/*分配阶、出牌段选中地图块*/
	private GameObject clickMap;
	/*卡牌使用阶段选择的卡牌*/
	private GameObject clickCard;
	/*增兵阶段的目前士兵数*/
	private int soldierNum_All;
	private int soldierNum_current;
	/*卡牌使用阶段统帅值*/
	private int leaderPoint_All;
	private int leaderPoint_current;
	/*记录玩家行为步骤*/
	public PlayerStep save_Steps;
	/*箭头预制体*/
	private GameObject arrow;
	/*获取单步派兵数量(由UI控制)*/
	private int soldierNum;
	/*指挥所用UI*/
	private GameObject commandUI;
	/*地图数据显示UI*/
	public GameObject dataUI;
	public Text attckText;
	public Text defendText; 
	/*士兵数UI*/
	public Text soldierNumUI;
	/*统帅值UI*/
	public Text leaderPointUI;
	//选择进阶卡
	public bool getUpCard = false;

	/// <summary>
	/// 操作类的构造函数
	/// </summary>
	/// <param name="player">所属玩家</param>
	public void Init_Operation(Player player, PlayerStep steps)
	{
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
		leaderPoint_All = player.LeaderPoint;
		soldierNum_All = player.SoldierNum;
		soldierNumUI.text = "增兵  " + soldierNum_current.ToString() + "/" + soldierNum_All;
		leaderPointUI.text = "统率值: " + leaderPoint_All.ToString();
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
						if (hitInfo.collider.tag == "Map" && !commandUI.activeSelf)
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
					default: break;
				}
			}
		}
		//数据显示
		if (Input.GetMouseButtonDown(1))
		{
			Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hitInfo = Physics2D.Raycast(mousPos, Vector2.zero);
			if (hitInfo.collider != null)
			{
				if (hitInfo.collider.tag == "Map")
				{
					dataUI.SetActive(true);
					Map map = hitInfo.collider.gameObject.GetComponent<Map>();
					dataUI.transform.position = map.flagUI.transform.position + new Vector3(2, 0, 0);
					attckText.text = "攻击力:" + map.AttackPower.ToString();
					defendText.text = "防御力:" + map.DefendPower.ToString();
				}
			}
			else
			{
				dataUI.SetActive(false);
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
			soldierNumUI.text = "增兵  " + soldierNum_current.ToString() + "/" + soldierNum_All;
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
			for (int i = 0; i < ArrowManager.Instance.Arrows_Able.Count; i++)
			{
				if (ArrowManager.Instance.Arrows_Remain.Contains(ArrowManager.Instance.Arrows_Able[i]))
				{
					continue;
				}
				ArrowManager.Instance.Arrows_Able[i].SetActive(false);
			}
		}
		clickMap = mapBlock;
		Debug.Log("选择地图块");
		Map mapBlockData = clickMap.GetComponent<Map>();
		//生成箭头
		if (mapBlockData.Arrows_Red.Count == 0)
		{
			GameObject arrow = null;
			for (int i = 0; i < mapBlockData.NextMaps.Count; i++)
			{
				arrow = ArrowManager.Instance.InitArrow(mapBlockData.transform.position, mapBlockData.NextMaps[i].transform.position, 0);
				mapBlockData.Arrows_Red.Add(arrow);
				arrow = ArrowManager.Instance.InitArrow(mapBlockData.transform.position, mapBlockData.NextMaps[i].transform.position, 1);
				mapBlockData.Arrows_Green.Add(arrow);
			}
		}
		//显示箭头
		for (int i = 0; i < mapBlockData.NextMaps.Count; i++)
		{
			if (mapBlockData.NextMaps[i].PlayerID != mapBlockData.PlayerID)
			{
				mapBlockData.Arrows_Red[i].SetActive(true);
				ArrowManager.Instance.Arrows_Able.Add(mapBlockData.Arrows_Red[i]);
			}
			else
			{
				mapBlockData.Arrows_Green[i].SetActive(true);
				ArrowManager.Instance.Arrows_Able.Add(mapBlockData.Arrows_Green[i]);
			}
		}
	}

	/// <summary>
	/// 显示指挥UI
	/// </summary>
	/// <param name="arrow">点击的箭头</param>
	private void CommandSoilderUI(GameObject arrow, Map startMap)
	{
		int index;
		if (startMap.Arrows_Green.Contains(arrow))
		{
			index = startMap.Arrows_Green.IndexOf(arrow);
		}
		else
		{
			index = startMap.Arrows_Red.IndexOf(arrow);
		}
		commandUI.SetActive(true);
		//把步骤储存交给UI控制类
		commandUI.GetComponent<CommandUIUpdate>().SetCommandUI(startMap.BaseSoldierNum, startMap, startMap.NextMaps[index], save_Steps, arrow);
	}

	#endregion

	#region 使用卡牌阶段

	/// <summary>
	/// 选择使用的卡牌
	/// </summary>
	/// <param name="card">选中的卡牌</param>
	public void ChooseCard(GameObject card)
	{
		if (state != OperateState.USE_CARDS)
		{
			return;
		}
		if (getUpCard)
		{
			return;
		}
		if (!card.GetComponent<Card>().inHand)
		{
			return;
		}
		if (clickCard == card)
		{
			return;
		}
		if (clickCard != null)
		{
			clickCard.GetComponent<Card>().CancelClick();
		} 
		clickCard = card;
		clickCard.GetComponent<Card>().Click(this);
	}

	public void UpdateLeaderPointUI()
	{
		leaderPointUI.text = "统率值: " + leaderPoint_current.ToString();
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

	public GameObject ClickCard 
	{
		set
		{
			clickCard = value;
		}
	}

	public int LeaderPoint 
	{
		get
		{
			return leaderPoint_current;
		}
		set
		{
			leaderPoint_current = value;
		}
	}
}
