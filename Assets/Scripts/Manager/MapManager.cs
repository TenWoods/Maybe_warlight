using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour 
{
	/*管理的地图方块的地形*/
	[SerializeField]
	private Terrain terrain;
	/*管理的地图块*/
	[SerializeField]
	private Map[] m_mapBlocks;
	/*士兵数收益*/
	[SerializeField]
	[Header("士兵数收益")]
	private int addSoldierNum = 1;
	/*初始时所属玩家ID*/
	[SerializeField]
	private int playerID = -1;
	/*在GameManager处的ID*/
	[SerializeField]
	private int mapManagerID = -1;

	public void CheckUpdateAdd()
	{
		int playerID = m_mapBlocks[0].PlayerID;
		foreach(Map m in m_mapBlocks)
		{
			if (m.PlayerID != playerID)
			{
				return;
			}
		}
		Debug.Log("增加兵力数");
		GameManager.Instance.Players[playerID].SoldierNum += addSoldierNum;
	}

	/// <summary>
	/// 对地图上所有的地图块初始化数据
	/// </summary>
	/// <param name="gm"></param>
	public void InitBlocksData(GameManager gm)
	{
		for(int i = 0; i < m_mapBlocks.Length; i++)
		{
			m_mapBlocks[i].Terrain = terrain; //地形
			m_mapBlocks[i].MapID_MapManager = i; //在此处的索引
			m_mapBlocks[i].MapManagerID = mapManagerID; //manager的编号
			m_mapBlocks[i].PlayerID = playerID; //所属玩家的ID
			m_mapBlocks[i].Init_Start(); //开始初始化地图
			gm.Players[playerID].Maps.Add(m_mapBlocks[i]);
		}
	}

	public Map[] M_mapBlocks
	{
		get
		{
			return m_mapBlocks;
		}
	}

	public int MapManagerID
	{
		set
		{
			mapManagerID = value;
		}
	}

	public int AddSoldierNum 
	{
		get
		{
			return addSoldierNum;
		}
	}
}
