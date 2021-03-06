using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	// Y 축을 기준으로 하는 각도를 얻습니다.
	public static float ToAngle(this Vector3 dirVector, bool castDirVector = false)
	{
		if (castDirVector) dirVector.Normalize();
		return Mathf.Atan2(dirVector.x, dirVector.z) * Mathf.Rad2Deg;
	}

	// from 에서 to 로의 방향을 반환합니다.
	public static Vector3 To(this Vector3 from, Vector3 to) =>
		(to - from).normalized;
}
