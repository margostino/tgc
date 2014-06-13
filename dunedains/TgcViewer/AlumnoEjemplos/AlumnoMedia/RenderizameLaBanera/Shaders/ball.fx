/*/////////////////////////////////////////GLOBALS*/
#include <light_effect.fx>

float4x4 xWorld;
float4x4 xViewProj;
float3 xCameraEye;
float xTime;
float xBallRad;
float xKBallBooble;

texture  xCubeTex;
samplerCUBE envMapSampler = 
sampler_state
{
    Texture = <xCubeTex>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
};

//Genera el efecto de movimiento de los vertices de la esfera
float4 VS_effect_booble(float4 Pos)
{
	///no es un MAIN, espera coordenadas en world
	float variacion		= sin(0.3f * xTime * Pos.x)
						 -sin(0.3f * xTime * Pos.y)
						 -sin(0.3f * xTime * Pos.z); 
	float3 axis_radial	= Pos - xBallPos;
	float4 P;
	P.xyz = Pos.xyz + variacion * axis_radial * xKBallBooble;
	P.w = Pos.w;

	return P;
}

/*//////////////////////////////////////////BASIC*/
struct VS_BASIC_INPUT
{
	float4 Pos : POSITION;
	float3 Norm: NORMAL;
};
struct VS_BASIC_OUTPUT
{
	float4 Pos : POSITION;
	float3 Pos3D : TEXCOORD0;
	float3 Norm  : TEXCOORD1;
	float3 Reflection	: TEXCOORD2;
	float3 Refraxion	: TEXCOORD3;
	
};
VS_BASIC_OUTPUT VS_main_basic(VS_BASIC_INPUT In)
{
	VS_BASIC_OUTPUT Out = (VS_BASIC_OUTPUT)0;
	float4 WorldP = VS_effect_booble(mul(In.Pos, xWorld));
	
	//Projecta la posicion
	Out.Pos = mul(WorldP, xViewProj);
	//Guardo la posicion real en 3D
	Out.Pos3D = WorldP;
	//normal
	float3 Norm  = -normalize(mul(In.Norm, (float3x3) xWorld));
	Out.Norm = Norm;
	
	//Reflexion y refraxion
	float3 vEyeR = normalize(WorldP-xCameraPos);
	
	Out.Reflection	= reflect(vEyeR,Norm);
	Out.Refraxion	= refract(vEyeR,Norm,0.99f);
	
	return Out;
}
float4 PS_main_basic(VS_BASIC_OUTPUT In): COLOR
{
	float4 Out = (float4)0;
	
	float3 ViewDirection = normalize(xCameraEye - In.Pos3D); 
	float3 Norm = normalize(In.Norm);
	
	//Obtengo los colores desde la textura
	float4 color_reflejado = texCUBE( envMapSampler, In.Reflection);
	float4 color_refractado= texCUBE( envMapSampler, In.Refraxion);
	
	//Fresnel
	float fresnelTerm = dot(ViewDirection, Norm);
	float4 colorCombinado = lerp(color_reflejado, color_refractado, fresnelTerm);
	//Calculo la luz
	Out = calculate_light(colorCombinado, 1.0f, Norm, In.Pos3D);
	return Out;
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