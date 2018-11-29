using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseTexture
{
	public static void Apply(Texture2D tex)
	{
		if (tex == null)
			return;

		int xSize = tex.width;
		int ySize = tex.height;
		float xStep = 1.0f / (float)xSize;
		float yStep = 1.0f / (float)ySize;
		float xOffset = xStep * 0.5f;
		float yOffset = yStep * 0.5f;

		PerlinNoise perlin = new PerlinNoise();
		for (int y = 0; y < ySize; y++)
		{
			float vy = y * yStep + yOffset;
			for (int x = 0; x < xSize; x++)
			{
				float vx = x * xStep + xOffset;
				tex.SetPixel(x, y, Color.white * perlin.Eval(vx, vy));
			}
		}
		tex.Apply();
	}

	public static void ApplyLegacy(Texture2D tex)
	{
		if (tex == null)
			return;

		int xSize = tex.width;
		int ySize = tex.height;
		float xStep = 1.0f / xSize;
		float yStep = 1.0f / ySize;
		float xOffset = xStep * 0.5f;
		float yOffset = yStep * 0.5f;

		PerlinNoiseLegacy perlin = new PerlinNoiseLegacy();
		for (int y = 0; y < ySize; y++)
		{
			for (int x = 0; x < xSize; x++)
			{
				float vx = x * xStep + xOffset;
				float vy = y * yStep + yOffset;
				var f = 0.0f;
				var w = 0.5f;
				for (int i = 0; i < 5; i++)
				{
					f += w * perlin.Eval(vx, vy);
					vx *= 2;
					vy *= 2;
					w *= 0.5f;
				}
				f = (f / 1.4f + 0.5f);
				tex.SetPixel(x, y, Color.white * f);
			}
		}
		tex.Apply();
	}
}
