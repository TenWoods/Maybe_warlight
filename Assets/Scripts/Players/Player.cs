using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Operator 
{
	/*玩家操作类*/
	private Operation selfOperate;
	/*卡牌的大小*/
	[SerializeField]
	private float cardSize = 1.5f;
	/*卡牌生成的位置*/
	public Transform CardSpawnPoint;
	
	private void Start() 
	{
		base.Start();
		selfOperate = new Operation(this, steps);
	}

	private void Update() 
	{
		if (!gameStart)
		{
			return;
		}
		if (opState != OperateState.OP_END)
		{
			if (!hasUpdated)
			{
				selfOperate.UpdateData(this);
				hasUpdated = true;
			}
			selfOperate.Operate(this, opState);
		}
	}

	#region 玩家操作状态改变
	
	/// <summary>
	/// 变为开始状态
	/// </summary>
	public void ChangeOperateStateStart()
	{
		opState = OperateState.OP_START;
		GetCard(getCardNum);
		getCardNum = 1;   //重置每回合的抽牌数量
		//TODO:每回合刚开始的初始化
	}

	/// <summary>
	/// 变为增兵状态
	/// </summary>
	public void ChangeOperateStateAdd()
	{
		opState = OperateState.ADD_SOLDIER;
		if (opState == OperateState.COMMAND_SOLDIER)
		{
			DisabledArrows();
			commandUI.SetActive(false);
		}
	}

	/// <summary>
	/// 变为指挥状态
	/// </summary>
	public void ChangeOperateStateCommand()
	{
		opState = OperateState.COMMAND_SOLDIER;
	}

	/// <summary>
	/// 变为卡片使用状态
	/// </summary>
	public void ChangeOperateStateCard()
	{
		opState = OperateState.USE_CARDS;
		if (opState == OperateState.COMMAND_SOLDIER)
		{
			DisabledArrows();
			commandUI.SetActive(false);
		}
	}
	
	/// <summary>
	/// 变为回合结束
	/// </summary>
	public void ChangeOperateStateEnd()
	{
		opState = OperateState.OP_END;
		if (opState == OperateState.COMMAND_SOLDIER)
		{
			DisabledArrows();
			commandUI.SetActive(false);
		}
	}

	#endregion

	/// <summary>
	/// 新的回合开始更新统帅值
	/// </summary>
	public void UpdateLeaderPoint()
	{
		soldierNum = maps.Count;
		hasUpdated = false;
	}

	/// <summary>
	/// 清除上个回合的步骤
	/// </summary>
	public void CleanSteps()
	{
		steps.CleanSteps();
	}

	/// <summary>
	/// 玩家抽卡(由GameManager调用)
	/// </summary>
	public void GetCard(int num)
	{
		Debug.Log("抽牌");
		//初始化数组
		if (cardObjects == null)
		{
			cardObjects = new List<Card>();
		}
		//判断是否抽完牌
		if ((cards_index + 1) > allCards.Length)
		{
			Debug.Log("牌库空了");
			return;
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
			GameObject card = Instantiate((GameObject)Resources.Load("Card"), CardSpawnPoint.position, CardSpawnPoint.rotation);
			cardObjects.Add(card.GetComponent<Card>());
		}
		SortCardObject();
	}

	/// <summary>
	/// 整理手牌(计算每张牌应在的位置)
	/// </summary>
	public void SortCardObject()
	{
		int half = cardObjects.Count / 2;
		Vector3 screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		Vector3 cardPos = new Vector3(-half * cardSize, -(screenPoint.y * 11 / 13), 0);
		//Debug.Log(cardPosY);
		int i;
		for (i = 0; i < cardObjects.Count; i++)
		{
			cardObjects[i].SetCardMoveDir(cardPos);
			cardPos += new Vector3(cardSize, 0, 0);
		}
	}

	/// <summary>
	/// 让操作类隐藏箭头
	/// </summary>
	public void DisabledArrows()
	{
		selfOperate.DisabledArrows();
	}
}
