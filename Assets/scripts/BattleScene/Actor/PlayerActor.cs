using UnityEngine.Rendering;

public class PlayerActor : BattleActor
{
    // 코스트, 학과, 
    public int MaxDCost;
    int _DCost;
    public int DCost
    {
        get
        {
            return _DCost;
        }
        set
        {
            if (value < 0) value = 0;
            if (value > MaxDCost) value = MaxDCost;
            _DCost = value;
            UpdateHPBar();

        }
    }
    public SortingGroup group;

    public DepartmentKind departmentKind;
    public CharacterSpriteRenderers sr;
    public override bool isEnemy => false;
    public override void InitActor(int atk, int def, int speed, int hp)
    {
        this.MaxDCost = 100;
        this._DCost = 100;
        base.InitActor(atk, def, speed, hp);
    }
    public void InitActor(int atk, int def, int speed, int hp, int md)
    {
        this.MaxDCost = md;
        this._DCost = md;
        base.InitActor(atk, def, speed, hp);
    }
    public override void UpdateHPBar()
    {
        base.UpdateHPBar();
        if (hpBar is PlayerBar)
        {
            ((PlayerBar)hpBar).SetCost((float)DCost / (float)MaxDCost);
        }
    }
    private void Awake()
    {
        sr = GetComponent<CharacterSpriteRenderers>();
    }

    public override string ToString()
    {
        return $"{actorName}||{departmentKind}\n체력: {hp}/{maxHP}\n보호막: {shieldAmount}\n공격력: {atk}\n방어력: {def}\n속도: {speed}\n학과 코스트: {DCost}/{MaxDCost}";
    }
}