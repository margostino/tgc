// ---------------------------------------------------------
// Ejemplo shader Minimo:
// ---------------------------------------------------------

float4x4 matWorld;
float4x4 matWorldView;
float4x4 matWorldViewProj;
float4x4 matWorldInverseTranspose;

float time = 0;

// Textura y sampler de textura
texture base_Tex;
texture fire_dist;
texture fire_Op;

sampler2D baseMap =
sampler_state
{
   Texture = (base_Tex);
   MINFILTER = LINEAR;
   MAGFILTER = LINEAR;
   MIPFILTER = LINEAR;
};


sampler2D fire_distortion = 
sampler_state
{
   Texture = (fire_dist);
   MINFILTER = LINEAR;
   MAGFILTER = LINEAR;
   MIPFILTER = LINEAR;
};

sampler2D fire_opacity = 
sampler_state
{
   Texture = (fire_Op);
   MINFILTER = LINEAR;
   MAGFILTER = LINEAR;
   MIPFILTER = LINEAR;
};




//Input del Vertex Shader
struct VS_INPUT 
{
   float4 Position : POSITION0;
   float4 Color : COLOR0;
   float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT
{
   float4 Pos       : POSITION;
   float3 TexCoord0 : TEXCOORD0;
   float3 TexCoord1 : TEXCOORD1;
   float3 TexCoord2 : TEXCOORD2;
   float3 TexCoord3 : TEXCOORD3;
  
};




// Bias and double a value to take it from 0..1 range to -1..1 range
float4 bx2(float x)
{
   return 2.0f * x - 1.0f;
}



float4 ps_main (float4 tc0 : TEXCOORD0, float4 tc1 : TEXCOORD1,
             float4 tc2 : TEXCOORD2, float4 tc3 : TEXCOORD3) : COLOR
{
   // Sample noise map three times with different texture coordinates
   float4 noise0 = tex2D(fire_distortion, tc1);
   float4 noise1 = tex2D(fire_distortion, tc2);
   float4 noise2 = tex2D(fire_distortion, tc3);

   // Weighted sum of signed noise
   float4 noiseSum = bx2(noise0.r) * 0.02 + bx2(noise1.r) * 0.02 + bx2(noise2.r) * 0.02;

   // Perturb base coordinates in direction of noiseSum as function of height (y)
   float4 perturbedBaseCoords = tc0 + noiseSum * (tc0.y * 0.9 + 2.9 );

   // Sample base and opacity maps with perturbed coordinates
   float4 base = tex2D(baseMap, perturbedBaseCoords);
   float4 opacity = tex2D(fire_opacity, perturbedBaseCoords);

   return base *  opacity;
}



VS_OUTPUT vs_main (float4 vPosition: POSITION, float3 vTexCoord0 : TEXCOORD0, VS_INPUT Input)
{
   VS_OUTPUT Out = (VS_OUTPUT) 0; 
   

   // Align quad with the screen
   Out.Pos = float4 (vPosition.x, vPosition.y, 0.0f, 1.0f);

   // Output TexCoord0 directly
   Out.TexCoord0 = vTexCoord0;


   // Base texture coordinates plus scaled time
   Out.TexCoord1.x = vTexCoord0.x;
   Out.TexCoord1.y = vTexCoord0.y + 0.1 * time;

   // Base texture coordinates plus scaled time
   Out.TexCoord2.x = vTexCoord0.x;
   Out.TexCoord2.y = vTexCoord0.y + 0.2 * time;

   // Base texture coordinates plus scaled time
   Out.TexCoord3.x = vTexCoord0.x;
   Out.TexCoord3.y = vTexCoord0.y + 0.3 * time;


   Out.Pos = mul(Input.Position, matWorldViewProj);
   
   //Propago las coordenadas de textura
   //Out.TexCoord0 = Input.Texcoord;


   return Out;
}



// ------------------------------------------------------------------
technique RenderScene
{
   pass Pass_0
   {
	  VertexShader = compile vs_2_0 vs_main();
	  PixelShader = compile ps_2_0 ps_main();
   }

}
