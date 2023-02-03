using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{

    public float _UnitHP; //체력
    public float _UnitMAXHP; //체력
    public float _UnitMS; //속도
    public float _UnitAT; //공격력
    public float _UnitAS; //공격속도

    public Transform AtkAT;
    public Vector2 _UnitAR; //공격범위

    public bool isFight;
    public bool isUsing;

    public float nowTime;
    public GameObject Fireball;

    bool isAttack;

    Animator anim;

    public enum UnitJob
    {
        Knight,
        Wijard,
        Acher,
        Healer,
        EndDoor
    }
    public enum UnitState
    {
        Player,
        Enemy
    }

    public UnitState _unitState = UnitState.Player;
    public UnitJob _unitJob = UnitJob.Knight;

    // Start is called before the first frame update
    void Start()
    {
        _UnitMAXHP = _UnitHP;
        isFight = false;

        anim = GetComponent<Animator>();

        if(gameObject.tag == "Enemy")
        {
            _UnitAS = Random.Range(1, 1.5f);
        }
        isAttack = false;
    }

    private void OnEnable()
    {
        isAttack = false;
        isFight = false;
    }

    void attack()
    {
        if(anim != null)
        {
            anim.SetTrigger("isFight");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetTarget();
        MoveUnit();
        if ( _UnitHP <= 0)
        {
            switch (gameObject.tag)
            {
                case "Player":
                    gameObject.SetActive(false);
                    break;
                case "EndGate":
                    gameObject.SetActive(false);
                    break;
                case "Enemy":
                    Destroy(gameObject);
                    break;
            }
        }

        GameObject En = GameObject.FindGameObjectWithTag("Enemy");
        if (En == null && !isAttack)
        {
            isFight = false;
            isAttack = true;
        }

    }



    public void GetTarget()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(AtkAT.position, _UnitAR, 0);
        if(nowTime == 0 && isFight) attack();
        foreach (Collider2D collider in collider2Ds)
        {
            if( collider.tag != gameObject.tag && collider.tag != "ball")
            {
                isFight = true;
                nowTime += Time.deltaTime;
                if ( nowTime >= _UnitAS)
                {
                    if(_unitJob == UnitJob.Knight)
                    {
                        collider.GetComponent<Character>().TakeDamege(_UnitAT);
                        //attack();
                    }
                    if(_unitJob == UnitJob.Wijard)
                    {
                        if(Fireball != null)
                        {
                            GameObject FB = Instantiate(Fireball, transform.position, Quaternion.identity);
                            FB.transform.LookAt(collider.transform);
                        }
                    }
                    nowTime = 0;
                }
            }
            else if ( collider.tag == gameObject.tag && !isFight)
            {
                isFight = false;
            }
        }
        if ( isFight && collider2Ds.Length < 1 )
        {
            isFight = false;
        }
    }





    public void TakeDamege(float Damage)
    {
        _UnitHP -= Damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AtkAT.position, _UnitAR);
    }

    public void MoveUnit()
    {
        Vector2 pos = transform.position;

        if ( !isFight )
        {
            switch (gameObject.tag)
            {
                case "Player":
                    pos.x += _UnitMS * Time.deltaTime * 0.2f * _UnitMS;
                    break;
                case "Enemy":
                    pos.x -= _UnitMS * Time.deltaTime * 0.2f * _UnitMS;
                    break;
            }

        }/*
        switch (gameObject.tag)
        {
            case "Player":
                pos.x += (!isFight) ? _UnitMS * Time.deltaTime * 0.2f * _UnitMS : _UnitMS * Time.deltaTime * 0.05f * _UnitMS;
                break;
            case "Enemy":
                pos.x -= (!isFight) ? _UnitMS * Time.deltaTime * 0.2f * _UnitMS : _UnitMS * Time.deltaTime * 0.05f * _UnitMS;
                break;
        }
        */
        transform.position = pos;

    }












}
