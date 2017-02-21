using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int maxSize;
    public int currentSize;
    public int xBound;
    public int yBound;
    public int score;
    public GameObject foodPrefab;
    public GameObject currentfood;
    public GameObject snakePrefab;
    public Snake Head;
    public Snake Tail;
    public int NESW;
    public Vector2 nextPos;
    public Text scoreText;

    // Use this for initialization
    void OnEnable()
    {
        Snake.hit += hit;
    }

    void Start()
    {
        InvokeRepeating("TimerInvoke", 0, .5f);
        FoodFunction();
    }
    void OnDisable()
    {
        Snake.hit -= hit;
    }

	
	// Update is called once per frame
	void Update () {
        ComChangeD();
	}

    void TimerInvoke()
    {
        Movement();
        StartCoroutine(checkVisable());
        if(currentSize >= maxSize)
        {
            TailFunction();
        }
        else
        {
            currentSize++;
        }
    }


    
    void Movement()
            
    {
        GameObject temp;
        nextPos = Head.transform.position;
        switch(NESW)
        {
            case 0:
                nextPos = new Vector2(nextPos.x, nextPos.y + 1);
                break;
            case 1:
                nextPos = new Vector2(nextPos.x + 1, nextPos.y);
                break;
            case 2:
                nextPos = new Vector2(nextPos.x, nextPos.y - 1);
                break;
            case 3:
                nextPos = new Vector2(nextPos.x - 1, nextPos.y);
                break;
            
        }
        temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);
        Head.Setnext(temp.GetComponent<Snake>());
        Head = temp.GetComponent<Snake>();

        return;

    }

    void ComChangeD()
    {
        if(NESW != 2 && Input.GetKeyDown(KeyCode.W))
        {
            NESW = 0;
        }
        if (NESW != 3 && Input.GetKeyDown(KeyCode.D))
        {
            NESW = 1;
        }
        if (NESW != 0 && Input.GetKeyDown(KeyCode.S))
        {
            NESW = 2;
        }
        if (NESW != 1 && Input.GetKeyDown(KeyCode.A))
        {
            NESW = 3;
        }
    }

    void TailFunction()
    {
        Snake tempSnake = Tail;
        Tail = Tail.GetNext();
        tempSnake.RemoveTail();
    }

    void FoodFunction()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPox = Random.Range(-yBound, yBound);

        currentfood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPox), transform.rotation);
        StartCoroutine(CheckRender(currentfood));
    }

    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if(IN.GetComponent<Renderer>().isVisible == false)
        {
            if(IN.tag == "Food")
            {
                Destroy(IN);
                FoodFunction();
            }
        }
    }

    void hit(string WhatWasSent)
    {
        if(WhatWasSent == "Food")
        {
            FoodFunction();
            maxSize++;
            score++;
            scoreText.text = score.ToString();
            int temp = PlayerPrefs.GetInt("HighScore");
            if(score > temp)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        if(WhatWasSent == "Snake")
        {
            CancelInvoke("TimerInvoke");
            Exit();
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    void wrap()
    {
        if (NESW == 0)
        {
            Head.transform.position = new Vector2(Head.transform.position.x, -(Head.transform.position.y - 1));
        }
        else if (NESW == 1)
        {
            Head.transform.position = new Vector2(-(Head.transform.position.x - 1), Head.transform.position.y);
        }
        else if (NESW == 2)
        {
            Head.transform.position = new Vector2(Head.transform.position.x, -(Head.transform.position.y + 1));
        }
        else if (NESW == 3)
        {
            Head.transform.position = new Vector2(-(Head.transform.position.x + 1), Head.transform.position.y);
        }
    }

    IEnumerator checkVisable()
    {
        yield return new WaitForEndOfFrame();
        if (Head.GetComponent<Renderer>().isVisible)
        {
            wrap();
        }
    }
}
