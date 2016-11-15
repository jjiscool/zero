using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIStatus : MonoBehaviour {
	private int HP;
	private int HPMAX;
	private int Money;
	private int AP;
	private int APMAX;
	private int LV;
	private float hpProportion;


	//UI元素
	private Slider hpBar;
	private Slider apBar;
	private Slider expBar;

	private Text hpBarText;
	private Text apBarText;
	private Text expBarText;
	private Text moneyText;

	//存放player对象状态
	private playerStatus pStatus;

	void Start () {
		setUIStatus ();

	}
	
	// Update is called once per frame
	void Update () {
		setUIStatus ();

	}

	void setUIStatus(){
		hpBar = transform.Find ("hpBar").GetComponent<Slider>();
		apBar = transform.Find ("apBar").GetComponent<Slider>();
		expBar = transform.Find ("expBar").GetComponent<Slider>();

		hpBarText = transform.Find ("hpBar/Text").GetComponent<Text>();
		apBarText = transform.Find ("apBar/Text").GetComponent<Text>();
		expBarText = transform.Find ("expBar/Text").GetComponent<Text>();
		moneyText = transform.Find ("moneyNum/Text").GetComponent<Text>();

		pStatus = GameObject.Find ("Player").GetComponent<playerStatus> ();

		HP = pStatus.HP;
		HPMAX = pStatus.HPMAX;
		Money = pStatus.Money;
		AP = pStatus.AP;
		APMAX = pStatus.APMAX;

		hpProportion = (float)HP / HPMAX;
		//Debug.Log (hpProportion);

		//设值
		hpBarText.text = HP + "/" + HPMAX;
		apBarText.text = AP + "/" + APMAX;
		moneyText.text = Money + "";
		hpBar.value = hpProportion;
	}


}
