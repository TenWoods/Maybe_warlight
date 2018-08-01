using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	/*是否进阶*/
	protected bool upgrade = false;
	/*所需统帅值*/
	[SerializeField]//调试用
	protected int leaderPoint = 1;
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
	protected bool hasUsed = false;
	/*卡牌跟随鼠标移动*/
	private bool moveWithMouse = false;
	protected bool effective = false;
	private RectTransform rt;
	private Vector3 startPos;
	private Vector3 handPos = Vector3.zero;
	private Operation playerOP;

	private void Start() 
	{
		rt = GetComponent<RectTransform>();
		startPos = rt.position;
	}

	private void Update() 
	{
		//自动移动
		if (timeToGo)
		{
			if ((transform.position - destination).magnitude >= 0.1)
			{
				transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
				return;
			}
			timeToGo = false;
			return;
		}
		//玩家操控移动
		if (moveWithMouse)
		{
			//左键取消选中
			if (Input.GetMouseButton(1))
			{
				CancelClick();
				playerOP.ClickCard = null;
				return;
			}
			rt.position = Input.mousePosition;
			//出牌
			Debug.Log(rt.position.y);
			Debug.Log(Screen.height / 2);
			if (rt.position.y > Screen.height / 2)
			{
				if (playerOP.LeaderPoint - leaderPoint < 0)
				{
					Debug.Log("Use");
					CancelClick();
				}
				else
				{
					moveWithMouse = false;
					rt.position = startPos;
					rt.localScale /= 2;	
					effective = true;
				}
			}
		}
		if (effective)
		{
			CardEffect();
		}
		if (!hasUsed)
		{
			return;
		}
		rt.position = startPos;
		this.gameObject.SetActive(false);
	}

	public virtual void CardEffect()
	{

	}

	/// <summary>
	/// 卡牌效果   指定地图
	/// </summary>
	/// <param name="player">选中的玩家</param>
	/// <param name="map">选中的地图</param>
	public virtual bool CardEffect(int playerID, GameObject target)
	{
		return true;
		//TODO:卡牌的操作
	}

	/// <summary>
	/// 卡牌效果   
	/// </summary>
	/// <param name="player"></param>
	public virtual bool CardEffect(int playerID, GameObject[] targets)
	{
		return true;
		//TODO:卡牌的操作
	}

	/// <summary>
	/// 卡牌进阶
	/// </summary>
	public virtual void UpGrade()
	{

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

	/// <summary>
	/// 点击卡牌
	/// </summary>
	/// <param name="op"></param>
	public void Click(Operation op)
	{
		playerOP = op;
		moveWithMouse = true;
	}

	/// <summary>
	/// 取消选中
	/// </summary>
	public void CancelClick()
	{
		moveWithMouse = false;
		rt.position = handPos;
		rt.localScale /= 2;
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

	public int LeaderPoint
	{
		get
		{
			return leaderPoint;
		}
	}

	public Vector3 HandPos 
	{
		set
		{
			handPos = value;
		}
	}

}
