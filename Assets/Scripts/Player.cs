using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Player Speed/Thruster
    /*[Summary]
     * This section handles the player's speed
     * 
     * 
     * 
     */
    [SerializeField] 
    private float BaseSpeed = 5;
    [SerializeField]
    private float MaxSpeed = 20;
    private float ThrusterSpeed;
    [SerializeField]
    private float speed;
    [SerializeField][Range(0,1)]
    private float ThrusterSpeedAnim;
    private bool CanBoost = true;
    private bool IsBoosting;
    private float InvulnTime = -1;
    private float DashTime = -1; //dash input timer
    [SerializeField]
    private float DashCoolDown; //variable to control input cooldown
    [SerializeField]
    public bool IsInvul = false;
    private bool IsDodging;
    #endregion
    #region Projectile Properties
    /*[Summary]
     * This section handles all laser instantantiation and Cooldowns
     * [End]
     * 
     * 
     * projectile array list
     * 
     * [0 - Normal Shot]
     * [1 - Triple Shot]
     * [2 - Explosive Rockets]
     * [3 - Homing Missles]
     * 
     * [Variable Doc]
     * 
     * Projectile array is used to store our projectile prefabs for later use. 
     * ie: instantiate on fire();
     * 
     * SkillCD handles the Cool down of each abillity set in input S D F G.
     * Each ship will have four abillities that will differ from each other, Thus the reason to create an array.
     * 
     * TripleShotTime and RocketShotTime is the duration of its suggested ammo type.
     * Note: The Difference bettween RocketShot and Missle is that:
     *         
     *         Rocket Explodes, Missle follows.
     * 
     * CanFire  is the general cooldown for the player's Basic Projectile.
     * 
     * Ammo1 is a volitle variable that will alter when the player fires. 
     * Its used in a way to check the amount of ammo the player has
     * at all times.
     * 
     * Ammoref is a "static" variable that keep track of the players orginal starting ammo. 
     * We refer to this when the player needs to reload.
     * ie: ammo1 = ammoref
     * 
     * FireRate Affects the rate of fire of all projectiles. This variable will change based on what
     * Projectile the player is shooting
     * FireRateRef keeps the original value of our FireRate variable. It is used to return FireRate back
     * its original value.
     *
     *ShotID is used to switch bettween various projectiles
     * 
     * [Variable Doc End]
     * 
     */
    [SerializeField]
    GameObject[] Projectiles;
    [SerializeField]
    float[] SkillCD;
    [SerializeField]
    int Ammo1;
    int Ammoref1;
    private float TriplerShotTime = -1;
    private float RocketShotTime = -1;
    private float CanFire = -1;
    [SerializeField]
    private float FireRate = 0.5f;
    private float FireRateRef;

    [SerializeField]
    int ShotID;

    #endregion
    #region Health,Shield,Damage Effect
    [SerializeField]
    private GameObject Shield;
    [SerializeField]
    private float BulletOffSet;

    [SerializeField]
    private int PlayerHealth = 3;
    private int ShieldHealth = 0;
   
    [SerializeField]
    GameObject RightEngine;
    [SerializeField]
    GameObject LeftEngine;
    [SerializeField]
    GameObject PlayerExplode;
    #endregion

    Camera Main;

    [SerializeField]
    private int Score;

    [SerializeField]
    AudioClip[] PlayerFX;
    AudioSource PlayerAudio;

    private bool IsPaused = false;

    [SerializeField]
    Animator Thruster;
    #region Player Assist
    [SerializeField]
    private List<GameObject> AssistGameObject = new List<GameObject>();
    private GameObject TempTarget;
    Player_Assist PARetarget;
    private List<GameObject> ListOfAssist = new List<GameObject>();
    private int AssistCount;
    #endregion

    SpawnManager spawnManager;
    UIManager PUI;

    void Start()
    {
        speed = BaseSpeed;
        FireRateRef = FireRate;
        ThrusterSpeed = speed * 4;
        GameObject FindSpawnManager = GameObject.Find("Spawn Manager");
        if(FindSpawnManager != null)
        {
            spawnManager = FindSpawnManager.GetComponent<SpawnManager>();
        } else
        {
            Debug.LogError("Spawn Manager null reference");
        }
        GameObject FindPlayerUI = GameObject.Find("UIManager");
        if (FindPlayerUI != null)
        {
            PUI = FindPlayerUI.GetComponent<UIManager>();
        }
        else
        {
            Debug.LogError("PLAYER:UIManager is null");
        }
        Main = Camera.main;
        PUI.UpdateLives(PlayerHealth);
        PlayerAudio = GetComponent<AudioSource>();
        PlayerAudio.clip = PlayerFX[0];

        Ammoref1 = Ammo1;
        
    }
    void Update()
    {
        if (IsPaused == false)
        {
            Fire();
            Abillites();
            Boundaries();
            Movement();
            if (Input.GetKeyDown(KeyCode.O))
            {
                SpawnAssist();
            }
        }
        PauseGame();
    }
    void Fire()
    {
        if (Input.GetKey(KeyCode.A) && Time.time >= CanFire)
        {
            CanFire = Time.time + FireRate;
            switch (ShotID)
            {
                case 0: 
                    Instantiate(Projectiles[0], new Vector3(transform.position.x, transform.position.y + BulletOffSet, transform.position.z), Quaternion.identity);
                    PlayerAudio.Play();
                    PlayerAudio.volume = 4;


                    break;
                case 1:
                    if (ShotID == 1 && Time.time < TriplerShotTime)
                    {
                        Instantiate(Projectiles[1], new Vector3(transform.position.x, transform.position.y + BulletOffSet, transform.position.z), Quaternion.identity);
                        PlayerAudio.Play();
                        PlayerAudio.volume = 6;
                    }
                    else
                    {
                        ShotID = 0;
                    }
                    break;
                case 2:
                    if (ShotID == 2 && Time.time < RocketShotTime)
                    {
                        FireRate = 0.55f;
                        Instantiate(Projectiles[2], new Vector3(transform.position.x, transform.position.y + BulletOffSet, transform.position.z), Quaternion.identity);
                        PlayerAudio.Play();
                        PlayerAudio.volume = 6;
                    }
                    else
                    {
                        ShotID = 0;
                        FireRate = FireRateRef;
                    }
                    break;
            }
        }
    }
    
    void Movement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        //check if object collider if dodging
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > DashTime)
        {
            IsBoosting = true;
            StartCoroutine(Boost(ThrusterSpeed / 1.5f));
            DashTime = Time.time + DashCoolDown;
        } 

            transform.Translate(new Vector3(HorizontalInput, VerticalInput, 0) * speed * Time.deltaTime);
    }
    
     void Boundaries()
    {
    
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f,5), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        } else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }
   public void Damage(int dmg,GameObject obj)
    {
        
        if (ShieldHealth > 1)
        {
            Shield.transform.localScale = Shield.transform.localScale * .8f;
            ShieldHealth--;
            return;
        } else
        {
            Shield.SetActive(false);
        }
        if(obj.tag == "Enemy")
        {
            PlayerHealth -= dmg;
            PUI.UpdateLives(PlayerHealth);
            Destroy(obj);
            switch (PlayerHealth)
            {
                case 2:
                    LeftEngine.SetActive(true);
                    Main.GetComponent<Animator>().SetTrigger("Shake");
                    break;
                case 1:
                    Main.GetComponent<Animator>().SetTrigger("Shake");
                    RightEngine.SetActive(true);
                    break;
            }
        }
        if(IsInvul == false && IsDodging == false)
        {
            PlayerHealth -= dmg;
            PUI.UpdateLives(PlayerHealth);
            Destroy(obj);
            switch (PlayerHealth)
            {
                case 2:
                    LeftEngine.SetActive(true);
                    Main.GetComponent<Animator>().SetTrigger("Shake");
                    break;
                case 1:
                    Main.GetComponent<Animator>().SetTrigger("Shake");
                    RightEngine.SetActive(true);
                    break;
            }
        }
        if(PlayerHealth < 1)
        {
            Main.GetComponent<Animator>().SetTrigger("Shake");
            Destroy(gameObject.GetComponent<Collider2D>()); 
            spawnManager.OnPlayerDeath();
            Destroy(gameObject,0.2f);
            Instantiate(PlayerExplode, transform.position, Quaternion.identity);
        }
    }

    public void RocketActive()
    {
        if (ShotID == 2)
        {
            RocketShotTime += 5;
        }
        else
        {
            ShotID = 2;
            RocketShotTime = Time.time + 5;
        }
    }
    public void TripleShotActive()
    {
        if(ShotID == 1)
        {
            TriplerShotTime += 5;
        } else
        {
            ShotID = 1;
            TriplerShotTime = Time.time + 5;
        }
    }
    public void SpeedBoost()
    {
        if (BaseSpeed < MaxSpeed)
        {
            BaseSpeed += 1;
            speed = BaseSpeed;
            ThrusterSpeed = speed * 3;
        } else
        {
            BaseSpeed = MaxSpeed;
            IsInvul = true;
            InvulnTimer();
        }
    }
    void InvulnTimer()
    {
        if (IsDodging == false)
        {
            //speedboost 
            if (Time.time < InvulnTime)
            {
                InvulnTime = Time.time + 5;
                IsInvul = true;
            }
            else
            {
                IsInvul = false;
            }
        }
        if(IsDodging == true)
        {
            if (Time.time < InvulnTime)
            {
                InvulnTime = Time.time + 0.9f;
                IsInvul = true;
                print("true");
            }
            else
            {
                print("false");
                IsInvul = false;
                IsDodging = false;
            }
        }
    }
    
    public void ShieldActive()
    {
        ShieldHealth = 3;
        Shield.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
        Shield.SetActive(true);
    }
    public void AddPoints(int PointsToAdd)
    {
        Score += PointsToAdd;
        PUI.UpdateScore(Score);
    }
    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            if (IsPaused == true)
            {
                PUI.PauseMenu();
            }
        }
        if (Time.timeScale == 1)
        {
            IsPaused = false;
        }
    }
    private void Abillites()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            if(Time.time >= SkillCD[0])
            {
                if (Ammo1 > 0)
                {
                    Ammo1--;
                    SkillCD[0] = Time.time + 1;
                    StartCoroutine(BurstfireTimer());
                    PUI.UpdateAmmo(Ammo1,Ammoref1);
                }
            }
        }
        /*
        if (Input.GetKey(KeyCode.D))
        {
            if (Time.time > SkillCD[1])
            {

            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (Time.time > SkillCD[2])
            {

            }
        }
        if (Input.GetKey(KeyCode.G))
        {
            if (Time.time > SkillCD[3])
            {

            }
        }
        */
    }
    IEnumerator BurstfireTimer()
    {
        for (int i = 0; i < 7;)
        {
            Instantiate(Projectiles[3], transform.position, Quaternion.identity);
            PlayerAudio.Play();
            PlayerAudio.volume = 4;
            yield return new WaitForSeconds(0.1f);
            i++;
        }

    }
    public void Reload()
    {
        Ammo1 = Ammoref1;
        PUI.UpdateAmmo(Ammo1,Ammoref1);
    }
    public void Heal()
    {
        PlayerHealth++;
        if(PlayerHealth > 3)
        {
            PlayerHealth = 3;
        }
        switch (PlayerHealth)
        {
            case 2:
                LeftEngine.SetActive(false);
                PUI.UpdateLives(PlayerHealth);
                break;
            case 1:
                RightEngine.SetActive(false);
                PUI.UpdateLives(PlayerHealth);
                break;
        }
    }

    IEnumerator Boost(float boostnum)
    {
        if(speed < ThrusterSpeed)
        {
            IsDodging = true;
            while (IsBoosting == true)
            {
                yield return new WaitForSeconds(0.1f);
                speed += boostnum;
                PUI.UpdateBoostUI(speed * 0.001f,2);
                Thruster.SetFloat("BFloatTrigger", ThrusterSpeedAnim += speed * .10f);
                if (ThrusterSpeedAnim > 1)
                {  
                    ThrusterSpeedAnim = 1;
                }
                if (speed > ThrusterSpeed)
                {  
                    speed = ThrusterSpeed;
                    PUI.UpdateBoostUI(0, 1);
                    CanBoost = false;
                    StartCoroutine(Deccerlate(-ThrusterSpeed * 0.5f));
                    IsBoosting = false;
                }
            }
        }
    }
    IEnumerator Deccerlate(float boostnum)
    {
        if(speed > BaseSpeed)
        {
            while (CanBoost == false)
            {
                yield return new WaitForSeconds(0.1f);
                speed += boostnum;
                PUI.UpdateBoostUI(-speed * 0.001f,2);
                Thruster.SetFloat("BFloatTrigger", ThrusterSpeedAnim -= 0.1f);
                if (ThrusterSpeedAnim < 0)
                {
                    ThrusterSpeedAnim = 0;
                }
                if (speed < BaseSpeed)
                {
                    speed = BaseSpeed;
                    PUI.UpdateBoostUI(0, 0);
                    IsDodging = false;
                    CanBoost = true;
                }
            }
        } 
        
            
        
    }
    public void SpawnAssist()
    {
        AssistCount = ListOfAssist.Count;
        GameObject NewAssist = Instantiate(AssistGameObject[0], this.transform.position, Quaternion.identity);
        ListOfAssist.Add(NewAssist);
        PARetarget = NewAssist.GetComponent<Player_Assist>();
        if (AssistCount == 0)
        {
            PARetarget.Retarget(this.gameObject);
        }
        else if (AssistCount > 0)
        {
            TempTarget = ListOfAssist[AssistCount - 1];
            PARetarget.Retarget(TempTarget);
        }
    }

}
