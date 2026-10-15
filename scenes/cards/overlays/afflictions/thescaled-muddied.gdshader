shader_type canvas_item;

// Uniforms to customize the wobble behavior directly from the inspector
uniform float speed : hint_range(0.0, 10.0) = 3.0;      // How fast the wobble oscillates
uniform float frequency : hint_range(0.0, 50.0) = 15.0; // How many waves ripple down the image
uniform float intensity : hint_range(0.0, 0.1) = 0.008; // How far left and right it shifts

// Parameterized color uniform with a color picker hint
uniform vec4 tint_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
	// Create a horizontal wobble effect based on the vertical coordinate (UV.y) and TIME
	vec2 wobbled_uv = UV;
	wobbled_uv.x += sin(UV.y * frequency + TIME * speed) * intensity;
	
	// Sample the texture using the modified UV coordinates
	vec4 tex_color = texture(TEXTURE, wobbled_uv);
	
	// Multiply the sampled texture color by your custom tint color
	COLOR = tex_color * tint_color;
}