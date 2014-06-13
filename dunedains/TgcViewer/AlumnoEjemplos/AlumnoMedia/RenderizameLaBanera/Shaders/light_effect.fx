/*/////////////////////////////////////////GLOBALS*/
float3	xBallPos;
float	xBallRad2;
float4	xLightDir;
float4	xCameraPos;
float4x4 xWorldViewProj;

float xSpecularPower;


float4 calculate_shadow(float4 inCol, float3 pos)
{
	float dx, dy, dz;
	dx = pos.x - xBallPos.x;
	dy = pos.y - xBallPos.y;
	dz = pos.z - xBallPos.z;
	float4 Out = inCol - step(dx*dx + dz*dz - 7*dy, xBallRad2)*inCol/2;
	return Out;	
}
float4 calculate_light(float4 color, float ambient, float3 inNorm, float3 pos)
{
	//float3	vLightDir	=	normalize(xLightDir);
	float3	vNormal		=	normalize(inNorm);
	float	NdotLD		=	dot (vNormal, xLightDir);
	
	
	float3 vReflection	= normalize ( ( ( 2.0f * vNormal ) * (NdotLD) ) - xLightDir );
	//float3 vReflection  = normalize (-reflect(xLightDir, vNormal));
	float3 vViewDir		= normalize ( xCameraPos-pos );
	float	RdotV		= max (0.0f, dot (vReflection, vViewDir) );
	//float RdotV			= dot(normalize(vReflection), normalize(vViewDir));
	
	//Sumar Ambient + Diffuse + Specular
   float4 fvTotalAmbient   = ambient * color; 
   float4 fvTotalDiffuse   = NdotLD * color; 
   float4 fvTotalSpecular  = pow( RdotV, xSpecularPower );
   
   //Saturar para no pasarse del máximo color
   return( saturate( fvTotalAmbient + fvTotalDiffuse + fvTotalSpecular ) );
}

///ULTRA BASIC
struct ULTRA_BASIC
{
	float4 Pos : POSITION;
	float4 Col : COLOR;
};
ULTRA_BASIC VS_ultraBasic(ULTRA_BASIC In)
{
	ULTRA_BASIC Out = (ULTRA_BASIC)0;
	Out.Pos = mul(In.Pos, xWorldViewProj);
	Out.Col = In.Col;
	return Out;
}
float4 PS_ultraBasic(ULTRA_BASIC In) : COLOR
{
	return In.Col;
}
technique ultraBasic
{
	pass p0
	{
		VertexShader = compile vs_2_0 VS_ultraBasic();
		PixelShader = compile ps_2_0 PS_ultraBasic();
	}
}