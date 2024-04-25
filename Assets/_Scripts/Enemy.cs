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
        health -= amount;

        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthBar();

        if (health <= 0)
        {
            anim.Play("Die", 0, 0f); // "Die" is the name of your death animation
        }
    }



    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.UpdateHealth(maxHealth, health);
        }
        else
        {
            Debug.LogWarning("HealthBar not assigned in Enemy script!");
        }
    }
}
