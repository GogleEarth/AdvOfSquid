using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using GenericScript;
using System.IO;

public class GameManager : MonoBehaviour
{
    bool game_start;
    public bool game_init = false;
    public int stage = 0;
    public int floor = 0;
    public GameObject selected_stage;
    public Player player;
    public Image battle_stage;
    public Image boss_stage;
    public Image heal_stage;
    public Image shop_stage;
    public Image void_stage;
    public GameObject mBattleManger;

    List<Card> CardList;
    List<Artifact> ArtifactList;
    List<Monster> mMonsters;
    List<Skill> mSkills;
    Sprite[] mSprites;

    void Start()
    {
        game_start = false;
        selected_stage = null;

        CardList = new List<Card>();
        ArtifactList = new List<Artifact>();
        mSkills = new List<Skill>();
        mMonsters = new List<Monster>();
        mSprites = Resources.LoadAll<Sprite>("Image");

        foreach (var item in mSprites)
        {
            Debug.Log(item.texture.name);
        }

        loadCardData();
        //loadArtiData();
        loadSkillData();
        loadMonsterData();
        InitPlayerDeck();

        game_init = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(selected_stage)
        {
            if (selected_stage.GetComponent<StageObject>().stage == stage)
            {
                switch (selected_stage.GetComponent<StageObject>().type)
                {
                    case 0:
                        Debug.Log("전투 스테이지");
                        battle_stage.gameObject.SetActive(true);
                        mBattleManger.GetComponent<BattleManager>().Init(true);
                        break;
                    case 1:
                        Debug.Log("회복 스테이지");
                        heal_stage.gameObject.SetActive(true);

                        break;
                    case 2:
                        Debug.Log("상점 스테이지");
                        shop_stage.gameObject.SetActive(true);

                        break;
                    case 3:
                        Debug.Log("보스 스테이지");
                        boss_stage.gameObject.SetActive(true);

                        break;
                    case 4:
                        Debug.Log("빈 스테이지");
                        void_stage.gameObject.SetActive(true);

                        break;
                }
                stage++;
                selected_stage = null;
            }
        }
    }

    void loadCardData()
    {
        FileStream file = new FileStream("Assets/Resources/cardtest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        if (file.CanRead)
        {
            while (!streamReader.EndOfStream)
            {
                string input_text = streamReader.ReadLine();
                List<CardEffect> cardEffects = new List<CardEffect>();
                List<CardTarget> cardTargets = new List<CardTarget>();
                List<CardCategory> cardCategorys = new List<CardCategory>();
                List<int> cardValues = new List<int>();

                if (input_text == "Info")
                {
                    string card_name = streamReader.ReadLine();
                    string image_name = streamReader.ReadLine();
                    string flavor_text = streamReader.ReadLine();
                    string effect_text = streamReader.ReadLine();
                    string cost = streamReader.ReadLine();
                    string data = streamReader.ReadLine();
                    if (data == "Target")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Category")
                        {
                            cardTargets.Add(GetEnumFromString<CardTarget>(data));
                            data = streamReader.ReadLine();
                        }
                    }
                    if (data == "Category")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Value")
                        {
                            cardCategorys.Add(GetEnumFromString<CardCategory>(data));
                            data = streamReader.ReadLine();
                        }
                    }
                    if (data == "Value")
                    {
                        data = streamReader.ReadLine();
                        while (data != "End")
                        {
                            cardValues.Add(int.Parse(data));
                            data = streamReader.ReadLine();
                        }
                    }

                    for (int i = 0; i < cardTargets.Count; i++)
                    {
                        cardEffects.Add(new CardEffect(cardTargets[i], cardCategorys[i], cardValues[i]));
                    }

                    CardList.Add(new Card(card_name, image_name, flavor_text, effect_text, int.Parse(cost), cardEffects));
                }
            }

            foreach (Card card in CardList)
            {
                Debug.Log(card.Display());
            }
        }
        else
        {
            Debug.Log("flie not found!");
        }

