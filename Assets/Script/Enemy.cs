using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericScript;

public class Enemy : MonoBehaviour
{
    #region SERIALRIZE_FIELD

    [SerializeField]

    GameObject GameManager;

    #endregion

    #region PUBLIC

    #endregion

    #region PRIVATE

    int mLV;
    int mEXP;
    int mMaxHP;
    int mCurrentHP;
    int mATK;
    int mDEF;
    float mSPD;
    string mSpriteName;
    string mIconName;
    List<Skill> mSkills;
    List<int> mSkillCooltime;
    List<GameObject> mBuffs;

    #endregion


    #region PUBLIC_METHOD

    public void Init(int floor)
    {
        mLV += floor;
        mEXP += floor * mLV;
        mATK += (int)(floor * 0.5f);
        mDEF += (int)(floor * 0.5f);
        mMaxHP += (int)(floor * 2.5f);
        mSPD += (int)(floor * 0.5f);
        mCurrentHP = mMaxHP;
    }

    public void InitByMonsterData(Monster monster)
    {
        mEXP = monster.EXP;
        mLV = monster.LV;
        mATK = monster.ATK;
        mDEF = monster.DEF;
        mMaxHP = monster.HP;
        mSPD = monster.SPD;
        mSpriteName = monster.SpriteName;
        mIconName = monster.IconName;
        initSkill(monster.Skills);
    }

    public float Speed => mSPD;

    public string SpriteName => mSpriteName;

    public string IconName => mIconName;

    public int CurrentHP => mCurrentHP;

    public int MaxHP => mMaxHP;

    public int EXP => mEXP;

    public int ReadySkill
    {
        get
        {
            for (int i = mSkillCooltime.Count - 1; i > 0; i--)
            {
                if (mSkillCooltime[i] <= 0)
                {
                    return i;
                }
            }

            return 0;
        }
    }

    public List<Skill> GetSkills()
    {
        return mSkills;
    }

    public void ApplyEffect(Category category, int value)
    {
        switch (category)
        {
            case Category.Deal:
                {
                    int deal = value - mDEF;
                    if (deal > 0)
                        mCurrentHP -= deal;
                    break;
                }
            case Category.Heal:
                {
                    mCurrentHP += value;
                    if (mCurrentHP > mMaxHP)
                        mCurrentHP = mMaxHP;
                    break;
                }
            case Category.Bleeding:
                {
                    GameObject buff = Instantiate(mBuffPrefab);
                    mBuffs.Add(buff);
                    // 버프의 델리케이드 추가
                    break;
                }
        }
    }

    public void ReduceAllSkillCooltime(int reduction)
    {
        for (int i = 0; i < mSkillCooltime.Count; i++)
        {
            if(mSkillCooltime[i] > 0)
                mSkillCooltime[i] -= reduction;
        }
    }

    public void SetSkillCooltimeMax(int skillIndex)
    {
        mSkillCooltime[skillIndex] = mSkills[skillIndex].Cooltime + 1; 
    }

    public List<int> SkillCooltime => mSkillCooltime;

    public int ATK => mATK;

    #endregion

    #region PRIVATE_METHOD

    // Start is called before the first frame update
    void Start()
    {
        mSkills = new List<Skill>();
        mSkillCooltime = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initSkill(List<int> skillIndex)
    {
        mSkills.Clear();
        mSkillCooltime.Clear();

        foreach (int index in skillIndex)
        {
            Skill skill = GameManager.GetComponent<GameManager>().GetSkillByIndex(index);
            mSkills.Add(skill);
            mSkillCooltime.Add(skill.Cooltime);
        }
    }

    #endregion
}
