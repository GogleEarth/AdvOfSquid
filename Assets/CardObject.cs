using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GenericScript;

public class CardObject : MonoBehaviour, IPointerClickHandler
{
    #region PUBLIC
    #endregion

    #region PRIVATE
    Card mCardData;

    #endregion

    #region PUBLIC_METHOD

    public void Init(Card cardData)
    {
        mCardData = cardData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) 
        { 
            Debug.Log(mCardData.Display()); 
        } 
        else if (eventData.button == PointerEventData.InputButton.Middle) 
        { 
            Debug.Log("Mouse Click Button : Middle");
        } 
        else if (eventData.button == PointerEventData.InputButton.Right) 
        {
            Debug.Log("Mouse Click Button : Right"); 
        } 
        Debug.Log("Mouse Position : " + eventData.position);
        Debug.Log("Mouse Click Count : " + eventData.clickCount);
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
