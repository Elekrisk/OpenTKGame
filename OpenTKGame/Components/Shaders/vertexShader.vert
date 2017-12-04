#version 440 core

uniform float time;
out vec4 frag_color;

void main(void)
{
    gl_Position = vec4(0.25, -0.25, 0.5, 1.0);
    frag_color = vec4(sin(time) * 0.5 + 0.5, cos(time) * 0.5 + 0.5, 0.0, 0.0);
}