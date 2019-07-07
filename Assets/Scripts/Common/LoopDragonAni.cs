/****************************************************
    文件：LoopDragonAni.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/7 4:29:4
	功能：飞龙循环动画
*****************************************************/

using UnityEngine;

public class LoopDragonAni : MonoBehaviour
{
    private Animation ani;
    private void Awake()
    {
        ani = transform.GetComponent<Animation>();
    }
    private void Start()
    {
        if (ani != null)
        {
            InvokeRepeating("PlayDragonAni", 0, 20);
        }
    }

    private void PlayDragonAni()
    {
        if (ani != null)
        {
            ani.Play();
        }
    }
}