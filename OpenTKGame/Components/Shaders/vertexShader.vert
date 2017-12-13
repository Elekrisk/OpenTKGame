#version 450 core

layout (location = 1) in vec4 position; // Sätts av RenderObject
layout (location = 2) in vec4 color;    // Sätts också av RenderObject
out vec4 frag_color;    // Output-variabel till FragmentShader

void main(void)
{
    gl_Position = position;
    frag_color = color;
}