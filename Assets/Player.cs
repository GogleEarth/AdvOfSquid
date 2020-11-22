using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericScript;

public class Player : MonoBehaviour
{
    List<int> inventory;
    List<int> deck;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<int>();
        deck = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
