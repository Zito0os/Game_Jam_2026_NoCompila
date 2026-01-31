using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject[] weapons;

    public int selectedWeapon = 0;



    void Start()
    {
        SelectWeapon();
    }


    void Update()
    {

        int previousWeapon = selectedWeapon;

        //para que se pueda con la rueda del raton seleccionar el arma 
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedWeapon >= weapons.Length - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = weapons.Length - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        //mover con los numeros del 1 al 9

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length >= 2)
        {
            selectedWeapon = 1;
        }
        

        //SI TIENES MAS ARMAS PONES MAS DE ESTO







        //para evitar si tenemos el arma llame al mismo metodo
        if (previousWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (weapon.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            {
                if (i == selectedWeapon)
                {
                    weapon.gameObject.SetActive(true);
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }

                i++;

                
            }


            
        }
    }
}
