shader_type spatial;
render_mode blend_mix;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	ALBEDO = vec3(1.0);
    // Set the transparency (50% opaque)
    ALPHA = sin(TIME * 2.0) / 2.0 + 0.5; 
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
