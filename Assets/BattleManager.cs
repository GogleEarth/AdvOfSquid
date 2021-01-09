using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GenericScript;

public class BattleManager : MonoBehaviour
{
    #region PUBLIC
    public GameObject mCardPrefab;
    public GameObject mBattleStage;
    public GameObject mPlayer;
    public GameObject mGameManager;
    #endregion

    #region PRIVATE
    bool mIsBattleStart;
    float mPlayerTurnGuage;
    float mEnemyTurnGuage;
    int mPlayerHand;
    int mCurrnetCardIndex;
    #endregion

    #region PUBLIC_METHOD
    public void Init(bool gameStart)
    {
        mIsBattleStart = gameStart;
        mPlayerTurnGuage = 0.0f;
        mEnemyTurnGuage = 0.0f;
        mPlayerHand = 0;
        mCurrnetCardIndex = 0;
    }

    #endregion

    #region PRIVATE_METHOD
    void Start()
    {
        Init(false);
    }

    void Update()
    {
        if(mIsBattleStart)
        {
            if(mPlayerTurnGuage >= 100.0f)
            {
                // 플레이어 버프/디버프 체크
                // 카드 드로우
                doCardDrow(5-mPlayerHand);
            }
            mPlayerTurnGuage += 10.0f;
        }
    }

    void doCardDrow(int numberOfCard)
    {
        if (numberOfCard > 0)
        {
            for (int i = 0; i < numberOfCard; i++)
            {
                int drawnCardIndex = mPlayer.GetComponent<Player>().drawCard(mCurrnetCardIndex++);
                if (drawnCardIndex == -1) Debug.Log("OutOfIndex");
                else
                {
                    GameObject card = Instantiate(mCardPrefab);
                    RectTransform cardPosition = card.GetComponent<RectTransform>();
                    cardPosition.SetParent(mBattleStage.transform);
                    Card drawnCard = mGameManager.GetComponent<GameManager>().findCard(drawnCardIndex);
                    string imageName = drawnCard.getImageName();
                    card.transform.Find("CardImage").gameObject.GetComponent<Image>().sprite =
                        mGameManager.GetComponent<GameManager>().findImageByName(imageName.Substring(0, imageName.Length-4));
                    card.transform.Find("TextPanel").transform.Find("CardText").GetComponent<Text>().text =
                        drawnCard.getEffectText();

                    mPlayerHand++;
                }
            }
        }
    }

    #endregion

}
