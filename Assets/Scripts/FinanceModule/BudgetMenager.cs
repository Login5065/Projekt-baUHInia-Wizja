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

    public GameObject budynek;
    public GameObject niskibudynek;
    public GameObject bench;
    public GameObject tree;
    public GameObject fountain;

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
    }

    void Start()
    {
        budget = FindObjectOfType<Budget>();
    }

    // Update is called once per frame
    void Update()
    {
        setBudgetText();
    }

    public bool checkForBestScore(int score)
    {
        scores.isNewBestScore(score);
        return true;
    }

    public void setBudgetAmount()
    {

    }

    public void setBuildingsNumLimit()
    {

    }

    public void setBuildingsPrice()
    {
        bdprice = int.Parse(budynekPriceInput.text);
        nisbdprice = int.Parse(niskibudynekPriceInput.text);
        bdlimits = int.Parse(budynekLimitsInput.text);
        nisbdlimits = int.Parse(niskibudynekLimitsInput.text);
        benprice = int.Parse(benchPriceInput.text);
        benlimits = int.Parse(benchLimitsInput.text);
        treeprice = int.Parse(treePriceInput.text);
        treelimits = int.Parse(treeLimitsInput.text);
        ftprice = int.Parse(fountainPriceInput.text);
        ftlimits = int.Parse(fountainLimitsInput.text);
        Budget = int.Parse(BudgetInput.text);
        budynek.gameObject.GetComponent<BuildingInfo>().changeProperty(bdprice, 0, bdlimits);
        niskibudynek.gameObject.GetComponent<BuildingInfo>().changeProperty(nisbdprice, 0, nisbdlimits);
        bench.gameObject.GetComponent<BuildingInfo>().changeProperty(benprice, 0, benlimits);
        tree.gameObject.GetComponent<BuildingInfo>().changeProperty(treeprice, 0, treelimits);
        fountain.gameObject.GetComponent<BuildingInfo>().changeProperty(ftprice, 0, ftlimits);
        budget.setCurrentAmount(Budget);
    }

    public void setBudgetText()
    {
        int tmp = budget.getCurrentAmount();
        budgettext.text = "Current budget: " + tmp.ToString();
    }

    public int getBudget()
    {
        return Budget;
    }
}
