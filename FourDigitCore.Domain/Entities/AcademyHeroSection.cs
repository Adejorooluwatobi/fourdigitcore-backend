using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class AcademyHeroSection : BaseEntity
{
    public string BadgeText { get; set; } = "01 // SYSTEMS ENGINEERING ACADEMY";
    public string Headline { get; set; } = "Master the art of bare-metal invariant computing.";
    public string Subheadline { get; set; } = "An intensive systems curriculum covering lock-free concurrency, direct-to-metal eBPF kernel instrumentation, and deterministic distributed state.";
    public string HeroImageUrl { get; set; } = "assets/img/academy-hero.jpg";
    public string LabStatusPill { get; set; } = "● SYSTEMS LAB // IN SESSION";
    public string Phase1Telemetry { get; set; } = "CACHE / KERNEL";
    public string Phase2Telemetry { get; set; } = "CONCURRENCY / SEC";
    public string CapstoneTelemetry { get; set; } = "6174 EQUILIBRIUM";
    public string LabRigTelemetry { get; set; } = "BARE-METAL";
}
