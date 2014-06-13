/*/////////////////////////////////////////GLOBALS*/
#include <light_effect.fx>

float4x4 xWorld;
float4x4 xReflexVP;

texture xLeafTex;

sampler leafSampler = sampler_state {
	texture = <xLeafTex> ; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = mirror; 
	AddressV = mirror;
};



/*//////////////////////////////////////////BASIC*/
struct VS_BASIC_INPUT
{
	float4 Pos : POSITION;
	float3 Norm: NORMAL;
	float2 TexCoord : TEXCOORD0;
};
struct VS_BASIC_OUTPUT
{
	float4 Pos : POSITION;
	float3 Pos3D : TEXCOORD0;
	float3 Norm  : TEXCOORD1;
	float2 TexCoord : TEXCOORD2;
};
VS_BASIC_OUTPUT VS_main_basic(VS_BASIC_INPUT In)
{
	VS_BASIC_OUTPUT Out = (VS_BASIC_OUTPUT)0;
	Out.Pos = mul(In.Pos, xWorldViewProj);
	Out.Pos3D = mul(In.Pos, xWorld);
	Out.Norm  = mul(In.Norm, (float3x3) xWorld);
	Out.TexCoord = In.TexCoord;
	
	return Out;
}
float4 PS_main_basic(VS_BASIC_OUTPUT In): COLOR
{
	float4 Out = (float4)0;
	float2 CoordTex;
	//CoordTex.x = -In.TexCoord.y;
	//CoordTex.y = -In.TexCoord.x;
	float4 temp_col = calculate_shadow(tex2D(leafSampler, In.TexCoord), In.Pos3D);
	Out = calculate_light(temp_col, 0.01f, In.Norm, In.Pos3D);
	
	return saturate(Out);
}

/*////////////////////////////////////////
///////////////////////////////TECHNIQUES
////////////////////////////////////////*/
technique Basic
{
	pass p0
	{
		VertexShader = compile vs_2_0 VS_main_basic();
		PixelShader = compile ps_2_0 PS_main_basic();
	}
}