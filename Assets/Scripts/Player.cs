﻿using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration params
    [Header("Player")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;
    [SerializeField] float destroyPlayerDelay = 0.2f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    [Header("Audio Config")] 
    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip dieSFX;
    [SerializeField] [Range(0, 1)] float shotSFXVolume = 0.1f;
    [SerializeField] [Range(0, 1)] float dieSFXVolume = 0.2f;

    // Cached references
    Coroutine firingCoroutine;

    // Stats
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundries();
    }

    private void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuosly());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuosly()
    {
        while (true) 
        { 
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shotSFXVolume);

            // Quaternin.identity - use the current rotation
            GameObject laser =
                Instantiate(laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject;

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        // Create a var wich is horizontal change based on keyboard or joystick
        // We multiply the delta per frame in the duration of 1 frame in the system
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } // Return if the other doesn't have damage dealer
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        health = 0;
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject, destroyPlayerDelay);
        AudioSource.PlayClipAtPoint(dieSFX, Camera.main.transform.position, dieSFXVolume);
    }

    private void SetUpMoveBoundries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    public int GetHealth()
    {
        return health;
    }

}
