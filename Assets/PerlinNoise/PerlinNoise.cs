using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise
{
	private const int tableSize = 256;
	private const int tableMask = tableSize - 1;

	private int[] hashLUT = new int[tableSize * 2];

	public PerlinNoise()
	{
		for (int i = 0; i < tableSize; i++)
			hashLUT[i] = i;

		for (int i = 0; i < tableSize; i++)
		{
			int j = Random.Range(0, tableSize - 1);
			var tmp = hashLUT[i];
			hashLUT[i] = hashLUT[j];
			hashLUT[j] = tmp;
		}

		for (int i = 0; i < tableSize; i++)
			hashLUT[tableSize + i] = hashLUT[i];
	}

	public float Eval (float x, float y)
	{
		return Eval(x, 0, y);
	}

	public float Eval (float x, float y, float z)
	{
		int x0 = Mathf.FloorToInt(x) & tableMask;
		int y0 = Mathf.FloorToInt(y) & tableMask;
		int z0 = Mathf.FloorToInt(z) & tableMask;

		int x1 = (x0 + 1) & tableMask;
		int y1 = (y0 + 1) & tableMask;
		int z1 = (z0 + 1) & tableMask;

		float tx0 = x - Mathf.FloorToInt(x);
		float ty0 = y - Mathf.FloorToInt(y);
		float tz0 = x - Mathf.FloorToInt(z);

		float tx1 = tx0 - 1;
		float ty1 = ty0 - 1;
		float tz1 = tz0 - 1;

		float a = DotGridGradient(Hash(x0, y0, z0), tx0, ty0, tz0);
		float b = DotGridGradient(Hash(x1, y0, z0), tx1, ty0, tz0);
		float c = DotGridGradient(Hash(x0, y1, z0), tx0, ty1, tz0);
		float d = DotGridGradient(Hash(x1, y1, z0), tx1, ty1, tz0);
		float e = DotGridGradient(Hash(x0, y0, z1), tx0, ty0, tz1);
		float f = DotGridGradient(Hash(x1, y0, z1), tx1, ty0, tz1);
		float g = DotGridGradient(Hash(x0, y1, z1), tx0, ty1, tz1);
		float h = DotGridGradient(Hash(x1, y1, z1), tx1, ty1, tz1);

		float k0 = a;
		float k1 = (b - a);
		float k2 = (c - a);
		float k3 = (e - a);
		float k4 = (a + d - b - c);
		float k5 = (a + f - b - e);
		float k6 = (a + g - c - e);
		float k7 = (b + c + e + h - a - d - f - g);

		float u = Quintic(tx0);
		float v = Quintic(ty0);
		float w = Quintic(tz0);

		return k0 + k1 * u + k2 * v + k3 * w + k4 * u * v + k5 * u * w + k6 * v * w + k7 * u * v * w;
	}

	private float Quintic(float t)
	{
		return t * t * t * (t * (t * 6 - 15) + 10);
	}

	private int Hash(int x, int y, int z)
	{
		return hashLUT[hashLUT[hashLUT[x] + y] + z];
	}

	private float DotGridGradient(int hash, float x, float y, float z)
	{
		switch (hash & 15)
		{
			case 0: return x + y; // (1,1,0) 
			case 1: return -x + y; // (-1,1,0) 
			case 2: return x - y; // (1,-1,0) 
			case 3: return -x - y; // (-1,-1,0) 
			case 4: return x + z; // (1,0,1) 
			case 5: return -x + z; // (-1,0,1) 
			case 6: return x - z; // (1,0,-1) 
			case 7: return -x - z; // (-1,0,-1) 
			case 8: return y + z; // (0,1,1), 
			case 9: return -y + z; // (0,-1,1), 
			case 10: return y - z; // (0,1,-1), 
			case 11: return -y - z; // (0,-1,-1) 
			case 12: return y + x; // (1,1,0) 
			case 13: return -x + y; // (-1,1,0) 
			case 14: return -y + z; // (0,-1,1) 
			case 15: return -y - z; // (0,-1,-1) 
		}

		return 0;
	}
}
