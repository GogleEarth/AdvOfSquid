using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    #region SERIALIZE_FIELD

    [SerializeField]
    Player mPlayer;

    #endregion

    #region PUBLIC

    #endregion

    #region PRIVATE

    #endregion

    #region PUBLIC_METHOD

    #endregion

    #region PRIVATE_METHOD

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.enabled)
        {
            Debug.Log("결과창 진행중");
        }
    }

    IEnumerator EnemyTurnCoroutine
    {
        get
        {
            yield return null;
        }
    }

    #endregion
}
