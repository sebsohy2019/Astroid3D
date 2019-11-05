using UnityEngine;

//ADD TIMER FOR TOO SMALL DEATH



public class AlienController : MonoBehaviour
{
    Rigidbody rb;
    private float thrust;
    public int scoreValue;
    GameObject targetFound;
    ShotScript shotScript;
    Vector3 direction;
    private float shotTimer = 2f;
    private float lastShot = 0f;
    public ParticleSystem explosion;
    public AudioSource explosionSound;


    // Start is called before the first frame update
    void Start()
    {
        scoreValue = 50;
        rb = GetComponent<Rigidbody>();
        targetFound = GameObject.FindGameObjectWithTag("Player");
        direction = targetFound.transform.position;
        thrust = 10f;
        shotScript = transform.GetComponentInChildren<ShotScript>();
        FindPlayer();
        if (transform.localScale.x < 1f || transform.localScale.y < 1f || transform.localScale.z < 1f)
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {


        FindPlayer();

        //AI Shot
        if (Time.time > shotTimer + lastShot)
        {
            if (Vector3.Distance(transform.position, targetFound.transform.position) < 50f)
            {
                if (Vector3.Distance(transform.position, targetFound.transform.position) < 25f)
                {
                    rb.velocity = new Vector3(0, 0, 0);
                }
                transform.LookAt(targetFound.transform);
                shotScript.Shoot(0);
                GetComponent<AudioSource>().Play();
                lastShot = Time.time;
                shotTimer = 2f;
                FindPlayer();
            }
        }

        //Bounds check
        if (transform.position.z > 53)
        {
            transform.position = new Vector3(transform.position.x, 0, -53);
        }
        if (transform.position.z < -53)
        {
            transform.position = new Vector3(transform.position.x, 0, 53);
        }
        if (transform.position.x > 120)
        {
            transform.position = new Vector3(-120, 0, transform.position.z);
        }
        if (transform.position.x < -120)
        {
            transform.position = new Vector3(120, 0, transform.position.z);
        }

    }

    public void getHit()
    {
        explosion.Play();
        explosion.transform.SetParent(null);
        explosionSound.Play();
        explosion.transform.SetParent(null);
        Destroy(gameObject);
        Destroy(explosionSound.gameObject, 1f);
        Destroy(explosion.gameObject, 1f);
    }

    public void FindPlayer()
    {
        if (targetFound.transform != null)
        {
            transform.LookAt(targetFound.transform);
            Vector3 direction = (targetFound.transform.position - transform.position).normalized;
            rb.velocity = direction * thrust;
        }
        else { return; }

    }

  
}
