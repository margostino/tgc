float4x4	world;
float4x4	view;
float4x4	proj;

float	alpha,maxHeightSuperficial;
float	desplazamiento,amplitud,frecuencia;
float	radio,largo,dev,smallvalue;

float4		view_position;
float		LODbias;
float		sun_shininess, sun_strength;
float3		sun_vec;
float		reflrefr_offset;

bool	FogActiva;
float4	FogColor;
float	FogStart;
float	FogEnd;

bool	noche;

float4	moneda_pos;
float4	isla_pos;

/////////////////////////////	
// DECLARACION DE VERTICES //
/////////////////////////////
struct VS_INPUT 
{
    float4 position	: POSITION;
	float3 normal   : NORMAL;
    float4 uv		: TEXCOORD0;
};

struct VS_STRUCT
{
    float4  position	: POSITION;    
	float	dot			: TEXCOORD0;
	float3	R			: TEXCOORD1;
	float2  refr_tc  	: TEXCOORD2;
	float2  refl_tc  	: TEXCOORD3;
	float3  sunlight	: COLOR0;
	float3	fog			: TEXCOORD4;
	float	moneda		: TEXCOORD5;	
};
	
////////////////////////////	
// TEXTURAS DE HEIGHTMAPS //
////////////////////////////
texture perlinNoise1;
sampler heightmap1 = sampler_state
{
    Texture   = <perlinNoise1>;
    MipFilter = Point;
    MinFilter = Point;
    MagFilter = Point;
    AddressU  = Clamp;
    AddressV  = Clamp;
};

texture perlinNoise2;
sampler heightmap2 = sampler_state
{
    Texture   = <perlinNoise2>;
    MipFilter = Point;
    MinFilter = Point;
    MagFilter = Point;
    AddressU  = Clamp;
    AddressV  = Clamp;
};

///////////////////////////////////
// TEXTURAS PARA ENVIROMENT MAP  //
///////////////////////////////////
texture EnvironmentMap;
samplerCUBE sky = sampler_state
{  
    Texture = <EnvironmentMap>; 
    MipFilter = NONE; 
    MinFilter = LINEAR; 
    MagFilter = LINEAR; 
	AddressU  = WRAP;		
    AddressV  = WRAP;
    AddressW  = WRAP;
};

texture FresnelMap;
sampler fresnel = sampler_state
{  
    Texture = <FresnelMap>; 
    MipFilter = NONE; 
    MinFilter = LINEAR; 
    MagFilter = LINEAR; 
	AddressU  = CLAMP;		
	AddressV  = CLAMP;		
	SRGBTexture = true;	
};

texture Reflectionmap;
sampler reflmap = sampler_state
{  
    Texture = <Reflectionmap>; 
    MipFilter = LINEAR; 
    MinFilter = LINEAR; 
    MagFilter = LINEAR; 
	AddressU  = CLAMP;		
	AddressV  = CLAMP;			
	MipMapLodBias = (LODbias);
};

texture Refractionmap;
sampler refrmap = sampler_state
{  
    Texture = <Refractionmap>; 
    MipFilter = LINEAR; 
    MinFilter = LINEAR; 
    MagFilter = LINEAR; 
	AddressU  = CLAMP;		
	AddressV  = CLAMP;			
	MipMapLodBias = (LODbias);
};

// Genera movimiento del oceano modificando la coordenada Y de los vertices
float3 MovimientoOceano(float x, float z) 
{
	//float atenuacion = length(float2(x,z)-isla_pos.xz);
	//atenuacion = lerp(1,1-(1/atenuacion+1)+0.1,(1/atenuacion+1)*0.1);
		
	float y = 1;
	
	// calculo coordenadas de textura
	float u = (x/dev + radio) / (largo);	
	float v = (z/dev + radio) / (largo);
	
	// calculo de la onda (movimiento grande)
	float ola = sin(u * 2 * 3.14159 * frecuencia + desplazamiento) * cos(v * 2 * 3.14159 * frecuencia + desplazamiento);
	y = y * ola * amplitud; //se lo aplicamos al eje y
	
	// interpolacion entre los dos heightmaps (movimiento superficial pequeño)	
	// busco la altura de cada heightmap usando coordenadas de textura 
	float height1 = tex2Dlod( heightmap1, float4(u,v,0,0)).r;
	float height2 = tex2Dlod( heightmap2, float4(u,v,0,0)).r;
	
	// interpolo entre ambas alturas
    y = y + lerp(height1, height2, alpha) * maxHeightSuperficial;	
		
	// devolvemos la nueva posicion del vertice
	return float3(x,y,z);
}

