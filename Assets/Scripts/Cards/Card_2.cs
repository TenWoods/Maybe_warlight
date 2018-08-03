using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_2 : Card 
{
    [SerializeField]
    private int addNum = 1;

	public override void CardEffect()
    {
        Debug.Log("武装");
        foreach (Map m in GameManager.Instance.Players[playerID].Maps)
        {
            m.BaseSoldierNum += addNum;
            m.UpdateMapUI();
        }
        playerOP.LeaderPoint -= leaderPoint;
        playerOP.UpdateLeaderPointUI();
		hasUsed = true;
    }

    
	public override void UpGrade()
	{
		addNum = 2;
		leaderPoint = 8;
        upgrade = true;
	}
}
