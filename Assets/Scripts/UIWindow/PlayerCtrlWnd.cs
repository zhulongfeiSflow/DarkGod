/****************************************************
    文件：PlayerCtrlWnd.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/21 9:16:41
	功能：玩家控制界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCtrlWnd : WindowRoot
{
    #region UI
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;

    public Text txtLevel;
    public Text TxtName;
    public Text txtExpPrg;

    public Transform expPrgTrans;

    #endregion

    #region 摇杆相关
    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;
    #endregion

    public Vector2 currentDir;

    protected override void InitWnd() {
        base.InitWnd();
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeght * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint, false);

        RegisterTouchEvts();

        RefreshUI();
    }

    public void RefreshUI() {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtLevel, pd.lv);
        SetText(TxtName, pd.name);

        #region Expprg
        int expPrgVal = (int)(pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv) * 100);
        SetText(txtExpPrg, expPrgVal + "%");
        int index = expPrgVal / 10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();

        float globalRate = 1.0f * Constants.ScreenStandardHeght / Screen.height;
        float screenWidth = Screen.width * globalRate;
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);

        for (int i = 0; i < expPrgTrans.childCount; i++) {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index) {
                img.fillAmount = 1;
            }
            else if (i == index) {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else {
                img.fillAmount = 0;
            }
        }
        #endregion
    }

    public void RegisterTouchEvts() {
        OnClickDown(imgTouch.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position;
            SetActive(imgDirPoint, true);
            imgDirBg.transform.position = evt.position;
        });

        OnClickUp(imgTouch.gameObject, (PointerEventData evt) =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint, false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            currentDir = Vector2.zero;
            BattleSys.Instance.SetMoveDir(currentDir);
        });

        OnDrag(imgTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis) {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else {
                imgDirPoint.transform.position = evt.position;
            }
            currentDir = dir.normalized;
            BattleSys.Instance.SetMoveDir(currentDir);
        });
    }

    public void ClickNormalAtk() {
        BattleSys.Instance.ReqReleaseSkill(0);
    }

    public void ClickSkill1Atk() {
        BattleSys.Instance.ReqReleaseSkill(1);
    }

    public void ClickSkill2Atk() {
        BattleSys.Instance.ReqReleaseSkill(2);
    }

    public void ClickSkill3Atk() {
        BattleSys.Instance.ReqReleaseSkill(3);
    }

    public void ClickResetCfg() {//动态测试技能效果
        resSvc.ResetSkillCfgs();
    }

}