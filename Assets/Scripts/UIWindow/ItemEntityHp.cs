/****************************************************
    文件：ItemEntityHp.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/23 15:29:17
	功能：血条物体
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ItemEntityHp : MonoBehaviour
{
    #region UIDefine
    public Image imgHPGray;
    public Image imgHPRed;

    public Animation criticalAni;
    public Text txtCritical;

    public Animation dodgeAni;
    public Text txtDodge;

    public Animation hpAni;
    public Text txtHp;
    #endregion

    private RectTransform rect;
    private Transform rootTrans;
    private int hpVal;
    private float scaleRate = 1.0f * Constants.ScreenStandardHeght / Screen.height;

    private void Update() {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    SetCritical(666);
        //    SetHurt(3131);
        //    SetDodge();
        //}
        Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
        rect.anchoredPosition = screenPos * scaleRate;

        UpdateMixBlend();
    }

    private void UpdateMixBlend() {
        if (Mathf.Abs(currentPrg - targetPrg) < Constants.AccelerHPSpeed * Time.deltaTime) {
            currentPrg = targetPrg;
        }
        else if (currentPrg > targetPrg) {
            currentPrg -= Constants.AccelerHPSpeed * Time.deltaTime;
        }
        else {
            currentPrg += Constants.AccelerHPSpeed * Time.deltaTime;
        }
        imgHPGray.fillAmount = currentPrg;
    }

    public void InitItemInfo(Transform trans, int hp) {

        rect = transform.GetComponent<RectTransform>();
        rootTrans = trans;
        hpVal = hp;
        imgHPGray.fillAmount = 1;
        imgHPRed.fillAmount = 1;
    }

    public void SetCritical(int critical) {
        criticalAni.Stop();
        txtCritical.text = "暴击 " + critical;
        criticalAni.Play();
    }

    public void SetDodge() {
        dodgeAni.Stop();
        txtDodge.text = "闪避";
        dodgeAni.Play();
    }

    public void SetHurt(int hurt) {
        hpAni.Stop();
        txtHp.text = "-" + hurt;
        hpAni.Play();
    }

    private float currentPrg;
    private float targetPrg;

    public void SetHPVal(int oldVal, int newVal) {
        currentPrg = oldVal * 1.0f / hpVal;
        targetPrg = newVal * 1.0f / hpVal;
        imgHPRed.fillAmount = targetPrg;

    }
}