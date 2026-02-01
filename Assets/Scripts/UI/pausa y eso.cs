using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausayeso : MonoBehaviour
{
    
    [SerializeField] private GameObject menuPausa;
    private bool juegoPausado = false;
    private float nextToggleTime = 0f;
    public float toggleCooldown = 0.1f;
    


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            // Un solo punto de control para cambiar estado
            TogglePausa();
            nextToggleTime = Time.unscaledTime + toggleCooldown;
        }
    }
    private void TogglePausa()
    {
        if (juegoPausado)
            Reanudar();
        else
            Pausa();
    }
    public void Pausa()
    {
        
        juegoPausado =true;
        Time.timeScale = 0f;
        
        menuPausa.SetActive(true);
    }
    public void Reanudar()
    {
        Cursor.lockState = CursorLockMode.Locked;
        juegoPausado =false;
        Time.timeScale = 1f;
        
        menuPausa.SetActive(false);
    }
    public void Reiniciar()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
    public void Cerrar()
    {
         
        Application.Quit();
    }

}