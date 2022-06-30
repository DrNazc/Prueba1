using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
 
public class PlayerMovement : MonoBehaviour
{
    //<>
    //-------------------------- Variables definidas
    public Rigidbody2D rbody;
    public Animator anim;
    private CinemachineVirtualCamera cm;
    private Vector2 direccionAtaque;
 
    [Header("Estadisticas")]
    public float jumpForce = 10;
    public float walkSpeed = 10;
    private float currentSpeed = 0;
    public float velocidadDash = 20;
 
    [Header("Colisiones")]
    public Vector2 abajo; //Vector para conocer la posicion de los pies del personaje
    public float radioColision; //Separacion entre suelo y pies
    public LayerMask layerPiso; //La capa con la que existira dicha colision
 
    [Header("Booleanos")]
    public bool enSuelo = true; //Indica si estamos en el suelo o no
    public bool haciendoDash; // Nos indica si se encuentra haciendo la accion de dash
    public bool puedeDash; // Nos limitara si se puede o no realizar la accion
    public bool puedoMover = true; // Limitara el movimiento al personaje
    public bool vibrando;
    public bool atacando; // Nos indica si el personaje esta atacando en ese momento
 
    private void Awake(){
        cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }
    
    //-------------------------- Ciclo de repeticion continua
    private void Update() {
        // Permite al personaje permanecer con su velocidad al caer al suelo
        
        if(puedoMover && !haciendoDash)
        {
            rbody.velocity = new Vector2(currentSpeed, rbody.velocity.y);
            TocarSuelo();
 
 
            if(!enSuelo)
            {
                if(rbody.velocity.y > 0)
                {
                    anim.SetBool("saltar", true);
                    anim.SetFloat("velocidadVertical", 1);
                }
                else if(rbody.velocity.y < 0)
                {
                    anim.SetBool("saltar", true);
                    anim.SetFloat("velocidadVertical", -1);
                }
            }
            else
            {
                anim.SetBool("saltar", false);
            }
        }
        
    }
 
    //-------------------------- Metodo de salto
    private void OnJump() {
        if(enSuelo)
        {
            Debug.Log("Jump!");
            // Aplica fuerza de salto
            rbody.velocity = new Vector2(rbody.velocity.x, jumpForce);
        }
    }
 
    //-------------------------- Metodo de movimiento
    private void OnMove(InputValue inputValue) {
        float moveValue = inputValue.Get<float>();
        Debug.Log("Move! " + moveValue);
        // Aplica velocidad al movimiento
        currentSpeed = moveValue * walkSpeed;
 
        if(enSuelo)
        {
            anim.SetBool("caminar", true);
        }
 
        if(moveValue < 0 && transform.localScale.x > 0)
        {
            direccionAtaque.x = -1;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        else if(moveValue > 0 && transform.localScale.x < 0)
        {
            direccionAtaque.x = 1;
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else
        {
            anim.SetBool("caminar",false);
        }
 
 
    }
 
 
    //-------------------------- Metodo de Dash
    private void OnDash()
    {
        if(rbody.velocity.x != 0 && !haciendoDash)
        {
            Vector3 posicionJugador = Camera.main.WorldToViewportPoint(transform.position);
            Camera.main.GetComponent<RippleEffect>().Emit(posicionJugador);
            StartCoroutine(AgitarCamara());
            //anim.SetBool("dash",true);    Cambiamos la animacion de lugar y la colocamos dentro de la corutina
 
 
            //puedeDash = true;
            rbody.velocity = Vector2.zero;
            rbody.velocity += new Vector2(currentSpeed,rbody.velocity.y).normalized * velocidadDash;
 
 
            StartCoroutine(PrepararDash());
        }
    }
 
 
    //-------------------------- Co-rutina para preparar el dash del personaje
    private IEnumerator PrepararDash() 
    {
        //StartCoroutine(DashSuelo());  Esta corutina ya no es necesaria
        rbody.gravityScale = 0;
        haciendoDash = true;
        anim.SetBool("dash",true); // Se agrega la activacion del parametro
 
 
        yield return new WaitForSeconds(0.5f);
        rbody.gravityScale = 1;
        haciendoDash = false;
        anim.SetBool("dash",false);
    }
    
    //-------------------------- Metodo para identificar si el personaje esta tocando el suelo
    private void TocarSuelo()
    {
        // Colocar un vector de posicion de piernas
        enSuelo = Physics2D.OverlapCircle((Vector2)transform.position + abajo, radioColision, layerPiso);
    }
 
 
    //-------------------------- Metodo para finalizar la animacion de dash
    public void FinalizarDash()
    {
        anim.SetBool("dash",false);
    }
 
 
    //-------------------------- Metodo para agitar camara por 0.3 segundos
    private IEnumerator AgitarCamara()
    {
        vibrando = true;
        CinemachineBasicMultiChannelPerlin cmNoise = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cmNoise.m_AmplitudeGain = 10;
        yield return new WaitForSeconds(0.3f);
        cmNoise.m_AmplitudeGain = 0;
        vibrando = false;
    }
 
    //-------------------------- Metodo de polimorfismo donde se agrega una variable para controlar la duracion del temblor
    private IEnumerator AgitarCamara(float tiempo)
    {
        vibrando = true;
        CinemachineBasicMultiChannelPerlin cmNoise = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cmNoise.m_AmplitudeGain = 10;
        yield return new WaitForSeconds(tiempo);
        cmNoise.m_AmplitudeGain = 0;
        vibrando = false;
    }

    private void OnAttack()
    {
        if(!atacando && !haciendoDash)
        {
            atacando = true;
            anim.SetBool("ataque", true);
            anim.SetFloat("ataqueX", direccionAtaque.x);
            anim.SetFloat("ataqueY", direccionAtaque.y);
        }
    }

    private void FinalizarAtaque()
    {
        anim.SetBool("ataque", false);
        atacando = false;
    }

    private void OnLook(InputValue inputValue)
    {
        float lookValue = inputValue.Get<float>();
        Debug.Log("Look!" + lookValue);
        direccionAtaque.y = lookValue;
    }
}
