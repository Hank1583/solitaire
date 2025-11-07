using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackScreen : MonoBehaviour
{
	public static BackScreen instance;
	[SerializeField]
	private List<int> windows = new List<int>();
	private void Awake()
	{
		instance = this;
	}

	public void AddWindow()
	{
		windows.Add(1);
	}

	public void RemoveWindow()
	{
		if (windows.Count > 0)
		{
			windows.RemoveAt(0);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (windows.Count > 0)
			{
				PopUpManager.Instance.Close();
			}
			else
			{

				Application.Quit();

			}
		}
	}
}
