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
    public GameObject mGameManager;
    public GameObject mPlayer;
    public GameObject mPlayerIcon;
    public GameObject mEnemyIcon;
    public GameObject mEnemy;
    public Text mHPText;
    public Text mCostText;

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
    int mMaxHand;
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
        mMaxHand = 10;
        if (gameStart)
        {
            mEnemy.GetComponent<Enemy>().InitByMonsterData(mGameManager.GetComponent<GameManager>().GetMonsterByIndex(Random.Range(0,2)));
            mEnemy.GetComponent<Enemy>().Init(mGameManager.GetComponent<GameManager>().floor);
            mEnemyIcon.GetComponent<Image>().sprite = 
                mGameManager.GetComponent<GameManager>().
                FindImageByName(mEnemy.GetComponent<Enemy>().GetIconName());
            //mPlayerIcon.GetComponent<Image>().sprite = 
            //    mGameManager.GetComponent<GameManager>().
            //    FindImageByName(mPlayer.GetComponent<Player>().GetIconName());
        }
    }

    public void DoEndTurn()
    {
        if (mIsPlayerTurn)
        {
            mIsPlayerTurnEnd = true;
        }
    }

    public bool IsBattleStart()
    {
        return mIsBattleStart;
    }

    public int GetCurrnetHand()
    {
        return mPlayerHand;
    }

    public void BeginCardDrag()
    {
        mPlayerHand--;
        Debug.Log("현재 카드 매수 : " + mPlayerHand);

    }

    public void EndCardDrag()
    {
        mPlayerHand++;
        Debug.Log("현재 카드 매수 : " + mPlayerHand);

    }

    public bool IsPlayerTurn()
    {
        return mIsPlayerTurn;
    }

    public void UseCard(Card card)
    {
        if (mIsPlayerTurn)
        {
            mPlayer.GetComponent<Player>().SetCurrentCost(
                mPlayer.GetComponent<Player>().GetCurrentCost() - card.GetCost());
            foreach (CardEffect cardEffect in card.GetCardEffects())
            {
                switch (cardEffect.category)
                {
                    case CardCategory.Deal:
                    case CardCategory.Heal:
                        {
                            applyCardEffect(cardEffect.category, cardEffect.target, cardEffect.value);
                            break;
                        }
                }
            }
        }
    }

    public int GetCurrnetCost()
    {
        return mPlayer.GetComponent<Player>().GetCurrentCost();
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
            mHPText.text = mPlayer.GetComponent<Player>().GetCurrentHP() + " / " 
                + mPlayer.GetComponent<Player>().GetMaxHP();
            mCostText.text = mPlayer.GetComponent<Player>().GetCurrentCost() + "";
            if (mPlayerTurnGuage >= 100.0f)
            {
                mIsPlayerTurn = true;
                // 코스트 회복
                mPlayer.GetComponent<Player>().SetCurrentCost(mPlayer.GetComponent<Player>().GetMaxCost());
                // 플레이어 버프/디버프 체크
                // 카드 드로우
                Debug.Log("현재 카드 매수 : " + mPlayerHand);
                doCardDrow(5-mPlayerHand);
                // 코루틴스타트
                StartCoroutine("playerTurnCoroutine");
                mPlayerTurnGuage = 0.0f;
            }

            if (mEnemyTurnGuage >= 100.0f)
            {
                mIsSomebodysTurn = true;
                StartCoroutine("enemyTurnCoroutine");
                mEnemyTurnGuage = 0.0f;
            }

            if (!mIsSomebodysTurn && !mIsPlayerTurn)
            {
                mPlayerTurnGuage += mPlayer.GetComponent<Player>().GetSpeed() / 100.0f;
                Vector3 playerIconPositon = mPlayerIcon.transform.localPosition;
                playerIconPositon.y = mPlayerTurnGuage * -6.0f + 300.0f;
                mPlayerIcon.transform.localPosition = playerIconPositon;

                mEnemyTurnGuage += mEnemy.GetComponent<Enemy>().GetSpeed() / 100.0f;
                Vector3 enemyIconPositon = mEnemyIcon.transform.localPosition;
                enemyIconPositon.y = mEnemyTurnGuage * -6.0f + 300.0f;
                mEnemyIcon.transform.localPosition = enemyIconPositon;
            }
        }
    }

    void doCardDrow(int numberOfCard)
    {
        Debug.Log("드로우 매수 : " + numberOfCard);
        if (numberOfCard > 0)
        {
            for (int i = 0; i < numberOfCard; i++)
            {
                Debug.Log("draw card : " + mCurrnetCardIndex);
                if (mPlayerHand < mMaxHand)
                {
                    int drawnCardIndex = mPlayer.GetComponent<Player>().DrawCard(mCurrnetCardIndex++);
                    if (drawnCardIndex == -1)
                    {
                        Debug.Log("OutOfIndex");
                        mPlayer.GetComponent<Player>().Shuffle();
                        drawnCardIndex = 0;
                        mCurrnetCardIndex = 0;
                    }

                    GameObject card = Instantiate(mCardPrefab);
                    RectTransform cardPosition = card.GetComponent<RectTransform>();
                    cardPosition.SetParent(mBattleStage.transform.Find("Hand"));
                    cardPosition.position = mBattleStage.transform.Find("Hand").position;
                    Card drawnCard = mGameManager.GetComponent<GameManager>().FindCard(drawnCardIndex);
                    string imageName = drawnCard.GetImageName();
                    card.transform.Find("CardImage").gameObject.GetComponent<Image>().sprite =
                        mGameManager.GetComponent<GameManager>().FindImageByName(imageName);
                    card.transform.Find("TextPanel").transform.Find("CardText").GetComponent<Text>().text =
                        drawnCard.GetEffectText();
                    card.transform.Find("NamePanel").transform.Find("NameText").GetComponent<Text>().text =
                        drawnCard.GetCardName();
                    card.transform.Find("CostPanel").transform.Find("CostText").GetComponent<Text>().text =
                        drawnCard.GetCost() + "";

                    card.name = drawnCard.GetCardName();
                    card.GetComponent<CardObject>().Init(drawnCard, mPlayerHand);

                    mPlayerHand++;
                }
            }
        }
    }

    void applyCardEffect(CardCategory category, CardTarget target, int value)
    {
        switch (target)
        {
            case CardTarget.Own:
                {
                    mPlayer.GetComponent<Player>().ApplyCardEffect(category, value);
                    break;
                }
            case CardTarget.Both:
                {
                    mPlayer.GetComponent<Player>().ApplyCardEffect(category, value);
                    mEnemy.GetComponent<Enemy>().ApplyCardEffect(category, value);
                    break;
                }
            case CardTarget.Enemy:
                {
                    mEnemy.GetComponent<Enemy>().ApplyCardEffect(category, value);
                    break;
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
                mIsPlayerTurnEnd = false;
                mIsPlayerTurn = false;
                yield break;
            }
        }
    }

    IEnumerator enemyTurnCoroutine()
    {
        mIsSomebodysTurn = false;
        yield return null;
    }

    #endregion

}
