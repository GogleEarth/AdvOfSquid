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
    public GameObject mPlayerIcon;
    public GameObject mEnemyIcon;

    #endregion

    #region PRIVATE
    bool mIsBattleStart;
    bool mIsPause;
    bool mIsSomebodysTurn;
    bool mIsPlayerTurnEnd;
    bool mIsPlayerTurn;
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
        mIsPause = false;
        mIsSomebodysTurn = false;
        mIsPlayerTurnEnd = false;
        mIsPlayerTurn = false;
    }

    public void DoEndTurn()
    {
        if(mIsPlayerTurn)
            mIsPlayerTurnEnd = true;
    }

    public bool IsBattleStart()
    {
        return mIsBattleStart;
    }
    #endregion

    #region PRIVATE_METHOD
    void Start()
    {
        Init(false);
    }

    void Update()
    {
        if(mIsBattleStart && !mIsPause)
        {
            if(mPlayerTurnGuage >= 100.0f)
            {
                mIsSomebodysTurn = true;
                mIsPlayerTurn = true;
                // 플레이어 버프/디버프 체크
                // 카드 드로우
                doCardDrow(5-mPlayerHand);
                // 코루틴스타트
                StartCoroutine("playerTurnCoroutine");
                mPlayerTurnGuage = 0.0f;
            }

            if (!mIsSomebodysTurn)
            {
                mPlayerTurnGuage += mPlayer.GetComponent<Player>().getSpeed() / 100.0f;
                Vector3 playerIconPositon = mPlayerIcon.transform.localPosition;
                playerIconPositon.y = mPlayerTurnGuage * -6.0f + 300.0f;
                mPlayerIcon.transform.localPosition = playerIconPositon;


            }
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
                    cardPosition.SetParent(mBattleStage.transform.Find("Hand").transform.Find("HorizontalLayoutGroup"));
                    cardPosition.position = mBattleStage.transform.Find("Hand").position;
                    Card drawnCard = mGameManager.GetComponent<GameManager>().FindCard(drawnCardIndex);
                    string imageName = drawnCard.GetImageName();
                    card.transform.Find("CardImage").gameObject.GetComponent<Image>().sprite =
                        mGameManager.GetComponent<GameManager>().FindImageByName(imageName.Substring(0, imageName.Length-4));
                    card.transform.Find("TextPanel").transform.Find("CardText").GetComponent<Text>().text =
                        drawnCard.GetEffectText();
                    card.name = drawnCard.GetCardName();
                    card.GetComponent<CardObject>().Init(drawnCard);

                    mPlayerHand++;
                }
            }
        }
    }

    IEnumerator playerTurnCoroutine()
    {
        while (true)
        {
            yield return null;
            //Debug.Log("플레이어 차례진행중");
            if (mIsPlayerTurnEnd)
            {
                mIsSomebodysTurn = false;
                mIsPlayerTurnEnd = false;
                mIsPlayerTurn = false;
                yield break;
            }
        }
    }

    #endregion

}
