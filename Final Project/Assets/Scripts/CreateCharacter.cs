using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacter : MonoBehaviour {

    public List<GameObject> Characters;

    private GameObject player, pawn;

    public void Create(string character, bool isPlayer, string name) {
        player = new GameObject(name);
        GetPawn(character);
        SetLocation();
        CharacterSetup(isPlayer);
    }

    void GetPawn(string character) {
        for (int i = 0; i < Characters.Count; i++) {
            if (character == Characters[i].name) {
                pawn = Instantiate(Characters[i], player.transform.position, player.transform.rotation) as GameObject;
            }
        }
        pawn.transform.parent = player.transform;
    }

    void CharacterSetup(bool isPlayer) {
        if (isPlayer) {
            player.tag = "Player";
            player.AddComponent<PlayerController>();
        } else {
            player.tag = "AI";
            player.AddComponent<AIController>();
        }
    }

    void SetLocation() {
        int num = Random.Range(0, GameManager.instance.startLocation.Count);
        while (num == GameManager.instance.randomNum) {
            num = Random.Range(0, GameManager.instance.startLocation.Count);
        }

        if (GameManager.instance.startLocation[num] != null) {
            player.transform.position = GameManager.instance.startLocation[num].position;
            GameManager.instance.randomNum = num;
        }
    }
}
