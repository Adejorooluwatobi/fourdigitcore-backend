using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class ConvergenceSection : BaseEntity
{
    public string BadgeText { get; set; } = "EXPERIMENTAL BENCHMARK";
    public string Title { get; set; } = "The 6174 Iterative Convergence Proof";
    public string Description { get; set; } = "Every cluster transaction undergoes bounded Kaprekar transformations until it reaches the 6174 attractor equilibrium.";
    public ICollection<ConvergenceStep> Steps { get; set; } = new List<ConvergenceStep>();
}

public class ConvergenceStep : BaseEntity
{
    public Guid SectionId { get; set; }
    public ConvergenceSection? Section { get; set; }
    public string StepNumberText { get; set; } = string.Empty; // "TRANSFORMATION 01", "ATTRACTOR INVARIANT"
    public string FormulaText { get; set; } = string.Empty;    // "5432 - 2345 = 3087", "6174"
    public string? SubFormulaText { get; set; }                // "7641 - 1467 = 6174"
    public string Description { get; set; } = string.Empty;    // "High-entropy ingress sorted by latency priorities."
    public bool IsAttractor { get; set; } = false;
    public int SortOrder { get; set; } = 0;
}
