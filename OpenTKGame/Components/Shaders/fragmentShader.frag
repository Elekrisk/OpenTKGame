#version 450 core


in vec2 texCoord;
out vec4 color;     // Obligatorisk output-variabel för färg
layout (binding = 0) uniform sampler2D tex;

void main(void)
{
    float shadow = min(texCoord.x, texCoord.y);
    color = vec4(shadow, shadow, shadow, 1) * texture(tex, texCoord);
}