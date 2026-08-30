shader_type canvas_item;

// Allows you to assign your mud texture in the Inspector
uniform sampler2D mud_texture : repeat_enable;

// Parameters to tweak the wobbling effect in the Inspector
uniform float wave_speed : hint_range(0.1, 10.0) = 2.0;
uniform float wave_frequency : hint_range(1.0, 50.0) = 10.0;
uniform float wave_amplitude : hint_range(0.0, 0.1) = 0.01;

void fragment() {
    // 1. Calculate an offset for the UV coordinates based on sine waves and TIME
    vec2 wobbled_uv = UV;
    wobbled_uv.x += sin(UV.y * wave_frequency + TIME * wave_speed) * wave_amplitude;
    wobbled_uv.y += cos(UV.x * wave_frequency + TIME * wave_speed) * wave_amplitude;

    // 2. Sample the mud texture using the displaced UVs
    vec4 mud_color = texture(mud_texture, wobbled_uv);

    // 3. Get the original card texture pixel
    vec4 card_color = COLOR;

    // 4. Blend the mud over the card using the mud's alpha channel
    COLOR = mix(card_color, mud_color, mud_color.a);
}