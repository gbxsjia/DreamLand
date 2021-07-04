using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public CharacterManager character;
    private void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    private void Update()
    {
        if (character)
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            character.Move(direction);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                Vector3 actionDirection = hit.point - transform.position;
                actionDirection.y = 0;

                character.Attack(actionDirection);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                Vector3 actionDirection = hit.point - transform.position;
                actionDirection.y = 0;

                character.Skill(actionDirection);
            }
        }
    }
}
