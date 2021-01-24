using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericScript;

public class Enemy : MonoBehaviour
{
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
    List<int> mSkills;

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
        mSkills = monster.GetSkills();
        mSpriteName = monster.GetSpriteName();
        mIconName = monster.GetIconName();
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

    public void ApplyCardEffect(CardCategory category, int value)
    {
        switch (category)
        {
            case CardCategory.Deal:
                {
                    int deal = value - mDEF;
                    if (deal > 0)
                        mCurrentHP -= deal;
                    break;
                }
            case CardCategory.Heal:
                {
                    mCurrentHP += value;
                    break;
                }
        }
        Debug.Log("enemy hp : " + mCurrentHP);
    }

    #endregion

    #region PRIVATE_METHOD

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
