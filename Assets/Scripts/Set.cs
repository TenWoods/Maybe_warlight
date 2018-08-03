using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Set : MonoBehaviour 
{
	public GameObject setUI;
	public GameObject setButtons;
	public GameObject[] ruleText;
	public GameObject ruleButtons;
	private int page = 0;

	public void SetOn()
	{
		setButtons.SetActive(true);
		setUI.SetActive(true);
	}

	public void SetOff()
	{
		setUI.SetActive(false);
		setButtons.SetActive(false);
	}

	public void GameRuleOn()
	{
		setButtons.SetActive(false);
		ruleButtons.SetActive(true);
		ruleText[0].SetActive(true);
	}

	public void GameRuleOff()
	{
		setButtons.SetActive(true);
		ruleButtons.SetActive(false);
		foreach (GameObject g in ruleText)
		{
			g.SetActive(false);
		}
	}

	public void NextText()
	{
		ruleText[page].SetActive(false);
		page = (page + 1) % ruleText.Length;
		ruleText[page].SetActive(true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void Restart()
	{
		SceneManager.LoadScene(1);
	}
}
