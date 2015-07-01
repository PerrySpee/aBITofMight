using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image AbilityTimer1,
                    AbilityTimer2,
                    AbilityTimer3,
                    AbilityTimer4;

    public float AbilityTime1,
                    AbilityTime2,
                    AbilityTime3,
                    AbilityTime4;

    private float CurHealth, MaxHealth;
    private float curExp, maxExp;

    private Knight knight;

    public Color[] Colors;

    public Image playerIndicator;
    public Image playerHealthBar;

    private Image healthBar;
    private float HealthPercentage;
    private Text healthNumber;
    private Text ability1TimerNumber;
    private Text ability2TimerNumber;
    private Text ability3TimerNumber;
    private Text ability4TimerNumber;
    private Image experienceBar;
    private Text skillpoints;
    private Text level;

    private Image agilityBG;
    private Image powerBG;
    private Image defenseBG;
    private Text agilityAmount;
    private Text powerAmount;
    private Text defAmount;

    void Start()
    {
        knight = GetComponent<Knight>();
        GetIndicatorSprite();
        //basic stats
        healthBar = GameObject.Find("Player" + knight.PlayerID + "Health").GetComponent<Image>();
        healthNumber = GameObject.Find("Player" + knight.PlayerID + "HealthNumber").GetComponent<Text>();
        skillpoints = GameObject.Find("Player" + knight.PlayerID + "SkillPoints").GetComponent<Text>();
        level = GameObject.Find("Player" + knight.PlayerID + "Level").GetComponent<Text>();
        //cooldown count
        ability1TimerNumber = GameObject.Find("Player" + knight.PlayerID + "Ability1TimerNumber").GetComponent<Text>();
        ability2TimerNumber = GameObject.Find("Player" + knight.PlayerID + "Ability2TimerNumber").GetComponent<Text>();
        ability3TimerNumber = GameObject.Find("Player" + knight.PlayerID + "Ability3TimerNumber").GetComponent<Text>();
        ability4TimerNumber = GameObject.Find("Player" + knight.PlayerID + "Ability4TimerNumber").GetComponent<Text>();
        //cooldowns fill
        AbilityTimer1 = GameObject.Find("Player" + knight.PlayerID + "Ability1Timer").GetComponent<Image>();
        AbilityTimer2 = GameObject.Find("Player" + knight.PlayerID + "Ability2Timer").GetComponent<Image>();
        AbilityTimer3 = GameObject.Find("Player" + knight.PlayerID + "Ability3Timer").GetComponent<Image>();
        AbilityTimer4 = GameObject.Find("Player" + knight.PlayerID + "Ability4Timer").GetComponent<Image>();
        experienceBar = GameObject.Find("Player" + knight.PlayerID + "ExperienceBar").GetComponent<Image>();
        //skill backgrounds
        agilityBG = GameObject.Find("Player" + knight.PlayerID + "AGI_BG").GetComponent<Image>();
        powerBG = GameObject.Find("Player" + knight.PlayerID + "PWR_BG").GetComponent<Image>();
        defenseBG = GameObject.Find("Player" + knight.PlayerID + "DEF_BG").GetComponent<Image>();
        //skillpoints counter
        agilityAmount = GameObject.Find("Player" + knight.PlayerID + "AGI").GetComponent<Text>();
        powerAmount = GameObject.Find("Player" + knight.PlayerID + "POW").GetComponent<Text>();
        defAmount = GameObject.Find("Player" + knight.PlayerID + "DEF").GetComponent<Text>();

        //default not on
        agilityAmount.enabled = false;
        powerAmount.enabled = false;
        defAmount.enabled = false;
    }


    void Update()
    {
        MaxHealth = knight.MaxHealth;

        GetCurHealth();
        SetHealthColor();
        ShowCooldowns();
        GetExpNumbers();
        SetStats();
        ShowStats();
        SetRotation();

        
        healthNumber.text = knight.CurHealth.ToString();

        experienceBar.rectTransform.localScale = new Vector3(experienceBar.rectTransform.localScale.x, curExp / maxExp, experienceBar.rectTransform.localScale.z);

        HealthPercentage = CurHealth / MaxHealth * 100;
        healthBar.rectTransform.localScale = new Vector3(healthBar.rectTransform.localScale.x, CurHealth / MaxHealth, healthBar.rectTransform.localScale.z);
        playerHealthBar.rectTransform.localScale = new Vector3(CurHealth / MaxHealth, playerHealthBar.rectTransform.localScale.y, playerHealthBar.rectTransform.localScale.z);

        AbilityTime1 = knight.AbilityTime1;
        AbilityTime2 = knight.AbilityTime2;
        AbilityTime3 = knight.AbilityTime3;
        AbilityTime4 = knight.AbilityTime4;

        level.text = "lv" + knight.level.ToString();
        skillpoints.text = knight.skillPoints.ToString() + "sp";

    }
    
    private void GetIndicatorSprite()
    {
        Renderer rend = transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>();
        switch (knight.PlayerID)
        {
            case 1:
                playerIndicator.color = Color.red;
                //transform.GetChild(0).transform.GetChild(0).GetComponent<Material>().SetColor("outline", Color.red);
                rend.material.shader = Shader.Find("Standard Outlined");
                rend.material.SetColor("_OutColor", Color.red);
                break;
            case 2:
                playerIndicator.color = Color.cyan;
                rend.material.shader = Shader.Find("Standard Outlined");
                rend.material.SetColor("_OutColor", Color.cyan);
                break;
            case 3:
                playerIndicator.color = Color.green;
                rend.material.shader = Shader.Find("Standard Outlined");
                rend.material.SetColor("_OutColor", Color.green);
                break;
            case 4:
                playerIndicator.color = Color.yellow;
                rend.material.shader = Shader.Find("Standard Outlined");
                rend.material.SetColor("_OutColor", new Color(255,111,0));
                break;
        }
    }

    private void GetCurHealth()
    {
        CurHealth = knight.CurHealth;
    }

    private void GetExpNumbers()
    {
        curExp = knight.curExp;
        maxExp = knight.maxExp;
    }

    private void ShowCooldowns()
    {
        AbilityTimer1.rectTransform.localScale = new Vector3(AbilityTimer1.rectTransform.localScale.x, AbilityTime1 / knight.Ability1Cooldown, AbilityTimer1.rectTransform.localScale.z);
        AbilityTimer2.rectTransform.localScale = new Vector3(AbilityTimer2.rectTransform.localScale.x, AbilityTime2 / knight.Ability2Cooldown, AbilityTimer2.rectTransform.localScale.z);
        AbilityTimer3.rectTransform.localScale = new Vector3(AbilityTimer3.rectTransform.localScale.x, AbilityTime3 / knight.Ability3Cooldown, AbilityTimer3.rectTransform.localScale.z);
        AbilityTimer4.rectTransform.localScale = new Vector3(AbilityTimer4.rectTransform.localScale.x, AbilityTime4 / knight.Ability4Cooldown, AbilityTimer4.rectTransform.localScale.z);
        if (knight.AbilityTime1 >= 0.1f)
        {
            ability1TimerNumber.text = (knight.Ability1Cooldown - knight.AbilityTime1).ToString();
        }
        else
        {
            ability1TimerNumber.text = "";
        }

        if (knight.AbilityTime2 >= 0.1f)
        {
            ability2TimerNumber.text = (knight.Ability2Cooldown - knight.AbilityTime2).ToString();
        }
        else
        {
            ability2TimerNumber.text = "";
        }

        if (knight.AbilityTime3 >= 0.1f)
        {
            ability3TimerNumber.text = (knight.Ability3Cooldown - knight.AbilityTime3).ToString();
        }
        else
        {
            ability3TimerNumber.text = "";
        }

        if (knight.AbilityTime4 >= 0.1f)
        {
            ability4TimerNumber.text = (knight.Ability4Cooldown - knight.AbilityTime4).ToString();
        }
        else
        {
            ability4TimerNumber.text = "";
        }
    }

    private void SetHealthColor()
    {
        if (HealthPercentage >= 65)
        {
            healthBar.color = Colors[0];
            playerHealthBar.color = Colors[0];
        }
        else if (HealthPercentage < 65 && HealthPercentage >= 30)
        {
            healthBar.color = Colors[1];
            playerHealthBar.color = Colors[1];
        }
        else if (HealthPercentage < 30)
        {
            healthBar.color = Colors[2];
            playerHealthBar.color = Colors[2];
        }
    }

    private void SetStats()
    {
        if (knight.skillPoints > 0)
        {
            agilityBG.color = Colors[4];
            powerBG.color = Colors[4];
            defenseBG.color = Colors[4];
        }
        else
        {
            agilityBG.color = Colors[3];
            powerBG.color = Colors[3];
            defenseBG.color = Colors[3];
        }

        agilityAmount.text = knight.Agility.ToString();
        powerAmount.text = knight.Power.ToString();
        defAmount.text = knight.Defense.ToString();
    }

    private void ShowStats()
    {
        if (Input.GetButton("SkillP" + knight.PlayerID))
        {
            agilityAmount.enabled = true;
            powerAmount.enabled = true;
            defAmount.enabled = true;
        }
        if (Input.GetButtonUp("SkillP" + knight.PlayerID))
        {
            agilityAmount.enabled = false;
            powerAmount.enabled = false;
            defAmount.enabled = false;
        }
    }

    void SetRotation()
    {
        if (knight.facing == 1)
        {
            playerHealthBar.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
            playerHealthBar.rectTransform.localPosition = new Vector3(-2, 2, 0);
        }
        else
        {
            playerHealthBar.rectTransform.localEulerAngles = new Vector3(0, 0, 180);
            playerHealthBar.rectTransform.localPosition = new Vector3(2, 2, 0);
        }
    }
}