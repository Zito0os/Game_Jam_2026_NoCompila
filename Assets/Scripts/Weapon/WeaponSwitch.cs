using UnityEngine;
using System.Collections.Generic;

public class WeaponSwitch : MonoBehaviour
{

    public GameObject[] weapons; 

    public int selectedWeapon = 0;

    private readonly List<GameObject> availableWeapons = new List<GameObject>();

    void Start()
    {
        RefreshAvailableWeapons();
        SelectWeapon();
    }

    void Update()
    {
        // NO tiene aramas no hace nada
        if (availableWeapons.Count == 0) return;

        int previousWeapon = selectedWeapon;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
            selectedWeapon = (selectedWeapon + 1) % availableWeapons.Count;
        else if (scroll < 0f)
            selectedWeapon = (selectedWeapon - 1 + availableWeapons.Count) % availableWeapons.Count;

        if (previousWeapon != selectedWeapon)
            SelectWeapon();

    }

    /// 
    /// Metodo para desbloquear arma cuando se recoja
    ///
    public void UnlockWeapon(GameObject weaponToUnlock, bool autoEquip = true)
    {
        if (weaponToUnlock == null) return;

        // Si ya la tienes, opcionalmente la equipas y ya
        int existingIndex = availableWeapons.IndexOf(weaponToUnlock);
        if (existingIndex >= 0)
        {
            if (autoEquip)
            {
                selectedWeapon = existingIndex;
                SelectWeapon();
            }
            return;
        }

        // Asegura que esté activa para que se vea si se equipa
        weaponToUnlock.SetActive(true);

        // Guarda orden estable (orden de pickup)
        availableWeapons.Add(weaponToUnlock);

        // Auto equip (o no)
        if (autoEquip)
            selectedWeapon = availableWeapons.Count - 1;

        SelectWeapon();
    }


    /// 
    /// Recolecta SOLO armas en layer Weapon que estén activas.
    /// 
    public void RefreshAvailableWeapons()
    {
        availableWeapons.Clear();

        foreach (Transform child in transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Weapon") && child.gameObject.activeSelf)
            {
                availableWeapons.Add(child.gameObject);
            }
        }

        // Ajusta selectedWeapon para no salirse
        if (availableWeapons.Count == 0) selectedWeapon = 0;
        else selectedWeapon = Mathf.Clamp(selectedWeapon, 0, availableWeapons.Count - 1);
    }

    void SelectWeapon()
{
    for (int i = 0; i < availableWeapons.Count; i++)
    {
        availableWeapons[i].SetActive(i == selectedWeapon);
    }
}

}
