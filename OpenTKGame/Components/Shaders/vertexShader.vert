#version 450 core

layout (location = 1) in vec4 position; // Sätts av RenderObject
layout (location = 2) in vec2 texcoord;    // Sätts också av RenderObject
out vec2 texCoord;

void main(void)
{
    gl_Position = position;
    texCoord = texcoord;
}