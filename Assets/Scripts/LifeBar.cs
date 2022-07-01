using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public Image barraVida;
    public float vidaActual;
    public float vidaMax;

    void Update()
    {
        barraVida.fillAmount = vidaActual/vidaMax;
    }
}
