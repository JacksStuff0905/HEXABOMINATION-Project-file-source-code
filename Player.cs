using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Board board;
    public GameObject LoseScreen;

    public void TpRandom(Vector3 pos)
    {
        Tp(pos);
    }

    // Start is called before the first frame update
    void Start()
    {

        Music = GameObject.Find("Music").GetComponent<AudioSource>();
        transform.position = Vector3.zero;
    }
    Vector2 pos = new Vector2(0, 0);
    bool CanControl = true;
    // Update is called once per frame
    void Update()
    {
        if (CanControl)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Vector2 value = new Vector2(-0.5f, 0.5f);
                Move(value);
                //pos += new Vector2(Mathf.Round(Mathf.Round(value.y / 0.8f) / 2) == Mathf.Round(value.y / 0.8f) / 2 ? Mathf.Round(value.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((value.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(value.y / 0.8f) * 0.8f);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector2 value = new Vector2(0.5f, 0.5f);
                Move(value);
                //pos += new Vector2(Mathf.Round(Mathf.Round(value.y / 0.8f) / 2) == Mathf.Round(value.y / 0.8f) / 2 ? Mathf.Round(value.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((value.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(value.y / 0.8f) * 0.8f);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Vector2 value = new Vector2(0.5f, 0f);
                Move(value);
                //pos += new Vector2(Mathf.Round(Mathf.Round(value.y / 0.8f) / 2) == Mathf.Round(value.y / 0.8f) / 2 ? Mathf.Round(value.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((value.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(value.y / 0.8f) * 0.8f);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                Vector2 value = new Vector2(0.5f, -0.5f);
                Move(value);
                //pos += new Vector2(Mathf.Round(Mathf.Round(value.y / 0.8f) / 2) == Mathf.Round(value.y / 0.8f) / 2 ? Mathf.Round(value.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((value.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(value.y / 0.8f) * 0.8f);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Vector2 value = new Vector2(-0.5f, -0.5f);
                Move(value);
                //pos += new Vector2(Mathf.Round(Mathf.Round(value.y / 0.8f) / 2) == Mathf.Round(value.y / 0.8f) / 2 ? Mathf.Round(value.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((value.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(value.y / 0.8f) * 0.8f);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Vector2 value = new Vector2(-0.5f, 0f);
                Move(value);
                //pos += new Vector2(Mathf.Round(Mathf.Round(value.y / 0.8f) / 2) == Mathf.Round(value.y / 0.8f) / 2 ? Mathf.Round(value.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((value.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(value.y / 0.8f) * 0.8f);
            }
        }
        //transform.position = pos;
        //transform.localPosition = new Vector3(Mathf.Round(Mathf.Round(transform.localPosition.y / 0.8f) / 2) == Mathf.Round(transform.localPosition.y / 0.8f) / 2 ? Mathf.Round(transform.localPosition.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((transform.localPosition.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(transform.localPosition.y / 0.8f) * 0.8f, 0);
        bool var = board.BoardSave.Contains(transform.position);
        if (!var)
        {
            if(!lost)
                Lose();
        }
        else if (board.CurrentBoard[board.BoardSave.IndexOf(transform.localPosition)] == null)
        {
            if (!lost)
                Lose();
        }

        
    }
    bool lost = false;
    public AudioSource moveSfx;
    void Move(Vector2 move)
    {
        Vector3 prev = transform.localPosition; 
        transform.localPosition += new Vector3(move.x, move.y, 0);
        transform.localPosition = new Vector3(Mathf.Round(Mathf.Round(transform.localPosition.y / 0.8f) / 2) == Mathf.Round(transform.localPosition.y / 0.8f) / 2 ? Mathf.Round(transform.localPosition.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((transform.localPosition.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(transform.localPosition.y / 0.8f) * 0.8f, 0);
        board.DestroyPlatform(prev);
        moveSfx.Play();

    }

    void Tp(Vector2 move)
    {
        Vector3 prev = transform.localPosition;
        transform.localPosition = new Vector3(move.x, move.y, 0);
        transform.localPosition = new Vector3(Mathf.Round(Mathf.Round(transform.localPosition.y / 0.8f) / 2) == Mathf.Round(transform.localPosition.y / 0.8f) / 2 ? Mathf.Round(transform.localPosition.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((transform.localPosition.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(transform.localPosition.y / 0.8f) * 0.8f, 0);

        CanControl = false;
        StartCoroutine(WaitForInput());
    }

    IEnumerator WaitForInput()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        CanControl = true;
    }
    public AudioSource LoseSfx;


    void Lose()
    {
        lost = true;
        LoseSfx.Play();
        Time.timeScale = 0;
        LoseScreen.SetActive(true);
        CanControl = false;
        Music.Stop();
        Destroy(Music.gameObject);
        LoseInfo.text = "FINAL SCORE: " + board.Score;
        ScoreText.SetActive(false);

        
        StartCoroutine(WaitToMenu());
    }

    IEnumerator WaitToMenu()
    {
        yield return new WaitForSecondsRealtime(2);
        board.gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(4);

        SceneManager.LoadScene("Menu");
    }

    public AudioSource Music;

    public TMPro.TextMeshProUGUI LoseInfo;
    public GameObject ScoreText;
}


