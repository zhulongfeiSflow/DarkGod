/****************************************************
    文件：PEListener.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/14 9:54:2
	功能：UI事件监听插件
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PEListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Action<PointerEventData> onClickDown;
    public Action<PointerEventData> onClickUp;
    public Action<PointerEventData> onDrag;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClickDown != null)
        {
            onClickDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onClickUp != null)
        {
            onClickUp(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
        {
            onDrag(eventData);
        }
    }
}