using UnityEngine;

public class BulletLogicController : MonoBehaviour
{
    Rigidbody bullet;
    private float speed = 50f;
    private float lifeTimer = 3f;
    public int scoreValue;
    private LogicController lc;
    private PlayerController pc;
    private AlienController ac;
    private EnnemyController ec;
    public int type;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>(); 
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        lc = gameControllerObject.GetComponent<LogicController>();
        bullet = GetComponent<Rigidbody>();
        bullet.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {

        lifeTimer -= Time.deltaTime;

        //Check Boundaries

        if (transform.position.z > 53)
        {
            transform.position = new Vector3(transform.position.x, 0, -53);
            checkLife();
        }
        if (transform.position.z < -53)
        {
            transform.position = new Vector3(transform.position.x, 0, 53);
            checkLife();
        }
        if (transform.position.x > 120)
        {
            transform.position = new Vector3(-120, 0, transform.position.z);
            checkLife();
        }
        if (transform.position.x < -120)
        {
            transform.position = new Vector3(120, 0, transform.position.z);
            checkLife();
        }

    }

    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = (1 << 8) | (1 << 10) | (1 << 9);
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, speed * Time.fixedDeltaTime, layerMask))
        {
            GameObject itemHit = hit.collider.gameObject;
            Vector3 breakOffPos = hit.transform.position;
            Quaternion rotation = Quaternion.identity;
            itemHit.transform.localScale = new Vector3(hit.transform.localScale.x / 3, hit.transform.localScale.y / 3, hit.transform.localScale.z / 3);

            //Identity Check
            if (hit.collider.tag == "Player")
            {

                    Debug.Log("Entered player Collision");
                    pc.GetHit();
                    Destroy(hit.collider.gameObject);
                    Destroy(gameObject);
                

            }
            else if (hit.collider.tag == "Enemy")
            {
                if (type == 1)
                {
                    ac = hit.collider.gameObject.GetComponent<AlienController>();
                    lc.AddScore(ac.scoreValue);
                    ac.getHit();
                    Destroy(hit.collider.gameObject);
                    Destroy(gameObject);
                }
            }
            else if (hit.collider.tag == "Asteroid")
            {
                Debug.Log("Entered asteroid Collision");
                ec = hit.collider.gameObject.GetComponent<EnnemyController>();
                lc.AddScore(ec.scoreValue);
                ec.getHit();
                for (int i = 0; i < 3; i++)
                {
                    GameObject go = Instantiate(itemHit, breakOffPos, rotation);
                    go.GetComponent<Collider>().enabled = false;

                }
                Destroy(hit.collider.gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void checkLife()
    {
        if (lifeTimer < 0)
        {
            Destroy(gameObject);
        }
    }


}
