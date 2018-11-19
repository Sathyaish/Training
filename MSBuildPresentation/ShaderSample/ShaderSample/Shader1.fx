/* Start File Header -------------------------------------------------------
Copyright (C) 2010 DigiPen Institute of Technology.
Reproduction or disclosure of this file or its contents without the prior written consent of DigiPen Institute of Technology is prohibited.
File Name: ClearToFloat.fx
Purpose: This shader clears the pixels it draws to a certain value.
Authors: Robert Francis - 100%
- End File Header --------------------------------------------------------*/

float value;
 
struct VS_INPUT
{
	float3 position : POSITION;
	float2 tex0 : TEXCOORD0;
};

struct VS_OUTPUT
{
	float4 position : POSITION;
	float2 tex0 : TEXCOORD0;
};

float4 Pixel (VS_INPUT IN) : COLOR
{
	return float4(value, value, value, 0);
}

VS_OUTPUT Vertex(VS_INPUT IN) 
{
	VS_OUTPUT OUT;
	OUT.position = float4(IN.position, 1.0);
	OUT.tex0 = IN.tex0;
	return OUT;
}

technique Technique0
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 Vertex();
		PixelShader = compile ps_2_0 Pixel();
		ZEnable = true;
		AlphaBlendEnable = false;
	}
}