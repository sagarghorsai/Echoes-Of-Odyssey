using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Inscribed: Enemy")]
    [SerializeField]
    private float maxHealth = 1;
    public float health;

    [SerializeField]
    private GameObject guaranteedDrop = null;
    public HealthBar healthBar;

    [Header("Dynamic: Enemy")]
    [SerializeField]
    private List<GameObject> randomItems;
    private float speed;

    protected Animator anim;
    private WaveSpawner waveSpawner;
    public AudioManager audioManager;


    protected virtual void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        waveSpawner = GetComponentInParent<WaveSpawner>();
        audioManager = FindObjectOfType<AudioManager>();

    }

    private void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    void Die()
    {

        // Spawn drop or random items
        if (guaranteedDrop != null)
        {
            Instantiate(guaranteedDrop, transform.position, Quaternion.identity);
        }
        else if (randomItems.Count > 0)
        {
            int n = Random.Range(0, randomItems.Count);
            GameObject prefab = randomItems[n];
            if (prefab != null)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
        // Inform the wave spawner that this enemy is dead
        waveSpawner.waves[waveSpawner.currWave].enemiesLeft--;
        audioManager.Play("SkeletonDeath");

    }



    public void TakeDamage(int amount)
    {

        // Reduce the health of the enemy by the amount of damage
        health -= amount;

        // Ensure health doesn't go below 0 or above maxHealth
        health = Mathf.Clamp(health, 0, maxHealth);

        // Update the health bar
        UpdateHealthBar();

        // Check if the enemy's health has dropped to or below zero
        if (health <= 0)
        {
            anim.Play("Die", 0, 0f); // "Die" is the name of your death animation
        }
    }



    private void UpdateHealthBar()
    {
        // Check if the health bar component is assigned
        if (healthBar != null)
        {
            // Update the health bar with current health and max health
            healthBar.UpdateHealth(maxHealth, health);
        }
        else
        {
            Debug.LogWarning("HealthBar not assigned in Enemy script!");
        }
    }
}
