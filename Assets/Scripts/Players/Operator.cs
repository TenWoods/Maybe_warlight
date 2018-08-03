using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*操作进行的阶段(枚举)定义*/
public enum OperateState
{
	OP_START,
	ADD_SOLDIER,
 	COMMAND_SOLDIER, 
	USE_CARDS, 
	OP_END
};

public abstract class Operator : MonoBehaviour 
{
	/*玩家所拥有的地图格*/
	[SerializeField]//调试用
	protected List<Map> maps;
	/*玩家在GameManager处的索引(-1为错误)*/
	[SerializeField]
	protected int playerID = 0;
	/*玩家当前操作状态*/
	[SerializeField]//调试用
	protected OperateState opState;
	/*玩家兵力数(每回合实时更新)*/
	[SerializeField]
	protected int soldierNum;
	/*玩家统帅值*/
	[SerializeField]
	protected int leaderPoint;
	/*手牌最大的数量*/
	protected static int cards_Num_Max = 12;
	/*本回合抽牌的数量*/
	protected int getCardNum = 1;
	/*手牌实体*/
	protected List<Card> cardObjects;
	/*牌库*/
	protected int[] allCards = new int[cards_Num_Max];
	/*抽牌指针在牌库位置*/
	protected int cards_index = 0;
	/*每回合开始是否为操作类更新数据*/
	protected bool hasUpdated = false;
	/*游戏开始*/
	[SerializeField]//调试用
	protected bool gameStart = false;
	/*玩家操作记录*/
	protected PlayerStep steps;

	protected virtual void Awake() 
	{
		maps = new List<Map>();
		cardObjects = new List<Card>();
		steps = new PlayerStep();
		opState = OperateState.OP_START;
	}

	/// <summary>
	/// 清除上个回合的步骤
	/// </summary>
	public void CleanSteps()
	{
		steps.CleanSteps();
	}

	/// <summary>
	/// 新的回合开始更新统帅值
	/// </summary>
	public void UpdateSoldierNum()
	{
		//TODO:更改统帅值更新数值
		hasUpdated = false;
	}

	/// <summary>
	/// 变为开始状态
	/// </summary>
	public abstract void ChangeOperateStateStart();

	/// <summary>
	/// 玩家抽卡(由GameManager调用)
	/// </summary>
	public abstract void GetCard(int num);



	public List<Map> Maps 
	{
		get
		{
			return maps;
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

	public OperateState OpState 
	{
		get
		{
			return opState;
		}
		set
		{
			opState = value;
		}
	}

	public int SoldierNum
	{
		get
		{
			return soldierNum;
		}
		set
		{
			soldierNum = value;
		}
	}

	public int LeaderPoint 
	{
		get
		{
			return leaderPoint;
		}
		set
		{
			leaderPoint = value;
		}
	} 

	public bool GameStart 
	{
		set
		{
			gameStart = value;
		}
	}

	public PlayerStep Steps
	{
		get
		{
			return steps;
		}
	}

	public int GetCardNum 
	{
		get
		{
			return getCardNum;
		}
		set
		{
			getCardNum = value;
		}
	}

	public List<Card> CardObjects 
	{
		get
		{
			return cardObjects;
		}
	}
}
