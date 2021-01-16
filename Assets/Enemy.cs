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
    float mEXP;
    int mHP;
    int mATK;
    int mDEF;
    float mSPD;

    #endregion


    #region PUBLIC_METHOD

    public void Init(int floor)
    {
        mEXP += floor * 2.5f;
        mLV += floor;
        mATK += (int)(floor * 0.5f);
        mDEF += (int)(floor * 0.5f);
        mHP += (int)(floor * 2.5f);
        mSPD += (int)(floor * 0.5f);
    }

    public void InitByMonsterData(Monster monster)
    {

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
