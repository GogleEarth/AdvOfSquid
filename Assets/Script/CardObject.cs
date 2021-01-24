using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GenericScript;

public class CardObject : MonoBehaviour, 
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region PUBLIC

     

    #endregion

    #region PRIVATE

    Card mCardData;
    Transform mStartParent;
    public int mIndexInHand;

    #endregion

    #region PUBLIC_METHOD

    public void Init(Card cardData, int index)
    {
        mCardData = cardData;
        mIndexInHand = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) 
        { 
            Debug.Log(mCardData.Display()); 
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mStartParent = transform.parent;
        transform.SetParent(GameObject.Find("BattleStage").transform);
        GameObject.Find("BattleManager").GetComponent<BattleManager>().BeginCardDrag();
        GameObject.Find("UseCard").GetComponent<CardUse>().SetSelectedCard(gameObject);
        gameObject.GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        Vector3 scale = new Vector3(2.0f, 2.0f);
        transform.localScale = scale;
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(mStartParent);
        GameObject.Find("BattleManager").GetComponent<BattleManager>().EndCardDrag();
        transform.localScale = Vector3.one;
        GameObject.Find("UseCard").GetComponent<CardUse>();
        gameObject.GetComponent<Image>().raycastTarget = true;
    }

    public void SetIndex(int index)
    {
        mIndexInHand = index;
    }
    
    public int GetIndex()
    {
        return mIndexInHand;
    }

    public Card GetCard()
    {
        return mCardData;
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
