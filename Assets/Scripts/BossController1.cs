using System;
using UnityEngine;

public class BossController1 : MonoBehaviour
{
    public float timeBetweenTicks;
    float timeSinceTick = 0f;
    int ticks = 0;

    [Header("Projectile Attack Settings")]
    public float ticksBetweenProjectileAttacks;
    public int projectileCount;
    public int projectileLifetime;
    public float projectileDistanceFromCenter;
    public float projectileVelocity;
    public float projectileScale;
    
    public GameObject projectilePrefab;
    
    void FixedUpdate()
    {
        timeSinceTick += Time.fixedDeltaTime;

        if(timeSinceTick >= timeBetweenTicks)
        {
            ticks++;
            timeSinceTick = 0;

            // Projectile Attack
            if(ticks % ticksBetweenProjectileAttacks == 0)
            {
                float angleBetweenProjectiles = (float) 360 / projectileCount;
                for(int i = 0; i < projectileCount; i++)
                {
                    float projectileAngle = angleBetweenProjectiles * i;
                    float projectileAngleRad = projectileAngle * Mathf.Deg2Rad;

                    Vector3 direction = new Vector3(Mathf.Cos(projectileAngleRad), Mathf.Sin(projectileAngleRad)).normalized;

                    GameObject projectileInstance = Instantiate(projectilePrefab);

                    projectileInstance.transform.position = transform.position + direction * projectileDistanceFromCenter;
                    projectileInstance.GetComponent<Projectile>().Initialize("Player", projectileLifetime);
                    projectileInstance.transform.localScale *= projectileScale;
                    projectileInstance.GetComponent<Rigidbody2D>().AddForce(direction * projectileVelocity, ForceMode2D.Impulse);

                    // Make half of the projectiles randomly blue
                    if(UnityEngine.Random.Range(0, 2) == 0)
                    {
                        projectileInstance.GetComponent<LayerChanger>().ChangeLayer();
                    }
                }
            }
        }
    }
}
