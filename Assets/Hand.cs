using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    #region PUBLIC

    public GameObject mBattleManager;
    public float mRotateSpeed;

    #endregion

    #region PRIVATE

    int mMaxHand;

    #endregion

    #region PUBLIC_METHOD

    #endregion

    #region PRIVATE_METHOD

    // Start is called before the first frame update
    void Start()
    {
        mMaxHand = 10;
    }

    // Update is called once per frame
    void Update()
    {
        int currentCard = mBattleManager.GetComponent<BattleManager>().GetCurrnetHand();
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform card = transform.GetChild(i);

                float interval = 1.5f / mMaxHand + 1;
                float t = -interval * 0.5f * (currentCard - 1) + interval * i;
                float ypos = 10.0f * Mathf.Cos(Mathf.PI / 2.0f * t);
                Vector3 pos = card.localPosition;
                pos.y = ypos;
                pos.x = 75.0f * t;
                StartCoroutine(RotateCard(card, pos, Quaternion.Euler(0.0f, 0.0f, -(Mathf.PI / 2.0f * t))));
            }
        }
    }

    IEnumerator RotateCard(Transform card, Vector3 destPos, Quaternion destRot)
    {
        card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, destPos, mRotateSpeed * Time.deltaTime);
        card.localRotation = Quaternion.Lerp(card.transform.localRotation, destRot, mRotateSpeed * Time.deltaTime);
        yield return null;
    }

    #endregion
}
