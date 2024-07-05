using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<Texture2D> list;
    private int i = 0;
    private float timer = 0;
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 2) {
            Debug.Log(111);
            timer = 0;
             var cp = GetComponent<MeshRenderer>();
            cp.materials[0].mainTexture = list[i];
            i = (i+1) % list.Count;
        }
    }
}
