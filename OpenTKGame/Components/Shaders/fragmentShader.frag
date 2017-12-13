#version 450 core


in vec4 frag_color; // tar emot variabel från VertexShader
out vec4 color;     // Obligatorisk output-variabel för färg

void main(void)
{
    color = frag_color;
}