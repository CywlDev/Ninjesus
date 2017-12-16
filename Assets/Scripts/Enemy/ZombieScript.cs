using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour {

    Animator animator;

    //animation states - the values in the animator conditions
    const int STATE_IDLE_LEFT = 0;
    const int STATE_IDLE_RIGHT = 2;
    const int STATE_WALK_LEFT = 1;
    const int STATE_WALK_RIGHT = 3;

    int _currentAnimationState = STATE_IDLE_LEFT;

    string _currentDirection = "left";



    int last_direction = 3;

    bool stop = true;
    public int dmg = 1;

    public float speed = 1.5f;
    
    private double akt_time;
    private double reactTime;
    private int charge_direction;
    public float size = 0.5f;

    private Rigidbody2D rigidbodyComponent;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        reactTime = Random.Range(1,2);
        akt_time = 0;
        charge_direction = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        stopZombie();
    }

    public void stopZombie()
    {
        stop = true;
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

        }

        _currentAnimationState = state;
    }

    void Update()
    {
        if(stop == true)
        {
            akt_time = 0;
            stop = false;
            do
            {
                charge_direction = (int)Random.Range(0,4);
            } while (charge_direction == last_direction);
            last_direction = charge_direction;
        }




        if(akt_time<reactTime)
        {
            akt_time += Time.deltaTime;
        }
        else
        {
            //charge_direction = 0;
            if (charge_direction==0)
            {
                //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left);
                var raycast = Physics2D.RaycastAll(transform.position, Vector2.left,size);
                if (raycast.Length <= 1)
                {

                    _currentDirection = "left";
                    changeState(STATE_WALK_LEFT);
                    
                    transform.position += Vector3.left * speed * Time.deltaTime;
                    
                }
                else
                {
                    if (raycast[1].collider.name == "Player")
                    {
                        HealthScript hp = raycast[1].collider.gameObject.GetComponent<HealthScript>();
                        hp.Damage(dmg);
                    }
                    
                    if(raycast[1].collider.name != "Shuriken(Clone)")
                    {
                        changeState(STATE_IDLE_LEFT);
                        stop = true;
                    }
                    
                }
            }
            if (charge_direction == 1)
            {
                var raycast = Physics2D.RaycastAll(transform.position, Vector2.up, size);
                if (raycast.Length <= 1)
                {
                    if (_currentDirection == "left")
                    {
                        changeState(STATE_WALK_LEFT);
                    }

                    if (_currentDirection == "right")
                    {
                        changeState(STATE_WALK_RIGHT);
                    }
                    transform.position += Vector3.up * speed * Time.deltaTime;
                }
                else
                {
                    if (raycast[1].collider.name == "Player")
                    {
                        HealthScript hp = raycast[1].collider.gameObject.GetComponent<HealthScript>();
                        hp.Damage(dmg);
                    }
                    if (raycast[1].collider.name != "Shuriken(Clone)")
                    {
                        if (_currentDirection == "left")
                        {
                            changeState(STATE_IDLE_LEFT);
                        }

                        if (_currentDirection == "right")
                        {
                            changeState(STATE_IDLE_RIGHT);
                        }
                        stop = true;
                    }
                    
                }
            }
            if (charge_direction == 2)
            {
                var raycast = Physics2D.RaycastAll(transform.position, Vector2.right, size);
                if (raycast.Length <= 1)
                {
                    changeState(STATE_WALK_RIGHT);
                    _currentDirection = "right";
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }
                else
                {
                    if (raycast[1].collider.name == "Player")
                    {
                        HealthScript hp = raycast[1].collider.gameObject.GetComponent<HealthScript>();
                        hp.Damage(dmg);
                    }
                    if (raycast[1].collider.name != "Shuriken(Clone)")
                    {
                        changeState(STATE_IDLE_RIGHT);
                        stop = true;
                    }
                }
            }
            if (charge_direction == 3)
            {
                var raycast = Physics2D.RaycastAll(transform.position, Vector2.down, size);
                if (raycast.Length <= 1)
                {
                    if (_currentDirection == "left")
                    {
                        changeState(STATE_WALK_LEFT);
                    }

                    if (_currentDirection == "right")
                    {
                        changeState(STATE_WALK_RIGHT);
                    }
                    transform.position += Vector3.down * speed * Time.deltaTime;
                }
                else
                {
                    if (raycast[1].collider.name == "Player")
                    {
                        HealthScript hp = raycast[1].collider.gameObject.GetComponent<HealthScript>();
                        hp.Damage(dmg);
                    }
                    if (raycast[1].collider.name != "Shuriken(Clone)")
                    {
                        if (_currentDirection == "left")
                        {
                            changeState(STATE_IDLE_LEFT);
                        }

                        if (_currentDirection == "right")
                        {
                            changeState(STATE_IDLE_RIGHT);
                        }
                        stop = true;
                    }
                }
            }
        }        

    }

}
