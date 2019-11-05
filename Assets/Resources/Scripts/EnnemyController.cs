using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    Rigidbody rb;
    private float thrust;
    GameObject targetFound;
    Vector3 direction;
    private float deathTimer = 0;
    public int scoreValue;
    private bool hasToDie = false;
    LogicController lc;
    public ParticleSystem explosion;
    public AudioSource explosionSound;
    PlayerController pc;
    AlienController ac;


    // Start is called before the first frame update
    void Awake()
    {
        if (transform.localScale.x < 0.5f || transform.localScale.y < 0.5f || transform.localScale.z < 0.5f)
        {
            Destroy(gameObject);
        }


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        GameObject alien = GameObject.FindGameObjectWithTag("Enemy");

        if (alien != null)
        {
            ac = alien.GetComponent<AlienController>();
        }
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        lc = gameControllerObject.GetComponent<LogicController>();


        rb = GetComponent<Rigidbody>();
        scoreValue = 10;
        targetFound = GameObject.FindGameObjectWithTag("Player");
        direction = new Vector3((transform.position.x * -1), 0, (transform.position.z * -1));

        //Add speed based on Scale
        if (transform.localScale.x < 4f || transform.localScale.y < 4f || transform.localScale.z < 4f)
        {
            thrust = Random.Range(10f, 20f);
        }
        else
        {
            thrust = Random.Range(5f, 10f);
        }

        //trans && dir && dir*thrust
        Debug.Log("transform" + transform.position + " // direction : " + direction + " // Dir*Thrust : " + (direction * thrust));
        Debug.Log("transform N" + transform.position.normalized + " // direction N : " + direction.normalized + " // Dir*Thrust N : " + (direction * thrust).normalized);
        rb.AddForce(direction * thrust, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        deathTimer -= Time.deltaTime;

        //Check Size 
        if (transform.localScale.x < 1.5f || transform.localScale.y < 1.5f || transform.localScale.z < 1.5f)
        {
            deathTimer = 0.3f;
            hasToDie = true;
        }
        //Destroy if too small
        if (deathTimer < 0 && hasToDie)
        {
            Destroy(gameObject);
        }




        //Bound Check
        if (transform.position.z > 53)
        {
            transform.position = new Vector3(transform.position.x, 0, -53);
            Debug.Log("Wraped");
        }
        if (transform.position.z < -53)
        {
            transform.position = new Vector3(transform.position.x, 0, 53);
            Debug.Log("Wraped");
        }
        if (transform.position.x > 120)
        {
            transform.position = new Vector3(-120, 0, transform.position.z);
            Debug.Log("Wraped");
        }
        if (transform.position.x < -120)
        {
            transform.position = new Vector3(120, 0, transform.position.z);
            Debug.Log("Wraped");
        }
    }

    private void FixedUpdate()
    {
        LayerMask layerMask = (1 << 9) | (1 << 10);

        if (transform.localScale.x > .5f || transform.localScale.y > .5f || transform.localScale.z > .5f)
        {
            Collider[] collisions = Physics.OverlapSphere(transform.position, transform.localScale.magnitude, layerMask);
            int i = 0;

            while (i < collisions.Length)
            {
                GameObject objHit = collisions[i].gameObject;

                if (objHit.tag == "Player")
                {
                    pc.GetHit();
                } else if (objHit.tag == "Enemy")
                {
                    ac = objHit.GetComponentInChildren<AlienController>();
                    ac.getHit();
                }
                i++;
            }
        }

    }

    public void getHit()
    {

        explosion.Play();
        explosion.transform.SetParent(null);
        explosionSound.Play();
        explosion.transform.SetParent(null);
        Destroy(explosionSound, 1f);
        Destroy(explosion, 1f);
    }

}
