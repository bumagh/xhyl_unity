using HW3L;
using UnityEngine;

public class FK3_BannedClick : MonoBehaviour
{
	private void OnEnable()
	{
		FK3_SimpleSingletonBehaviour<UserInput>.Get().allowInput = false;
	}

	private void OnDisable()
	{
		FK3_SimpleSingletonBehaviour<UserInput>.Get().allowInput = true;
	}
}
