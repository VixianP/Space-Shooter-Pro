using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float BaseSpeed = 5;
    [SerializeField]
    private float MaxSpeed = 20;
    [SerializeField]
    private float ThrusterSpeed;
    private float speed;
    [SerializeField][Range(0,1)]
    private float ThrusterSpeedAnim;
    private float BoostTime;

    [SerializeField]
    private GameObject LaserPrefab;

    [SerializeField]
    GameObject[] Projectiles;
    [SerializeField]
    float[] SkillCD;
    [SerializeField]
    int Ammo1;
    [SerializeField]
    int Ammoref1;


    [SerializeField]
    private GameObject TripleShot;
    [SerializeField]
    private GameObject Shield;
    [SerializeField]
    private float BulletOffSet;

    [SerializeField]
    private float InvulnTime = -1;
    [SerializeField]
    private bool IsInvul = false;

    [SerializeField]
    private float CanFire = -1;
    private bool CanBoost = true;

    [SerializeField]
    private float TriplerShotTime = -1;
    [SerializeField]
    private float FireRate = 0.5f;
    [SerializeField]
    private int PlayerHealth = 3;
    private int ShieldHealth = 0;
   
    
    SpawnManager spawnManager;
    UIManager PUI;
    Camera Main;

    [SerializeField]
    GameObject RightEngine;
    [SerializeField]
    GameObject LeftEngine;
    [SerializeField]
    GameObject PlayerExplode;

    [SerializeField]
    private bool IsTripleShotActive;

    [SerializeField]
    private int Score;

    [SerializeField]
    AudioClip[] PlayerFX;
    AudioSource PlayerAudio;

    private bool IsPaused = false;
    private bool IsDodging;

    [SerializeField]
    Animator Thruster;

    void Start()
    {
        speed = BaseSpeed;
        ThrusterSpeed = speed * ThrusterSpeed;
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
        }
        PauseGame();
    }
    void Fire()
    {
        if (Input.GetKey(KeyCode.A ) && Time.time >= CanFire)
        {
            CanFire = Time.time + FireRate;
            if(IsTripleShotActive == true && Time.time < TriplerShotTime)
            {
                Instantiate(TripleShot, new Vector3(transform.position.x, transform.position.y + BulletOffSet, transform.position.z), Quaternion.identity);
                PlayerAudio.Play();
                PlayerAudio.volume = 6;
            } else
            {
                IsTripleShotActive = false;
                Instantiate(LaserPrefab, new Vector3(transform.position.x, transform.position.y + BulletOffSet, transform.position.z), Quaternion.identity);
                PlayerAudio.Play();
                PlayerAudio.volume = 4;
            }
        }
    }
    
    void Movement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.LeftShift) && CanBoost == true)
        {
            StartCoroutine(Boost(0.2f));
            if (BaseSpeed >= ThrusterSpeed)
            {
                BaseSpeed = ThrusterSpeed;
                CanBoost = false;
            }
        } else
        {
            StartCoroutine(Deccerlate(-0.2f));
            if (BaseSpeed <= speed)
            {
                BaseSpeed = speed;
            }
        }
 
        if (IsDodging == false)
        {
            transform.Translate(new Vector3(HorizontalInput, VerticalInput, 0) * BaseSpeed * Time.deltaTime);
        }
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
   public void Damage(int dmg)
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
        if(IsInvul == false)
        {
            PlayerHealth -= dmg;
            PUI.UpdateLives(PlayerHealth);
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
        if(PlayerHealth < 1 && IsInvul == false)
        {
            Main.GetComponent<Animator>().SetTrigger("Shake");
            Destroy(gameObject.GetComponent<Collider2D>()); //show atomic bomb
            spawnManager.OnPlayerDeath();
            Destroy(gameObject,0.2f);
            Instantiate(PlayerExplode, transform.position, Quaternion.identity);
        }
    }

    public void TripleShotActive()
    {
        if(IsTripleShotActive == true)
        {
            TriplerShotTime += 5;
        } else
        {
            IsTripleShotActive = true;
            TriplerShotTime = Time.time + 5;
        }
    }
    public void SpeedBoost()
    {
        if (BaseSpeed < MaxSpeed)
        {
            BaseSpeed += 1;
            speed = BaseSpeed;
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
            }
            else
            {
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
                    PUI.UpdateAmmo(Ammo1);
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
            Instantiate(Projectiles[0], transform.position, Quaternion.identity);
            PlayerAudio.Play();
            PlayerAudio.volume = 4;
            yield return new WaitForSeconds(0.1f);
            i++;
        }

    }
    public void Reload()
    {
        Ammo1 = Ammoref1;
        PUI.UpdateAmmo(Ammo1);
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
        if(BaseSpeed < ThrusterSpeed)
        {
            yield return new WaitForSeconds(0.1f);
            BaseSpeed += boostnum;
            PUI.UpdateBoostUI(0.1f);
            Thruster.SetFloat("BFloatTrigger", ThrusterSpeedAnim+= 0.1f);
            if(ThrusterSpeedAnim < 0)
            {
                ThrusterSpeedAnim = 0;
            }
        }
    }
    IEnumerator Deccerlate(float boostnum)
    {
        if(BaseSpeed > speed)
        {
            yield return new WaitForSeconds(0.5f);
            BaseSpeed += boostnum;
            PUI.UpdateBoostUI(-0.1f);
            Thruster.SetFloat("BFloatTrigger", ThrusterSpeedAnim -= 0.1f);
            if (ThrusterSpeedAnim > 1)
            {
                ThrusterSpeedAnim = 1;
            }
        } else
        {
            CanBoost = true;
        }
    }
 
}
