﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Operator 
{
	/*玩家操作类*/
	private Operation selfOperate;
	/*卡牌的大小*/
	[SerializeField]
	private float cardSize = 1.5f;
	/*卡牌生成的位置*/
	public RectTransform CardSpawnPoint;
	/*手牌中心*/
	public RectTransform CardPointMiddle;
	/*指挥所用UI*/
	public GameObject commandUI;
	/*牌库实体*/
	public GameObject[] AllCardsObjects;
	
	protected override void Awake() 
	{
		base.Awake();
		for (int i = 0; i < cards_Num_Max; i++)
		{
			allCards[i] = i + 1;
		}
		selfOperate = this.GetComponent<Operation>();
		selfOperate.Init_Operation(this, steps);
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
	public override void ChangeOperateStateStart()
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
			ArrowManager.Instance.DisabledArrows();
			commandUI.SetActive(false);
		}
		foreach (Card c in cardObjects)
		{
			c.gameObject.GetComponent<Button>().enabled = true;
		}
	}

	/// <summary>
	/// 变为指挥状态
	/// </summary>
	public void ChangeOperateStateCommand()
	{
		opState = OperateState.COMMAND_SOLDIER;
		foreach (Card c in cardObjects)
		{
			c.gameObject.GetComponent<Button>().enabled = true;
		}
	}

	/// <summary>
	/// 变为卡片使用状态
	/// </summary>
	public void ChangeOperateStateCard()
	{
		opState = OperateState.USE_CARDS;
		if (opState == OperateState.COMMAND_SOLDIER)
		{
			ArrowManager.Instance.DisabledArrows();
			commandUI.SetActive(false);
		}
		foreach (Card c in cardObjects)
		{
			c.gameObject.GetComponent<Button>().enabled = true;
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
			ArrowManager.Instance.DisabledArrows();
			commandUI.SetActive(false);
		}
		foreach (Card c in cardObjects)
		{
			c.gameObject.GetComponent<Button>().enabled = true;
		}
	}

	#endregion

	/// <summary>
	/// 玩家抽卡(由GameManager调用)
	/// </summary>
	public override void GetCard(int num)
	{
		Debug.Log("抽牌");
		//初始化数组
		if (cardObjects == null)
		{
			cardObjects = new List<Card>();
		}
		//判断是否抽完牌
		Debug.Log(allCards.Length);
		if ((cards_index + num) > allCards.Length)
		{
			num = allCards.Length - cards_index;
			if (num == 0)
			{
				Debug.Log("牌库空了");
				return;
			}
		}
		//手牌满了
		if (cardObjects.Count == cards_Num_Max)
		{
			cards_index++;
			return;
		}
		//根据抽牌次数抽牌
		for (int i = 0; i < num; i++)
		{
			GameObject card = null;
			card = AllCardsObjects[cards_index];
			card.SetActive(true);
			card.GetComponent<Card>().inHand = true;
			cardObjects.Add(card.GetComponent<Card>());
			cards_index++;
		}
		SortCardObject();
	}

	/// <summary>
	/// 整理手牌(计算每张牌应在的位置)
	/// </summary>
	public void SortCardObject()
	{
		int half = cardObjects.Count / 2;
		Vector3 cardPos = new Vector3(CardPointMiddle.position.x - cardSize * half, CardPointMiddle.position.y, CardPointMiddle.position.z);
		//Debug.Log(cardPosY);
		int i;
		for (i = 0; i < cardObjects.Count; i++)
		{
			if (cardObjects[i].GetComponent<Card>().HasUsed)
			{
				continue;
			}
			cardObjects[i].SetCardMoveDir(cardPos);
			cardObjects[i].HandPos = cardPos;
			cardPos += new Vector3(cardSize, 0, 0);
		}
	}
}
