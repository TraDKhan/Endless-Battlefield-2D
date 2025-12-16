using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon pistol;
    public Weapon shotgun;
    public Weapon laser;
    public Weapon boomerang;

    public void EquipWeapon(WeaponType type)
    {
        pistol.gameObject.SetActive(type == WeaponType.Pistol);
        shotgun.gameObject.SetActive(type == WeaponType.Shotgun);
        laser.gameObject.SetActive(type == WeaponType.Laser);
        boomerang.gameObject.SetActive(type == WeaponType.Boomerang);
    }
}
