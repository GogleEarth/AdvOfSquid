using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericScript;

public class Player : MonoBehaviour
{
    List<int> inventory;
    List<int> deck;

    GameManager game_manager;
    int level;
    int mMaxEXP;
    int mCurrentEXP;
    int mMaxHP;
    int mCurrentHP;
    int mMaxCost;
    int mCurrentCost;
    int ATK;
    int DEF;
    float SPD;
    

    public List<int> Deck
    {
        set { deck = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        game_manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        inventory = new List<int>();
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (game_manager.game_init)
        {
            Shuffle();
            game_manager.game_init = false;
        }
        if (mCurrentHP <= 0)
        {
            deck = null;
        }
    }

    void init()
    {
        inventory.Clear();
        game_manager.InitPlayerDeck();
        level = 1;
        mCurrentEXP = 0;
        mMaxEXP = 100;
        mMaxHP = 10;
        mCurrentHP = mMaxHP;
        mMaxCost = 5;
        mCurrentCost = mMaxCost;
        ATK = 1;
        DEF = 0;
        SPD = 30.0f;
    }

    #region PUBLIC_METHOD
    public int DrawCard(int index)
    {
        if (index < deck.Count)
            return deck[index];
        else return -1;
    }

    public float GetSpeed()
    {
        return SPD;
    }

    public int GetAtk()
    {
        return ATK;
    }

    public int GetCurrentHP()
    {
        return mCurrentHP;
    }

    public int GetMaxHP()
    {
        return mMaxHP;
    }

    public int GetCurrentCost()
    {
        return mCurrentCost;
    }

    public int GetMaxCost()
    {
        return mMaxCost;
    }

    public void SetCurrentCost(int cost)
    {
        mCurrentCost = cost;
    }

    public void Shuffle()
    {
        for (int i = 0; i < deck.Count - 1; ++i)
        {
            int index = Random.Range(i, deck.Count);

            int temp = deck[i];
            deck[i] = deck[index];
            deck[index] = temp;
        }

        string log = "";

        foreach (int card in deck)
        {
            log += card + "\n";
        }
        Debug.Log(log);
    }

    public void ApplyEffect(Category category, int value)
    {
        switch (category)
        {
            case Category.Deal:
                {
                    int deal = value - DEF;
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
        }

        Debug.Log("Player hp : " + mCurrentHP);
    }

    public void AddEXP(int exp)
    {
        mCurrentEXP += exp;
        if(mCurrentEXP > mMaxEXP)
        {
            level += 1;
            mCurrentEXP -= mMaxEXP;
            mMaxEXP += level * 100;
            mCurrentEXP = 0;
        }
    }

    #endregion

}
