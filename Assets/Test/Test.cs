using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{
	void OnEnable()
	{
		var texture = new Texture2D(256, 256);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = FilterMode.Point;
		PerlinNoiseTexture.Apply(texture);

		var mat = new Material(Shader.Find("Unlit/Texture"));
		mat.mainTexture = texture;

		var mr = GetComponent<MeshRenderer>();
		mr.material = mat;
	}
}
