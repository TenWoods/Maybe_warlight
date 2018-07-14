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

public class Operator : MonoBehaviour 
{
	/*玩家所拥有的地图格*/
	[SerializeField]//调试用
	protected List<Map> maps;
	/*玩家在GameManager处的索引(-1为错误)*/
	[SerializeField]
	protected int playerID = 0;
	/*玩家当前操作*/
	[SerializeField]//调试用
	protected OperateState opState;
	/*玩家当前是否操作结束*/
	[SerializeField]//调试用
	protected bool operateEnd = true;
	/*玩家操作类*/
	protected Operation selfOperate;
	/*玩家兵力数(每回合实时更新)*/
	[SerializeField] //设定初始值
	protected int soldierNum;
	/*手牌最大的数量*/
	[SerializeField]
	protected int cards_Num_Max = 0;
	/*本回合抽牌的数量*/
	private int getCardNum = 1;
	/*手中的卡牌,-1为空*/
	protected List<int> cards_in_hand;
	/*牌库*/
	protected int[] cards = {0};
	/*抽牌指针在牌库位置*/
	protected int cards_index = 0;
	/*每回合开始是否为操作类更新数据*/
	protected bool hasUpdated = false;
	/*游戏开始*/
	protected bool gameStart = false;
	/*玩家操作记录*/
	protected PlayerStep steps;
	/*需要生成的箭头*/
	public GameObject arrow_Prefab;
	/*指挥所用UI*/
	public GameObject commandUI;

	
	public List<Map> Maps 
	{
		get
		{
			return maps;
		}
	}

	public bool OperateEnd 
	{
		set 
		{
			operateEnd = value;
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
}
