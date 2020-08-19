# unity-ragdoll-culler
A simple component that makes culling ragdolls possible


## How it works
Since skinned meshes bounds do not recalculate during runtime, this means when a ragdoll flies around, it may escape its precalculated bounds, and thus will be culled when undesired. The simplest solution is to merely disable the culling via the "update when offscreen" flag, but this means skinned meshes, an already expensive ordeal, will always render. Yuck!

This solution is simple - it scans all rigidbodies in its children, and detects if they are sleeping or not. Once all rigidbodies fall asleep, the skinned mesh's bounds are recalculated, and then culling is enabled. Once any rigidbodies wake up, culling is then disabled. 
