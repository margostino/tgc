// matriz de transformacion
float4x4	mViewProj;

// parametros del sol		
float	sun_alfa, sun_theta,			// posicion del sol
		sun_shininess, sun_strength;	// propiedades de luz del sol

float 	zfar;// = 4000; // radio del skydome

bool	noche, rayo;

static const float PI = 3.14159265f;

// extructura de input del vertex shader
struct VS_INPUT
{
    float3  Pos      		: POSITION;
    float4  displacement	: COLOR0;
};

// extructura de output del vertex shader
struct VS_OUTPUT
{
    float4  Pos     : POSITION;
	float3	v		: TEXCOORD1;
	float3  sun		: TEXCOORD2;
};

// enviroment map
texture EnvironmentMap;
samplerCUBE skySRGB = sampler_state
{  
    Texture = <EnvironmentMap>; 
    MipFilter = LINEAR; 
    MinFilter = LINEAR; 
    MagFilter = LINEAR; 
	AddressU  = CLAMP;		
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    SRGBTexture = true;
};

// vertex shader
VS_OUTPUT VertexShader(VS_INPUT i)
{
    VS_OUTPUT   o;
    
	// acomoda los vertices con un radio zfar
	o.Pos = mul(float4(zfar*i.Pos,1), mViewProj);	
	o.v = i.Pos;
    
	// coordenadas de textura para el sol
	o.sun.x = cos(sun_theta)*sin(sun_alfa);
	o.sun.y = sin(sun_theta);
	o.sun.z = cos(sun_theta)*cos(sun_alfa);

	return o;
}

// pixel shader 
float4 PixelShader(VS_OUTPUT i) : COLOR
{
	// crea el sol/luna en el skydome si corresponde de acuerdo al pixel
    float3 sunlight = sun_strength*pow(saturate(dot(normalize(i.v), i.sun)),sun_shininess)*float3(1.2, 0.4, 0.1);
	float3 moonlight = sun_strength*pow(saturate(dot(normalize(i.v), i.sun)),sun_shininess)*float3(0.3, 0.4, 0.5);
	
	float4 ut;
	ut.a = 1;
	
	// si es de noche cambio el sol por la luna
	if(noche)
		sunlight = moonlight;
	
	// acomoda las coordenadas de textura para el resto del cielo en base a la textura (cubemap-evul.dds)
	// es pixel por pixel, si corresponde a la zona del sol le aplica la sunlight		
	ut.rgb = pow(texCUBE(skySRGB,i.v) + sunlight, sin(abs((sun_theta-PI/2)))+0.2);
	
	// si es de noche oscuresco todo el cielo
	if(noche) 
		ut.rgb = ut.rgb*0.5;

	// si hay un rayo aclaro todo el cielo
	if(rayo) 
		ut.rgb = ut.rgb*3;
		
	return ut;
}

// define technique y pass
technique T0
{
    pass P0
    {       
		// aplica pixel shader
        pixelshader = compile ps_2_0 PixelShader();
		// aplica vertex shader
		vertexshader = compile vs_1_1 VertexShader(); 
    }
}