using UnityEngine;

public class BossController1 : MonoBehaviour
{
    float time = 0;

    [Header("Projectile Attack Settings")]
    public float timeBetweenProjectileAttacks;
    public int projectileCount;
    public int projectileLifetime;
    public float projectileDistanceFromCenter;
    public Vector2 projectileVelocity;
    
    public GameObject projectilePrefab;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        if(time % timeBetweenProjectileAttacks == 0)
        {
            Debug.Log("PROJECTILE ATTACK!");
            float angleBetweenProjectiles = (float) 360 / projectileCount;
            for(int i = 0; i < projectileCount; i++)
            {
                float projectileAngle = angleBetweenProjectiles * i;

                Vector3 direction = new Vector3(Mathf.Cos(projectileAngle), Mathf.Sin(projectileAngle)).normalized;

                GameObject projectileInstance = Instantiate(projectilePrefab);

                projectileInstance.transform.position = transform.position + direction * projectileDistanceFromCenter;
                projectileInstance.GetComponent<Projectile>().Initialize("Player", projectileLifetime);
                projectileInstance.GetComponent<Rigidbody2D>().AddForce(projectileVelocity, ForceMode2D.Impulse);

                // projectileInstance.GetComponent<Projectile>().Initialize(direction, projectileSpeed, projectileDamage, projectileLifetime, "Player", false);
            }

        }
    }
}
