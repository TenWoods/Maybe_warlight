using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Operator
{
	/*选择操作的地图*/
	[SerializeField]//调试用
	private List<Map> commandMap;
	/*AI手中的卡牌*/
	private List<int> cards_in_hand;
	public bool useAI = false;
	
	protected override void Awake() 
	{
		base.Awake();
		commandMap = new List<Map>();
		cards_in_hand = new List<int>();
	}

	private void Update() 
	{
		if (!useAI)
		{
			opState = OperateState.OP_END;
			return;
		}
		if (!gameStart)
		{
			return;
		}
		Operation();
	}

	/// <summary>
	/// AI能进行的所有操作
	/// </summary>
	private void Operation()
	{
		switch (opState)
		{
			case OperateState.OP_START : 
			{
				GetAddMap();
				opState = OperateState.ADD_SOLDIER;
				break;
			}
			
			case OperateState.ADD_SOLDIER :
			{
				Attack_AI();
				break;
			}
			case OperateState.COMMAND_SOLDIER :
			{
				break;
			}
			case OperateState.USE_CARDS : 
			{
				break;
			}
			case OperateState.OP_END : break;
		}
	}

	/// <summary>
	/// 变为开始状态
	/// </summary>
	public override void ChangeOperateStateStart()
	{
		opState = OperateState.OP_START;
		//GetCard(getCardNum);
		//getCardNum = 1;   //重置每回合的抽牌数量
		//TODO:每回合刚开始的初始化
	}

	/// <summary>
	/// 抽牌
	/// </summary>
	public override void GetCard(int num)
	{
		Debug.Log("AI抽牌");
		//初始化数组
		if (cardObjects == null)
		{
			cardObjects = new List<Card>();
		}
		//判断是否抽完牌
		if ((cards_index + num) > allCards.Length)
		{
			num = (cards_index + num) - allCards.Length;
			if (num == 0)
			{
				Debug.Log("AI牌库空了");
				return;
			}
		}
		if (cardObjects.Count == cards_Num_Max)
		{
			cards_index++;
			return;
		}
		//根据抽牌次数抽牌
		for (int i = 0; i < num; i++)
		{
			cards_in_hand.Add(allCards[cards_index]);
			cards_index++;
		}
	}

	/// <summary>
	/// 获取需要指挥的地图
	/// </summary>
	private void GetAddMap() 
	{
		Debug.Log("获取地图");
		commandMap.Clear();
		List<Map> outMaps = new List<Map>();
		bool isChosen;
		//挑出最外围的地图
		foreach(Map m in maps)
		{
			isChosen = false;
			foreach(Map nm in m.NextMaps)
			{
				if (nm.PlayerID != m.PlayerID)
				{
					isChosen = true;
					break;
				}
			}
			if (isChosen)
			{
				outMaps.Add(m);
			}
		}
		//从最外侧的地图中挑选最适合攻击的地图
		int max = 0;
		int temp = 0;
		foreach(Map m in outMaps)
		{
			temp = 0;
			//寻找进攻目标
			foreach(Map nm in m.NextMaps)
			{
				if (nm.PlayerID == m.PlayerID)
				{
					continue;
				}
				if (GameManager.Instance.MapManagers[nm.MapManagerID].AddSoldierNum > temp)
				{
					temp = GameManager.Instance.MapManagers[nm.MapManagerID].AddSoldierNum;
					m.MoveDirMap.Clear();
					m.MoveDirMap.Add(nm);
				}
				else if (GameManager.Instance.MapManagers[nm.MapManagerID].AddSoldierNum == temp)
				{
					m.MoveDirMap.Add(nm);
				}
			}
			//确定用于指挥的地图
			if (temp > max)
			{
				max = temp;
				commandMap.Clear();
				commandMap.Add(m);
			}
			else if (temp == max)
			{
				commandMap.Add(m);
			}
			else
			{
				m.MoveDirMap.Clear();
			}
		}
	}

	/// <summary>
	/// 增兵同时选定进攻方向(AI)
	/// </summary>
	private void Attack_AI()
	{
		Debug.Log(soldierNum);
		float attackPower = 0;
		float defendPower = 0;
		int addNum;
		if (commandMap.Count == 1)
		{
			addNum = soldierNum;
			steps.SaveAddSteps(addNum, commandMap[0]);
			steps.SaveCommamdSteps(commandMap[0], addNum);
			opState = OperateState.OP_END;
			return;
		}
		foreach(Map m in commandMap)
		{	
			for(int i = m.MoveDirMap.Count - 1; i >= 0; i--)
			{
				addNum = 0;
				attackPower = m.AttackPower;
				defendPower = m.MoveDirMap[i].DefendPower;
				#region 获取攻击力
				switch (m.Terrain)
				{
					case Terrain.HIGHLAND :
					{
						if (m.MoveDirMap[i].Terrain != Terrain.HIGHLAND)
						{
							attackPower += 0.2f;
						}
						break;
					}
					case Terrain.FOREST :
					{
						if (m.MoveDirMap[i].Terrain == Terrain.VALLY)
						{
							attackPower += 0.2f;
						}
						else if (m.MoveDirMap[i].Terrain == Terrain.HIGHLAND)
						{
							attackPower -= 0.2f;
						}
						attackPower += 0.1f; 
						break;
					} 
					case Terrain.DESERT : 
					{
						if (m.MoveDirMap[i].Terrain == Terrain.VALLY)
						{
							attackPower += 0.2f;
						}
						else if (m.MoveDirMap[i].Terrain == Terrain.HIGHLAND)
						{
							attackPower -= 0.2f;
						}
						attackPower -= 0.1f; 
						break;
					}
					case Terrain.FLATLAND :
					{
						if (m.MoveDirMap[i].Terrain == Terrain.VALLY)
						{
							attackPower += 0.2f;
						}
						else if (m.MoveDirMap[i].Terrain == Terrain.HIGHLAND)
						{
							attackPower -= 0.2f;
						}
						break;
					}
					case Terrain.VALLY : 
					{
						if (m.MoveDirMap[i].Terrain != Terrain.VALLY)
						{
							attackPower -= 0.2f;
						}
						break;
					}
					default : break;
				}
				#endregion
				#region 获取防御力
				if (m.MoveDirMap[i].Terrain != Terrain.DESERT)
				{
					defendPower += 0.1f;
				}
				#endregion
				//所需兵力
				addNum = (int)(m.MoveDirMap[i].BaseSoldierNum * defendPower / attackPower + 0.5f);
				addNum += 1; //剩下一个保留的在原地图
				//TODO:测试
				if ((soldierNum - addNum) <= 0)
				{
					//Debug.Log("数量不够");
					m.MoveDirMap.RemoveAt(i);

				}
				else
				{
					soldierNum -= addNum;
					Debug.Log("AI增兵" + m.gameObject.name + addNum);
					for (int j = 0; j < addNum; j++)
					{
						m.AddSoldier();
					}
					steps.SaveAddSteps(addNum, m);
					steps.SaveCommamdSteps(m, addNum);
				}
			}
		}
		//结束回合
		opState = OperateState.OP_END;
	}
}
