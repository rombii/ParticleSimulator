#version 460 core
in vec3 vertColor;

out vec4 FragColor;

uniform float uniOpacity;

void main() {
    FragColor = vec4(vertColor.x * uniOpacity, vertColor.y * uniOpacity, vertColor.z * uniOpacity, 1.0f); 
}