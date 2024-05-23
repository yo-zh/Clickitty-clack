using UnityEngine;

public class Target : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject target;
    [SerializeField] private ParticleSystem explosionParticle;

    [SerializeField] private int amountOfPoints;

    private Rigidbody targetRb;
    private float minSpeed = 10;
    private float maxSpeed = 20;
    private float maxTorque = 10;
    private float xSpawnRange = 5;
    private float ySpawnPosition = -1;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomUpwardForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomPosition();
    }

    private void OnMouseOver()
    {
        if (gameManager.isGameActive && Input.GetMouseButton(0))
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(amountOfPoints);

            if (gameObject.CompareTag("BadTarget"))
            {
                gameManager.IncreaseExplosions();
                gameManager.PlayExplosion();
            }
            else
            {
                gameManager.PlayRandomMeow();
            }
        }
    }

    private void OnTriggerEnter(Collider other) // MB check for collision in case targets collide w/ each other at spawn, because it looks bad
    {
        Destroy(gameObject);

        if (!gameObject.CompareTag("BadTarget"))
        {
            gameManager.UpdateScore(-amountOfPoints);
        }
    }

    Vector3 RandomUpwardForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-xSpawnRange, xSpawnRange), ySpawnPosition);
    }
}
