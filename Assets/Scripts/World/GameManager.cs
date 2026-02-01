using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;


public class GameManager : MonoBehaviour
{
    //entender mejo el game manager
    //SINGELTON


    public static GameManager Instance { get; private set; }


    //public Text ammoText; // UI Text to display ammo count
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI oxigenoText;
    public TextMeshProUGUI mascaraText;

    public int gunammo = 12;
    public int health = 100;
    public int oxigeno = 100;   
    public int mascara = 100;   


    public int maxHealth = 100;
    public int maxOxigeno = 100;
    public int maxMascara = 100;


    private Coroutine myCoroutineLosing_oxigeno;
    private Coroutine myCoroutineRegenerate_oxigeno;


    private Coroutine myCoroutineLosing_mask;
    private Coroutine myCoroutineRegenerate_mask;



    public Color verde = Color.green;
    public Color amarillo = Color.yellow;
    public Color rojo = Color.red;

    public bool esta_enmascarado;

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        ammoText.text = gunammo.ToString();
        healthText.text = health.ToString();
        oxigenoText.text = oxigeno.ToString();
        mascaraText.text = mascara.ToString();
        cambio_color();
    }


    public void LoseHealth(int healthToReduce)
    {
        health -= healthToReduce;
        
        CheckHealth();

    }


    public void CheckHealth()
    { 
        if (health <= 0)
        {
            Debug.Log("Haz muerto");
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void AddHealth(int healthToAdd)
    {
        // si mi vida mas la vida que viene es mayor o igual a mi vida maxima se queda en 100
        if (this.health + healthToAdd >= maxHealth)
        {
            this.health = 100;
        }
        else
        {
            this.health += healthToAdd;
        }

    }











    public void AgregarOxigeno(int OxigenoAdd)
    {
        // si mi oxigeno mas la vida que viene es mayor o igual a mi vida maxima se queda en 100
        if (this.oxigeno + OxigenoAdd >= maxOxigeno)
        {
            this.oxigeno = 100;
        }
        else
        {
            this.oxigeno += OxigenoAdd;
        }

    }




    public void PerderOxigeno(int oxigeno_perder)
    {
        oxigeno -= oxigeno_perder;
        oxigeno = Mathf.Max(oxigeno, 0);
    }



    //mascara


    public void AgregarMascara(int MascaraAdd)
    {
        // si mi vida mas la vida que viene es mayor o igual a mi vida maxima se queda en 100
        if (this.mascara + MascaraAdd >= maxMascara)
        {
            this.mascara = 100;
        }
        else
        {
            this.mascara += MascaraAdd;
        }

    }

    public void PerderMascara(int mascara_perder)
    {

        mascara -= mascara_perder;
        mascara = Mathf.Max(mascara, 0);
    }




    void cambio_color()
    {
        if (health > 66)
        {
            healthText.color = verde;
        }
        else if (health > 33 && health <= 66)
        {
            healthText.color = amarillo;
        }
        else
        {
            healthText.color = rojo;
        }




        if (oxigeno > 66)
        {
            oxigenoText.color = verde;
        }
        else if (health > 33 && health <= 66)
        {
            oxigenoText.color = amarillo;
        }
        else
        {
            oxigenoText.color = rojo;
        }


        if (mascara > 66)
        {
            mascaraText.color = verde;
        }
        else if (health > 33 && health <= 66)
        {
            mascaraText.color = amarillo;
        }
        else
        {
            mascaraText.color = rojo;
        }
    }

    public void cambiar_bool_enmascarado(bool mascarado)
    {
        esta_enmascarado = mascarado;
    }



}
