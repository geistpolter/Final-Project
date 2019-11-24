using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    private int scoreCount = 0;
    private int lives = 3;

    private bool facingRight = true;

    public float speed;

    public Text score;
    public Text winText;
    public Text livesText;
    public Text credits;

    public AudioSource musicSource;
    public AudioSource soundEffect1;
    public AudioSource soundEffect2;
    public AudioSource soundEffect3;
    
    public AudioClip winMusic;
    public AudioClip walkingSound;
    public AudioClip coinChime;
    public AudioClip enemyDing;


    Animator anim;

    void Start()
    {
        anim =GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreCount.ToString();
        livesText.text = "Lives: " + lives.ToString();
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    //Animations
    //0 = idle
    //1 = run
    //2 = jump
    //3 = death
    //4 = hurt
    void Update()
    {

        //Jump Animation
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
            soundEffect1.clip = walkingSound;
            soundEffect1.Play();
        }
        //Animations
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
            soundEffect1.clip = walkingSound;
            soundEffect1.Play();
            soundEffect1.loop = true;
        }
        if (Input.GetKeyUp(KeyCode.D)){
            anim.SetInteger("State", 0);
            soundEffect1.loop = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
            soundEffect1.clip = walkingSound;
            soundEffect1.Play();
            soundEffect1.loop = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
            soundEffect1.loop = false;
        }
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Collectible Coin
        if (collision.collider.tag == "Coin")
        {
            scoreCount += 1;
            score.text = "Score: " + scoreCount.ToString();
            Destroy(collision.collider.gameObject);
            soundEffect2.clip = coinChime;
            soundEffect2.Play();
            //Stage 2 Transform
            //For future reference, if this keeps executing and snapping object to this location, try nesting it in a different command
            if (scoreCount == 4)
            {
                transform.position = new Vector2(215f, 16f);
            }
        }

        //Resets Lives for Stage 2
        if (scoreCount == 4)
        {
            lives = 3;
            livesText.text = "Lives: " + lives.ToString();
        }

        //Enemy Coins
        if (collision.collider.tag == "Enemy")
        {
            lives -= 1;
            livesText.text = "Lives: " + lives.ToString();
            anim.SetInteger("State", 4);
            Destroy(collision.collider.gameObject);
            soundEffect3.clip = enemyDing;
            soundEffect1.Play();
        }

        //Spikes knock back
        if (collision.collider.tag == "Spike")
        {
            lives -= 1;
            livesText.text = "Lives: " + lives.ToString();
            if (facingRight == true)
            {
                anim.SetInteger("State", 4);
                rd2d.AddForce(new Vector2(-10, 20), ForceMode2D.Impulse);
            }
            else
            {
                anim.SetInteger("State", 4);
                rd2d.AddForce(new Vector2(10, 20), ForceMode2D.Impulse);
            }
        }

        //Win or Lose with Music
        //Destroys Enemies when Winning
        if (scoreCount == 8)
        {
            winText.text = "YOU WIN! Game created by Cheyenne Fiedler!";
            credits.text = "Win SFX is Realization by Jason Dagenet. BG Music is Snowfall by Joseph Gilbert/Kistol. Enemy/Coin SFX by Crazyduckgames. Footsteps by IgnasD.";
            musicSource.clip = winMusic;
            musicSource.Play();
            Destroy(GameObject.FindWithTag("Enemy"));

        }
        if (lives < 1)
        {
            winText.text = "You lose :(";
            anim.SetInteger("State", 3);
            Destroy(GameObject.FindWithTag("Coin"));
            Destroy(this);
            musicSource.Stop();
            soundEffect1.Stop();
            soundEffect2.Stop();
            soundEffect3.Stop();

}

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);
            }
        }
    }

}