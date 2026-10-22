using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class SiteSettings : BaseEntity
{
    public string SiteTitle { get; set; } = "FourDigitCore 6174 // Autonomous Compute & Zero-Trust Systems";
    public string Monogram { get; set; } = "6174";
    public string LogoLightUrl { get; set; } = "assets/img/logo-light.png";
    public string LogoDarkUrl { get; set; } = "assets/img/logo-dark.png";
    public string FaviconUrl { get; set; } = "assets/img/favicon.png";
    public string DefaultTheme { get; set; } = "light";
    public string MetaDescription { get; set; } = "Autonomous compute fabric engineered for high-throughput AI orchestration, distributed clouds, and zero-trust verification engines. Governed by Kaprekar invariance.";
    public string CopyrightText { get; set; } = "© 2026 FourDigitCore Inc. All rights reserved.";
    public string FooterTagline { get; set; } = "Autonomous compute fabric engineered for high-throughput AI orchestration, distributed clouds, and zero-trust verification engines. Governed by Kaprekar invariance.";
    public string KernelStatusBadge { get; set; } = "KERNEL//SEC_LEVEL_0";
    public string LatencyTickerText { get; set; } = "LATENCY: 0.14ms";
    public string ActiveClusterText { get; set; } = "GLOBAL CLUSTER ACTIVE";
    public string InvarianceObservedText { get; set; } = "6174 INVARIANCE OBSERVED";
}
