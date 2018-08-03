using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_23 : Card 
{
	//进阶的牌
	[SerializeField]
	private Card upgradeCard = null;
	[SerializeField]
	private bool getCard = false;

	public override void CardEffect()
	{
		Debug.Log("UP");
		getCard = true;
		playerOP.getUpCard = true;
		if (upgradeCard == null)
		{
			return;
		}
		upgradeCard.UpGrade();
		playerOP.LeaderPoint -= leaderPoint;
		playerOP.UpdateLeaderPointUI();
		hasUsed = true;
	}

	public void Choose(Card card)
	{
		if (!getCard)
		{
			return;
		}
		upgradeCard = card;
		playerOP.getUpCard = false;
	}
}
