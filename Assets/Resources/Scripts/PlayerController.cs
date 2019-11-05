using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    ShotScript shotscript;
    private float shotTimer = 0.3f;
    private float lastShot = 0f;
    public ParticleSystem explosion;
    public AudioSource explosionSound;
    LogicController lc;


    private float thrust = 45f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        lc = gameControllerObject.GetComponent<LogicController>();
        shotscript = transform.GetComponentInChildren<ShotScript>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > shotTimer + lastShot)
            {
                shotscript.Shoot(1);
                GetComponent<AudioSource>().Play();
                lastShot = Time.time;
            }
        }
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

    public void GetHit()
    {
        explosion.Play();
        explosion.transform.SetParent(null);
        explosionSound.Play();
        explosionSound.transform.SetParent(null);
        Destroy(gameObject);
        Destroy(explosionSound, 1f);
        Destroy(explosion, 1f);
        lc.GameOver();
    }

    void FixedUpdate()
    {
        rb.angularVelocity = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddRelativeForce(0f, 0f, thrust, ForceMode.Force);
            }
            if (Input.GetKey(KeyCode.A))
            {
               
                rb.angularVelocity = new Vector3(0f, -3f, 0f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddRelativeForce(0, 0, -thrust, ForceMode.Force);
            }
            if (Input.GetKey(KeyCode.D))
            {
                
                rb.angularVelocity = new Vector3(0f, 3f, 0f);
            }
            
    }   //Player Movement

}
