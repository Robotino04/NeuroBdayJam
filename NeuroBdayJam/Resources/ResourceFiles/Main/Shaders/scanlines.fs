#version 330

in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;
uniform vec4 colDiffuse;

out vec4 pixelColor;

float density = 1.3;
float opacityScanline = .8;
float opacityNoise = .2;
float flickering = 0.03;
vec2 resolution = vec2(1920, 1080);

uniform float time;

float random (vec2 st) {
    return fract(sin(dot(st.xy, vec2(12.9898,78.233)))*43758.5453123);
}

float blend(const in float x, const in float y) {
	return (x < 0.5) ? (2.0 * x * y) : (1.0 - 2.0 * (1.0 - x) * (1.0 - y));
}

vec3 blend(const in vec3 x, const in vec3 y, const in float opacity) {
	vec3 z = vec3(blend(x.r, y.r), blend(x.g, y.g), blend(x.b, y.b));
	return z * opacity + x * (1.0 - opacity);
}

void main()
{
    vec2 uv = fragTexCoord/resolution.xy;
    vec4 col = texture(texture0, fragTexCoord);
    
    float count = resolution.y * density;
    vec2 sl = vec2(sin(uv.y * count), cos(uv.y * count));
	vec4 scanlines = vec4(sl.x, sl.y, sl.x, 0.0);

    col += 0.5 * col * scanlines * opacityScanline;
    float tmp = random(uv*time);
    col += col * vec4(tmp, tmp, tmp, 0) * opacityNoise;
    col += col * sin(110.0*time) * flickering;

    pixelColor = vec4(col.r, col.g, col.b, 1.0);
}