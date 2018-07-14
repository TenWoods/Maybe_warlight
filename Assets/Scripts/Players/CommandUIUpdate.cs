using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUIUpdate : MonoBehaviour 
{
	/*上一帧的人数*/
	private int lastNum = 0;
	/*能指挥的总人数*/
	[SerializeField]
	private int commandMax = 0;
	/*输入的数字*/
	public InputField inputNum;
	/*用滑动条输入*/
	public Slider inputSlider;
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
		inputSlider.value = lastNum;
		inputNum.text = lastNum.ToString();
	}

	/// <summary>
	/// 实时更新UI显示
	/// </summary>
	private void Update()
	{
		if (string.Compare(inputNum.text, lastNum.ToString()) != 0)
		{
			int.TryParse(inputNum.text, out lastNum);
			inputSlider.value = lastNum;
		}
		else if ((int)inputSlider.value != lastNum)
		{
			lastNum = (int)inputSlider.value;
			inputNum.text = lastNum.ToString();
		}
	}

	/// <summary>
	/// 玩家确定派兵数量
	/// </summary>
	public void Comfirm()
	{
<<<<<<< HEAD
		save_Step.SaveCommamdSteps(commandMap, targetMap, lastNum);
		this.gameObject.SetActive(false);//不应该是true吗
=======
		if (lastNum == 0)
		{
			this.gameObject.SetActive(false);
			return;
		}
		save_Step.SaveCommamdSteps(commandMap, targetMap, lastNum, arrow);
		this.gameObject.SetActive(false);
>>>>>>> 5fbded97ccda4bfc261f31dfd925208ab53a024c
	}

	/// <summary>
	/// 取消派兵关闭UI
	/// </summary>
	public void Cancel()
	{
		lastNum = 0;
		this.gameObject.SetActive(false);
	}

	public void SetCommandUI(int maxNum, Map startMap, Map targetMap, PlayerStep saver, GameObject arrow)
	{
<<<<<<< HEAD
		commandMax = (int)(maxNum + 0.5);//？
=======
		commandMax = maxNum - 1;
>>>>>>> 5fbded97ccda4bfc261f31dfd925208ab53a024c
		inputSlider.maxValue = commandMax;
		commandMap = startMap;
		this.targetMap = targetMap;
		this.arrow = arrow;
		save_Step = saver;
		inputSlider.value = 0;
	}
}
