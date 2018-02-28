using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float health = 150;
    public GameObject projectile;
    public float projectileSpeed;
    public float enemyFiringRate = 0.5f;
    public int scoreValue = 125;
    public AudioClip enemyLaser;
    public AudioClip explosion;

    private ScoreKeeper scoreKeeper;

    private void Start()
    {
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }

    private void Fire()
    {
        GameObject beam = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(enemyLaser, transform.position);
    }

    private void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("1"))
        {
            print("animation is playing");
        }
        else
        {
            float probability = Time.deltaTime * enemyFiringRate;
            if (Random.value < probability)
            {
                Fire();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.GetComponent<Projectile>();
        if(missile)
        {
            health -= missile.GetDamage();
            missile.Hit();
            if(health <= 0)
            {
                Die();
            }
        }

    }

    private void Die()
    {
        Destroy(gameObject);
        scoreKeeper.Score(scoreValue);
        AudioSource.PlayClipAtPoint(explosion, transform.position);
    }
}
