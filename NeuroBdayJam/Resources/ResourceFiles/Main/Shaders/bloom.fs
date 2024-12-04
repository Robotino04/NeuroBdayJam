#version 330

in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;
uniform vec4 colDiffuse;

out vec4 pixelColor;

vec2 resolution = vec2(1920, 1080);
const float samples = 5.0;          // Pixels per axis; higher = bigger glow, worse performance
const float quality = 10.0;          // Defines size factor: Lower = smaller glow, better quality

uniform float time;

void main()
{
    vec4 sum = vec4(0);
    vec2 sizeFactor = vec2(1)/resolution*quality;

    vec4 col = texture(texture0, fragTexCoord);

    if (col.r != 0.0 || col.g != 0.0 || col.b != 0.0)
    {
        col = vec4(col.r, col.g, col.b, 1.0);
    }

    const int range = 2;            // should be = (samples - 1)/2;

    for (int x = -range; x <= range; x++)
    {
        for (int y = -range; y <= range; y++)
        {
            sum += texture(texture0, fragTexCoord + vec2(x, y)*sizeFactor);
        }
    }

    pixelColor = ((sum/(samples*samples)) + col)*colDiffuse;
}