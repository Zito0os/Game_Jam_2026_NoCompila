using UnityEngine;

public class WeaponPickupUnlock : MonoBehaviour
{
    [Tooltip("Nombre EXACTO del GameObject arma (hijo) dentro del WeaponSwitch")]
    public string weaponChildName; // ej: "Bat", "Knife", "Shotgun"

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Busca WeaponSwitch en hijos del player (ej: en FirstPersonCamera)
        WeaponSwitch ws = other.GetComponentInChildren<WeaponSwitch>();
        if (ws == null) return;

        // Busca el arma dentro del mismo objeto donde está WeaponSwitch
        Transform weapon = ws.transform.Find(weaponChildName);
        if (weapon == null)
        {
            Debug.LogWarning($"No encontré el arma hija llamada '{weaponChildName}' en {ws.name}");
            return;
        }

        ws.UnlockWeapon(weapon.gameObject, autoEquip: true);

        Destroy(gameObject);
    }
}
