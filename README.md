# unity-ragdoll-culler
A simple component that makes culling ragdoll's skinned meshes possible, as well as very performant! When ragdolls stop moving, they become a normal mesh, until they start moving again. 


## How it works
Since skinned meshes bounds do not recalculate during runtime, this means when a ragdoll flies around, it may escape its precalculated bounds, and thus will be culled when undesired. The simplest solution is to merely disable the culling via the "update when offscreen" flag, but this means skinned meshes, an already expensive ordeal, will always render. Yuck!

Originally, I built a solution that scans for changes in rigidbodies, then recalculates the skinned meshes's bounds when needed, toggling the culling. Then I realized there is a far better solution: baking the skinned mesh when all rigidbodies sleep, then switching to skinned mesh renderer when rigidbodies wake up! This is WAY better - it does mean a mesh has to be baked once the ragdoll sleeps, but that is such a small thing compared to the massive gain that is switching to a simple mesh. I LOVE IT!
