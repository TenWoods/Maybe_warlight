using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_18 : Card 
{
	//增加攻击点数
	[SerializeField]
	private float attackPower = 0.4f;
	//增加防御点数
	[SerializeField]
	private float defendPower = 0.4f;
	//持续回合数
	[SerializeField]
	private int turn = 1;
	private bool isAlone = true;

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
					foreach (Map m in targetMap.NextMaps)
					{
						if (m.PlayerID == targetMap.PlayerID)
						{
							isAlone = false;
							break;
						}
					}
					if (isAlone)
					{
						targetMap.AttackPower += attackPower;
						targetMap.DefendPower += defendPower;
						targetMap.all_buff.Add(new Buff(turn, attackPower, defendPower));
						//在gamemanager处注册
						GameManager.Instance.buffMap.Add(targetMap);
					}
					else
					{
						Debug.Log("Add");
						if (attackPower == 0.5f)
						{
							targetMap.AttackPower += 0.3f;
							targetMap.DefendPower += 0.3f;
						}
						else 
						{
							targetMap.AttackPower += (attackPower / 2.0f);
							targetMap.DefendPower += (defendPower / 2.0f);
						}
						targetMap.all_buff.Add(new Buff(turn, (int)(attackPower / 2.0f + 0.5), (int)(defendPower / 2.0f + 0.5)));
						//在gamemanager处注册
						GameManager.Instance.buffMap.Add(targetMap);
					}
					playerOP.LeaderPoint -= leaderPoint;
					playerOP.UpdateLeaderPointUI();
					hasUsed = true;
				}
			}
		}
	}
	public override void UpGrade()
	{
		defendPower = 0.5f;
		leaderPoint = 5;
		upgrade = true;
	}
}
