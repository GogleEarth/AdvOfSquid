using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    #region PUBLIC

    public GameManager game_manager;
    public GameObject mBattleManager;
    public Image fade_image;
    public float fade_speed;
    public Image battle_stage;
    public Image boss_stage;
    public Image heal_stage;
    public Image shop_stage;
    public Image void_stage;

    #endregion

    void Start()
    {
        fade_image.gameObject.SetActive(true);

        StartCoroutine(FadeIn);
    }

    void Update()
    {

    }

    IEnumerator FadeOut
    {
        get
        {
            for (float i = 0f; i <= 1; i += fade_speed * Time.deltaTime)
            {
                Color color = new Vector4(0.0f, 0.0f, 0.0f, i);
                fade_image.color = color;
                yield return 0;
            }
        }
    }

    IEnumerator FadeIn
    {
        get
        {
            for (float i = 1f; i >= 0; i -= fade_speed * Time.deltaTime)
            {
                Color color = new Vector4(0.0f, 0.0f, 0.0f, i);
                fade_image.color = color;
                yield return 0;
            }
        }
    }

    #region PUBLIC_METHOD
    public void TestButton()
    {
        if (battle_stage.IsActive())
            battle_stage.gameObject.SetActive(false);
        else if (heal_stage.IsActive())
            heal_stage.gameObject.SetActive(false);
        else if (boss_stage.IsActive())
            boss_stage.gameObject.SetActive(false);
        else if (shop_stage.IsActive())
            shop_stage.gameObject.SetActive(false);
        else
            void_stage.gameObject.SetActive(false);
    }

    public void TurnEnd()
    {
        mBattleManager.GetComponent<BattleManager>().DoEndTurn();
    }

    #endregion
}
