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
	[SerializeField]
	protected int playerID;
	/*是否进阶*/
	protected bool upgrade = false;
	/*卡牌效果说明*/
	[SerializeField]
	protected GameObject[] tips;
	/*卡牌图片*/
	[SerializeField]
	protected Sprite[] images;
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
	private float moveSpeed = 800;
	/*卡牌使用信号*/
	protected bool hasUsed = false;
	/*音效*/
	public AudioSource ados;
	/*卡牌操作*/
	private bool moveWithMouse = false;
	/*卡牌效果生效,卡牌回位*/
	protected bool effective = false;
	private RectTransform rt;
	private Vector3 startPos;
	private Vector3 handPos = Vector3.zero;
	protected Operation playerOP;
	/*卡牌是否变小*/
	private bool isSmall = false;
	/*卡牌是否在手上(游戏开始的时候所有的卡牌都在场景中)*/
	public bool inHand = false;
	

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
			if (rt.position.y > Screen.height / 4 && (tips[0].activeSelf || tips[1].activeSelf))
			{
				//音效播放
				ados.Play();
				rt.localScale /= 2;
				isSmall = true;
				if (!upgrade)
				{
					tips[0].SetActive(false);
				}
				else
				{
					tips[1].SetActive(false);
				}
			}
			if (rt.position.y > Screen.height / 2)
			{
				if (playerOP.LeaderPoint - leaderPoint < 0)
				{
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
	/// <param name="des"></param>
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
		rt.localScale *= 2;
		if (!upgrade)
		{
			this.GetComponent<Image>().sprite = images[0];
			tips[0].SetActive(true);
		}
		else
		{
			this.GetComponent<Image>().sprite = images[1];
			tips[1].SetActive(true);
		}
	}

	/// <summary>
	/// 取消选中
	/// </summary>
	public void CancelClick()
	{
		moveWithMouse = false;
		rt.position = handPos;
		if (!upgrade)
		{
			tips[0].SetActive(false);
		}
		else
		{
			tips[1].SetActive(false);
		}
		if (isSmall)
		{
			isSmall = false;
			return;
		}
		rt.localScale /= 2;
		isSmall = false;
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
