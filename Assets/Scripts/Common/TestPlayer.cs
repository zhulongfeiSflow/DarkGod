/****************************************************
    文件：TestPlayer.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/21 15:32:48
	功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{

    public CharacterController ctrl;
    public GameObject daggerskill1fx;

    private Transform camTrans;
    private Vector3 camOffset;

    private float targetBlend;
    private float currentBlend;

    public Animator ani;

    protected bool isMove = false;
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir {
        get {
            return dir;
        }
        set {
            if (value == Vector2.zero) {
                isMove = false;
            }
            else {
                isMove = true;
            }
            dir = value;
        }
    }

    private void Start() {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
    }

    private void Update() {

        #region Input
#if true
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 _dir = new Vector2(h, v).normalized;
        if (_dir != Vector2.zero) {
            Dir = _dir;
            SetBlend(Constants.BlendMove);
        }
        else {
            Dir = Vector2.zero;
            SetBlend(Constants.BlendIdle);
        }
#endif
        #endregion

        if (currentBlend != targetBlend) {
            UpDateMixBlend();
        }

        if (isMove) {
            //设置方向
            SetDir();

            //产生位置
            SetMove();

            //相机跟随
            SetCam();
        }
    }

    private void SetDir() {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove() {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCam() {
        if (camTrans != null) {
            camTrans.position = transform.position - camOffset;
        }
    }

    public void SetBlend(float blend) {
        targetBlend = blend;
    }

    private void UpDateMixBlend() {
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime) {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend) {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        ani.SetFloat("Blend", currentBlend);
    }

    public void ClickSkillBtn() {
        ani.SetInteger("Action", 1);
        daggerskill1fx.gameObject.SetActive(true);
        StartCoroutine(Delay());
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(0.9f);
        ani.SetInteger("Action", -1);
        daggerskill1fx.gameObject.SetActive(false);

    }
}