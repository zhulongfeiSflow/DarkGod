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

    #region SK1
    public Image imgSk1Cd;
    public Text txtSk1Cd;
    private bool isSk1CD = false;
    private float sk1CDTime;
    private int sk1Num;
    private float sk1FillCouunt = 0;
    private float sk1NumCount = 0;
    #endregion

    #region SK2
    public Image imgSk2Cd;
    public Text txtSk2Cd;
    private bool isSk2CD = false;
    private float sk2CDTime;
    private int sk2Num;
    private float sk2FillCouunt = 0;
    private float sk2NumCount = 0;
    #endregion

    #region SK3
    public Image imgSk3Cd;
    public Text txtSk3Cd;
    private bool isSk3CD = false;
    private float sk3CDTime;
    private int sk3Num;
    private float sk3FillCouunt = 0;
    private float sk3NumCount = 0;
    #endregion

    public Text txtSelfHP;
    public Image imgSelfHp;
    private int HPSum;

    protected override void InitWnd() {
        base.InitWnd();
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeght * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint, false);

        HPSum = GameRoot.Instance.PlayerData.hp;
        SetText(txtSelfHP, HPSum + "/" + HPSum);
        imgSelfHp.fillAmount = 1;

        RegisterTouchEvts();
        sk1CDTime = resSvc.GetSkillCfg(101).cdTime / 1000;
        sk2CDTime = resSvc.GetSkillCfg(102).cdTime / 1000;
        sk3CDTime = resSvc.GetSkillCfg(103).cdTime / 1000;

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

    private void Update() {
        //test
        if (Input.GetKeyDown(KeyCode.Space)) {
            ClickNormalAtk();
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            ClickSkill1Atk();
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            ClickSkill2Atk();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            ClickSkill3Atk();
        }

        float delta = Time.deltaTime;
        if (isSk1CD) {
            sk1FillCouunt += delta;
            if (sk1FillCouunt >= sk1CDTime) {
                isSk1CD = false;
                SetActive(imgSk1Cd, false);
                sk1FillCouunt = 0;
            }
            else {
                imgSk1Cd.fillAmount = 1 - sk1FillCouunt / sk1CDTime;
            }

            sk1NumCount += delta;
            if (sk1NumCount >= 1) {
                sk1NumCount -= 1;
                sk1Num -= 1;
                SetText(txtSk1Cd, sk1Num);
            }
        }

        if (isSk2CD) {
            sk2FillCouunt += delta;
            if (sk2FillCouunt >= sk2CDTime) {
                isSk2CD = false;
                SetActive(imgSk2Cd, false);
                sk2FillCouunt = 0;
            }
            else {
                imgSk2Cd.fillAmount = 1 - sk2FillCouunt / sk2CDTime;
            }

            sk2NumCount += delta;
            if (sk2NumCount >= 1) {
                sk2NumCount -= 1;
                sk2Num -= 1;
                SetText(txtSk2Cd, sk2Num);
            }
        }

        if (isSk3CD) {
            sk3FillCouunt += delta;
            if (sk3FillCouunt >= sk3CDTime) {
                isSk3CD = false;
                SetActive(imgSk3Cd, false);
                sk3FillCouunt = 0;
            }
            else {
                imgSk3Cd.fillAmount = 1 - sk3FillCouunt / sk3CDTime;
            }

            sk3NumCount += delta;
            if (sk3NumCount >= 1) {
                sk3NumCount -= 1;
                sk3Num -= 1;
                SetText(txtSk3Cd, sk3Num);
            }
        }
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
        if (isSk1CD == false && BattleSys.Instance.GetPlayerState() != AniState.Attack) {
            BattleSys.Instance.ReqReleaseSkill(1);
            isSk1CD = true;
            SetActive(imgSk1Cd);
            imgSk1Cd.fillAmount = 1;
            sk1Num = (int)sk1CDTime;
            SetText(txtSk1Cd, sk1Num);
        }
    }

    public void ClickSkill2Atk() {
        if (isSk2CD == false && BattleSys.Instance.GetPlayerState() != AniState.Attack) {
            BattleSys.Instance.ReqReleaseSkill(2);
            isSk2CD = true;
            SetActive(imgSk2Cd);
            imgSk2Cd.fillAmount = 2;
            sk2Num = (int)sk2CDTime;
            SetText(txtSk2Cd, sk2Num);
        }
    }

    public void ClickSkill3Atk() {
        if (isSk3CD == false && BattleSys.Instance.GetPlayerState() != AniState.Attack) {
            BattleSys.Instance.ReqReleaseSkill(3);
            isSk3CD = true;
            SetActive(imgSk3Cd);
            imgSk3Cd.fillAmount = 3;
            sk3Num = (int)sk3CDTime;
            SetText(txtSk3Cd, sk3Num);
        }
    }

    public void ClickResetCfg() {//动态测试技能效果
        resSvc.ResetSkillCfgs();
    }

    public void SetSelfHPBarVal(int hp) {
        SetText(txtSelfHP, hp + "/" + HPSum);
        imgSelfHp.fillAmount = hp * 1.0f / HPSum;
    }
}