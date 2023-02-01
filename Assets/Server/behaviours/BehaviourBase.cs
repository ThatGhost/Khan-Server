using UnityEngine;
using System.Collections;

public class BehaviourBase : MonoBehaviour
{
	private bool m_move = false;
	public void doSomething()
	{
		m_move = true;
	}

    private void Update()
    {
		if (m_move)
		{
			gameObject.transform.position += new Vector3(0, 1* Time.deltaTime, 0);
		}
    }
}

