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
    // Start is called before the first frame update
    public int stage = 0;
    public int floor = 0;
    public GameObject selected_stage;
    public Image battle_stage;
    public Image boss_stage;
    public Image heal_stage;
    public Image shop_stage;
    public Image void_stage;

    List<Card> CardList;
    List<Artifact> ArtifactList;

    void Start()
    {
        game_start = false;
        selected_stage = null;

        FileStream file = new FileStream("Assets/Resources/cardtest.txt", FileMode.Open);
        StreamReader streamReader = new StreamReader(file);

        CardList = new List<Card>();
        ArtifactList = new List<Artifact>();

        while(!streamReader.EndOfStream)
        {
            string input_text = streamReader.ReadLine();
            if (input_text == "Info")
            {
                string card_name = streamReader.ReadLine();
                string image_name = streamReader.ReadLine();
                string flavor_text = streamReader.ReadLine();
                string effect_text = streamReader.ReadLine();
                CardEffect cardEffect = new CardEffect();
                List<CardCategory> cardCategories = new List<CardCategory>();
                List<int> cardValue = new List<int>();
                string data = streamReader.ReadLine();
                if (data == "Target")
                {
                    data = streamReader.ReadLine();
                    while(data != "Category")
                    {
                        cardEffect.targets.Add((CardTarget)int.Parse(data));
                        data = streamReader.ReadLine();
                    }
                }
                if (data == "Category")
                {
                    data = streamReader.ReadLine();
                    while (data != "Value")
                    {
                        cardCategories.Add((CardCategory)int.Parse(data));
                        data = streamReader.ReadLine();
                    }
                }
                if (data == "Value")
                {
                    data = streamReader.ReadLine();
                    while (data != "End")
                    {
                        cardValue.Add(int.Parse(data));
                        data = streamReader.ReadLine();
                    }
                }

                for (int i = 0; i < cardCategories.Count; i++)
                {
                    cardEffect.effection.Add(cardCategories[i], cardValue[i]);
                }

                CardList.Add(new Card(card_name, image_name, flavor_text, effect_text, cardEffect));
            }
        }

        foreach (Card card in CardList)
        {
            Debug.Log(card.Display());
        }

        file.Close();

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

    public void SetGameStart(bool start)
    {
        game_start = start;
    }
}
