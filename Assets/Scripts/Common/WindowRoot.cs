/****************************************************
    文件：WindowRoot.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/7 0:57:55
	功能：UI界面基类
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    protected ResSvc resSvc = null;
    protected AudioSvc audioSvc = null;
    protected NetSvc netSvc = null;

    public void SetWndState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }

        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }

    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
    }

    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        netSvc = null;
    }

    #region Tool Function

    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    //protected void SetActive(Transform trans, bool state = true)
    //{
    //    trans.gameObject.SetActive(state);
    //}
    //protected void SetActive(RectTransform rectTrans, bool state = true)
    //{
    //    rectTrans.gameObject.SetActive(state);
    //}
    //protected void SetActive(Image img, bool state = true)
    //{
    //    img.transform.gameObject.SetActive(state);
    //}
    //protected void SetActive(Text txt, bool state = true)
    //{
    //    txt.transform.gameObject.SetActive(state);
    //}
    protected void SetActive<T>(T t, bool state = true) where T : Component
    {
        t.gameObject.SetActive(state);
    }


    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    //protected void SetText(Text txt, int num = 0)
    //{
    //    SetText(txt, num.ToString());
    //}
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }
    protected void SetText<T, S>(T t, S s)
        where T : Text
        where S : IFormattable
    {
        t.text = s.ToString();
    }

    protected void SetSprite(Image img, string path)
    {
        Sprite sp = resSvc.LoadSprite(path, true);
        img.sprite = sp;
    }

    protected T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    #endregion

    #region Click Evts
    protected void OnClickDown(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = cb;
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = cb;
    }
    protected void OnDrag(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = cb;
    }
    #endregion
}