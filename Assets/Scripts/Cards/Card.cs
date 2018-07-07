using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardOpKind
{
	SingleMap,
	MapArea,
	EffectPlayer
};

public class Card : MonoBehaviour 
{
	/*使用玩家编号*/
	protected int playerID;
	/*卡牌的操作类型*/
	protected CardOpKind opKind;
	/*卡牌移动信号*/
	private bool timeToGo = false;
	/*卡牌移动目标*/
	private Vector3 destination;
	/*卡牌移动速度*/
	[SerializeField]
	private float moveSpeed = 1;
	

	private void Update() 
	{
		if (!timeToGo)
		{
			return;
		}
		if ((transform.position - destination).magnitude >= 0.1)
		{
			Vector3.Lerp(transform.position, destination, Time.deltaTime * moveSpeed);
			return;
		}
	}
	
	/// <summary>
	/// 卡牌效果   指定地图
	/// </summary>
	/// <param name="player">选中的玩家</param>
	/// <param name="map">选中的地图</param>
	public void CardEffect(Player player, Map map)
	{
		//TODO:卡牌的操作
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="player"></param>
	public void CardEffect(Player player)
	{
		//TODO:卡牌的操作
	}

	/// <summary>
	/// 设置卡牌移动方向并开启移动
	/// </summary>
	/// <param name="destination"></param>
	public void SetCardMoveDir(Vector3 des)
	{
		destination = des;
		timeToGo = true;
	}	

	public CardOpKind OpKind
	{
		get
		{
			return opKind;
		}
	}
}
