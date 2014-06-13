texture rendertarget;
sampler RenderSampler = sampler_state
{
    Texture = (rendertarget);
    MipFilter = NONE;
    MinFilter = NONE;
    MagFilter = NONE;
};

float4		watercolour;

//Input del Vertex Shader
struct VS_INPUT
{
   float4 position : POSITION0;
   float2 uv : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT
{
	float4 position : POSITION0;
	float2 uv : TEXCOORD0;
};

VS_OUTPUT VertexShader(VS_INPUT In) 
{ 
   VS_OUTPUT Out;
   Out.position = In.position; 
   Out.uv = In.uv; 
   return Out; 
} 

float4 PixelShader(float2 Tex : TEXCOORD0) : COLOR0 
{ 
	float2 TexCentro = {0.5, 0.5};
    float Samples   = 16;

    float4 color = 0;
   
    for(int i = 0; i < Samples; i++)
    {
       float scale = 0.9 +-0.07 *(i / (Samples - 1));
       color      += tex2D(RenderSampler, (Tex - 0.5) * scale + TexCentro );
    }
    color /= Samples;

    return lerp(color,watercolour,0.3);
} 

technique PostProcess 
{ 
   pass Pass_0 
   { 
      VertexShader = compile vs_1_1 VertexShader(); 
      PixelShader = compile ps_2_0 PixelShader(); 
   } 
}