        file.Close();
    }

    void loadArtiData()
    {
        FileStream file = new FileStream("Assets/Resources/artitest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        if (file.CanRead)
        {
            while (!streamReader.EndOfStream)
            {
                string input_text = streamReader.ReadLine();
                List<ArtifactEffect> artiEffects = new List<ArtifactEffect>();
                List<ArtifactTarget> artiTargets = new List<ArtifactTarget>();
                List<ArtifactCategory> artiCategorys = new List<ArtifactCategory>();
                List<int> artiValues = new List<int>();

                if (input_text == "Info")
                {
                    string card_name = streamReader.ReadLine();
                    string image_name = streamReader.ReadLine();
                    string flavor_text = streamReader.ReadLine();
                    string effect_text = streamReader.ReadLine();
                    string data = streamReader.ReadLine();
                    if (data == "Target")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Category")
                        {
                            artiTargets.Add((ArtifactTarget)int.Parse(data));
                            data = streamReader.ReadLine();
                        }
                    }
                    if (data == "Category")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Value")
                        {
                            artiCategorys.Add((ArtifactCategory)int.Parse(data));
                            data = streamReader.ReadLine();
                        }
                    }
                    if (data == "Value")
                    {
                        data = streamReader.ReadLine();
                        while (data != "End")
                        {
                            artiValues.Add(int.Parse(data));
                            data = streamReader.ReadLine();
                        }
                    }

                    for (int i = 0; i < artiTargets.Count; i++)
                    {
                        artiEffects.Add(new ArtifactEffect(artiTargets[i], artiCategorys[i], artiValues[i]));
                    }

                    ArtifactList.Add(new Artifact(card_name, image_name, flavor_text, effect_text, artiEffects));
                }
            }

            foreach (Artifact arti in ArtifactList)
            {
                Debug.Log(arti.Display());
            }
        }
        else
        {
            Debug.Log("flie not found!");
        }

        file.Close();
    }

    void loadMonsterData()
    {
        FileStream file = new FileStream("Assets/Resources/monstertest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        if (file.CanRead)
        {
            while (!streamReader.EndOfStream)
            {
                string input_text = streamReader.ReadLine();

                if (input_text == "Info")
                {
                    string monstername = streamReader.ReadLine();
                    string imagename = streamReader.ReadLine();
                    string iconname = streamReader.ReadLine();
                    int exp = int.Parse(streamReader.ReadLine());
                    float spd = float.Parse(streamReader.ReadLine());
                    int lv = int.Parse(streamReader.ReadLine());
                    int atk = int.Parse(streamReader.ReadLine());
                    int def = int.Parse(streamReader.ReadLine());
                    int hp = int.Parse(streamReader.ReadLine());
                    List<int> skills = new List<int>();
                    string data = streamReader.ReadLine();

                    if (data == "Skill")
                    {
                        data = streamReader.ReadLine();
                        while (data != "End")
                        {
                            skills.Add(int.Parse(data));
                            data = streamReader.ReadLine();
                        }
                    }

                    mMonsters.Add(new Monster(monstername, imagename, iconname, exp, lv, atk, def, hp, spd, skills));
                }
            }

            foreach (Monster monster in mMonsters)
            {
                Debug.Log(monster.Display());
            }
        }
        else
        {
            Debug.Log("flie not found!");
        }

        file.Close();
    }

    void loadSkillData()
    {
        FileStream file = new FileStream("Assets/Resources/skilltest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        if (file.CanRead)
        {
            while (!streamReader.EndOfStream)
            {
                string input_text = streamReader.ReadLine();
                List<SkillEffect> skillEffects = new List<SkillEffect>();
                List<SkillTarget> skillTargets = new List<SkillTarget>();
                List<SkillCategory> skillCategorys = new List<SkillCategory>();
                List<int> skillValues = new List<int>();

                if (input_text == "Info")
                {
                    string skillName = streamReader.ReadLine();
                    string skillIconName = streamReader.ReadLine();
                    string skillText = streamReader.ReadLine();
                    int cooltime = int.Parse(streamReader.ReadLine());
                    string data = streamReader.ReadLine();

                    if (data == "Target")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Category")
                        {
                            skillTargets.Add(GetEnumFromString<SkillTarget>(data));
                            data = streamReader.ReadLine();
                        }
                    }

                    if (data == "Category")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Value")
                        {
                            skillCategorys.Add(GetEnumFromString<SkillCategory>(data));
                            data = streamReader.ReadLine();
                        }
                    }

                    if (data == "Value")
                    {
                        data = streamReader.ReadLine();
                        while (data != "End")
                        {
                            skillValues.Add(int.Parse(data));
                            data = streamReader.ReadLine();
                        }
                    }

                    for (int i = 0; i < skillTargets.Count; i++)
                    {
                        skillEffects.Add(new SkillEffect(skillTargets[i], skillCategorys[i], skillValues[i]));
                    }

                    mSkills.Add(new Skill(skillName, skillIconName, skillText, skillEffects, cooltime));
                }
            }

            foreach (Skill skill in mSkills)
            {
                Debug.Log(skill.Display());
            }
        }
        else
        {
            Debug.Log("flie not found!");
        }

        file.Close();
    }

    T GetEnumFromString<T>(string data)
    {
        return EnumUtil<T>.Parse(data);
    }

    #region PUBLIC_METHOD
    public void SetGameStart(bool start)
    {
        game_start = start;
    }

    public Sprite FindImageByName(string ImageName)
    {
        Sprite image = null;

        foreach (Sprite item in mSprites)
        {
            if (item.texture.name == ImageName)
            {
                image = item;
                break;
            }
        }

        return image;
    }

    public Card FindCard(int index)
    {
        return CardList[index];
    }

    public void InitPlayerDeck()
    {
        List<int> deck = new List<int>();

        FileStream file = new FileStream("Assets/Resources/decktest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        if (file.CanRead)
        {
            while (!streamReader.EndOfStream)
            {
                string input_text = streamReader.ReadLine();
                deck.Add(int.Parse(input_text));
            }

            player.Deck = deck;
        }
        else
        {
            Debug.Log("flie not found!");
        }

        file.Close();
    }

    public Monster GetMonsterByIndex(int index)
    {
        if (index <= mMonsters.Count)
            return mMonsters[index];
        else
            return mMonsters[0];
    }

    #endregion
}
