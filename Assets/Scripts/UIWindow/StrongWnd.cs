/****************************************************
    文件：StrongWnd.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/17 10:28:48
	功能：强化升级界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class StrongWnd : WindowRoot
{
    #region UI Define
    public Image imgCurtPos;
    public Text txtStartLv;
    public Transform starTransGrp;
    public Text propHP1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHp2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1;
    public Image propArr2;
    public Image propArr3;

    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;


    public Transform costTransRoot;
    public Text txtCoin;

    StrongCfg nextSd;
    #endregion

    #region Data Area
    public Transform posBtnTrans;
    private Image[] imgs = new Image[6];
    private int currentIndex;
    private PlayerData pd;
    #endregion


    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        RegClickEvts();

        ClickPosItem(0);
    }

    private void RegClickEvts()
    {
        for (int i = 0; i < posBtnTrans.childCount; i++)
        {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();

            OnClick(img.gameObject, (object args) =>
            {
                audioSvc.PlayUIAudio(Constants.UIClickBtn);
                ClickPosItem((int)args);
            }, i);

            imgs[i] = img;
        }
    }

    private void ClickPosItem(int index)
    {
        PECommon.Log("Click Item:" + index);

        currentIndex = index;
        for (int i = 0; i < imgs.Length; i++)
        {
            Transform trans = imgs[i].transform;
            Vector3 pos = trans.localPosition;
            if (i == currentIndex)
            {
                //箭头显示
                pos.x = 10;
                trans.localPosition = pos;
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 95);
                SetSprite(imgs[i], PathDefine.ItemArrorBG);
            }
            else
            {
                pos.x = 0;
                trans.localPosition = pos;
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 85);
                SetSprite(imgs[i], PathDefine.ItemPlatBG);
            }
        }

        RefreshItem();
    }

    private void RefreshItem()
    {
        //金币
        SetText(txtCoin, pd.coin);
        switch (currentIndex)
        {
            case 0:
                SetSprite(imgCurtPos, PathDefine.ItemToukui);
                break;
            case 1:
                SetSprite(imgCurtPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurtPos, PathDefine.ItemYaobu);
                break;
            case 3:
                SetSprite(imgCurtPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurtPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurtPos, PathDefine.ItemFoot);
                break;
            default:
                break;
        }

        SetText(txtStartLv, pd.strongArr[currentIndex] + "星级");

        int curtStarLv = pd.strongArr[currentIndex];
        for (int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curtStarLv)
            {
                SetSprite(img, PathDefine.SpStar2);
            }
            else
            {
                SetSprite(img, PathDefine.SpStar1);
            }
        }

        int nextStarLv = curtStarLv + 1;
        int sumAddHp = resSvc.GetPropAddValPreLv(currentIndex, nextStarLv, 1);
        int sumAddHurt = resSvc.GetPropAddValPreLv(currentIndex, nextStarLv, 2);
        int sumAddDef = resSvc.GetPropAddValPreLv(currentIndex, nextStarLv, 3);

        SetText(propHP1, "生命 +" + sumAddHp);
        SetText(propHurt1, "伤害 +" + sumAddHurt);
        SetText(propDef1, "防御 +" + sumAddDef);

        nextSd = resSvc.GetStrongCfg(currentIndex, nextStarLv);
        if (nextSd != null)
        {
            IsShowUpgradePanel(true);

            SetText(propHp2, "强化后 +" + nextSd.addhp);
            SetText(propHurt2, "+" + nextSd.addhurt);
            SetText(propDef2, "+" + nextSd.adddef);

            SetText(txtNeedLv, "需要等级：" + nextSd.minlv);
            SetText(txtCostCoin, "需要消耗：      " + nextSd.coin);
            SetText(txtCostCrystal, nextSd.crystal + "/" + pd.crystal);
        }
        else
        {
            IsShowUpgradePanel(false);
        }
    }

    public void IsShowUpgradePanel(bool boolean)
    {
        SetActive(propHp2, boolean);
        SetActive(propHurt2, boolean);
        SetActive(propDef2, boolean);

        SetActive(costTransRoot, boolean);
        SetActive(propArr1, boolean);
        SetActive(propArr2, boolean);
        SetActive(propArr3, boolean);
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        if (pd.strongArr[currentIndex] < 10)
        {
            if (pd.lv < nextSd.minlv)
            {
                GameRoot.AddTips("角色等级不够");
                return;
            }
            if (pd.coin < nextSd.coin)
            {
                GameRoot.AddTips("金币数量不够");
                return;
            }
            if (pd.crystal < nextSd.crystal)
            {
                GameRoot.AddTips("水晶不够");
                return;
            }

            netSvc.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqStrong,
                reqStrong = new ReqStrong
                {
                    pos = currentIndex
                }
            });
        }
        else
        {
            GameRoot.AddTips("星级已经升满");

        }
    }

    public void UpdateUI()
    {
        audioSvc.PlayUIAudio(Constants.FBItemEnter);
        ClickPosItem(currentIndex);
    }
}