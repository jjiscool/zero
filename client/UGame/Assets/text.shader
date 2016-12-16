Shader "Custom/text" {
Properties {
    _Color ("Main Color", Color) = (1, 1, 1, 0.5)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        Material {
            Diffuse [_Color]
        }


        Lighting Off Cull Off ZTest Always ZWrite Off Fog { Mode Off }

        SetTexture [_MainTex] {
            constantColor [_Color]
            combine texture * constant
        }
    }
}

}
