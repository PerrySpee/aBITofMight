using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{

    private Canvas enemyCanvas;
    private float currentHealth;
    private float maxHealth;
    private Image healthBar;

    private Vector3 rotationRight;


    void Start()
    {
        rotationRight = new Vector3(0, 0, 0);
        enemyCanvas = GetComponentInChildren<Canvas>();

        if (GetComponent<MeleeEnemy>() != null)
        {
            currentHealth = GetComponent<MeleeEnemy>().CurHealth;
            maxHealth = GetComponent<MeleeEnemy>().MaxHealth;
        }
        else if (GetComponent<RangedEnemy>() != null)
        {
            currentHealth = GetComponent<RangedEnemy>().CurHealth;
            maxHealth = GetComponent<RangedEnemy>().MaxHealth;
        }
        else if (GetComponent<TrollEnemy>() != null)
        {
            currentHealth = GetComponent<TrollEnemy>().CurHealth;
            maxHealth = GetComponent<TrollEnemy>().MaxHealth;
        }

        healthBar = enemyCanvas.transform.GetChild(0).GetComponent<Image>();
    }
       
    void Update()
    {
        GetCurHealth();
        SetRotation();

        healthBar.rectTransform.localScale = new Vector3(currentHealth / maxHealth, healthBar.rectTransform.localScale.y, healthBar.rectTransform.localScale.z);
      
    }


    private void GetCurHealth()
    {
        if (GetComponent<MeleeEnemy>() != null)
        {
            currentHealth = GetComponent<MeleeEnemy>().CurHealth;
            maxHealth = GetComponent<MeleeEnemy>().MaxHealth;
        }
        else if (GetComponent<RangedEnemy>() != null)
        {
            currentHealth = GetComponent<RangedEnemy>().CurHealth;
            maxHealth = GetComponent<RangedEnemy>().MaxHealth;
        }
        else if (GetComponent<TrollEnemy>() != null)
        {
            currentHealth = GetComponent<TrollEnemy>().CurHealth;
            maxHealth = GetComponent<TrollEnemy>().MaxHealth;
        }
    }

    void SetRotation()
    {
        if (transform.eulerAngles.y > 178 && transform.eulerAngles.y < 182)
        {
            healthBar.rectTransform.localEulerAngles = new Vector3(0, 180, 0);
            healthBar.rectTransform.localPosition = new Vector3(2, healthBar.rectTransform.localPosition.y, 0);
        }
        else if(transform.localEulerAngles == rotationRight)
        {
            healthBar.rectTransform.localEulerAngles = new Vector3(0, 0, 0);

            healthBar.rectTransform.localPosition = new Vector3(-2, healthBar.rectTransform.localPosition.y, 0);
        }
    }
}
