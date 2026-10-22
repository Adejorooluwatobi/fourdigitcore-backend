using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class SdkCodeSnippet : BaseEntity
{
    public Guid SectionId { get; set; }
    public SdkShowcaseSection? Section { get; set; }
    public Guid? CategoryId { get; set; }
    public SdkCategory? Category { get; set; }
    public string LanguageKey { get; set; } = string.Empty; // "rust", "swift", "python", "go", "ts", "cpp", "c", "cs", "java", "zig"
    public string DisplayName { get; set; } = string.Empty; // "Rust", "Swift", "Python", etc.
    public string? IconName { get; set; }
    public string FileName { get; set; } = string.Empty;    // "cluster_init.rs", "ClusterInit.swift"
    public string BindingTag { get; set; } = string.Empty;  // "— immutable kernel binding"
    public string CodeContent { get; set; } = string.Empty; // Syntax highlighted raw code text
    public string PackageTag { get; set; } = string.Empty;  // "CRATE: fourdigitcore-v4.8", "SPM: fourdigitcore-swift"
    public string MemoryTag { get; set; } = string.Empty;   // "MEM: < 3.8 MB"
    public string ThreadsTag { get; set; } = string.Empty;  // "THREADS: LOCK_FREE"
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
