using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GenericScript;

public class CardUse : MonoBehaviour, IDropHandler
{
    #region PUBLIC

    public GameObject mBattleManager;

    #endregion

    #region PRIVATE

    static GameObject mSelectedCard;

    #endregion

    #region PUBLIC_METHOD

    public void OnDrop(PointerEventData eventData)
    {
        if (mSelectedCard != null)
        {
            if (mBattleManager.GetComponent<BattleManager>().CurrentCost
                >= mSelectedCard.GetComponent<CardObject>().Card.Cost &&
                mBattleManager.GetComponent<BattleManager>().IsPlayerTurn)
            {
                mBattleManager.GetComponent<BattleManager>().
                    UseCard(mSelectedCard.GetComponent<CardObject>().Card);
                Destroy(mSelectedCard);
                mSelectedCard = null;
            }
        }
    }

    public void SetSelectedCard(GameObject card)
    {
        mSelectedCard = card;
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
