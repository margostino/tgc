/*/////////////////////////////////////////GLOBALS*/
#include <light_effect.fx>

float4x4 xWorld;
float4x4 xReflectionViewProj;

float xTime;
float xWindForce;

texture xReflexTex;
texture xNoiseTex;
texture xRiverBottom;

float xWaveLength = 1.0f;
float xWaveHeight = 0.3f;

sampler reflexSampler = sampler_state {
	texture = <xReflexTex> ; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = mirror; 
	AddressV = mirror;
};
sampler refractSampler = sampler_state {
	texture = <xRiverBottom> ; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = mirror; 
	AddressV = mirror;
};
sampler WaterBumpMapSampler = sampler_state { 
	texture = <xNoiseTex> ; 
	magfilter = LINEAR; 
	minfilter = LINEAR;
	mipfilter=POINT; 
	AddressU = mirror; 
	AddressV = mirror;
};


/*//////////////////////////////////////////BASIC*/
struct VS_BASIC_INPUT
{
	float4 Pos : POSITION;
	float4 inTex: TEXCOORD;
};
struct VS_BASIC_OUTPUT
{
	float4 Pos : POSITION;
	float3 Pos3D : TEXCOORD0;
	float4 ReflexionMap : TEXCOORD1;
	float2 BumpMapPos : TEXCOORD2;
	float4 RefractionMap: TEXCOORD3;
};
VS_BASIC_OUTPUT VS_main_basic(VS_BASIC_INPUT In)
{
	VS_BASIC_OUTPUT Out = (VS_BASIC_OUTPUT)0;
	
	float4 wvpPos = mul(In.Pos, xWorldViewProj);
	float4x4 preWorldReflextViewProj = mul(xWorld, xReflectionViewProj);
	//Posicion real en 3D
	Out.Pos3D = mul(In.Pos, xWorld);
	//Projeccion de posicion
	Out.Pos = wvpPos;
	//Proyeccion de reflexion
	Out.ReflexionMap = mul (In.Pos, preWorldReflextViewProj);
	float2 moveVector = float2(0, xTime*xWindForce);
	//poscion en textura de normales
	Out.BumpMapPos = (In.inTex + moveVector) / xWaveLength;
	Out.RefractionMap = wvpPos;

	return Out;
}
float4 PS_main_basic(VS_BASIC_OUTPUT In): COLOR
{
	
	float4 Out = (float4)0;
	
	//Bumps
	float4 bumpColor = tex2D(WaterBumpMapSampler, In.BumpMapPos); 
	float2 perturbation = xWaveHeight*(bumpColor.rg - 0.5f)*2.0f;
	
	float3 Normal = (bumpColor.rbg-0.5f)*2.0f;
	
	//Reflection
	float2 ProjectedTexCoords;
    ProjectedTexCoords.x =  In.ReflexionMap.x/In.ReflexionMap.w/2.0f + 0.5f;
    ProjectedTexCoords.y = -In.ReflexionMap.y/In.ReflexionMap.w/2.0f + 0.5f;   
    
    float2 perturbatedTexCoords = ProjectedTexCoords + perturbation;
    float4 reflectiveColor = tex2D(reflexSampler, perturbatedTexCoords);
    
    //Refraction
    float2 ProjectedRefrTexCoords;
	ProjectedRefrTexCoords.x =  In.RefractionMap.x/In.RefractionMap.w/2.0f + 0.5f;
	ProjectedRefrTexCoords.y = -In.RefractionMap.y/In.RefractionMap.w/2.0f + 0.5f;    
	
	float2 perturbatedRefrTexCoords = ProjectedRefrTexCoords + perturbation;    
	float4 refractiveColor = tex2D(refractSampler, perturbatedRefrTexCoords);
	
	//Fresnel
	float3 eyeVector = normalize(xCameraPos - In.Pos3D);
	float3 normalVector = Normal;// float3(0,1,0);
	float fresnelTerm = dot(eyeVector, normalVector);
	float4 combinedColor = lerp(reflectiveColor, refractiveColor, fresnelTerm);
	
	//Dull color and shadow
	//float4 finalColor = lerp(combinedColor, float4(0.3f, 0.3f, 0.5f, 1.0f), 0.2f);
	
	//
	Out = calculate_shadow(calculate_light(combinedColor, 0.2f, Normal , In.Pos3D), In.Pos3D);
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
