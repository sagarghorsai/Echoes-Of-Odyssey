using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private int defaultDamageAmount = 1; // Default damage amount
    [SerializeField] private int swordDamageAmount = 2; // Damage amount for sword
    [SerializeField] private int axeDamageAmount = 3; // Damage amount for axe
    [SerializeField] private int bowDamageAmount = 1; // Damage amount for bow

    // Get the amount of damage this object deals based on the weapon type
    public int GetDamage(PlayerController.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case PlayerController.WeaponType.Sword:
                return swordDamageAmount;
            case PlayerController.WeaponType.Axe:
                return axeDamageAmount;
            case PlayerController.WeaponType.Bow:
                return bowDamageAmount;
            default:
                return defaultDamageAmount;
        }
    }
}
