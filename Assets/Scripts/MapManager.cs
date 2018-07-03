using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour 
{
	/*管理的地图方块的地形*/
	[SerializeField]
	private int terrain;
	/*管理的地图块*/
	[SerializeField]
	private Map[] m_mapBlocks;
	/*初始时所属玩家ID*/
	[SerializeField]
	private int playerID = -1;
	/*在GameManager处的ID*/
	[SerializeField]
	private int mapManagerID = -1;
	/*相邻的大地图块(MapManager)*/
	[SerializeField]
	private MapManager[] nextManager;

	/// <summary>
	/// 对地图上所有的地图块
	/// </summary>
	/// <param name="gm"></param>
	public void InitBlocksData(GameManager gm)
	{
		int i;
		for(i = 0; i < m_mapBlocks.Length; i++)
		{
			m_mapBlocks[i].Terrain = terrain; //地形
			m_mapBlocks[i].MapID_MapManager = i; //在此处的索引
			m_mapBlocks[i].MapManagerID = mapManagerID; //manager的编号
			m_mapBlocks[i].NextMapManager = nextManager; //相邻的manager
			m_mapBlocks[i].PlayerID = playerID; //所属玩家的ID
			m_mapBlocks[i].Init_Start(gm);
			gm.Players[playerID].Maps.Add(m_mapBlocks[i]);
			//TODO:可能会有其他的数据初始化
		}
	}

	public Map[] M_mapBlocks
	{
		get
		{
			return m_mapBlocks;
		}
	}
}
