using UnityEngine;
using System;
public class PopUpData
{
	private GameObject obj;
	private RectTransform rt;
	private Vector2 position;
	private Action callback;
	public GameObject Obj {get { return obj;}}
	public RectTransform RT {get { return rt;}}
	public Vector2 Position {get { return position;}}
	public Action Callback {get { return callback;}}
	public PopUpData(GameObject obj, Vector2 position, Action func)
	{
		this.obj = obj;
		this.rt = obj.GetComponent<RectTransform> ();
		this.position = position;
		this.callback = func;
	}
	public void SetInitPositionTransform()
	{
		rt.localPosition = position;
		rt.sizeDelta = Vector2.zero;
		rt.localScale = Vector3.one;
	}
	public void SetZeroPositionTransform()
	{
		rt.localPosition = Vector2.zero;
		rt.sizeDelta = Vector2.zero;
		rt.localScale = Vector3.one;
	}
}