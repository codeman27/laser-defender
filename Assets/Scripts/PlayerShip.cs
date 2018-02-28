using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour {
    public float shipSpeed = 7f;
    public float padding = 1f;
    public GameObject projectile;
    public float projectileSpeed;
    public float firingRate = 0.2f;
    public float health = 250;

    ScoreKeeper scoreKeeper;

    float xmin;
    float xmax;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost= Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftMost.x + padding;
        xmax = rightMost.x - padding;
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }

    void Fire()
    {
        
        GameObject beam = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        GetComponent<AudioSource>().Play();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Projectile missile = col.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0)
            {
                Destroy(gameObject);
                LevelManager manager = GameObject.FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
                manager.LoadLevel("Win Screen");
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            InvokeRepeating("Fire", 0.0000001f, firingRate);
        }
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("Fire");
        }

        Vector2 acc = Input.acceleration;;
        transform.Translate(acc.x * shipSpeed * Time.deltaTime, 0, 0);
        /*if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * shipSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * shipSpeed * Time.deltaTime;
        }
        */

        // Restrict player to gamespace
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
}
