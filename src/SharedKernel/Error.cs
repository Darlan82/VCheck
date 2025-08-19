namespace VCheck.SharedKernel
{
    public sealed record Error(string Code, string? Description = null)
    {
        public static Error None = new(string.Empty);
    }
}
