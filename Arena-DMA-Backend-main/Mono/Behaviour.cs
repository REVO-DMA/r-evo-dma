using arena_dma_backend.Arena;

namespace arena_dma_backend.Mono
{
    public readonly struct Behaviour
    {
        public static implicit operator ulong(Behaviour x) => x.Base;
        private readonly ulong Base;

        public Behaviour(ulong baseAddress)
        {
            Base = baseAddress;
        }

        public readonly bool GetState() => Memory.ReadValue<bool>(this + UnityOffsets.Behaviour.IsEnabled);

        public readonly bool SetState(bool newState) => NativeHelper.SetBehaviorState(this, newState);
    }
}
