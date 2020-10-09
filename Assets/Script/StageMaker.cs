using System.Collections;
using System.Collections.Generic;
using Unity.UNetWeaver;
using UnityEngine;
using UnityEngine.UI;

public class StageMaker : MonoBehaviour
{
    int count = 0;
    int stagecount = 0;
    float timelag = 0.0f;
    GameObject btnprefab;
    public class StageNode
    {
        public GameObject btn { get; set; }
        public StageNode left { get; set; }
        public StageNode right { get; set; }
        public StageNode center { get; set; }
        public StageNode(GameObject data)
        {
            this.btn = data;
            left = null;
            right = null;
            center = null;
        }
    }

    public class StageTree
    {
        public StageNode root { get; set; }
        public void PreOrderTraversal(StageNode node)
        {
            if (node == null) return;

            Debug.LogFormat(node.btn.name);
            PreOrderTraversal(node.left);
            PreOrderTraversal(node.right);
            PreOrderTraversal(node.center);
        }
    }

    StageTree stagetree;

    // Start is called before the first frame update
    void Start()
    {
        btnprefab = Resources.Load<GameObject>("prefap/stage");
        if (btnprefab == null)
        { 
            Debug.Log("mbtnpreprefab==null"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (btnprefab != null)
        {
            if (timelag > 1.0f)
            {
                if (0 < count && count < 9)
                {
                    int nodecount = Random.Range(1, 4);
                    for (int i = 0; i < nodecount; ++i)
                    {
                        StageNode node = new StageNode(Instantiate(btnprefab));
                        Vector3 position = new Vector3(5.0f * count, 5.0f * i, 0.0f);
                        node.btn.transform.position = position;
                        node.btn.name = "나는" + stagecount++ + "번째 버튼";
                    }
                }
                else if (count < 10)
                {
                    StageNode node = new StageNode(Instantiate(btnprefab));
                    Vector3 position = new Vector3(5.0f * count, 0.0f, 0.0f);
                    node.btn.transform.position = position;
                    node.btn.name = "나는" + stagecount++ + "번째 버튼";
                }
                count++;
                timelag = 0.0f;
            }
            else timelag += Time.deltaTime;
        }

    }
}
