using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 10;
    public int currentHealth;

    public float jumpForce = 10f;
    public float groundCheckDistance = 0.1f;
    public float rotationSpeed = 5f;
    public Transform cameraTransform;

    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded;
    private bool isRotating;
    private Interactor interactor;
    public Inventory inventory;
    private bool isWalking = false;
    public bool isDead = false;
    private AudioManager audioManager;


    [SerializeField] private HealthBar healthBar;
    [SerializeField] private WeaponType currentWeapon = WeaponType.None; // Default weapon type is None
    public GameObject[] weapons;
    // Enum to represent weapon types
    public enum WeaponType
    {
        None,
        Sword,
        Axe,
        Bow
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar.UpdateHealth(maxHealth, currentHealth);
        inventory = Inventory.Instance;
        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCam");
        if (cameraObj != null)
        {
            cameraTransform = cameraObj.transform;
            cameraObj.GetComponent<CameraFollow>().target = transform;
        }
        interactor = GetComponent<Interactor>();
        audioManager = FindObjectOfType<AudioManager>();

    }

    void Update()
    {
        // Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement = transform.TransformDirection(movement) * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        // Check if the player is moving
        bool isMoving = movement.magnitude > 0.01f;

        // Update the walking state
        if (isMoving && !isWalking)
        {
            isWalking = true;
            audioManager.Play("Footstep");
        }

        else if (!isMoving && isWalking)
        {
            isWalking = false;
            audioManager.Stop("Footstep");

        }

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            audioManager.Play("Jump");

        }

        animator.SetFloat("X", horizontal);
        animator.SetFloat("Y", vertical);


        if (Input.GetMouseButtonDown(0) && !isDead) // Left mouse button for hit
        {

            if (inventory.HasWeapon(currentWeapon))
            {
                switch (currentWeapon)
                {
                    case WeaponType.Sword:
                        animator.SetTrigger("HitWithSword");
                        audioManager.Play("HitWithSword");


                        break;
                    case WeaponType.Axe:
                        animator.SetTrigger("HitWithAxe");
                        audioManager.Play("HitWithAxe");
                        break;
                    case WeaponType.Bow:
                        // Implement bow shot animation
                        break;
                }
            }
            else
            {
                animator.SetTrigger("HitWithFist");
                audioManager.Play("HitWithFist");

            }
            Hit();
        }

        // Camera rotation with mouse movement
        if (Input.GetMouseButtonDown(1)) // Right mouse button for camera rotation
        {
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            transform.Rotate(Vector3.up, mouseX);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {

            // Check the current weapon and equip the next one in the sequence
            switch (currentWeapon)
            {
                case WeaponType.None:
                    audioManager.Play("Equip");
                    EquipWeapon(WeaponType.Sword); // Equip sword
                    break;
                case WeaponType.Sword:
                    audioManager.Play("Equip");
                    EquipWeapon(WeaponType.Axe); // Equip axe
                    break;
                case WeaponType.Axe:
                    audioManager.Play("Equip");
                    EquipWeapon(WeaponType.Bow); // Equip bow
                    break;
                case WeaponType.Bow:
                    audioManager.Play("Unequip");

                    UnequipWeapon(); // Unequip weapon
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void FixedUpdate()
    {
        // Check if the player is grounded
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance);
    }

    public void TakeDamage(int amount)
    {
        if (!isDead)
        {


            audioManager.Play("Damage");

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0 or above maxHealth
            healthBar.UpdateHealth(maxHealth, currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("IsDead");
        audioManager.Play("PlayerDeath");

        Sound deathSound = Array.Find(audioManager.sounds, sound => sound.name == "PlayerDeath");
        if (deathSound == null)
        {
            Debug.LogWarning("Death sound clip not found in AudioManager!");
            StartCoroutine(WaitForAnimation("Lose"));
        }
        else
        {
            float deathSoundLength = deathSound.clip.length;
            StartCoroutine(WaitForAnimation("Lose", deathSoundLength + .2f));
        }
    }

    IEnumerator WaitForAnimation(string sceneName, float delay = 1f)
    {
        // Wait for the animation to finish
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        yield return new WaitForSeconds(delay);

        // Load the next scene
        SceneManager.LoadScene(sceneName);
    }


    void Hit()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position + transform.forward * 0.5f, new Vector3(0.5f, 0.5f, 0.5f));
        foreach (Collider collider in hitColliders)
        {
            DamageEffect damageEffect = collider.GetComponentInChildren<DamageEffect>();
            if (damageEffect != null)
            {
                int damageAmount = damageEffect.GetDamage(currentWeapon);

                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount); // Pass the damage amount to TakeDamage method
                }
            }
        }

    }

    public void EquipWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;
        Inventory inventory = Inventory.Instance;
        Item weaponItem = inventory.items.Find(item => item.name == weapon.ToString() && item.type == ItemType.Weapon);

        if (weaponItem != null && weaponItem.count > 0)
        {
            foreach (var weaponObj in weapons)
            {
                weaponObj.SetActive(weaponObj.name == weapon.ToString());
            }
        }
        else
        {
            Debug.Log("The player does not have the " + weapon + " in their inventory.");
        }
    }

    public void UnequipWeapon()
    {
        currentWeapon = WeaponType.None;
        foreach (var weaponObj in weapons)
        {
            weaponObj.SetActive(false);
        }
    }

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }
}
