using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Operator
{
	/*选择操作的地图*/
	[SerializeField]//调试用
	private List<Map> commandMap;
	
	private void Start() 
	{
		base.Start();
		commandMap = new List<Map>();
	}

	private void Update() 
	{
		if (!gameStart)
		{
			return;
		}
		
	}

	/// <summary>
	/// AI能进行的所有操作
	/// </summary>
	private void Operation()
	{

		switch (opState)
		{
			case OperateState.OP_START : break;
			case OperateState.ADD_SOLDIER :
			{
				ADD_AI();
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
		GetCard(getCardNum);
		getCardNum = 1;   //重置每回合的抽牌数量
		GetAddMap();
		//TODO:每回合刚开始的初始化
	}

	/// <summary>
	/// 变为开始状态
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
			cards_index++;
			GameObject card = Instantiate((GameObject)Resources.Load("Card"), this.transform.position, this.transform.rotation);
			card.SetActive(false);
			cardObjects.Add(card.GetComponent<Card>());
		}
	}

	/// <summary>
	/// 获取需要指挥的地图
	/// </summary>
	private void GetAddMap() 
	{
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
			foreach(Map nm in m.NextMaps)
			{
				if (GameManager.Instance.MapManagers[nm.MapManagerID].AddSoldierNum > temp)
				{
					temp = GameManager.Instance.MapManagers[nm.MapManagerID].AddSoldierNum;
				}
			}
			if (temp > max)
			{
				max = temp;
				commandMap.Clear();
				commandMap.Add(m);
				Debug.Log(m.gameObject.name);
			}
			else if (temp == max)
			{
				Debug.Log(m.gameObject.name + "==");
				commandMap.Add(m);
			}
		}
		Debug.Log(max);
	}

	/// <summary>
	/// 增兵(AI)
	/// </summary>
	private void ADD_AI()
	{
		float attackPower = 0;
		float defendPower = 0;
		foreach(Map m in commandMap)
		{
			attackPower = m.AttackPower;
			foreach(Map nm in m.NextMaps)
			{
				
			}
		}
	}

	/// <summary>
	/// 指挥(AI)
	/// </summary>
	private void COMMAND_AI()
	{

	}

	/// <summary>
	/// 出牌(AI)
	/// </summary>
	private void CARD_AI()
	{

	}
}