// Genera una normal aproximada para un vertice dado
float3 GenerarNormal(float3 pos)
{
	// valor de offset para buscar al vecino mas cercano para el calculo de tangente y bitangente
	//float smallvalue = 1;
	
	// busco un punto cercano con desplazamiento en x
	float3 neighbour1 = MovimientoOceano(pos.x + smallvalue, pos.z);
	
	// busco un punto cercano con desplazamiento en z
	float3 neighbour2 = MovimientoOceano(pos.x, pos.z+smallvalue);
	
	// tomo la tangetne como vector de la posicion al primer vecino	
	float3 tangent = neighbour1 - pos;
	
	// tomo la bitangetne como vector de la posicion al segundo vecino	
	float3 bitangent = neighbour2 - pos;
	
	// producto vectorial de tangente y bitangente da vector ortogonal a los dos anteriores
	// normal al plano que contienen a la tangente y bitangetne
	return cross(bitangent, tangent);
}

VS_STRUCT VertexShader(VS_INPUT In)
{
    VS_STRUCT Out = (VS_STRUCT)0;
    float4x4 viewProj = mul(view, proj);
    float4x4 worldViewProj= mul(world, viewProj);
    
	// altero la posicion para generar movimiento de oceano   
	float3 pos = MovimientoOceano(In.position.x, In.position.z);
	In.position = float4(pos,1);
	
	// transformo posicion de entrada a posicion de salida
	Out.position = mul(In.position, worldViewProj);  
	
	// calculo de la normal del vertice
	In.normal = GenerarNormal(In.position.xyz);
	
	float3 normal = normalize(In.normal);	
	
	// Enviroment mapping //
	
	// vector de incidencia
	float3 v = normalize(In.position.xyz - view_position.xyz);
	
	// vector reflejado
	Out.R = reflect(v,normal);
	
	// lookup para el coeficiente de fresnel
	Out.dot = dot(Out.R,normal);

	// specular light del sol (Phong shading)
	float sunlight = pow(saturate(dot(Out.R, sun_vec)),sun_shininess);
	Out.sunlight = sunlight*float3(1.2, 0.4, 0.1);
	
	// si es de noche cambio el reflejo del sol por el de la luna
	if(noche)	
		Out.sunlight = sunlight*float3(0.3, 0.4, 0.5);

	// reflexion y refraccion
	float4 tpos = mul(float4(In.position.x,0,In.position.z,1), worldViewProj);
	tpos.xyz = tpos.xyz/tpos.w;
	tpos.xy = 0.5 + 0.5*tpos.xy*float2(1,-1);
	tpos.z = reflrefr_offset/tpos.z;
	
	Out.refr_tc = tpos.xy - tpos.z*normal.xz;
	Out.refl_tc = tpos.xy - tpos.z*normal.xz;

	// niebla 
    float d = length(Out.position);    
    Out.fog = saturate((d - FogStart) / (FogEnd - FogStart));
    
	// area de efecto del brillo de la moneda, lo uso como peso para interpolar
    Out.moneda = 10/length(mul(In.position - moneda_pos, worldViewProj).xyz);  
        
    return Out;
}


float4 PixelShader(VS_STRUCT In) : COLOR
{
    float4 ut;
	ut.a = 1;
	
	// lookup del coeficiente de fresnel
	float f = tex1D(fresnel, In.dot);	
	
	// reflexion global del skybox		
    float3 global_refl = texCUBE(sky,In.R)  + In.sunlight.rgb*sun_strength;	
    
    // lookup en la textura de relexion local
    float4 local_refl = tex2D(reflmap, In.refl_tc);
    
    // lookup en la textura de refraccion
    float3 refr = tex2D(refrmap, In.refr_tc);
    
    // interpolacion entre reflexion local y global
	float3 refl = lerp( global_refl, local_refl.rgb, local_refl.a);
	
	// resultado final, interpolacion de reflexion y refraccion segun el coeficiente de fresnel
	ut.rgb = lerp( refr, refl, f);
	
	// aplico niebla
	if(FogActiva)
		ut.rgb = lerp(ut.rgb, FogColor.xyz, In.fog);
	
	// agrego brillo de moenda sobre la superficie
	ut.rgb = lerp(ut.rgb,float3(5,5,0),In.moneda);	
	
    return ut;
}

technique PerlinNoise
{
    pass P0
    {
		pixelShader = compile ps_2_0 PixelShader();
        vertexShader = compile vs_3_0 VertexShader();		
    }
}