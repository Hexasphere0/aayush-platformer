using UnityEngine;
using UnityEngine.UI;

public class BossFightCheckpoint : MonoBehaviour
{
    float initialTime;
    float timeRemaining = 0;
    bool active = false;

    public Image timerRadial;

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timerRadial.fillAmount = timeRemaining / initialTime;

        if(timeRemaining <= 0 && active)
        {
            BossFightManager.LooseBossFight();
            Destroy(gameObject);
        }
    }

    public void Initialize(float time)
    {
        initialTime = time;
        timeRemaining = time;

        active = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(active && collision.gameObject.name == "Player")
        {
            Debug.Log("TAKING DAMGE!");
            BossController2.instance.TakeDamage();
            Destroy(gameObject);
        }
    }
}
