using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public Text player1Lives, player1Damage, player2Lives, player2Damage;
	
	// Update is called once per frame
	void Update () {
        player1Lives.text = "Lives: " + GameManager.instance.playerLives;
        player1Damage.text = GameManager.instance.playerDamageTaken + "%";
        player2Lives.text = "Lives: " + GameManager.instance.AILives;
        player2Damage.text = GameManager.instance.AIDamageTaken + "%";
    }
}
