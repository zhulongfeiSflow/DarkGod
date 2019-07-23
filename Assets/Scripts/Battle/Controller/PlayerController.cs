/****************************************************
    文件：PlayerController.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/14 12:26:31
	功能：主角表现实体角色控制器类
*****************************************************/

using UnityEngine;

public class PlayerController : Controller
{
    public GameObject daggeSkill1fx;
    public GameObject daggeSkill2fx;
    public GameObject daggeSkill3fx;

    public GameObject daggeratk1fx;
    public GameObject daggeratk2fx;
    public GameObject daggeratk3fx;
    public GameObject daggeratk4fx;
    public GameObject daggeratk5fx;

    private Vector3 camOffset;

    private float targetBlend;
    private float currentBlend;

    public override void Init() {
        base.Init();

        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;

        if (daggeSkill1fx != null) {
            fxDic.Add(daggeSkill1fx.name, daggeSkill1fx);
        }
        if (daggeSkill2fx != null) {
            fxDic.Add(daggeSkill2fx.name, daggeSkill2fx);
        }
        if (daggeSkill3fx != null) {
            fxDic.Add(daggeSkill3fx.name, daggeSkill3fx);
        }

        if (daggeratk1fx != null) {
            fxDic.Add(daggeratk1fx.name, daggeratk1fx);
        }
        if (daggeratk2fx != null) {
            fxDic.Add(daggeratk2fx.name, daggeratk2fx);
        }
        if (daggeratk3fx != null) {
            fxDic.Add(daggeratk3fx.name, daggeratk3fx);
        }
        if (daggeratk4fx != null) {
            fxDic.Add(daggeratk4fx.name, daggeratk4fx);
        }
        if (daggeratk5fx != null) {
            fxDic.Add(daggeratk5fx.name, daggeratk5fx);
        }
    }

    private void Update() {
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

        if (skillMove) {
            SetSkillMove();

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

    private void SetSkillMove() {
        ctrl.Move(transform.forward * Time.deltaTime * skillMoveSpeed);
    }

    public void SetCam() {
        if (camTrans != null) {
            camTrans.position = transform.position - camOffset;
        }
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

    //////////////////////////////////////////////////////////////////////////
    public override void SetBlend(float blend) {
        targetBlend = blend;
    }

    public override void SetFX(string name, float delay) {
        GameObject go;
        if (fxDic.TryGetValue(name, out go)) {
            go.SetActive(true);
            TimerSvc.AddTimeTask((int tid) =>
            {
                go.SetActive(false);
            }, delay);
        }
    }

}