using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardOpKind
{
	SingleMap,
	MultiMap,
	MapArea
};

public class Card : MonoBehaviour 
{
	/*使用玩家编号*/
	protected int playerID;
	/*卡牌的操作类型*/
	[SerializeField]
	protected CardOpKind opKind;
	/*卡牌移动信号*/
	[SerializeField]
	private bool timeToGo = false;
	/*卡牌移动目标*/
	[SerializeField]
	private Vector3 destination;
	/*卡牌移动速度*/
	[SerializeField]
	private float moveSpeed = 1;
	/*卡牌使用信号*/
	private bool hasUsed = false;

	private void Update() 
	{
		if (!timeToGo)
		{
			return;
		}
		if ((transform.position - destination).magnitude >= 0.1)
		{
			transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * moveSpeed);
			return;
		}
		if (!hasUsed)
		{
			return;
		}
		Destroy(this.gameObject);
	}
	
	/// <summary>
	/// 卡牌效果   指定地图
	/// </summary>
	/// <param name="player">选中的玩家</param>
	/// <param name="map">选中的地图</param>
	public void CardEffect(int playerID, GameObject target)
	{
		//TODO:卡牌的操作
	}

	/// <summary>
	/// 卡牌效果   
	/// </summary>
	/// <param name="player"></param>
	public void CardEffect(int playerID, GameObject[] targets)
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

	public bool HasUsed
	{
		get
		{
			return hasUsed;
		}
		set
		{
			hasUsed = value;
		}
	}

}
