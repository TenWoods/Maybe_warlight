using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_7 : Card 
{
	//抽牌数量
	[SerializeField]
	private int getCardNum = 3;

	public override void CardEffect()
	{
		hasUsed = true;
		GameManager.Instance.Players[playerID].GetCard(getCardNum);
		playerOP.LeaderPoint -= leaderPoint;
        playerOP.UpdateLeaderPointUI();
	}

	public override void UpGrade()
	{
		getCardNum = 4;
		leaderPoint = 7;
		upgrade = true;
	}
}
