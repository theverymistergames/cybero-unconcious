//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

void GetRipples_float(
    const float2 uv,
    const float t,
    const float multiplier,
    const float max_frequency,
    const float speed,
    out float output
) {
    const float l = length(uv);
    const float inverted_radial_gradient = l * l;

    output = sin(t * speed - inverted_radial_gradient * max_frequency) * multiplier;
}

#endif //MYHLSLINCLUDE_INCLUDED
