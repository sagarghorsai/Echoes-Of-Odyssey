using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private float _reduceSpeed = 2;
    private float _target = 1;
    private Camera _cam;

    private void Start()
    {
        _cam= Camera.main;
    }

    public void UpdateHealth(float maxHealth, float currentHealth)
    {
        _target = currentHealth/maxHealth; 
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        _healthbarSprite.fillAmount = Mathf.MoveTowards(_healthbarSprite.fillAmount,_target, _reduceSpeed*Time.deltaTime);

    }



}
