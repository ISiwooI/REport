using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testsprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var sprite in GetComponentsInChildren<SpriteRenderer>(true))
            {

                sprite.material.color = Color.white;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (var sprite in GetComponentsInChildren<SpriteRenderer>(true))
            {

                sprite.material.color = Color.red;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GetComponentInChildren<EnemyActor>().animator.SetTrigger("Dead");
        }
        if (Input.GetKeyDown(KeyCode.D))
            GetComponentInChildren<EnemyActor>().animator.SetTrigger("Idle");
    }
}
