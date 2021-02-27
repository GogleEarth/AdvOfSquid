using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GenericScript;

public class BattleManager : MonoBehaviour
{
    #region SERIALIZE_FIELD
    [SerializeField]
    GameObject mCardPrefab;
    [SerializeField]
    GameObject mSkillPrefab;
    [SerializeField]
    GameObject mBattleStage;
    [SerializeField]
    GameObject mGameManager;
    [SerializeField]
    GameObject mPlayer;
    [SerializeField]
    GameObject mPlayerIcon;
    [SerializeField]
    GameObject mEnemyIcon;
    [SerializeField]
    GameObject mEnemy;
    [SerializeField]
    Text mHPText;
    [SerializeField]
    Text mCostText;
    [SerializeField]
    Text mEnemyHPText;
    [SerializeField]
    GameObject mEnemySKillPanel;

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
    public void Init(bool battleStart)
    {
        mIsBattleStart = battleStart;
        mPlayerTurnGuage = 0.0f;
        mEnemyTurnGuage = 0.0f;
        mPlayerHand = 0;
        mCurrnetCardIndex = 0;
        mIsPause = false;
        mIsSomebodysTurn = false;
        mIsPlayerTurnEnd = false;
        mIsPlayerTurn = false;
        mMaxHand = 10;
        if (battleStart)
        {
            mEnemy.GetComponent<Enemy>().InitByMonsterData(
                mGameManager.GetComponent<GameManager>().GetMonsterByIndex(Random.Range(0, 2)));
            mEnemy.GetComponent<Enemy>().Init(mGameManager.GetComponent<GameManager>().floor);
            mEnemyIcon.GetComponent<Image>().sprite = 
                mGameManager.GetComponent<GameManager>().
                FindImageByName(mEnemy.GetComponent<Enemy>().GetIconName());
            List<Skill> skills = mEnemy.GetComponent<Enemy>().GetSkills();
            foreach (var skill in skills)
            {
                GameObject skillIcon = Instantiate(mSkillPrefab);
                RectTransform skillPosition = skillIcon.GetComponent<RectTransform>();
                skillPosition.SetParent(mEnemySKillPanel.transform);
                skillPosition.position = Vector3.zero;
                skillPosition.localScale = Vector3.one;
                string imageName = skill.GetSkillIcon();
                skillIcon.GetComponent<Image>().sprite =
                    mGameManager.GetComponent<GameManager>().FindImageByName(imageName);
                skillIcon.name = skill.GetSkillName();
                int cooltime = skill.GetCooltime();
                if (cooltime > 0)
                {
                    skillIcon.transform.Find("Cooltime").
                        transform.Find("CooltimeText").
                        GetComponent<Text>().text = "" + cooltime;
                }
                else
                {
                    skillIcon.transform.Find("Cooltime").
                        transform.Find("CooltimeText").
                        GetComponent<Text>().text = "";
                }
            }
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
    }

    public void EndCardDrag()
    {
        mPlayerHand++;
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
                    case Category.Deal:
                        {
                            applyEffect(cardEffect.category, cardEffect.target, 
                                cardEffect.value + mPlayer.GetComponent<Player>().GetAtk());
                            break;
                        }
                    case Category.Heal:
                        {
                            applyEffect(cardEffect.category, cardEffect.target, cardEffect.value);
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
        //mPlayerIcon.GetComponent<Image>().sprite = 
        //    mGameManager.GetComponent<GameManager>().
        //    FindImageByName(mPlayer.GetComponent<Player>().GetIconName());
    }

    void Update()
    {
        if(mIsBattleStart && !mIsPause)
        {
            mHPText.text = mPlayer.GetComponent<Player>().GetCurrentHP() + " / " 
                + mPlayer.GetComponent<Player>().GetMaxHP();
            mCostText.text = mPlayer.GetComponent<Player>().GetCurrentCost() + "";
            mEnemyHPText.text = mEnemy.GetComponent<Enemy>().GetCurrentHP() + " / "
                + mEnemy.GetComponent<Enemy>().GetMaxHP();

            if (mPlayer.GetComponent<Player>().GetCurrentHP() > 0 &&
                mEnemy.GetComponent<Enemy>().GetCurrentHP() > 0)
            {
                if (mPlayerTurnGuage >= 100.0f)
                {
                    mIsPlayerTurn = true;
                    // 코스트 회복
                    mPlayer.GetComponent<Player>().SetCurrentCost(mPlayer.GetComponent<Player>().GetMaxCost());
                    // 플레이어 버프/디버프 체크
                    // 카드 드로우
                    doCardDrow(5 - mPlayerHand);
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
            else
            {
                mIsBattleStart = false;
                if (mPlayer.GetComponent<Player>().GetCurrentHP() <= 0)
                {
                    mPlayer.GetComponent<Player>().AddEXP(mEnemy.GetComponent<Enemy>().GetEXP());
                }
                else if(mEnemy.GetComponent<Enemy>().GetCurrentHP() <= 0)
                {
                    
                }
                Init(false);
                mBattleStage.SetActive(false);
                
            }
        }
    }

    void doCardDrow(int numberOfCard)
    {
        if (numberOfCard > 0)
        {
            for (int i = 0; i < numberOfCard; i++)
            {
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
                    cardPosition.position = mBattleStage.transform.Find("Deck").position;
                    cardPosition.localScale = Vector3.one;
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

    void applyEffect(Category category, Target target, int value)
    {
        switch (target)
        {
            case Target.Own:
                {
                    mPlayer.GetComponent<Player>().ApplyEffect(category, value);
                    break;
                }
            case Target.Both:
                {
                    mPlayer.GetComponent<Player>().ApplyEffect(category, value);
                    mEnemy.GetComponent<Enemy>().ApplyEffect(category, value);
                    break;
                }
            case Target.Enemy:
                {
                    mEnemy.GetComponent<Enemy>().ApplyEffect(category, value);
                    break;
                }
        }
    }

    void applyEffect(Category category, Target target, int value, bool isSkill)
    {
        switch (target)
        {
            case Target.Own:
                {
                    mEnemy.GetComponent<Enemy>().ApplyEffect(category, value);
                    break;
                }
            case Target.Both:
                {
                    mPlayer.GetComponent<Player>().ApplyEffect(category, value);
                    mEnemy.GetComponent<Enemy>().ApplyEffect(category, value);
                    break;
                }
            case Target.Enemy:
                {
                    mPlayer.GetComponent<Player>().ApplyEffect(category, value);
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
        mIsSomebodysTurn = true;
        Enemy enemy = mEnemy.GetComponent<Enemy>();
        int skillIndex = enemy.GetReadySkill();
        Skill skill = mGameManager.GetComponent<GameManager>().GetSkillByIndex(skillIndex);

        print(skill.Display());
        foreach (SkillEffect effect in skill.GetEffects())
        {
            applyEffect(effect.category, effect.target, effect.value + enemy.GetATK(), true);
        }

        enemy.SetSkillCooltimeMax(skillIndex);
        enemy.ReduceAllSkillCooltime(1);

        List<int> cooltimes = mEnemy.GetComponent<Enemy>().GetSkillCooltime();
        for (int i = 0; i < mEnemySKillPanel.transform.childCount; i++)
        {

            mEnemySKillPanel.transform.GetChild(i).transform.Find("Cooltime").
                        transform.Find("CooltimeText").
                        GetComponent<Text>().text = "" + cooltimes[i];
        }

        mIsSomebodysTurn = false;
        yield return null;
    }

    #endregion

}
