using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class Board : MonoBehaviour
{
    public int Score = 0;
    public TMPro.TextMeshProUGUI ScoreText;

    public Player Player;
    public List<Vector3> BoardSave;
    public List<GameObject> CurrentBoard;

    public void Generate(int iterations)
    {
        foreach(int i in Enumerable.Range(0, iterations * 6))
        {
            GameObject h = Instantiate(gameObject);
            Destroy(h.GetComponent<Board>());
            h.transform.localPosition = Rotation(gameObject.transform.localPosition + new Vector3(0.8f * (i - i % 6), 0, 0), 60*(i%6), gameObject.transform.localPosition);
        }
    }


    public List<Tile> Tiles;
    public List<int> Destroyed;

    int GoldenCap = 4;
    public int Golden = 0;
    int GreenCap = 6;
    public int Green = 0;

    GameObject self;
    private void Start()
    {
        v.profile.TryGet(out vg);
        Destroyed = new List<int> { };
        self = Instantiate (gameObject);
        self.SetActive(false);
        Destroy(self.GetComponent<Board>());
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        foreach (GameObject h in CurrentBoard)
        {
            Destroy(h);
        }
        CurrentBoard.Clear();


        foreach (Vector3 v in BoardSave)
        {

            GameObject h = Instantiate(self);
            h.transform.localPosition = v;
            h.transform.SetParent(transform);
            h.SetActive(true);

            Tiles.Add(new Tile(Tile.Type.Normal, true));

            
            
            //h.GetComponent<SpriteRenderer>().color += Color.HSVToRGB(0, Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            CurrentBoard.Add(h);
        }


        foreach(int i in Enumerable.Range(0, GoldenCap))
        {
            int tmp = Random.Range(0, CurrentBoard.Count - 1);
            CurrentBoard[tmp].GetComponent<SpriteRenderer>().color = new Color(255, 245, 0, 255);
            Tiles[tmp].type = Tile.Type.Golden;
            Golden++;
        }

        foreach (int i in Enumerable.Range(0, GreenCap))
        {
            int tmp = Random.Range(0, CurrentBoard.Count - 1);
            if (Tiles[tmp].type == Tile.Type.Normal)
            {
                CurrentBoard[tmp].GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 255);
                Tiles[tmp].type = Tile.Type.Green;
                Green++;
            }
        }

        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);

        foreach (GameObject o in CurrentBoard)
        {
            if (o != null)
            {
                SpriteRenderer rend;
                if (o.TryGetComponent<SpriteRenderer>(out rend))
                {
                    if (Tiles[CurrentBoard.IndexOf(o)].type == Tile.Type.Normal)
                    {
                        rend.color = new Color(212, 212, 212, 255);
                    }
                    if (Tiles[CurrentBoard.IndexOf(o)].type == Tile.Type.Golden)
                    {
                        rend.color = new Color(255, 245, 0, 255);
                    }
                    if (Tiles[CurrentBoard.IndexOf(o)].type == Tile.Type.Green)
                    {
                        rend.color = new Color(0, 255, 0, 255);
                    }
                }
            }
        }

        StartCoroutine(WaitTenSecs(1));
    }
    int Destroyindex = -1;
    int SaveIndex = -1;
    IEnumerator WaitTenSecs(int i)
    {
        yield return new WaitForSeconds(5);
        if (Destroyindex != -1)
        {
            GameObject g = CurrentBoard[Destroyindex];
            
            Destroy(g);
            CurrentBoard[Destroyindex] = null;
            Tiles[Destroyindex] = null;
        }

        
        Destroyindex = (Random.Range(0, CurrentBoard.Count - 1));
        Debug.Log(Destroyindex);

        while (Destroyed.Contains(Destroyindex) || CurrentBoard[Destroyindex] == null)
            Destroyindex = (Random.Range(0, CurrentBoard.Count - 1));
        CurrentBoard[Destroyindex].transform.localScale = new Vector3(0.8f, 0.8f, 0);


        if (i%2 == 0)
        {
            StartCoroutine(Vignette());
            RandomMapSfx.Play();
            foreach (GameObject o in CurrentBoard)
            {
                if (o != null)
                {
                    int tmp = Random.Range(0, 10);
                    if (tmp < 8)
                    {
                        Tiles[CurrentBoard.IndexOf(o)].type = Tile.Type.Normal;
                    }
                    if (tmp == 8)
                    {
                        Tiles[CurrentBoard.IndexOf(o)].type = Tile.Type.Golden;
                    }
                    if (tmp == 9)
                    {
                        Tiles[CurrentBoard.IndexOf(o)].type = Tile.Type.Green;
                    }

                    SpriteRenderer rend;
                    if (o.TryGetComponent<SpriteRenderer>(out rend))
                    {
                        if (Tiles[CurrentBoard.IndexOf(o)].type == Tile.Type.Normal)
                        {
                            rend.color = new Color(212, 212, 212, 255);
                        }
                        if (Tiles[CurrentBoard.IndexOf(o)].type == Tile.Type.Golden)
                        {
                            rend.color = new Color(255, 245, 0, 255);
                        }
                        if (Tiles[CurrentBoard.IndexOf(o)].type == Tile.Type.Green)
                        {
                            rend.color = new Color(0, 255, 0, 255);
                        }
                    }
                }
            }
        }

        
        StartCoroutine(WaitTenSecs(i + 1));
    }

    public AudioSource RandomMapSfx;

    IEnumerator Vignette()
    {
        vg.intensity.value = 5;
        yield return new WaitForSecondsRealtime(0.2f);
        vg.intensity.value = 0.434f;
    }

    private void Update()
    {
        ScoreText.text = "Score: " + Score;
    }


    Vector2 Rotation(Vector2 point, float angle, Vector2 Offset)
    {
        return new Vector2((Mathf.Cos(angle) * (point.x - Offset.x)) - (Mathf.Sin(angle) * (point.y - Offset.y)) + Offset.x, (Mathf.Sin(angle) * (point.x - Offset.x)) + (Mathf.Cos(angle) * (point.y - Offset.y)) + Offset.y);
    }


    int normalValue = 1;
    int goldValue = 3;
    int greenValue = 5;

    public void DestroyPlatform(Vector3 Pos)
    {
        Pos = new Vector3(Mathf.Round(Mathf.Round(Pos.y / 0.8f) / 2) == Mathf.Round(Pos.y / 0.8f) / 2 ? Mathf.Round(Pos.x / (0.46f * 2)) * 0.46f * 2 : ((Mathf.Round((Pos.x / (0.46f * 2) + 0.46f))) * 0.46f * 2) - 0.46f, Mathf.Round(Pos.y / 0.8f) * 0.8f, 0);
        if (Tiles[BoardSave.IndexOf(Pos)].type == Tile.Type.Golden)
        {
            Golden--;
            Score += goldValue;
            List<GameObject> objects = new List<GameObject> { };
            foreach (int i in Enumerable.Range(0, 4))
            {

                if (CurrentBoard.Contains(null))
                {
                    SaveIndex = (Random.Range(0, Destroyed.Count - 1));

                    objects.Add(CurrentBoard[Destroyed[SaveIndex]]);

                    CurrentBoard[Destroyed[SaveIndex]] = Instantiate(self);
                    CurrentBoard[Destroyed[SaveIndex]].SetActive(true);

                    CurrentBoard[Destroyed[SaveIndex]].transform.SetParent(gameObject.transform);
                    CurrentBoard[Destroyed[SaveIndex]].transform.position = BoardSave[Destroyed[SaveIndex]];

                    int tmp = Random.Range(0, 2);

                    if (tmp == 0)
                    {
                        Tiles[Destroyed[SaveIndex]].type = Tile.Type.Normal;
                    }
                    if (tmp == 1)
                    {
                        Tiles[Destroyed[SaveIndex]].type = Tile.Type.Golden;
                    }
                    if (tmp == 2)
                    {
                        Tiles[Destroyed[SaveIndex]].type = Tile.Type.Green;
                    }

                    if (Tiles[Destroyed[SaveIndex]].type == Tile.Type.Normal)
                    {
                        CurrentBoard[Destroyed[SaveIndex]].GetComponent<SpriteRenderer>().color = new Color(212, 212, 212, 255);
                    }

                    if (Tiles[Destroyed[SaveIndex]].type == Tile.Type.Golden)
                    {
                        CurrentBoard[Destroyed[SaveIndex]].GetComponent<SpriteRenderer>().color = new Color(255, 245, 0, 255);
                    }
                    if (Tiles[Destroyed[SaveIndex]].type == Tile.Type.Green)
                    {
                        CurrentBoard[Destroyed[SaveIndex]].GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 255);
                    }

                    Destroyed.RemoveAt(SaveIndex);

                }
            }
            StartCoroutine(LightUp(objects));
            GoldSfx.Play();
        }
        else if (Tiles[BoardSave.IndexOf(Pos)].type == Tile.Type.Green)
        {
            Green--;
            Score += greenValue;


            int tmpi = Random.Range(0, BoardSave.Count - 1);
            while (Destroyed.Contains(tmpi) || tmpi == BoardSave.IndexOf(Pos) || CurrentBoard[tmpi] == null)
            {
                if(Destroyed.Contains(tmpi) || tmpi == BoardSave.IndexOf(Pos) || CurrentBoard[tmpi] == null)
                    tmpi = Random.Range(0, BoardSave.Count - 1);
            }
            Vector3 tmp = BoardSave[tmpi];

            Player.TpRandom(tmp);
            TpSfx.Play();
        }
        else if (Tiles[BoardSave.IndexOf(Pos)].type == Tile.Type.Green)
        {
            Score += goldValue;

        }
        else if(Tiles[BoardSave.IndexOf(Pos)].type == Tile.Type.Normal)
        {
            Score += normalValue;
        }
        Destroyed.Add(BoardSave.IndexOf(Pos));



        GameObject g = CurrentBoard[BoardSave.IndexOf(Pos)];

        Destroy(g);
        CurrentBoard[BoardSave.IndexOf(Pos)] = null;
    }

    IEnumerator LightUp(List<GameObject> objects)
    {
        foreach(GameObject o in objects)
        {
            o.GetComponent<SpriteRenderer>().material = Lit;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        foreach (GameObject o in objects)
        {
            o.GetComponent<SpriteRenderer>().material = Normal;
        }
    }


    public AudioSource TpSfx;
    public AudioSource GoldSfx;

    public Volume v;
    public Vignette vg;

    public Material Lit;
    public Material Normal;
}


[System.Serializable]
public class Tile
{
    public enum Type { Normal, Golden, Green}
    public Type type = Type.Normal;
    public bool Exists = true;

    public Tile (Type _type, bool _Exists)
    {
        type = _type;
        Exists = _Exists;
    }
}
