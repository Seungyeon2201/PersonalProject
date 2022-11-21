using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    private int index = 0;
    public List<TextMeshProUGUI> storeMonsterName = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> monsterPrice = new List<TextMeshProUGUI>();
    public List<Sprite> storePriceBoard = new List<Sprite>();
    public List<Image> buttonPriceBoard = new List<Image>();
    public List<Image> monsterImage = new List<Image>();
    public List<ButtonFunc> buyButtons = new List<ButtonFunc>();
    private Dictionary<COST_TYPE, Sprite> costToSpriteDic = new Dictionary<COST_TYPE, Sprite>();
    public TextMeshProUGUI curGold;
    public TextMeshProUGUI curPopulation;
    public TextMeshProUGUI totalPopulation;
    public TextMeshProUGUI curExp;
    public TextMeshProUGUI needExp;
    public Slider expSlider;

    private void Awake()
    {
        costToSpriteDic.Add(COST_TYPE.One, storePriceBoard[0]);
        costToSpriteDic.Add(COST_TYPE.Two, storePriceBoard[1]);
        costToSpriteDic.Add(COST_TYPE.Three, storePriceBoard[2]);
        GameManager.Instance.goldAction += MoneySetting;
        GameManager.Instance.expAction += ExperienceSetting;
        GameManager.Instance.storeLevelUpAction += TotalPopulationSetting;
        GameManager.Instance.pupulationAction += PopulationSetting;
    }

    private void Start()
    {
        ExperienceSetting();
        TotalPopulationSetting();
        PopulationSetting();
    }
    //상점 정보 보여주기
    public void StoreShowInfo(Monsters monster)
    {
        buttonPriceBoard[index].sprite = costToSpriteDic[monster.cost];
        storeMonsterName[index].text = monster.MonsterName;
        buyButtons[index].monsterType = monster.monsterType;
        monsterImage[index].sprite = monster.monsterSprite;
        monsterPrice[index].text = ((int)monster.cost).ToString();
        index++;
        if (index > 4)
        {
            index = 0;
        }
    }

    public void ButtonInteractableInit()
    {
        for(int i = 0; i < buyButtons.Count; i++)
        {
            buyButtons[i].OnInteractable();
        }
    }

    public void MoneySetting()
    {
        curGold.text = GameManager.Instance.Gold.ToString();
    }

    public void ExperienceSetting()
    {
        needExp.text = GameManager.Instance.needExp.ToString();
        curExp.text = GameManager.Instance.CurExp.ToString();
        expSlider.value = (float)GameManager.Instance.CurExp / GameManager.Instance.needExp;
    }

    public void TotalPopulationSetting()
    {
        totalPopulation.text = GameManager.Instance.totalPopulation.ToString();
    }

    public void PopulationSetting()
    {
        curPopulation.text = GameManager.Instance.CurPopulation.ToString();
    }
}
