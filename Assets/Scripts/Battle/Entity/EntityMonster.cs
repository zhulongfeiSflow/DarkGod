/****************************************************
	文件：EntityMonster.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/22 14:23   	
	功能：怪物逻辑实体类
*****************************************************/

public class EntityMonster : EntityBase
{
    public MonsterData md;

    public override void SetBattleProps(BattleProps props) {
        int level = md.mLevel;

        BattleProps p = new BattleProps {
            hp = props.hp * level,
            ad = props.ad * level,
            ap = props.ap * level,
            addef = props.addef * level,
            apdef = props.apdef * level,
            dodge = props.dodge * level,
            pierce = props.pierce * level,
            critical = props.critical * level,
        };
        Props = p;
        HP = p.hp;
    }
}