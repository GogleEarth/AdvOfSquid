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
    #endregion


    #region PUBLIC_METHOD

    public void Init(int floor)
    {
        mEXP += floor * 2;
        mLV += floor;
        mATK += (int)(floor * 0.5f);
        mDEF += (int)(floor * 0.5f);
        mMaxHP += (int)(floor * 2.5f);
        mSPD += (int)(floor * 0.5f);
        mCurrentHP = mMaxHP;
    }

    public void InitByMonsterData(Monster monster)
    {
        Debug.Log(monster.Display());
        mEXP = monster.GetEXP();
        mLV = monster.GetLV();
        mATK = monster.GetATK();
        mDEF = monster.GetDEF();
        mMaxHP = monster.GetHP();
        mSPD = monster.GetSPD();
        mSpriteName = monster.GetSpriteName();
        mIconName = monster.GetIconName();
        initSkill(monster.GetSkills());
    }

    public float GetSpeed()
    {
        return mSPD;
    }

    public string GetSpriteName()
    {
        return mSpriteName;
    }

    public string GetIconName()
    {
        return mIconName;
    }

    public int GetCurrentHP()
    {
        return mCurrentHP;
    }

    public int GetEXP()
    {
        return mEXP;
    }

    public int GetReadySkill()
    {
        for (int i = mSkillCooltime.Count-1; i > 0; i--)
        {
            if (mSkillCooltime[i] <= 0)
            {
                return i;
            }
        }

        return 0;
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
                    break;
                }
        }
        Debug.Log("enemy hp : " + mCurrentHP);
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
        mSkillCooltime[skillIndex] = mSkills[skillIndex].GetCooltime() + 1; 
    }

    public int GetATK()
    {
        return mATK;
    }

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
        foreach (int index in skillIndex)
        {
            Skill skill = GameManager.GetComponent<GameManager>().GetSkillByIndex(index);
            mSkills.Add(skill);
            mSkillCooltime.Add(skill.GetCooltime());
        }
    }

    #endregion
}
