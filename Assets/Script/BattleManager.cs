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
    [SerializeField]
    GameObject mResultPanel;

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
            int remainCard = mBattleStage.transform.Find("Hand").transform.childCount;
            if (remainCard > 0)
            {
                for (int i = 0; i < remainCard; i++)
                {
                    Destroy(mBattleStage.transform.Find("Hand").transform.GetChild(i).gameObject);
                }
            }

            int remainSkill = mEnemySKillPanel.transform.childCount;
            if (remainSkill > 0)
            {
                for (int i = 0; i < remainSkill; i++)
                {
                    Destroy(mEnemySKillPanel.transform.GetChild(i).gameObject);
                }
            }

            mEnemy.GetComponent<Enemy>().InitByMonsterData(
                mGameManager.GetComponent<GameManager>().GetMonsterByIndex(Random.Range(0, 2)));
            mEnemy.GetComponent<Enemy>().Init(mGameManager.GetComponent<GameManager>().floor);
            mEnemyIcon.GetComponent<Image>().sprite = 
                mGameManager.GetComponent<GameManager>().
                FindImageByName(mEnemy.GetComponent<Enemy>().IconName);
            List<Skill> skills = mEnemy.GetComponent<Enemy>().GetSkills();
            foreach (var skill in skills)
            {
                GameObject skillIcon = Instantiate(mSkillPrefab);
                RectTransform skillPosition = skillIcon.GetComponent<RectTransform>();
                skillPosition.SetParent(mEnemySKillPanel.transform);
                skillPosition.position = Vector3.zero;
                skillPosition.localScale = Vector3.one;
                string imageName = skill.SkillIcon;
                skillIcon.GetComponent<Image>().sprite =
                    mGameManager.GetComponent<GameManager>().FindImageByName(imageName);
                skillIcon.name = skill.SkillName;
                int cooltime = skill.Cooltime;
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

    public bool IsBattleStart => mIsBattleStart;

    public int CurrnetHand => mPlayerHand;

    public void BeginCardDrag()
    {
        mPlayerHand--;
    }

    public void EndCardDrag()
    {
        mPlayerHand++;
    }

    public bool IsPlayerTurn => mIsPlayerTurn;

    public void UseCard(Card card)
    {
        if (mIsPlayerTurn)
        {
            mPlayer.GetComponent<Player>().SetCurrentCost(
                mPlayer.GetComponent<Player>().CurrentCost - card.Cost);
            foreach (CardEffect cardEffect in card.CardEffects)
            {
                switch (cardEffect.category)
                {
                    case Category.Deal:
                        {
                            ApplyEffect(cardEffect.category, cardEffect.target, 
                                cardEffect.value + mPlayer.GetComponent<Player>().Atk);
                            break;
                        }
                    case Category.Heal:
                        {
                            ApplyEffect(cardEffect.category, cardEffect.target, cardEffect.value);
                            break;
                        }
                }
            }
        }
    }

    public int CurrentCost => mPlayer.GetComponent<Player>().CurrentCost;

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
            mHPText.text = mPlayer.GetComponent<Player>().CurrentHP + " / " 
                + mPlayer.GetComponent<Player>().MaxHP;
            mCostText.text = mPlayer.GetComponent<Player>().CurrentCost + "";
            mEnemyHPText.text = mEnemy.GetComponent<Enemy>().CurrentHP + " / "
                + mEnemy.GetComponent<Enemy>().MaxHP;

            if (mPlayer.GetComponent<Player>().CurrentHP > 0 &&
                mEnemy.GetComponent<Enemy>().CurrentHP > 0)
            {
                if (mPlayerTurnGuage >= 100.0f)
                {
                    mIsPlayerTurn = true;
                    // 코스트 회복
                    mPlayer.GetComponent<Player>().SetCurrentCost(mPlayer.GetComponent<Player>().MaxCost);
                    // 플레이어 버프/디버프 체크
                    // 카드 드로우
                    DoCardDrow(5 - mPlayerHand);
                    // 코루틴스타트
                    StartCoroutine(PlayerTurnCoroutine);
                    mPlayerTurnGuage = 0.0f;
                }

                if (mEnemyTurnGuage >= 100.0f)
                {
                    mIsSomebodysTurn = true;
                    StartCoroutine(EnemyTurnCoroutine);
                    mEnemyTurnGuage = 0.0f;
                }

                if (!mIsSomebodysTurn && !mIsPlayerTurn)
                {
                    mPlayerTurnGuage += mPlayer.GetComponent<Player>().Speed / 100.0f;
                    Vector3 playerIconPositon = mPlayerIcon.transform.localPosition;
                    playerIconPositon.y = mPlayerTurnGuage * -6.0f + 300.0f;
                    mPlayerIcon.transform.localPosition = playerIconPositon;

                    mEnemyTurnGuage += mEnemy.GetComponent<Enemy>().Speed / 100.0f;
                    Vector3 enemyIconPositon = mEnemyIcon.transform.localPosition;
                    enemyIconPositon.y = mEnemyTurnGuage * -6.0f + 300.0f;
                    mEnemyIcon.transform.localPosition = enemyIconPositon;
                }
            }
            else
            {
                if (mPlayer.GetComponent<Player>().CurrentHP <= 0)
                {
                    mResultPanel.GetComponent<Result>();
                }
                else if(mEnemy.GetComponent<Enemy>().CurrentHP <= 0)
                {
                    mResultPanel.SetActive(true);
                    mPlayer.GetComponent<Player>().AddEXP(mEnemy.GetComponent<Enemy>().EXP);
                    mIsBattleStart = false;
                }

                mIsPause = true;
            }
        }
    }

    void DoCardDrow(int numberOfCard)
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
                    string imageName = drawnCard.ImageName;
                    card.transform.Find("CardImage").gameObject.GetComponent<Image>().sprite =
                        mGameManager.GetComponent<GameManager>().FindImageByName(imageName);
                    card.transform.Find("TextPanel").transform.Find("CardText").GetComponent<Text>().text =
                        drawnCard.EffectText;
                    card.transform.Find("NamePanel").transform.Find("NameText").GetComponent<Text>().text =
                        drawnCard.CardName;
                    card.transform.Find("CostPanel").transform.Find("CostText").GetComponent<Text>().text =
                        drawnCard.Cost + "";

                    card.name = drawnCard.CardName;
                    card.GetComponent<CardObject>().Init(drawnCard, mPlayerHand);

                    mPlayerHand++;
                }
            }
        }
    }

    void ApplyEffect(Category category, Target target, int value)
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

    void ApplyEffect(Category category, Target target, int value, bool isSkill)
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

    IEnumerator PlayerTurnCoroutine
    {
        get
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
    }

    IEnumerator EnemyTurnCoroutine
    {
        get
        {
            mIsSomebodysTurn = true;
            Enemy enemy = mEnemy.GetComponent<Enemy>();
            int skillIndex = enemy.ReadySkill;
            Skill skill = mGameManager.GetComponent<GameManager>().GetSkillByIndex(skillIndex);

            print(skill.Display);
            foreach (SkillEffect effect in skill.Effects)
            {
                ApplyEffect(effect.category, effect.target, effect.value + enemy.ATK, true);
            }

            enemy.SetSkillCooltimeMax(skillIndex);
            enemy.ReduceAllSkillCooltime(1);

            List<int> cooltimes = mEnemy.GetComponent<Enemy>().SkillCooltime;
            for (int i = 0; i < mEnemySKillPanel.transform.childCount; i++)
            {

                mEnemySKillPanel.transform.GetChild(i).transform.Find("Cooltime").
                            transform.Find("CooltimeText").
                            GetComponent<Text>().text = "" + cooltimes[i];
            }

            mIsSomebodysTurn = false;
            yield return null;
        }
    }

    #endregion

}
