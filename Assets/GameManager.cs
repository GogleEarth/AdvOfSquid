using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool game_start;
    // Start is called before the first frame update
    void Start()
    {
        game_start = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameStart(bool start)
    {
        game_start = start;
    }
}
