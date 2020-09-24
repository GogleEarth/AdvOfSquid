using System.Collections;
using System.Collections.Generic;
using Unity.UNetWeaver;
using UnityEngine;
using UnityEngine.UI;

public class StageMaker : MonoBehaviour
{
    public Canvas canvas;

    int count = 0;
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
        btnprefab = Resources.Load<GameObject>("prefab/stage");
        btnprefab.transform.SetParent(canvas.transform);
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
            if (count < 10)
            {
                int nodecount = Random.Range(1, 3);
                for (int i = 0; i < nodecount; ++i)
                {
                    StageNode node = new StageNode(Instantiate(btnprefab));
                    node.btn.transform.position = gameObject.transform.position;
                    node.btn.name = "나는" + i + "번째 버튼";
                    if (stagetree.root.left == null)
                        stagetree.root.left = node;
                    else if (stagetree.root.center == null)
                        stagetree.root.center = node;
                    else
                        stagetree.root.right = node;
                }
                count += 11;
            }
        }
    }
}
