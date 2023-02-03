using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerV02 : MonoBehaviour
{
    public int Money;

    [ Header ("PlayStsts")]
    public int MaxSpawn;
    public int NowSpawn;
    float SpawnCool;
    public int INMaxSpawn;

    public static int INNowSpawn;
    
    public float NowSpawnCool;

    public TMP_Text SpawnMergeTxt;
    public TMP_Text INSpawnMergeTxt;

    public GameObject LeftTomSpw;
    public GameObject RightDownSpw;

    public GameObject FirstMerge;

    [Header("Upgrade Part")]
    public int UpgradeLv01;
    public int UpgradeLv02;
    public int UpgradeLv03;
    public TMP_Text UpTxt01, UpTxt02, UpTxt03;
    public TMP_Text UpLvTxt01, UpLvTxt02, UpLvTxt03;
    public TMP_Text UpMonTxt01, UpMonTxt02, UpMonTxt03;
    public TMP_Text MoneyTxt;

    public GameObject BackGround;
    public int Stage;
    public TMP_Text StageTxt;

    public GameObject EnemyObj;
    public Transform LeftTopEnemySpw;
    public Transform RightBottompEnemySpw;

    public static GameManagerV02 instance;
    int nowHero;

    public GameObject[] PoringHero;
    public int MergeLevel;



    void Awake()
    {
        int width = 980;    
        int height = 1920;

        Screen.SetResolution(width, height, true);
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnCool = 0;
        MaxSpawn = 5;
        INMaxSpawn = 10;
        NowSpawnCool = 10;
        Money = 10000;

        UpgradeLv01 = 1;
        UpgradeLv02 = 1;
        UpgradeLv03 = 1;

        Stage = 1;
        MergeLevel = 0;

        SetUnitList();
        TextUpdate();
        GetHero();
        HeroPowerSet();
    }


    public void GetHero()
    {
        for(int i = 0; i < 3; i++)
        {
            if( i <= MergeLevel)
            {
                PoringHero[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( NowSpawn < MaxSpawn)
        {
            SpawnCool += Time.deltaTime;
            if( SpawnCool >= NowSpawnCool)
            {
                NowSpawn += 1;
                SpawnCool = 0;
            }
        }

        SpawnMergeTxt.text = NowSpawn + "/" + MaxSpawn;
        INSpawnMergeTxt.text = INNowSpawn + "/" + INMaxSpawn;
        MoneyTxt.text = "Money : " + Money;

        StageFail();
        StageUP();

    }

    public void MergeSpawn()
    {
        if (NowSpawn > 0 && INNowSpawn < INMaxSpawn)
        {
            Instantiate(FirstMerge, new Vector3(Random.Range(LeftTomSpw.transform.position.x, RightDownSpw.transform.position.x), Random.Range(LeftTomSpw.transform.position.y, RightDownSpw.transform.position.y), 0), transform.rotation);
            NowSpawn -= 1;
            INNowSpawn += 1;
        }

    }


    public void TextUpdate()
    {
        UpTxt01.text = MaxSpawn + "     >     " + (MaxSpawn + 1);
        UpTxt02.text = NowSpawnCool + "초  >  " + (NowSpawnCool - 0.5f) + "초";
        UpTxt03.text = INMaxSpawn + "     >     " + (INMaxSpawn + 1);
        
        UpLvTxt01.text = "Lv. " + UpgradeLv01;
        UpLvTxt02.text = "Lv. " + UpgradeLv02;
        UpLvTxt03.text = "Lv. " + UpgradeLv03;

        UpMonTxt01.text = "레벨업:" + (1000 * UpgradeLv01);
        UpMonTxt02.text = "레벨업:" + (1500 * UpgradeLv02);
        UpMonTxt03.text = "레벨업:" + (1000 * UpgradeLv03);

        StageTxt.text = Stage + "";
    }

    public void Upgrade01()
    {
        if( Money-(1000 * UpgradeLv01) >= 0)
        { 
            Money -= (1000 * UpgradeLv01);
            MaxSpawn += 1;
            UpgradeLv01 += 1;
        }

        TextUpdate();
    }
    public void Upgrade02()
    {
        if(Money - (1500 * UpgradeLv02) >= 0)
        {
            Money -= (1500 * UpgradeLv02);
            NowSpawnCool -= 0.5f;
            UpgradeLv02 += 1;
        }
        TextUpdate();
    }    
    public void Upgrade03()
    {
        if(Money - (1000 * UpgradeLv02) >= 0)
        {
            Money -= (1000 * UpgradeLv02);
            INMaxSpawn += 1;
            UpgradeLv03 += 1;
        }

        TextUpdate();
    }


    [Header("Menu")]
    public GameObject Menu01;
    public GameObject Menu02;
    public GameObject Menu03;
    public GameObject UpgradeSite;
    public GameObject PoringSite;

    public void OnMergeMenu()
    {
        Menu01.SetActive(false);
        Menu02.SetActive(true);
        Menu03.SetActive(false);

        UpgradeSite.SetActive(false);
        PoringSite.SetActive(false);

    }
    public void OnUpgradeMenu()
    {
        Menu01.SetActive(false);
        Menu02.SetActive(false);
        Menu03.SetActive(true);

        UpgradeSite.SetActive(true);
        PoringSite.SetActive(false);

    }
    public void OnPoringMenu()
    {
        Menu01.SetActive(true);
        Menu02.SetActive(false);
        Menu03.SetActive(false);

        UpgradeSite.SetActive(false);
        PoringSite.SetActive(true);


    }



    public GameObject FinalEnmy;
    public List<Transform> _unitPool = new List<Transform>();
    public List<Character> _HeroUnitList = new List<Character>();

    public float[] HeroPower = new float[3];
    public float AllPower;

    public TMP_Text[] HeroPowerTxt;

    public int[] HLevelValue;
    public TMP_Text[] HeroLevel;
    public TMP_Text[] HeroUpCost;

    public void SetUnitList()
    {
        _HeroUnitList.Clear();

        for (int i = 0; i < _unitPool.Count; i++)
        {
            for (int j = 0; j < _unitPool[i].childCount; j++)
            {
                _HeroUnitList.Add(_unitPool[i].GetChild(j).GetComponent<Character>());
                _unitPool[i].GetChild(j).tag = "Player";
            }
        }
    }

    public void HeroPowerSet()
    {
        AllPower = 0;
        for (int i = 0; i < _HeroUnitList.Count; i++)
        {
            HeroPower[i] = _HeroUnitList[i]._UnitMAXHP + _HeroUnitList[i]._UnitAT;
            if( _HeroUnitList[i].gameObject.activeSelf)
            {
                AllPower += HeroPower[i];
                HeroPowerTxt[i].text = "" + HeroPower[i];
            }
        }
        HeroPowerTxt[3].text = "" + AllPower;
        HeroPowerTxt[4].text = "" + AllPower;
    }

    public void HeroLevelUP(int num)
    {
        if ((Money - HLevelValue[num] * 1000) >= 0)
        {
            Money -= HLevelValue[num] * 1000;

            HLevelValue[num] += 1;
            HeroLevel[num].text = "" + HLevelValue[num];

            HeroUpCost[num].text = "레벨업\n" + HLevelValue[num] * 1000;

            _HeroUnitList[num].GetComponent<Character>()._UnitMAXHP += 200;
            _HeroUnitList[num].GetComponent<Character>()._UnitAT += 50;
        }

        HeroPowerSet();

    }

    void HeroReset()
    {

        for(int i = 0; i < _HeroUnitList.Count; i++)
        {
            if( _HeroUnitList[i].isUsing )
            {
                
                _HeroUnitList[0].transform.position = new Vector3(-1.4f, 2.7f, 0);
                _HeroUnitList[1].transform.position = new Vector3(-2f, 2.4f, 0);
                _HeroUnitList[2].transform.position = new Vector3(-1.3f, 2.3f, 0);


                _HeroUnitList[i].gameObject.SetActive(true);
                _HeroUnitList[i]._UnitHP = _HeroUnitList[i]._UnitMAXHP;

                _HeroUnitList[i].isFight = false;
            }
        }
        HeroPowerSet();
    }



    void StageFail()
    {
        bool isFail = false;

        if ( !isFail )
        {
            int Hcount = 0;
            for (int i = 0; i < _HeroUnitList.Count; i++)
            {
                if (!_HeroUnitList[i].gameObject.activeSelf)
                {
                    Hcount += 1;
                }
            }

            if( Hcount >= 3)
            {
                isFail = true;
            }
        }
            


        if ( isFail )
        {
            if( Stage > 1)
            {
                Stage -= 1;
                Money -= ((Stage + 4) / 5) * 1000;
            }
            isFail = false;
            
            GameObject[] Emy = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < Emy.Length; i++)
            {
                Destroy(Emy[i]);
            }

            FinalEnmy.SetActive(false);

        }

    }

    public void ResetBtn(Button my)
    {
        Money += Stage * 10000;
        Stage = 0;
        GameObject[] enmy = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enmy.Length; i++)
        {
            Destroy(enmy[i]);
        }
        my.interactable = false;

        FinalEnmy.SetActive(false);
        StartCoroutine(ResetCool(my));
    }

    IEnumerator ResetCool(Button my)
    {
        yield return new WaitForSeconds(10f);
        my.interactable = true;
    }



    public void StageUP()
    {

        if ( FinalEnmy.activeSelf == false )
        {
            Character _FinalEnemy = FinalEnmy.GetComponent<Character>();

            Stage += 1;
            _FinalEnemy._UnitHP = ((Stage + 4) / 5) * 1000;
            FinalEnmy.SetActive(true);

            _HeroUnitList[0].gameObject.SetActive(false);
            _HeroUnitList[1].gameObject.SetActive(false); 
            _HeroUnitList[2].gameObject.SetActive(false);


            for (int j = 0; j < ((Stage + 4) / 5) + 2; j++)
            {
                
                GameObject go = Instantiate(EnemyObj, new Vector3(Random.Range(LeftTopEnemySpw.position.x,
                    RightBottompEnemySpw.position.x), Random.Range(LeftTopEnemySpw.position.y,
                    RightBottompEnemySpw.position.y), 0), Quaternion.identity);

                go.GetComponent<Character>()._UnitHP = ((Stage + 4) / 5) * 1000;
                go.GetComponent<Character>()._UnitAT = ((Stage + 4) / 5) * 100;

                int index = go.name.IndexOf("(Clone)");
                if (index > 0)
                    go.name = go.name.Substring(0, index);
            }

            BackGround.GetComponent<BackGroundMove>().isFight = false;
            Money += ((Stage + 4) / 5) * 1000;

            TextUpdate();
            HeroReset();
        }
    }



    public void BtnTest(GameObject obj)
    {
        Debug.Log(obj.name);
    }

}
