using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BudgetMenager : MonoBehaviour
{

    public Score scores;

    public InputField budynekPriceInput;
    public InputField budynekLimitsInput;
    public InputField niskibudynekPriceInput;
    public InputField niskibudynekLimitsInput;
    public InputField benchPriceInput;
    public InputField benchLimitsInput;
    public InputField treePriceInput;
    public InputField treeLimitsInput;
    public InputField fountainPriceInput;
    public InputField fountainLimitsInput;
    public InputField BudgetInput;
    public Text budgettext;
    public Text budgetInputInfo;
    public Button confirmButton;

    public GameObject budynek;
    public GameObject niskibudynek;
    public GameObject bench;
    public GameObject tree;
    public GameObject fountain;
    public GameObject budynki;

    private Budget budget;
    private int bdprice;
    private int nisbdprice;
    private int bdlimits;
    private int nisbdlimits;
    private int benprice;
    private int benlimits;
    private int treeprice;
    private int treelimits;
    private int ftprice;
    private int ftlimits;
    private int Budget;

    public Text BPriText;
    public Text BLimText;
    public Text nBPriText;
    public Text nBLimText;
    public Text BenPriText;
    public Text BenLimText;
    public Text TrePriText;
    public Text TreLimText;
    public Text FoPriText;
    public Text FoLimText;

    // Start is called before the first frame update
    private void Awake()
    {
        budynekPriceInput.contentType = InputField.ContentType.IntegerNumber;
        budynekPriceInput.characterValidation = InputField.CharacterValidation.Integer;
        niskibudynekPriceInput.contentType = InputField.ContentType.IntegerNumber;
        niskibudynekPriceInput.characterValidation = InputField.CharacterValidation.Integer;
        budynekLimitsInput.contentType = InputField.ContentType.IntegerNumber;
        budynekLimitsInput.characterValidation = InputField.CharacterValidation.Integer;
        niskibudynekLimitsInput.contentType = InputField.ContentType.IntegerNumber;
        niskibudynekLimitsInput.characterValidation = InputField.CharacterValidation.Integer;
        benchPriceInput.contentType = InputField.ContentType.IntegerNumber;
        benchPriceInput.characterValidation = InputField.CharacterValidation.Integer;
        benchLimitsInput.contentType = InputField.ContentType.IntegerNumber;
        benchLimitsInput.characterValidation = InputField.CharacterValidation.Integer;
        treePriceInput.contentType = InputField.ContentType.IntegerNumber;
        treePriceInput.characterValidation = InputField.CharacterValidation.Integer;
        treeLimitsInput.contentType = InputField.ContentType.IntegerNumber;
        treeLimitsInput.characterValidation = InputField.CharacterValidation.Integer;
        fountainPriceInput.contentType = InputField.ContentType.IntegerNumber;
        fountainPriceInput.characterValidation = InputField.CharacterValidation.Integer;
        fountainLimitsInput.contentType = InputField.ContentType.IntegerNumber;
        fountainLimitsInput.characterValidation = InputField.CharacterValidation.Integer;
        BudgetInput.contentType = InputField.ContentType.IntegerNumber;
        BudgetInput.characterValidation = InputField.CharacterValidation.Integer;
        setBuildingsPriceLoad();
        setInfoTextLoad();
    }

    void Start()
    {
        budget = FindObjectOfType<Budget>();
        if(MainMenu.isAdmin == false)
        {
            budynekPriceInput.gameObject.SetActive(false);
            niskibudynekPriceInput.gameObject.SetActive(false);
            budynekLimitsInput.gameObject.SetActive(false);
            niskibudynekLimitsInput.gameObject.SetActive(false);
            benchPriceInput.gameObject.SetActive(false);
            benchLimitsInput.gameObject.SetActive(false);
            treePriceInput.gameObject.SetActive(false);
            treeLimitsInput.gameObject.SetActive(false);
            fountainPriceInput.gameObject.SetActive(false);
            fountainLimitsInput.gameObject.SetActive(false);
            BudgetInput.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(false);
            budgetInputInfo.gameObject.SetActive(false);
        }
        if (MainMenu.isAdmin == true)
        {
            budynekPriceInput.gameObject.SetActive(true);
            niskibudynekPriceInput.gameObject.SetActive(true);
            budynekLimitsInput.gameObject.SetActive(true);
            niskibudynekLimitsInput.gameObject.SetActive(true);
            benchPriceInput.gameObject.SetActive(true);
            benchLimitsInput.gameObject.SetActive(true);
            treePriceInput.gameObject.SetActive(true);
            treeLimitsInput.gameObject.SetActive(true);
            fountainPriceInput.gameObject.SetActive(true);
            fountainLimitsInput.gameObject.SetActive(true);
            BudgetInput.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(true);
            budgetInputInfo.gameObject.SetActive(true);
        }
        setInfoText();
    }

    // Update is called once per frame
    void Update()
    {
        setBudgetText();
        setInfoText();
        FindObjectOfType<Score>().isNewBestScore();
        FindObjectOfType<Score>().setScoreText();
    }

    public void setBudgetAmount()
    {

    }

    public void setBuildingsNumLimit()
    {

    }

    public void setBuildingsPrice()
    {
        if(budynekPriceInput.text == "")
            bdprice = 0;
        else
            bdprice = int.Parse(budynekPriceInput.text);
        
        if (niskibudynekPriceInput.text == "")
            nisbdprice = 0;
        else
            nisbdprice = int.Parse(niskibudynekPriceInput.text);
        
        if (budynekLimitsInput.text == "")
            bdlimits = 0;
        else
            bdlimits = int.Parse(budynekLimitsInput.text);

        if (niskibudynekLimitsInput.text == "")
            nisbdlimits = 0;
        else
            nisbdlimits = int.Parse(niskibudynekLimitsInput.text);

        if (benchPriceInput.text == "")
            benprice = 0;
        else
            benprice = int.Parse(benchPriceInput.text);

        if (benchLimitsInput.text == "")
            benlimits = 0;
        else
            benlimits = int.Parse(benchLimitsInput.text);

        if (treePriceInput.text == "")
            treeprice = 0;
        else
            treeprice = int.Parse(treePriceInput.text);

        if (treeLimitsInput.text == "")
            treelimits = 0;
        else
            treelimits = int.Parse(treeLimitsInput.text);

        if (fountainPriceInput.text == "")
            ftprice = 0;
        else
            ftprice = int.Parse(fountainPriceInput.text);

        if (fountainLimitsInput.text == "")
            ftlimits = 0;
        else
            ftlimits = int.Parse(fountainLimitsInput.text);

        if (BudgetInput.text == null)
            Budget = 0;
        else
            Budget = int.Parse(BudgetInput.text);

        budynek.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(bdprice, bdlimits);
        niskibudynek.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(nisbdprice, nisbdlimits);
        bench.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(benprice, benlimits);
        tree.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(treeprice, treelimits);
        fountain.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(ftprice, ftlimits);
        budget.setCurrentAmount(Budget);
    }

    public void setBuildingsPriceLoad()
    {
        bdprice = GameObject.Find("budynki").GetComponent<PrefabInfo>().price[0];
        nisbdprice = budynki.GetComponent<PrefabInfo>().price[1];
        bdlimits = GameObject.Find("budynki").GetComponent<PrefabInfo>().limits[0];
        nisbdlimits = budynki.GetComponent<PrefabInfo>().limits[1];
        benprice = budynki.GetComponent<PrefabInfo>().price[2];
        benlimits = budynki.GetComponent<PrefabInfo>().limits[2];
        treeprice = budynki.GetComponent<PrefabInfo>().price[3];
        treelimits = budynki.GetComponent<PrefabInfo>().limits[3];
        ftprice = GameObject.Find("budynki").GetComponent<PrefabInfo>().price[4];
        ftlimits = GameObject.Find("budynki").GetComponent<PrefabInfo>().limits[4];
        //Debug.Log(GameObject.Find("budynki").GetComponent<PrefabInfo>().price[4]);
        budynek.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(bdprice, bdlimits);
        niskibudynek.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(nisbdprice, nisbdlimits);
        bench.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(benprice, benlimits);
        tree.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(treeprice, treelimits);
        fountain.gameObject.GetComponent<BuildingInfo>().changePropertyBudget(ftprice, ftlimits);
    }

    public int getBud()
    {
        int budAm = budynki.GetComponent<PrefabInfo>().budAmount;
        return budAm;
    }

    public int getNisBud()
    {
        int nisbudAm = budynki.GetComponent<PrefabInfo>().nisbudAmount;
        return nisbudAm;
    }

    public int getBen()
    {
        int benAm = budynki.GetComponent<PrefabInfo>().benchAmount;
        return benAm;
    }

    public int getTree()
    {
        int treeAm = budynki.GetComponent<PrefabInfo>().treeAmount;
        return treeAm;
    }

    public int getFou()
    {
        int fouAm = budynki.GetComponent<PrefabInfo>().fountainAmount;
        return fouAm;
    }

    public void setBudgetText()
    {
        int tmp = budget.getCurrentAmount();
        budgettext.text = "Current budget: " + tmp.ToString();
    }

    public void setInfoText()
    {
        BPriText.text = "Price: " + bdprice.ToString();
        BLimText.text = "Limits: " + bdlimits.ToString();
        nBPriText.text = "Price: " + nisbdprice.ToString();
        nBLimText.text = "Limits: " + nisbdlimits.ToString();
        BenPriText.text = "Price: " + benprice.ToString();
        BenLimText.text = "Limits: " + benlimits.ToString();
        TrePriText.text = "Price: " + treeprice.ToString();
        TreLimText.text = "Limits: " + treelimits.ToString();
        FoPriText.text = "Price: " + ftprice.ToString();
        FoLimText.text = "Limits: " + ftlimits.ToString();
    }

    public void setInfoTextLoad()
    {
        bdprice = GameObject.Find("budynki").GetComponent<PrefabInfo>().price[0];
        nisbdprice = budynki.GetComponent<PrefabInfo>().price[1];
        bdlimits = GameObject.Find("budynki").GetComponent<PrefabInfo>().limits[0];
        nisbdlimits = budynki.GetComponent<PrefabInfo>().limits[1];
        benprice = budynki.GetComponent<PrefabInfo>().price[2];
        benlimits = budynki.GetComponent<PrefabInfo>().limits[2];
        treeprice = budynki.GetComponent<PrefabInfo>().price[3];
        treelimits = budynki.GetComponent<PrefabInfo>().limits[3];
        ftprice = GameObject.Find("budynki").GetComponent<PrefabInfo>().price[4];
        ftlimits = GameObject.Find("budynki").GetComponent<PrefabInfo>().limits[4];
        BPriText.text = "Price: " + bdprice.ToString();
        BLimText.text = "Limits: " + bdlimits.ToString();
        nBPriText.text = "Price: " + nisbdprice.ToString();
        nBLimText.text = "Limits: " + nisbdlimits.ToString();
        BenPriText.text = "Price: " + benprice.ToString();
        BenLimText.text = "Limits: " + benlimits.ToString();
        TrePriText.text = "Price: " + treeprice.ToString();
        TreLimText.text = "Limits: " + treelimits.ToString();
        FoPriText.text = "Price: " + ftprice.ToString();
        FoLimText.text = "Limits: " + ftlimits.ToString();
    }

    public int getBudget()
    {
        return Budget;
    }
}
