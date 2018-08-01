using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUIUpdate : MonoBehaviour 
{
	/*移动数量*/
	[SerializeField]
	private float addNum = 0;
	/*能指挥的总人数*/
	[SerializeField]
	private int commandMax = 0;
	/*输入的数字*/
	public Text numUI;
	/*用滑动条输入*/
	public Slider inputSlider;
	/*起始地图地形*/
	public Image startTerrain;
	/*目标地图地形*/
	public Image endTerrain;
	/*地形图片*/
	public Sprite[] terrainSprites;
	/*控制的地图块*/
	private Map commandMap;
	/*移动目标地图块*/
	private Map targetMap;
	/*被点击的箭头*/
	private GameObject arrow;
	/*玩家步骤储存*/
	private PlayerStep save_Step;

	private void Start() 
	{
		inputSlider.value = addNum;
		numUI.text = addNum.ToString();
	}

	/// <summary>
	/// 实时更新UI显示
	/// </summary>
	private void Update()
	{
		addNum = inputSlider.value;
		numUI.text = addNum.ToString();
	}
	
	/// <summary>
	/// 增加按钮
	/// </summary>
	public void UpButton()
	{
		if (addNum + 1 > commandMax)
		{
			return;
		}
		addNum += 1;
		inputSlider.value = addNum;
	}

	/// <summary>
	/// 减少按钮
	/// </summary>
	public void DownButton()
	{
		if (addNum - 1 < 0)
		{
			return;
		}
		addNum -= 1;
		inputSlider.value = addNum;
	}

	/// <summary>
	/// 玩家确定派兵数量
	/// </summary>
	public void Comfirm()
	{
		if (addNum == 0)
		{
			this.gameObject.SetActive(false);
			return;
		}
		save_Step.SaveCommamdSteps(commandMap, targetMap, (int)addNum, arrow);
		for (int i = 0; i < ArrowManager.Instance.Arrows_Able.Count; i++)
		{
			if (ArrowManager.Instance.Arrows_Remain.Contains( ArrowManager.Instance.Arrows_Able[i]))
			{
				continue;
			}
			ArrowManager.Instance.Arrows_Able[i].SetActive(false);
		}
		// foreach(GameObject a in ArrowManager.Instance.Arrows_Able)
		// {
		// 	if (ArrowManager.Instance.Arrows_Remain.Contains(a))
		// 	{
		// 		continue;
		// 	}
		// 	a.SetActive(false);
		// }
		this.gameObject.SetActive(false);
	}

	/// <summary>
	/// 取消派兵关闭UI
	/// </summary>
	public void Cancel()
	{
		addNum = 0;
		foreach(GameObject a in ArrowManager.Instance.Arrows_Able)
		{
			if (ArrowManager.Instance.Arrows_Remain.Contains(a))
			{
				continue;
			}
			a.SetActive(false);
		}
		this.gameObject.SetActive(false);
	}

	/// <summary>
	/// 设置指挥UI
	/// </summary>
	/// <param name="maxNum">最大士兵数</param>
	/// <param name="startMap">起始地图</param>
	/// <param name="targetMap">目标地图</param>
	/// <param name="saver">玩家步骤</param>
	/// <param name="arrow">箭头</param>
	public void SetCommandUI(int maxNum, Map startMap, Map targetMap, PlayerStep saver, GameObject arrow)
	{
		commandMax = maxNum - 1;
		inputSlider.maxValue = commandMax;
		commandMap = startMap;
		this.targetMap = targetMap;
		this.arrow = arrow;
		save_Step = saver;
		inputSlider.value = 0;
		switch (startMap.Terrain)
		{
			case Terrain.HIGHLAND:
			{
				startTerrain.sprite = terrainSprites[0];
				break;
			}
			case Terrain.FLATLAND:
			{
				startTerrain.sprite = terrainSprites[1];
				break;
			}
			case Terrain.FOREST:
			{
				startTerrain.sprite = terrainSprites[2];
				break;
			}
			case Terrain.VALLY:
			{
				startTerrain.sprite = terrainSprites[3];
				break;
			}
			case Terrain.DESERT:
			{
				startTerrain.sprite = terrainSprites[4];
				break;
			}
		}
		switch (targetMap.Terrain)
		{
			case Terrain.HIGHLAND:
			{
				endTerrain.sprite = terrainSprites[0];
				break;
			}
			case Terrain.FLATLAND:
			{
				endTerrain.sprite = terrainSprites[1];
				break;
			}
			case Terrain.FOREST:
			{
				endTerrain.sprite = terrainSprites[2];
				break;
			}
			case Terrain.VALLY:
			{
				endTerrain.sprite = terrainSprites[3];
				break;
			}
			case Terrain.DESERT:
			{
				endTerrain.sprite = terrainSprites[4];
				break;
			}
		}
	}
}
