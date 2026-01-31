using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public Camera thirdPersonCamera;
    public Camera firstPersonCamera;

    private bool firtPersonEnable = true;

    //Armas cambio de vista

    public Transform[] weaponsTransformFirstPerson;
    public Transform[] weaponsTransformThirdPerson;

    public GameObject[] weapons;

    //variables para desabilitar el mesh del player en first person

    public bool disableMeshPlayerFirstPerson = true;

    //public SkinnedMeshRenderer meshPlayer;

    //variables para desabilitar el mesh del player en first person




    //para desactivar mesh del player en first person
    void Start()
    {
        if (disableMeshPlayerFirstPerson)
        {
            //meshPlayer.enabled = false;
        }
    }

    //para desactivar mesh del player en first person


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            firtPersonEnable = !firtPersonEnable;
            ChangedCamera();
        }
        
    }


    public void ChangedCamera()
    {
        if (firtPersonEnable)
        {

            //para desactivar mesh del player en first person    

            if (disableMeshPlayerFirstPerson)
            {
                //meshPlayer.enabled = false;
            }
            //para desactivar mesh del player en first person


            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;

            ChangedWeaponFirtPerson();
        }
        else
        {
            //para desactivar mesh del player en first person
            //meshPlayer.enabled = true;
            //para desactivar mesh del player en first person

            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;

            ChangedWeaponThirdPerson();
        }

    }



    public void ChangedWeaponFirtPerson()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].transform.position = weaponsTransformFirstPerson[i].transform.position;

            weapons[i].transform.rotation = weaponsTransformFirstPerson[i].transform.rotation;

            weapons[i].transform.localScale = weaponsTransformFirstPerson[i].transform.localScale;
        }
    }

    public void ChangedWeaponThirdPerson()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].transform.position = weaponsTransformThirdPerson[i].transform.position;

            weapons[i].transform.rotation = weaponsTransformThirdPerson[i].transform.rotation;

            weapons[i].transform.localScale = weaponsTransformThirdPerson[i].transform.localScale;
        }


    }
}
