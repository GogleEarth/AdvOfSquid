using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using GenericScript;
using System.IO;

public class GameManager : MonoBehaviour
{
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
    
    bool game_start;
    List<Card> CardList;
    List<Artifact> ArtifactList;
    List<Monster> mMonsters;
    List<Skill> mSkills;
    List<Buff> mBuffList;
    Sprite[] mSprites;

    #region PRIVATE_METHOD

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

        LoadCardData();
        //loadArtiData();
        LoadSkillData();
        LoadMonsterData();
        LoadBuffData();
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

    void LoadCardData()
    {
        FileStream file = new FileStream("Assets/Resources/cardtest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        if (file.CanRead)
        {
            while (!streamReader.EndOfStream)
            {
                string input_text = streamReader.ReadLine();
                List<Effect> cardEffects = new List<Effect>();
                List<Target> cardTargets = new List<Target>();
                List<Category> cardCategorys = new List<Category>();
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
                            cardTargets.Add(GetEnumFromString<Target>(data));
                            data = streamReader.ReadLine();
                        }
                    }
                    if (data == "Category")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Value")
                        {
                            cardCategorys.Add(GetEnumFromString<Category>(data));
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
                        cardEffects.Add(new Effect(cardTargets[i], cardCategorys[i], cardValues[i]));
                    }

                    CardList.Add(new Card(card_name, image_name, flavor_text, effect_text, int.Parse(cost), cardEffects));
                }
            }
        }
        else
        {
            Debug.Log("flie not found!");
        }

        file.Close();
    }

    void LoadArtiData()
    {
        FileStream file = new FileStream("Assets/Resources/artitest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        if (file.CanRead)
        {
            while (!streamReader.EndOfStream)
            {
                string input_text = streamReader.ReadLine();
                List<ArtifactEffect> artiEffects = new List<ArtifactEffect>();
                List<Target> artiTargets = new List<Target>();
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
                            artiTargets.Add((Target)int.Parse(data));
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
                Debug.Log(arti.Display);
            }
        }
        else
        {
            Debug.Log("flie not found!");
        }

        file.Close();
    }

    void LoadMonsterData()
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
        }
        else
        {
            Debug.Log("flie not found!");
        }

        file.Close();
    }

    void LoadSkillData()
    {
        FileStream file = new FileStream("Assets/Resources/skilltest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        if (file.CanRead)
        {
            while (!streamReader.EndOfStream)
            {
                string input_text = streamReader.ReadLine();
                List<Effect> skillEffects = new List<Effect>();
                List<Target> skillTargets = new List<Target>();
                List<Category> skillCategorys = new List<Category>();
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
                            skillTargets.Add(GetEnumFromString<Target>(data));
                            data = streamReader.ReadLine();
                        }
                    }

                    if (data == "Category")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Value")
                        {
                            skillCategorys.Add(GetEnumFromString<Category>(data));
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
                        skillEffects.Add(new Effect(skillTargets[i], skillCategorys[i], skillValues[i]));
                    }

                    mSkills.Add(new Skill(skillName, skillIconName, skillText, skillEffects, cooltime));
                }
            }
        }
        else
        {
            Debug.Log("flie not found!");
        }

        file.Close();
    }

    void LoadBuffData()
    {
        FileStream file = new FileStream("Assets/Resources/bufftest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        if (file.CanRead)
        {
            while (!streamReader.EndOfStream)
            {
                string input_text = streamReader.ReadLine();
                if (input_text == "Info")
                {
                    string buffName = streamReader.ReadLine();
                    string iconName = streamReader.ReadLine();
                    string effectText = streamReader.ReadLine();
                    string data = streamReader.ReadLine();
                    Category buffCategory = Category.Bleeding;
                    int buffValue = 0;
                    int buffDuration = 0;

                    if (data == "Category")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Value")
                        {
                            buffCategory = GetEnumFromString<Category>(data);
                            data = streamReader.ReadLine();
                        }
                    }

                    if (data == "Value")
                    {
                        data = streamReader.ReadLine();
                        while (data != "Duration")
                        {
                            buffValue = int.Parse(data);
                            data = streamReader.ReadLine();
                        }
                    }

                    if (data == "Duration")
                    {
                        data = streamReader.ReadLine();
                        while (data != "End")
                        {
                            buffDuration = int.Parse(data);
                            data = streamReader.ReadLine();
                        }
                    }

                    mBuffList.Add(new Buff(buffName, effectText, iconName,
                        buffCategory, buffValue, buffDuration));
                }
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

    #endregion

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
        return mMonsters[0];
    }

    public Skill GetSkillByIndex(int index)
    {
        if (index <= mSkills.Count)
        {
            Debug.Log(mSkills[index].Display);
            return mSkills[index];
        }

        return mSkills[0];
    }

    #endregion
}
