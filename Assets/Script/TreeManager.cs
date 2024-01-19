using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager: MonoSingleton<TreeManager>
{
    class TreeData
    {
        public int count;
        public List<Vector2> treePos;
    }
    
    List<Vector2> treePos = new List<Vector2>();//树的位置

    public GameObject treePrefab;//树的预制体

    Dictionary<int, GameObject> trees;//树的字典
    Queue<int> ids;//当前树的序列
    void Start()
    {
        this.ids = new Queue<int>();
        trees=this.GetTrees();//获取树的字典

        this.treePos = this.LoadData();//读取数据
        this.BuildTrees();//建造树

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            foreach (GameObject t in this.trees.Values)
            {
                if (!t.activeSelf)
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    t.transform.position = pos;
                    t.SetActive(true);
                    int id = t.GetComponent<Tree>().id;
                    this.ids.Enqueue(id);
                    return;
                }
            }
            while(ids.Count>0)
            {
                int id = ids.Dequeue();

                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                trees[id].transform.position = pos;
                trees[id].SetActive(true);
                this.ids.Enqueue(id);
                return;
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            foreach(int i in this.ids)
            {
                Tree t = trees[i].GetComponent<Tree>();
                if(t.sR.IsTorch(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    trees[i].SetActive(false);
                }
            }
        }
    }

    Dictionary<int,GameObject> GetTrees()
    {
        Dictionary<int, GameObject> trees = new Dictionary<int, GameObject>();
        for(int i = 0;i<10;i++)
        {
            GameObject tree = Instantiate(this.treePrefab,this.transform);
            tree.GetComponent<Tree>().id = i;
            tree.SetActive(false);
            trees.Add(i, tree);
        }
        return trees;
    }   

    void BuildTrees()
    {
        for(int i = 0;i<this.treePos.Count;i++)
        {
            GameObject tree = this.trees[i];
            tree.transform.position = this.treePos[i];
            tree.SetActive(true);
            this.ids.Enqueue(i);
        }
    }



    List<Vector2> LoadData()
    {
        string ts =DataManager.LoadByJson("TreeData.json");
        TreeData trees = JsonUtility.FromJson<TreeData>(ts);

        int count = trees.count;
        Debug.LogFormat("TreeCount: {0}", count);

        List<Vector2> treePos = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {
            treePos.Add(trees.treePos[i]);
        }

        return treePos;
    }

    public void SaveData()
    {
        this.treePos.Clear();
        foreach (GameObject t in this.trees.Values)
        {
            if (t.activeSelf)
            {
                this.treePos.Add(t.transform.position);
            }
        }

        TreeData data = new TreeData();
        data.count = this.treePos.Count;
        data.treePos = this.treePos;

        string json = JsonUtility.ToJson(data);
        DataManager.SaveByJson("TreeData.json", json);
    }

}
