using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeSystem : MonoBehaviour
{

    public int level;
    float pick_time;

    public bool isDrag; 
    public bool isMerge; 

    public Text levelTxt; 

    SpriteRenderer spriteRen;
    CircleCollider2D circle;

    public Sprite[] levelSprite = new Sprite[4];
    public GameObject[] _LevelAnimObj = new GameObject[4];

    public ParticleSystem spawnEffect;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRen = GetComponent<SpriteRenderer>();
        Circle = GetComponent<CircleCollider2D>();

        var main = spawnEffect.main;
        main.startColor = new Color(1, 0.7f, 0.6f);

        level = 1;
        isDrag = false;

        SpriteRen.sprite = levelSprite[level - 1];
        spawnEffect.Play(true);

    }

    // Update is called once per frame
    void Update()
    {
        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        if (pos_x < -2.4f || pos_x > 2.4f || pos_y > 0.8f || pos_y < -4.4)
            transform.position = new Vector3(0, -1, 0);
    }

    //객체 마우스 위치 이동
    void OnMouseDrag()
    {
        isDrag = true;

        pick_time += Time.deltaTime;

        if (pick_time < 0.1f) return;

        Vector3 mouse_pos = Input.mousePosition;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.z + 10f));
        transform.position = point;

    }

    void OnMouseUp()
    {
        isDrag = false;
        pick_time = 0;
    }

    //Trigger 사용하여 융합 판단
    void OnTriggerStay2D(Collider2D collision)
    {
        if ( collision.gameObject.tag == "Merge")
        {
            MergeSystem other = collision.gameObject.GetComponent<MergeSystem>();  
            if( level == other.level && !isMerge && !other.isMerge && level < 10 && !other.isDrag && !isDrag)
            {
                other.Hide(transform.position);
                LevelUp();
            }
        }    
    }
    
    //융합 시 기존 오브젝트 이동, 삭제
    void Hide(Vector3 targetPos)
    {
        isMerge = true;

        Circle.enabled = false;

        StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int count = 0;
        while(count < 20)
        {
            count++;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            yield return null;
        }

        isMerge = false;
        Destroy(gameObject);
    }

    void LevelUP()
    {
        isMerge = true;
        StartCoroutine(LevelUPRoutine());
    }

    //레벨 상승 파티클 설정
    IEnumerator LevelUPRoutine()
    {
        var main = spawnEffect.main;

        level = (level < 4) ? level + 1 : level;
        switch (level)
        {
            case 2:
                main.startColor = new Color(0.7f, 1, 0);
                break;
            case 3:
                main.startColor = new Color(0.4f, 0.65f, 1);
                break;
            case 4:
                main.startColor = new Color(1, 0.7f, 0.6f);
                break;
        }
        spawnEffect.Play(true);
        yield return new WaitForSeconds(0.1f);
        
        LevelTxt.text = level + "";
        SpriteRen.sprite = levelSprite[level - 1];
        GameManagerV02.INNowSpawn -= 1;
        isMerge = false;
    }

    //병합 시 레벨 증가
    void LevelUp()
    {
        isMerge = true;
        StartCoroutine(LevelUPRoutine());
    }
    //GameManager 수치 조정
    IEnumerator LevelUPRoutine()
    {
        var main = spawnEffect.main;

        level = level + 1;
        if(level > 4)
        {
            GameManagerV02 gameManager = GameObject.Find("GameManager").GetComponent<GameManagerV02>();
            gameManager.MergeLevel = ( gameManager.MergeLevel < 2) ? gameManager.MergeLevel + 1 : gameManager.MergeLevel;
            gameManager.GetHero();
            gameManager.Money += 50000;
            GameManagerV02.INNowSpawn -= 2;
            Destroy(gameObject, 0.1f);
        }

        switch (level)
        {
            case 2:
                main.startColor = new Color(0.7f, 1, 0);
                break;
            case 3:
                main.startColor = new Color(0.4f, 0.65f, 1);
                break;
            case 4:
                main.startColor = new Color(1, 0.7f, 0.6f);
                break;
        }
        spawnEffect.Play(true);
        yield return new WaitForSeconds(0.1f);

        LevelTxt.text = level + "";

        for(int i = 0; i < _LevelAnimObj.Length; i++)
        {
            if( level-1 == i )
            {
                _LevelAnimObj[i].SetActive(true);
            }
            else
            {
                _LevelAnimObj[i].SetActive(false);
            }
        }

        GameManagerV02.INNowSpawn -= 1;
        isMerge = false;
    }




}
