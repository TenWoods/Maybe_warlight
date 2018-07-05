using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUIUpdate : MonoBehaviour 
{
	/*指挥人数*/
	private int soldierNum = 0;
	/*能指挥的总人数*/
	[SerializeField]
	private int commandMax = 0;
	/*输入的数字*/
	public InputField inputNum;
	/*用滑动条输入*/
	public Slider inputSlider;

	/// <summary>
	/// 实时更新UI显示
	/// </summary>
	private void Update()
	{
		inputSlider.maxValue = commandMax;//调试用
		
	}

	public void SetCommandUI(int maxNum)
	{
		commandMax = maxNum;
		inputSlider.maxValue = commandMax;
		soldierNum = 1;
	}
}
