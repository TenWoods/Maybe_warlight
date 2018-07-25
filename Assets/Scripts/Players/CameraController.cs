using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
	/*滚轮灵敏度*/
	[SerializeField]
	private float scroll_Sensitivty = 1.0f;
	/*鼠标移动灵敏度*/
	[SerializeField]
	private float mouseMove_Sensitivty = 1.0f;
	/*宽度边界*/
	private float edge_width;
	/*高度边界*/
	private float edge_height;
	/*上一帧鼠标位置*/
	private Vector3 lastMousePos;


	private void Start() 
	{
		lastMousePos = new Vector3();
		lastMousePos = Input.mousePosition;
	}
	
	private void Update() 
	{
		//控制屏幕缩放
		Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * scroll_Sensitivty;
		if (Camera.main.orthographicSize > 13.5f)
		{
			Camera.main.orthographicSize = 13.5f;
		}
		else if (Camera.main.orthographicSize < 4.5f)
		{
			Camera.main.orthographicSize = 4.5f;
		}
		//更新屏幕范围
		Vector3 screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		edge_width = 24.0f - screenPoint.x;
		edge_height = 13.5f - screenPoint.y;
		//控制拖拽
		if (Input.GetMouseButton(1))
		{
			Vector3 dir = Vector3.Normalize(Input.mousePosition - lastMousePos) * mouseMove_Sensitivty;
			Vector3 targetPos = transform.position - dir;
			if (targetPos.x > edge_width)
			{
				targetPos.x = edge_width;
			}
			if (targetPos.x < -edge_width)
			{
				targetPos.x = -edge_width;
			}
			if (targetPos.y > edge_height)
			{
				targetPos.y = edge_height;
			}
			if (targetPos.y < -edge_height)
			{
				targetPos.y = -edge_height;
			} 
			transform.position = targetPos;
		}
		lastMousePos = Input.mousePosition;
	}
}
