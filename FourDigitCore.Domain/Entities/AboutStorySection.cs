using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class AboutStorySection : BaseEntity
{
    public string BadgeText { get; set; } = "THE INVARIANCE STORY";
    public string Title { get; set; } = "Why 6174?";
    public string Paragraph1 { get; set; } = "In 1949, mathematician D.R. Kaprekar discovered that any four-digit number with at least two distinct digits converges deterministically to 6174 in at most 7 routine transformations.";
    public string Paragraph2 { get; set; } = "In distributed systems, requests arrive with high entropy—varying latencies, unaligned tensors, and chaotic concurrency. FourDigitCore maps the Kaprekar iterative transformation to distributed kernel scheduling, guaranteeing that any workload reaches equilibrium within bounded iterations.";
}
