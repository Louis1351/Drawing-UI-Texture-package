Shader "Custom Shader/Draw Texture 0"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        //0 None
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "CustomDefine.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

          
            float2 _Position0;
            float2 _Position1;

            float _brushSize;
            float _opacity;
            int _drawOnTransparency;

            fixed4 _brushColor;

            sampler2D _MainTex;
            sampler2D _BrushTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 brush;
                
                fixed4 col = tex2D(_MainTex, i.uv); 
                
                float2 vec = _Position1 - _Position0;
                float2 nvec = normalize(vec);
                float dist = length(vec);

                float2 newpos;
                float pattern = 0.0f;
                float diffusion = max(0.001f,dist*0.001f);

                brush = tex2D(_BrushTex, (i.uv -_Position0 )*_brushSize + CENTER); 
                pattern = pattern + (1-brush); 

                for (float step = 0.0f; step <= dist; step += diffusion)
                {
                    newpos = _Position0 + (nvec * step);

                    brush = tex2D(_BrushTex, (i.uv -newpos )*_brushSize + CENTER); 

                    pattern = pattern + (1-brush);     
                }
                pattern*=_opacity;
                pattern = clamp(pattern, 0.0f, 1.0f);

                col = lerp(lerp(col, _brushColor, pattern), lerp(col, _brushColor, pattern*col.a), _drawOnTransparency);
                
                return  col;
            }
            ENDCG
        }
        //1 Eraser
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "CustomDefine.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

          
            float2 _Position0;
            float2 _Position1;

            float _brushSize;
            float _opacity;
            int _drawOnTransparency;

            sampler2D _MainTex;
            sampler2D _InitTex;
            sampler2D _BrushTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 initColor = tex2D(_InitTex, i.uv);
                fixed4 brushColor = initColor;
                fixed4 brush;
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 vec = _Position1 - _Position0;
                float2 nvec = normalize(vec);
                float dist = length(vec);

                float2 newpos;
                float pattern = 0.0f;
                float diffusion = max(0.001f,dist*0.001f);

                brush = tex2D(_BrushTex, (i.uv -_Position0 )*_brushSize + CENTER); 
                pattern = pattern + (1-brush); 

                for (float step = 0.0f; step <= dist; step += diffusion)
                {
                    newpos = _Position0 + (nvec * step);

                    brush = tex2D(_BrushTex, (i.uv -newpos )*_brushSize + CENTER); 

                    pattern = pattern + (1-brush);     
                }
                pattern*=_opacity;
                pattern = clamp(pattern, 0.0f, 1.0f);

                col = lerp(lerp(col, brushColor, pattern), lerp(col, brushColor, pattern*col.a), _drawOnTransparency);
                return col;
            }
            ENDCG
        }
        //2 Negative
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"  
            #include "CustomDefine.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

          
            float2 _Position0;
            float2 _Position1;

            float _brushSize;
        
            float _opacity;
            int _drawOnTransparency;

            fixed4 _brushColor;

            sampler2D _MainTex;
            sampler2D _InitTex;
            sampler2D _BrushTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 initColor = tex2D(_InitTex, i.uv);
                fixed4 brushColor = fixed4((1.0f - initColor.rgb),1.0f);
                fixed4 brush; 
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 vec = _Position1 - _Position0;
                float2 nvec = normalize(vec);
                float dist = length(vec);

                float2 newpos;
                float pattern = 0.0f;
                float diffusion = max(0.001f,dist*0.001f);

                brush = tex2D(_BrushTex, (i.uv -_Position0 )*_brushSize + CENTER); 
                pattern = pattern + (1-brush); 

                for (float step = 0.0f; step <= dist; step += diffusion)
                {
                    newpos = _Position0 + (nvec * step);

                    brush = tex2D(_BrushTex, (i.uv -newpos )*_brushSize + CENTER); 

                    pattern = pattern + (1-brush);     
                }
                pattern*=_opacity;
                pattern = clamp(pattern, 0.0f, 1.0f);

                col = lerp(lerp(col, brushColor, pattern), lerp(col, brushColor, pattern*col.a), _drawOnTransparency);
                return col;
            }
            ENDCG
        }
        //3 Additive
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"  
            #include "CustomDefine.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

          
            float2 _Position0;
            float2 _Position1;

            float _brushSize;
    
            float _opacity;
            int _drawOnTransparency;

            fixed4 _brushColor;

            sampler2D _MainTex;
            sampler2D _InitTex;
            sampler2D _BrushTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 brush; 
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 vec = _Position1 - _Position0;
                float2 nvec = normalize(vec);
                float dist = length(vec);

                float2 newpos;
                float pattern = 0.0f;
                float diffusion = max(0.001f,dist*0.001f);

                brush = tex2D(_BrushTex, (i.uv -_Position0 )*_brushSize + CENTER); 
                pattern = pattern + (1-brush); 

                for (float step = 0.0f; step <= dist; step += diffusion)
                {
                    newpos = _Position0 + (nvec * step);

                    brush = tex2D(_BrushTex, (i.uv -newpos )*_brushSize + CENTER); 

                    pattern = pattern + (1-brush);     
                }
                pattern*=_opacity;
                pattern = clamp(pattern, 0.0f, 1.0f);

                col = lerp(clamp(col + _brushColor * pattern, 0.0f, 1.0f), clamp(col + _brushColor * pattern*col.a, 0.0f, 1.0f), _drawOnTransparency);
                return col;
            }
            ENDCG
        }
        //4 Multiply
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"  
            #include "CustomDefine.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

          
            float2 _Position0;
            float2 _Position1;

            float _brushSize;
          
            float _opacity;
            int _drawOnTransparency;

            fixed4 _brushColor;

            sampler2D _MainTex;
            sampler2D _InitTex;
            sampler2D _BrushTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 initColor = tex2D(_InitTex, i.uv);
                fixed4 brush; 
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 vec = _Position1 - _Position0;
                float2 nvec = normalize(vec);
                float dist = length(vec);

                float2 newpos;
                float pattern = 0.0f;
                float diffusion = max(0.001f,dist*0.001f);

                brush = tex2D(_BrushTex, (i.uv -_Position0 )*_brushSize + CENTER); 
                pattern = pattern + (1-brush); 

                for (float step = 0.0f; step <= dist; step += diffusion)
                {
                    newpos = _Position0 + (nvec * step);

                    brush = tex2D(_BrushTex, (i.uv -newpos )*_brushSize + CENTER); 

                    pattern = pattern + (1-brush);     
                }
                pattern*=_opacity;
                pattern = clamp(pattern, 0.0f, 1.0f);

                col = lerp(lerp(col, _brushColor*col, pattern), lerp(col, _brushColor*col, pattern*col.a), _drawOnTransparency);
                return col;
            }
            ENDCG
        }
        //5 Subtract Alpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"  
            #include "CustomDefine.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

          
            float2 _Position0;
            float2 _Position1;

            float _brushSize;
          
            float _opacity;

            sampler2D _MainTex;
            sampler2D _InitTex;
            sampler2D _BrushTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 brush; 
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 vec = _Position1 - _Position0;
                float2 nvec = normalize(vec);
                float dist = length(vec);

                float2 newpos;
                float pattern = 0.0f;
                float diffusion = max(0.001f,dist*0.001f);

                brush = tex2D(_BrushTex, (i.uv -_Position0 )*_brushSize + CENTER); 
                pattern = pattern + (1-brush); 

                for (float step = 0.0f; step <= dist; step += diffusion)
                {
                    newpos = _Position0 + (nvec * step);

                    brush = tex2D(_BrushTex, (i.uv -newpos )*_brushSize + CENTER); 

                    pattern = pattern + (1-brush);     
                }
                pattern*=_opacity;
                pattern = clamp(pattern, 0.0f, 1.0f);

                col.a =  clamp(col.a - pattern, 0.0f, 1.0f);
                return col;
            }
            ENDCG
        }
         //6 Add Alpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"  
            #include "CustomDefine.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

          
            float2 _Position0;
            float2 _Position1;

            float _brushSize;
          
            float _opacity;

            sampler2D _MainTex;
            sampler2D _InitTex;
            sampler2D _BrushTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 brush; 
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 vec = _Position1 - _Position0;
                float2 nvec = normalize(vec);
                float dist = length(vec);

                float2 newpos;
                float pattern = 0.0f;
                float diffusion = max(0.001f,dist*0.001f);

                brush = tex2D(_BrushTex, (i.uv -_Position0 )*_brushSize + CENTER); 
                pattern = pattern + (1-brush); 

                for (float step = 0.0f; step <= dist; step += diffusion)
                {
                    newpos = _Position0 + (nvec * step);

                    brush = tex2D(_BrushTex, (i.uv -newpos )*_brushSize + CENTER); 

                    pattern = pattern + (1-brush);     
                }
                pattern*=_opacity;
                pattern = clamp(pattern, 0.0f, 1.0f);

                col.a =  clamp(col.a + pattern, 0.0f, 1.0f);
                return col;
            }
            ENDCG
        }
    }
}
