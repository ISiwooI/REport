using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTest : MonoBehaviour
{
    public List<test> tests = new List<test>();
    public delegate IEnumerator test(int i);
    test testlistener;
    // Start is called before the first frame update

    /*
            tests.Add(test1);
            tests.Add(test2);
            tests.Add(test3);
        }
        IEnumerator test1(int i)
        {
            Debug.Log("test1");
            yield return new WaitForSeconds(i);

        }
        IEnumerator test2(int i)
        {
            Debug.Log("test2");
            yield return new WaitForSeconds(i);

        }
        IEnumerator test3(int i)
        {
            Debug.Log("test3");
            yield return new WaitForSeconds(i);

        }
        IEnumerator main(int i)
        {
            foreach (test t in tests)
            {
                if (t != null)
                    yield return t(i);
            }
            yield break;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(main(1));
            }
        }*/
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            result();
        }
    }
    void result()
    {
        GameManager.Inst.gameState = GameState.BattleResult;
        int result = Utill.random.Next(0, 100);
        GameManager.Inst.battleresult = result;
        SceneManager.LoadScene("testMain");
    }
}
