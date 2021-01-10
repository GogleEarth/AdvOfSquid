using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericScript;

public class Player : MonoBehaviour
{
    List<int> inventory;
    List<int> deck;

    int level;
    int hp;
    int cost;
    float atk;
    float def;
    float spd;
    

    public List<int> Deck
    {
        set { deck = value; }
    }

    public GameManager game_manager;
    // Start is called before the first frame update
    void Start()
    {
        game_manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        inventory = new List<int>();
        hp = 10;
        spd = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (game_manager.game_init)
        {
            Suffle();
            game_manager.game_init = false;
        }
        if (hp <= 0)
        {
            deck = null;
        }
    }

    void Suffle()
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

    #region PUBLIC_METHOD
    public int drawCard(int index)
    {
        if (index < deck.Count)
            return deck[index];
        else return -1;
    }

    public float getSpeed()
    {
        return spd;
    }

    #endregion

}
