using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_5 : Card 
{
	//增加防御点数
	[SerializeField]
	private float defendPower = 0.25f;
	//持续回合数
	[SerializeField]
	private int turn = 4;

	public override void CardEffect()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hitInfo = Physics2D.Raycast(mousPos, Vector2.zero);
			if (hitInfo.collider != null)
			{
				if (hitInfo.collider.tag == "Map")
				{
					Map targetMap = hitInfo.collider.GetComponent<Map>();
					if (targetMap.PlayerID != playerID)
					{
						Debug.Log("对象错误(卡牌)");
						return;
					}
					targetMap.DefendPower += defendPower;
					targetMap.all_buff.Add(new Buff(turn, 0, defendPower));
					//在gamemanager处注册
					GameManager.Instance.buffMap.Add(targetMap);
					playerOP.LeaderPoint -= leaderPoint;
					playerOP.UpdateLeaderPointUI();
					hasUsed = true;
				}
			}
		}
	}
	public override void UpGrade()
	{
		defendPower = 0.35f;
		leaderPoint = 3;
		upgrade = true;
	}
}
