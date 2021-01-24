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
    float mMaxEXP;
    float mCurrentEXP;
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
        mCurrentEXP = 0.0f;
        mMaxEXP = 100.0f;
        mMaxHP = 10;
        mCurrentHP = mMaxHP;
        mMaxCost = 5;
        mCurrentCost = mMaxCost;
        ATK = 1;
        DEF = 1;
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

    public void ApplyCardEffect(CardCategory category, int value)
    {
        switch (category)
        {
            case CardCategory.Deal:
                {
                    mCurrentHP -= value;
                    break;
                }
            case CardCategory.Heal:
                {
                    if (mCurrentHP < mMaxHP)
                    {
                        mCurrentHP += value;
                    }
                    break;
                }
        }

        Debug.Log("Player hp : " + mCurrentHP);
    }

    #endregion

}
