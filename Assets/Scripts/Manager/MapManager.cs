﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour 
{
	/*管理的地图方块的地形*/
	[SerializeField]
	private Terrain terrain;
	/*管理的地图块*/
	[SerializeField]
	private Map[] manageBlocks;
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
		int playerID = manageBlocks[0].PlayerID;
		foreach(Map m in manageBlocks)
		{
			if (m.PlayerID != playerID)
			{
				return;
			}
		}
		if (playerID == -1)
		{
			return;
		}
		GameManager.Instance.Players[playerID].SoldierNum += addSoldierNum;
	}

	/// <summary>
	/// 对地图上所有的地图块初始化数据
	/// </summary>
	public void InitBlocksData()
	{
		int i;
		for(i = 0; i < manageBlocks.Length; i++)
		{
			manageBlocks[i].Terrain = terrain; //地形
			manageBlocks[i].MapID_MapManager = i; //在此处的索引
			manageBlocks[i].MapManagerID = mapManagerID; //manager的编号
			manageBlocks[i].PlayerID = playerID; //所属玩家的ID
			manageBlocks[i].Init_Start(); //开始初始化地图
			manageBlocks[i].UpdateFlagUI();
			if (playerID == -1)
			{
				continue;
			}
			GameManager.Instance.Players[playerID].Maps.Add(manageBlocks[i]);
		}
		//Debug.Log(i);
	}

	public Map[] M_mapBlocks
	{
		get
		{
			return manageBlocks;
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
