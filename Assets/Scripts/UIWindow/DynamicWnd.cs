/****************************************************
    文件：DynamicWnd.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/7 4:39:26
	功能：动态UI元素界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot
{
    public Animation tipsAni;
    public Text txtTips;


    private bool isTipsShow = false;
    private Queue<string> tipsQue = new Queue<string>();

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(txtTips, false);
    }

    public void AddTip(string tips)
    {
        lock (tipsQue)
        {
            tipsQue.Enqueue(tips);
        }
    }

    private void Update()
    {
        if (tipsQue.Count > 0 && isTipsShow == false)
        {
            lock (tipsQue)
            {
                string tips = tipsQue.Dequeue();
                isTipsShow = true;
                setTips(tips);
            }
        }
    }

    public void setTips(string tips)
    {
        SetActive(txtTips, true);
        SetText(txtTips, tips);

        AnimationClip clip = tipsAni.GetClip("TipsShowAni");
        tipsAni.Play();
        //延时关闭激活状态
        StartCoroutine(AniPlayDone(clip.length, () =>
        {
            isTipsShow = false;
            SetActive(txtTips, false);
        }));
    }

    private IEnumerator AniPlayDone(float sec, Action cb)
    {
        yield return new WaitForSeconds(sec);
        if (cb != null)
        {
            cb();
        }
    }

}