using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int vida = 2;
    public GameObject h1, h2, enemigo;

    void Start()
    {
        h1 = GameObject.Find("Heart_1");
        h2 = GameObject.Find("Heart_2");
        h1.gameObject.SetActive(true);
        h2.gameObject.SetActive(true);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("AtaquePlayer"))
        {
            
            if(vida>1)
            {
                h2.gameObject.SetActive(false);
                vida = 1;
                Debug.Log("Vida de enemigo es" + vida);
            }
            else
            {
                h1.gameObject.SetActive(false);
                vida = 0;
                Destroy(gameObject);
                Debug.Log("Vida de enemigo es" + vida);
            }

        }
    }
}

