using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericScript;

public class Player : MonoBehaviour
{
    List<Artifact> inventory;
    List<int> deck;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<Artifact>();
        deck = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
