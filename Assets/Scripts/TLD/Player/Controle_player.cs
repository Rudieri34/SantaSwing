using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Controle_player : MonoBehaviour
{
    public float jump;
    public float maxSpeed;
    public bool canJump, pause;
    public bool isGrabbingWall, canWallJump;

    public GameObject bullet;
    public LineRenderer line;
    public DistanceJoint2D hook;
    public LayerMask mask;
    public Rigidbody2D rigidbody;
    public Animator anim;
    public Transform characterImage;


    public float lifes = 1;

    public float imortal;
    public bool startBlinking = false;
    public bool canDash = true;

    public bool home;



    public bool hasDash;
    public bool hasWallJump;
    public bool hasMolotov;

    float movimento;
    public bool canMove = true;


    Vector3 targetPos;
    Collider2D[] raycast;
    bool isHook;
    bool isDead = false;

    public bool isDashing;
    private float dashingPower = 100;
    private int dashingTime = 200;
    private float dashingCooldown = 1f;



    public GameObject ghostPrefab; // Prefab do Ghost
    public float ghostSpawnInterval = 0.1f; // Intervalo de criação
    public Color ghostColor = new Color(1f, 1f, 1f, 0.5f); // Cor do ghost
    private bool showGhost;

    private float ghostTimer;


    float spriteBlinkingTimer = 0.0f;
    float spriteBlinkingMiniDuration = 0.1f;
    float spriteBlinkingTotalTimer = 0.0f;
    float spriteBlinkingTotalDuration = 1f;


    public enum WallJumpDirection
    {
        Left,
        Right
    }
    private WallJumpDirection wallJumpDirection;


    void Awake()
    {
        movimento = 1;
        // LoadSave();

        //fazer save
    }

    // Update is called once per frame  
    private void FixedUpdate()
    {

        anim.SetBool("Home", home);

        if (!isHook && !isDead && !pause && !isGrabbingWall)
        {
            if (isDashing)
            {
                return;
            }

            if (canMove)
            {
                if (movimento != 0)
                {
                    rigidbody.velocity = new Vector2(maxSpeed * movimento, rigidbody.velocity.y);
                    anim.SetBool("Running", true);
                }
                else
                {
                    if (canJump)
                        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                    anim.SetBool("Running", false);
                }

                if (movimento > 0)
                {
                    characterImage.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (movimento < 0)
                {
                    characterImage.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
        else
            anim.SetBool("Running", false);
    }
    void Update()
    {
        imortal -= Time.deltaTime;
        if (!isDead && !pause)
        {

            //pulo
            if (Input.GetButtonDown("Jump"))
            {
                if(canJump)
                    OnJump();
                else if(canWallJump)
                    OnWallJump();
                
            }

            if (Input.GetButtonDown("Hook"))
            {
                OnHookKeyPressed();
            }

            if (Input.GetButton("Hook") && isHook)
            {
                anim.SetBool("Hook", true);
                line.SetPosition(0, new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z));
            }

            if (Input.GetButtonUp("Hook"))
            {
                OnHookKeyReleased();
            }
            if (Input.GetButtonDown("Fire2"))
                    OnTrowBottle();

            if (Input.GetButtonDown("Fire3") && !isDashing && hasDash)
            {
                Dash();
            }
            /*
            if (Input.GetButtonDown("Save"))
            {
               OnSave();
            }*/
            if (lifes == 0)
            {
                Death();
            }

            // Atualiza o timer
            ghostTimer -= Time.deltaTime;

            // Cria um ghost se o tempo for atingido
            if ((ghostTimer <= 0) && showGhost)
            {
                SpawnGhost();
                ghostTimer = ghostSpawnInterval;
            }

        }
        else
            anim.SetBool("Jumping", false);

        if (startBlinking == true)
        {
            SpriteBlinkingEffect();
        }
    }

    void SpawnGhost()
    {
        // Instancia o ghost
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);

        // Configura o sprite e a cor do ghost
        SpriteRenderer playerSprite = characterImage.GetComponent<SpriteRenderer>();
        GhostEffect ghostEffect = ghost.GetComponent<GhostEffect>();
        ghostEffect.SetSprite(playerSprite.sprite, ghostColor, playerSprite.flipX);
    }
    void OnJump()
    {
        anim.SetBool("Jumping", true);
        if (canJump == true)
        {
            SoundManager.Instance.PlaySFX("Jump");

            canJump = false;
            rigidbody.velocity = Vector2.up * jump;
            characterImage.DOScaleY(1.2f, .2f).SetEase(Ease.Linear).OnComplete(() =>
                characterImage.DOScaleY(1f, .2f).SetEase(Ease.Linear)
                );
            characterImage.DOScaleX(.8f, .2f).SetEase(Ease.Linear).OnComplete(() =>
                characterImage.DOScaleX(1f, .2f).SetEase(Ease.Linear)
                );
        }
    }
    public void SetWallJumpDirection(WallJumpDirection direction)
    {
        wallJumpDirection = direction;
    }

    async void OnWallJump()
    {
        //put async lock

        anim.SetBool("Jumping", true);
        if (canWallJump == true)
        {
            SoundManager.Instance.PlaySFX("Jump");

            canWallJump = false;
            anim.SetBool("WallJump", false);

            rigidbody.velocity = Vector2.up * jump;

            if(wallJumpDirection == WallJumpDirection.Left)
                rigidbody.velocity += Vector2.right * jump;
            else
                rigidbody.velocity += Vector2.left * jump;

            characterImage.DOScaleY(1.2f, .2f).SetEase(Ease.Linear).OnComplete(() =>
                characterImage.DOScaleY(1f, .2f).SetEase(Ease.Linear)
                );
            characterImage.DOScaleX(.8f, .2f).SetEase(Ease.Linear).OnComplete(() =>
                characterImage.DOScaleX(1f, .2f).SetEase(Ease.Linear)
                );

            canMove = false;

           // await UniTask.Delay(200);

            movimento *= -1; 
            canMove = true;

        }
    }

    void OnTrowSalt()
    {
        anim.SetTrigger("AttackMelee");

    }

    void OnTrowBottle()
    {
        anim.SetTrigger("AttackRanged");
        GameObject fire = Instantiate(bullet) as GameObject;

        if (GetComponentInChildren<SpriteRenderer>().flipX == true)
        {
            fire.transform.position = new Vector2(transform.position.x - .2f, transform.position.y);
            Rigidbody2D rigidbodyBullet = fire.GetComponentInChildren<Rigidbody2D>();
            fire.GetComponentInChildren<SpriteRenderer>().flipX = true;
            rigidbodyBullet.velocity = new Vector2(-20, 0);
        }
        else
        {
            fire.transform.position = new Vector2(transform.position.x + .2f, transform.position.y);
            Rigidbody2D rigidbodyBullet = fire.GetComponentInChildren<Rigidbody2D>();
            rigidbodyBullet.velocity = new Vector2(20, 0);
        }
    }

    private void OnHookKeyPressed()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3.5f, mask);

        if (colliders.Length > 0)
        {
            // AudioManager.Instance.Play("Hook");

            // Cria uma lista de objetos do tipo Collider2D
            List<Collider2D> colliderList = new List<Collider2D>(colliders);

            // Ordena a lista com base na distância entre os objetos
            colliderList.Sort((c1, c2) =>
            {
                float dist1 = Vector2.Distance(transform.position, c1.transform.position);
                float dist2 = Vector2.Distance(transform.position, c2.transform.position);
                return dist1.CompareTo(dist2);
            });

            // Converte a lista ordenada em um array
            Collider2D[] sortedColliders = colliderList.ToArray();

            if (sortedColliders[0] != null && sortedColliders[0].gameObject.GetComponent<Rigidbody2D>() != null)
            {
                hook.enabled = true;
                line.enabled = true;
                //hook.connectedBody = raycast.collider.gameObject.GetComponent<Rigidbody2D>();
                hook.connectedAnchor = sortedColliders[0].transform.position;
                //hook.distance = Vector2.Distance(raycast.point, transform.position);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, sortedColliders[0].transform.position);

                isHook = true;
                rigidbody.drag = 0;

            }


        }
        //else AudioManager.Instance.Play("HookFalse");

    }

    private void OnHookKeyReleased()
    {
        hook.enabled = false;
        line.enabled = false;

        if(isHook)
            rigidbody.drag = 1;

        isHook = false;

    }

    private async void Dash()
    {
        if (movimento == 0)
            return;

        isDashing = true;
        float originalGravity = rigidbody.gravityScale;
        rigidbody.gravityScale = 0f;
        SoundManager.Instance.PlaySFX("Dash");


        float oldVelocity = rigidbody.velocity.x;


        rigidbody.velocity = new Vector2(oldVelocity, 0);

        if (movimento >= 0)
            rigidbody.AddForce(Vector3.right * dashingPower, ForceMode2D.Impulse);
        else if (movimento < 0)
            rigidbody.AddForce(Vector3.left * dashingPower, ForceMode2D.Impulse);

        characterImage.DOScaleX(1.3f, dashingTime / 2000).SetEase(Ease.Linear).OnComplete(() =>
              characterImage.DOScaleX(1f, dashingTime / 2000).SetEase(Ease.Linear)
            );
        characterImage.DOScaleY(.7f, dashingTime / 2000).SetEase(Ease.Linear).OnComplete(() =>
              characterImage.DOScaleY(1f, dashingTime / 2000).SetEase(Ease.Linear)
            );

        canMove = false;
        showGhost = true;

        await UniTask.Delay(dashingTime);
        rigidbody.gravityScale = originalGravity;
        rigidbody.velocity = new Vector2(oldVelocity, 0);
        canMove = true;
        showGhost = false;

        await UniTask.Delay(dashingTime);
        if(canJump)
            isDashing = false;

    }

    public async void Death()
    {
        hook.enabled = false;
        line.enabled = false;
        isHook = false;
        rigidbody.angularDrag = 1;
        rigidbody.drag = 1;
        startBlinking = true;
        isDead = true;
        pause = true;
        await UniTask.Delay(300);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Stop()
    {

    }
    /*
    void OnSave()
    {
        SaveSystem.Change("PlayerPosX", transform.position.x);
        SaveSystem.Change("PlayerPosY", transform.position.y);

        SaveSystem.Change("MaxLife", Maxlifes);
        SaveSystem.Change("BottleAmount", ammo);
    }

    public void LoadSave(bool UpgradesOnly = false)
    {
        SaveSystem.LoadGame();


        Maxlifes = SaveSystem.saves["MaxLife"];

        hasDash = SaveSystem.saves["Dash"] != 0;
        hasWallJump = SaveSystem.saves["WallJump"] != 0;
        hasMolotov = SaveSystem.saves["Molotov"] != 0;
        hasHook = SaveSystem.saves["Hook"] != 0;

        if (!UpgradesOnly)
        {
            transform.position = new Vector2(SaveSystem.saves["PlayerPosX"], SaveSystem.saves["PlayerPosY"]);
            lifes = Maxlifes;
            ammo = (int)SaveSystem.saves["BottleAmount"];
        }

    }
    */
    private void SpriteBlinkingEffect()
    {
        spriteBlinkingTotalTimer += Time.deltaTime;
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            startBlinking = false;
            spriteBlinkingTotalTimer = 0.0f;
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            return;
        }

        spriteBlinkingTimer += Time.deltaTime;
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            spriteBlinkingTimer = 0.0f;
            if (this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled == true)
            {
                this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            else
            {
                this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
        }
    }



}

