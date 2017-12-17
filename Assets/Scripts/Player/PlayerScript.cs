using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {


    Animator animator;

    private HealthScript health;
    
    //animation states - the values in the animator conditions
    const int STATE_IDLE_LEFT = 0;
    const int STATE_IDLE_RIGHT = 2;
    const int STATE_WALK_LEFT = 1;
    const int STATE_WALK_RIGHT = 3;    
    const int STATE_SHOOT_RIGHT = 6;
    const int STATE_SHOOT_LEFT = 5;


    int _currentAnimationState = STATE_IDLE_LEFT;

    string _currentDirection = "left";
    bool powerUpSpeed = false;
    float aktPowerUpTime = 0;
    float PowerUpTime = 0;

    float original_speed;


    public float speed = 1.5f;
    public Vector2 direction = new Vector2(-1, 0);

    private Vector2 movement;
    private Rigidbody2D rigidbodyComponent;

    public AudioClip throwSound;

    // Use this for initialization
    void Start()
    {
        //define the animator attached to the player
        animator = this.GetComponent<Animator>();
        original_speed = speed;
        health = this.GetComponent<HealthScript>();
        health.hp = GameManager.instance.playerLives;
    }

    void Update()
    {
        // 2 - Movement
        //  movement = new Vector2(
        //   speed.x * direction.x,
        //    speed.y * direction.y);
        if (powerUpSpeed && aktPowerUpTime < PowerUpTime)
        {
            aktPowerUpTime += Time.deltaTime;
        }
        else
        {
            powerUpSpeed = false;
            speed = original_speed;
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if (weapon != null)
            {
                // false because the player is not an enemy
               SoundManager.instance.PlaySingleJesus(throwSound);
                weapon.Attack(false,2);
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if (weapon != null)
            {
                SoundManager.instance.PlaySingleJesus(throwSound);
                weapon.Attack(false,1);
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if (weapon != null)
            {
                SoundManager.instance.PlaySingleJesus(throwSound);
                weapon.Attack(false,0);
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if (weapon != null)
            {
                SoundManager.instance.PlaySingleJesus(throwSound);
                weapon.Attack(false,3);
            }
        }
        
    }

    // FixedUpdate is used insead of Update to better handle the physics based jump
    void FixedUpdate()
    {
        //Check for keyboard input

        //if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

        // Apply movement to the rigidbody

        

        if (Input.GetKey("up"))
        {
            //rigidbodyComponent.velocity = movement;
            if (_currentDirection == "left")
            {
                changeState(STATE_WALK_LEFT);
            }

            if (_currentDirection == "right")
            {
                changeState(STATE_WALK_RIGHT);
            }

        }
        else if (Input.GetKey("down"))
        {
            if (_currentDirection == "left")
            {
                changeState(STATE_WALK_LEFT);
            }

            if (_currentDirection == "right")
            {
                changeState(STATE_WALK_RIGHT);
            }
            //rigidbodyComponent.velocity = movement;
        }
        else if (Input.GetKey("right"))
        {
            changeState(STATE_WALK_RIGHT);
            _currentDirection = "right";
        }
        else if (Input.GetKey("left"))
        {
            
            changeState(STATE_WALK_LEFT);
            _currentDirection = "left";
        }
        else
        {
            if (_currentDirection == "left")
            {
                changeState(STATE_IDLE_LEFT);
            }

            if (_currentDirection == "right")
            {
                changeState(STATE_IDLE_RIGHT);
            }

        }
        
        if(Input.GetKeyUp(KeyCode.D))
        {
            changeState(STATE_SHOOT_RIGHT);
            _currentDirection = "right";
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (_currentDirection == "left")
            {
                changeState(STATE_SHOOT_LEFT);
            }

            if (_currentDirection == "right")
            {
                changeState(STATE_SHOOT_RIGHT);
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            changeState(STATE_SHOOT_LEFT);
            _currentDirection = "left";
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (_currentDirection == "left")
            {
                changeState(STATE_SHOOT_LEFT);
            }

            if (_currentDirection == "right")
            {
                changeState(STATE_SHOOT_RIGHT);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        PowerUpScript powerUp = otherCollider.gameObject.GetComponent<PowerUpScript>();

        if (powerUp != null)
        {
            if (powerUp.speed != 0)
            {
                powerUpSpeed = true;
                speed = powerUp.speed;
                aktPowerUpTime = 0;
                PowerUpTime = powerUp.time;
                Destroy(powerUp.gameObject);
            }
            if (powerUp.health != 0)
            {
                HealthScript playerHealth = this.GetComponent<HealthScript>();
                playerHealth.hp = Math.Min(GameManager.instance.playerLives, playerHealth.hp + 1);
                Destroy(powerUp.gameObject);
            }

        }
        
        Key keyCollider = otherCollider.gameObject.GetComponent<Key>();

        if (keyCollider != null)
        {
            Destroy(keyCollider.gameObject);
            GameManager.instance.hasKeyImg.SetActive(true);
            GameManager.instance.hasKey = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bool damagePlayer = false;

        // Collision with enemy
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            // Kill the enemy
            /*
            HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
            if (enemyHealth != null) enemyHealth.Damage(0);
            */
            ZombieScript zombie = collision.gameObject.GetComponent<ZombieScript>();
            zombie.stopZombie();
            damagePlayer = true;
        }
        
        Door door = collision.gameObject.GetComponent<Door>();
        if (door != null)
        {
            // Hit a door!
            GameManager.instance.LoadLevelWithCoords(door.nextX, door.nextY);
        }

        // Damage the player
        if (damagePlayer)
        {
            HealthScript playerHealth = this.GetComponent<HealthScript>();
            if (playerHealth != null) playerHealth.Damage(1);
        }

        
    }

    //--------------------------------------
    // Change the players animation state
    //--------------------------------------
    void changeState(int state)
    {

        if (_currentAnimationState == state)
            return;

        switch (state)
        {

            case STATE_WALK_LEFT:
                animator.SetInteger("state", STATE_WALK_LEFT);
                break;

            case STATE_IDLE_LEFT:
                animator.SetInteger("state", STATE_IDLE_LEFT);
                break;
            case STATE_WALK_RIGHT:
                animator.SetInteger("state", STATE_WALK_RIGHT);
                break;

            case STATE_IDLE_RIGHT:
                animator.SetInteger("state", STATE_IDLE_RIGHT);
                break;
                 
            case STATE_SHOOT_RIGHT:
                animator.SetInteger("state", STATE_SHOOT_RIGHT);
                break;
            
            case STATE_SHOOT_LEFT:
                animator.SetInteger("state", STATE_SHOOT_LEFT);
                break;

        }

        _currentAnimationState = state;
    }

}
