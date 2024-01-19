using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public SpriteRenderer sR;

    public int id;

    void Start()
    {
        this.sR = this.GetComponent<SpriteRenderer>();
    }

}
