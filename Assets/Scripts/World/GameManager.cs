using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //entender mejo el game manager
    //SINGELTON


    public static GameManager Instance { get; private set; }


    //public Text ammoText; // UI Text to display ammo count
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;

    public int gunammo = 12;
    public int health = 100;


    public int maxHealth = 100;

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        ammoText.text = gunammo.ToString();
        healthText.text = health.ToString();
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



}
