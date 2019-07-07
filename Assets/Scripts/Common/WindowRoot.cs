/****************************************************
    文件：WindowRoot.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/7 0:57:55
	功能：UI界面基类
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    protected ResSvc resSvc = null;
    protected AudioSvc audioSvc = null;

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
    }

    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
    }

    #region Tool Function

    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }

    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }

    #endregion
}