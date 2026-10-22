using Microsoft.EntityFrameworkCore;
using FourDigitCore.Domain.Entities;
using FourDigitCore.Domain.Enums;
using FourDigitCore.Infrastructure.Data;

namespace FourDigitCore.Infrastructure.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // 1. Admin Users & Roles
        if (!await context.AdminUsers.AnyAsync())
        {
            var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "Adejorotgold1@yahoo.com";
            var adminName = Environment.GetEnvironmentVariable("ADMIN_FULLNAME") ?? "Oluwatobi Adejoro";
            var adminPass = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "AdminPassword123!";

            var adminUser = new AdminUser
            {
                Email = adminEmail.Trim(),
                FullName = adminName.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPass, workFactor: 11),
                IsActive = true,
                IsSuperAdmin = true
            };
            await context.AdminUsers.AddAsync(adminUser);

            var superAdminRole = new Role
            {
                Name = "SUPERADMIN",
                Description = "Unrestricted administrative control plane access."
            };
            await context.Roles.AddAsync(superAdminRole);

            await context.UserRoles.AddAsync(new UserRole
            {
                User = adminUser,
                Role = superAdminRole
            });
        }

        // 2. Site Settings
        if (!await context.SiteSettings.AnyAsync())
        {
            await context.SiteSettings.AddAsync(new SiteSettings
            {
                SiteTitle = "FourDigitCore 6174 // Autonomous Compute & Zero-Trust Systems",
                Monogram = "6174",
                LogoLightUrl = "assets/img/logo-light.png",
                LogoDarkUrl = "assets/img/logo-dark.png",
                FaviconUrl = "assets/img/favicon.png",
                DefaultTheme = "light",
                MetaDescription = "Autonomous compute fabric engineered for high-throughput AI orchestration, distributed clouds, and zero-trust verification engines. Governed by Kaprekar invariance.",
                CopyrightText = "© 2026 FourDigitCore Inc. All rights reserved.",
                FooterTagline = "Autonomous compute fabric engineered for high-throughput AI orchestration, distributed clouds, and zero-trust verification engines. Governed by Kaprekar invariance.",
                KernelStatusBadge = "KERNEL//SEC_LEVEL_0",
                LatencyTickerText = "LATENCY: 0.14ms",
                ActiveClusterText = "GLOBAL CLUSTER ACTIVE",
                InvarianceObservedText = "6174 INVARIANCE OBSERVED"
            });
        }

        // 3. Navigation Items
        if (!await context.NavItems.AnyAsync())
        {
            await context.NavItems.AddRangeAsync(
                new NavItem { Label = "Platform", Url = "index.html", DataPage = "index.html", SortOrder = 1 },
                new NavItem { Label = "Solutions", Url = "solutions.html", DataPage = "solutions.html", SortOrder = 2 },
                new NavItem { Label = "Company", Url = "about.html", DataPage = "about.html", SortOrder = 3 },
                new NavItem { Label = "Innovation", Url = "innovation.html", DataPage = "innovation.html", SortOrder = 4 },
                new NavItem { Label = "Academy", Url = "academy.html", DataPage = "academy.html", SortOrder = 5 },
                new NavItem { Label = "Careers", Url = "careers.html", DataPage = "careers.html", SortOrder = 6 }
            );
        }

        // 4. Footer Columns & Links
        if (!await context.FooterColumns.AnyAsync())
        {
            var colPlatform = new FooterColumn { Title = "Platform", SortOrder = 1 };
            colPlatform.Links.Add(new FooterLink { Label = "AI Matrix Kernels", Url = "solutions.html#ai", SortOrder = 1 });
            colPlatform.Links.Add(new FooterLink { Label = "Autonomous Cloud Mesh", Url = "solutions.html#software", SortOrder = 2 });
            colPlatform.Links.Add(new FooterLink { Label = "Zero-Trust Enclaves", Url = "solutions.html#hardware", SortOrder = 3 });
            colPlatform.Links.Add(new FooterLink { Label = "Connected Edge State", Url = "solutions.html#connected", SortOrder = 4 });

            var colResearch = new FooterColumn { Title = "Research & Docs", SortOrder = 2 };
            colResearch.Links.Add(new FooterLink { Label = "Kaprekar Proofs", Url = "innovation.html", SortOrder = 1 });
            colResearch.Links.Add(new FooterLink { Label = "Systems Academy", Url = "academy.html", SortOrder = 2 });
            colResearch.Links.Add(new FooterLink { Label = "SDK API Specs", Url = "solutions.html", SortOrder = 3 });
            colResearch.Links.Add(new FooterLink { Label = "Architecture Whitepaper", Url = "about.html", SortOrder = 4 });

            var colCompany = new FooterColumn { Title = "Company", SortOrder = 3 };
            colCompany.Links.Add(new FooterLink { Label = "About Team", Url = "about.html", SortOrder = 1 });
            colCompany.Links.Add(new FooterLink { Label = "Careers & Tracks", Url = "careers.html", SortOrder = 2 });
            colCompany.Links.Add(new FooterLink { Label = "Contact Engineering", Url = "contact.html", SortOrder = 3 });
            colCompany.Links.Add(new FooterLink { Label = "Partnerships", Url = "contact.html?intent=partner", SortOrder = 4 });

            await context.FooterColumns.AddRangeAsync(colPlatform, colResearch, colCompany);
        }

        // 5. Page Settings
        if (!await context.PageSettings.AnyAsync())
        {
            await context.PageSettings.AddRangeAsync(
                new PageSettings { PageKey = "index", SeoTitle = "FourDigitCore 6174 // Autonomous Compute & Zero-Trust Systems", SeoDescription = "Direct-to-metal agentic execution graphs." },
                new PageSettings { PageKey = "solutions", SeoTitle = "Solutions & Architectural Disciplines // FourDigitCore 6174", SeoDescription = "Technology engineered around deterministic scale." },
                new PageSettings { PageKey = "about", SeoTitle = "Philosophy & Mathematical Invariance // FourDigitCore 6174", SeoDescription = "Founded on the principle of Kaprekar Invariance." },
                new PageSettings { PageKey = "innovation", SeoTitle = "Research Labs & TSMC Photonic Tape-Outs // FourDigitCore 6174", SeoDescription = "Where invariant mathematics meets physical silicon." },
                new PageSettings { PageKey = "academy", SeoTitle = "Systems Academy & Fellowships // FourDigitCore 6174", SeoDescription = "Master the art of bare-metal invariant computing." },
                new PageSettings { PageKey = "careers", SeoTitle = "Careers & Systems Engineering Tracks // FourDigitCore 6174", SeoDescription = "Build the foundation of tomorrow's compute fabric." },
                new PageSettings { PageKey = "contact", SeoTitle = "Architectural Briefing Console // FourDigitCore 6174", SeoDescription = "Launch deterministic systems with FourDigitCore." }
            );
        }

        // 6. Home Hero Section & Pillars
        if (!await context.HomeHeroSections.AnyAsync())
        {
            var homeHero = new HomeHeroSection
            {
                BadgeText = "COMPUTATIONAL INVARIANCE // 6174",
                Headline = "Architecting Mathematical Determinism at Infinite Scale.",
                Subheadline = "Direct-to-metal agentic execution graphs, sub-millisecond tensor streaming, and self-stabilizing cloud clusters governed by Kaprekar invariance.",
                PrimaryCtaText = "Deploy Architecture →",
                PrimaryCtaUrl = "contact.html",
                SecondaryCtaText = "Inspect Kernels",
                SecondaryCtaUrl = "#strata",
                LatencyCeilingValue = "< 0.18ms",
                LatencyCeilingLabel = "LATENCY CEILING",
                StateModelValue = "Kaprekar 6174",
                StateModelLabel = "STATE MODEL",
                EnclaveSecurityValue = "FIPS 140-3 L4",
                EnclaveSecurityLabel = "ENCLAVE SECURITY"
            };
            homeHero.Slides.Add(new HomeHeroSlide { ImageUrl = "assets/img/hero-chip.jpg", AltText = "Silicon Photonic Accelerator", SortOrder = 1 });
            homeHero.Slides.Add(new HomeHeroSlide { ImageUrl = "assets/img/hero-workstation.jpg", AltText = "Kernel Engineering Console", SortOrder = 2 });
            await context.HomeHeroSections.AddAsync(homeHero);

            await context.HomeQuoteSections.AddAsync(new HomeQuoteSection
            {
                QuoteText = "Software engineering is not about accumulating complexity; it is about finding the invariant core that remains true across infinite scale.",
                Attribution = "KAPREKAR 6174 PRINCIPLE"
            });

            var p1 = new PlatformPillar { LayerNumber = "01", BadgeText = "HARDWARE ENCLAVES", Title = "Silicon-Rooted Enclaves", Description = "Hardware-attested cryptographic isolation running on AMD SEV-SNP and AWS Nitro chips.", Icon = "security", SortOrder = 1 };
            p1.Specs.Add(new PlatformPillarSpec { SpecText = "100% eBPF Isolation", SortOrder = 1 });
            p1.Specs.Add(new PlatformPillarSpec { SpecText = "FIPS 140-3 Level 4", SortOrder = 2 });
            p1.Applications.Add(new PlatformPillarApplication { ApplicationName = "Cryptographic Key Stores", SortOrder = 1 });

            var p2 = new PlatformPillar { LayerNumber = "02", BadgeText = "KERNEL INFRASTRUCTURE", Title = "Ring 0 Kernel Mesh", Description = "Lock-free ring-buffer state synthesis operating across distributed cloud regions.", Icon = "hub", SortOrder = 2 };
            p2.Specs.Add(new PlatformPillarSpec { SpecText = "< 1.2ms Replication Lag", SortOrder = 1 });
            p2.Specs.Add(new PlatformPillarSpec { SpecText = "GitOps Immutability", SortOrder = 2 });
            p2.Applications.Add(new PlatformPillarApplication { ApplicationName = "Global Microservice Mesh", SortOrder = 1 });

            var p3 = new PlatformPillar { LayerNumber = "03", BadgeText = "NEURAL ACCELERATION", Title = "Direct FP8 Tensor Streaming", Description = "Zero-copy tensor streaming direct to accelerators with sub-0.18ms latency.", Icon = "bolt", SortOrder = 3 };
            p3.Specs.Add(new PlatformPillarSpec { SpecText = "FP8 Tensor Streaming", SortOrder = 1 });
            p3.Specs.Add(new PlatformPillarSpec { SpecText = "100k Concurrency", SortOrder = 2 });
            p3.Applications.Add(new PlatformPillarApplication { ApplicationName = "Autonomous Agents", SortOrder = 1 });

            await context.PlatformPillars.AddRangeAsync(p1, p2, p3);

            await context.HomeClosingCallouts.AddAsync(new HomeClosingCallout
            {
                BadgeText = "ENTERPRISE DEPLOYMENTS",
                Title = "Have an infrastructure challenge worth solving?",
                Description = "Connect with our kernel infrastructure team to deploy dedicated sovereign clusters, hybrid-enclave meshes, or on-premise appliances.",
                PrimaryCtaText = "Start a Conversation →",
                PrimaryCtaUrl = "contact.html",
                SecondaryCtaText = "Explore Capabilities",
                SecondaryCtaUrl = "solutions.html"
            });
        }

        // 7. Dynamic Native SDK Showcase (8 Initial Languages)
        if (!await context.SdkShowcaseSections.AnyAsync())
        {
            var sdkSection = new SdkShowcaseSection
            {
                SectionKey = "home_sdk",
                BadgeText = "04 // DEVELOPER ECOSYSTEM",
                Title = "Native SDKs for Ultra-Low Latency",
                Description = "Deploy deterministic cluster connections in under 4 lines of code. Zero unneeded runtime abstractions."
            };

            var catSystems = new SdkCategory { Name = "Systems & Direct-to-Metal", Slug = "systems", SortOrder = 1 };
            var catManaged = new SdkCategory { Name = "Application Runtimes", Slug = "app-runtimes", SortOrder = 2 };
            await context.SdkCategories.AddRangeAsync(catSystems, catManaged);

            sdkSection.Snippets.Add(new SdkCodeSnippet
            {
                Category = catSystems,
                LanguageKey = "rust",
                DisplayName = "Rust",
                FileName = "cluster_init.rs",
                BindingTag = "— immutable kernel binding",
                PackageTag = "CRATE: fourdigitcore-v4.8",
                MemoryTag = "MEM: < 3.8 MB",
                ThreadsTag = "THREADS: LOCK_FREE",
                SortOrder = 1,
                CodeContent = @"use fourdigitcore::prelude::*;

#[tokio::main]
async fn main() -> Result<(), CoreError> {
    // Establish deterministic invariant connection to nearest edge cluster
    let cluster = CoreNode::connect(""mesh.6174.network"")
        .with_neural_pipeline(Quantization::FP8)
        .with_zero_trust(Enclave::Nitro)
        .with_recursion_limit(7) // Kaprekar bound
        .spawn()
        .await?;

    cluster.stream_tensor_batch(&batch_stream).await?;
    println!(""FourDigitCore [6174] Cluster converged and sealed."");
    Ok(())
}"
            });

            sdkSection.Snippets.Add(new SdkCodeSnippet
            {
                Category = catManaged,
                LanguageKey = "python",
                DisplayName = "Python",
                FileName = "cluster_init.py",
                BindingTag = "— async event-loop binding",
                PackageTag = "PYPI: fourdigitcore-py v4.8",
                MemoryTag = "MEM: < 12.4 MB",
                ThreadsTag = "ASYNCIO: UVLOOP",
                SortOrder = 2,
                CodeContent = @"import asyncio
from fourdigitcore import CoreNode, Quantization, Enclave

async def bootstrap():
    # Establish deterministic invariant connection to nearest edge cluster
    cluster = await CoreNode.connect(
        ""mesh.6174.network"",
        quantization=Quantization.FP8,
        enclave=Enclave.NITRO,
        max_cycles=7 # Kaprekar bound
    ).spawn()
    
    await cluster.stream_tensors(batch_stream)
    print(""FourDigitCore [6174] Invariance established in 0.42ms"")

asyncio.run(bootstrap())"
            });

            sdkSection.Snippets.Add(new SdkCodeSnippet
            {
                Category = catSystems,
                LanguageKey = "go",
                DisplayName = "Go",
                FileName = "cluster_init.go",
                BindingTag = "— goroutine context binding",
                PackageTag = "MODULE: sdk-go v4.8",
                MemoryTag = "MEM: < 5.1 MB",
                ThreadsTag = "GOROUTINES: GOMAXPROCS",
                SortOrder = 3,
                CodeContent = @"package main

import (
    ""context""
    ""fmt""
    ""github.com/fourdigitcore/sdk-go/kernel""
)

func main() {
    ctx := context.Background()
    cluster, err := kernel.Connect(ctx, ""mesh.6174.network"",
        kernel.WithPrecision(kernel.FP8),
        kernel.WithHardwareEnclave(kernel.SEV_SNP),
        kernel.WithKaprekarBound(7),
    )
    if err != nil {
        panic(err)
    }
    defer cluster.Close()

    _ = cluster.StreamTensorBatch(ctx, batchStream)
    fmt.Println(""FourDigitCore [6174] Go routine channel streaming."")
}"
            });

            sdkSection.Snippets.Add(new SdkCodeSnippet
            {
                Category = catManaged,
                LanguageKey = "ts",
                DisplayName = "TypeScript",
                FileName = "cluster_init.ts",
                BindingTag = "— zero-copy buffer binding",
                PackageTag = "NPM: @fourdigitcore/mesh v4.8",
                MemoryTag = "MEM: < 8.2 MB",
                ThreadsTag = "WORKER_THREADS: NAPI",
                SortOrder = 4,
                CodeContent = @"import { CoreNode, Enclave, Quantization } from '@fourdigitcore/mesh';

async function launchDeterministicNode(): Promise<void> {
    const cluster = await CoreNode.connect('mesh.6174.network', {
        quantization: Quantization.FP8,
        enclave: Enclave.Nitro,
        maxRecursionLimit: 7 // Kaprekar invariant limit
    });

    await cluster.streamTensorBatch(batchBuffer);
    console.log('FourDigitCore [6174] Node initialized on V8 isolate.');
}

launchDeterministicNode();"
            });

            sdkSection.Snippets.Add(new SdkCodeSnippet
            {
                Category = catSystems,
                LanguageKey = "cpp",
                DisplayName = "C++",
                FileName = "cluster_init.cpp",
                BindingTag = "— direct C++20 concept binding",
                PackageTag = "VCPKG: fourdigitcore-cpp",
                MemoryTag = "MEM: < 2.1 MB",
                ThreadsTag = "THREADS: STD::JTHREAD",
                SortOrder = 5,
                CodeContent = @"#include <fourdigitcore/mesh.hpp>
#include <iostream>

int main() {
    using namespace fourdigitcore;

    auto cluster = CoreNode::connect(""mesh.6174.network"")
        .with_quantization(quantization::fp8)
        .with_enclave(enclave::nitro)
        .with_bound<7>() // Compile-time Kaprekar invariant
        .spawn();

    cluster.stream_tensor_batch(batch_stream);
    std::cout << ""FourDigitCore [6174] C++ zero-overhead node live.\n"";
    return 0;
}"
            });

            sdkSection.Snippets.Add(new SdkCodeSnippet
            {
                Category = catSystems,
                LanguageKey = "c",
                DisplayName = "C",
                FileName = "cluster_init.c",
                BindingTag = "— pure POSIX ABI socket binding",
                PackageTag = "LIB: libfourdigitcore.so",
                MemoryTag = "MEM: < 1.1 MB",
                ThreadsTag = "PTHREADS: KERNEL_PINNED",
                SortOrder = 6,
                CodeContent = @"#include <fourdigitcore/c_api.h>
#include <stdio.h>

int main(void) {
    fdc_config_t cfg = {
        .endpoint = ""mesh.6174.network"",
        .quantization = FDC_FP8,
        .enclave = FDC_ENCLAVE_NITRO,
        .max_cycles = 7
    };

    fdc_handle_t* cluster = fdc_connect(&cfg);
    if (!cluster) return 1;

    fdc_stream_batch(cluster, &batch_stream);
    printf(""FourDigitCore [6174] Pure C Ring 0 interface initialized.\n"");

    fdc_disconnect(cluster);
    return 0;
}"
            });

            sdkSection.Snippets.Add(new SdkCodeSnippet
            {
                Category = catManaged,
                LanguageKey = "cs",
                DisplayName = "C#",
                FileName = "ClusterInit.cs",
                BindingTag = "— memory marshaling binding",
                PackageTag = "NUGET: FourDigitCore.Mesh v4.8",
                MemoryTag = "MEM: < 6.4 MB",
                ThreadsTag = "RUNTIME: CORECLR",
                SortOrder = 7,
                CodeContent = @"using FourDigitCore.Mesh;

await using var cluster = await CoreNode.ConnectAsync(""mesh.6174.network"", new ClusterOptions
{
    Quantization = Quantization.FP8,
    Enclave = EnclaveType.Nitro,
    MaxRecursionLimit = 7 // Kaprekar bound
});

await cluster.StreamTensorBatchAsync(batchStream);
Console.WriteLine(""FourDigitCore [6174] .NET deterministic pipeline active."");"
            });

            sdkSection.Snippets.Add(new SdkCodeSnippet
            {
                Category = catManaged,
                LanguageKey = "java",
                DisplayName = "Java",
                FileName = "ClusterInit.java",
                BindingTag = "— Project Panama foreign memory binding",
                PackageTag = "MAVEN: com.fourdigitcore:mesh:4.8",
                MemoryTag = "MEM: < 14.8 MB",
                ThreadsTag = "VIRTUAL_THREADS: LOOM",
                SortOrder = 8,
                CodeContent = @"import com.fourdigitcore.mesh.CoreNode;
import com.fourdigitcore.mesh.Enclave;
import com.fourdigitcore.mesh.Quantization;

public class ClusterInit {
    public static void main(String[] args) throws Exception {
        try (var cluster = CoreNode.connect(""mesh.6174.network"")
                .withQuantization(Quantization.FP8)
                .withEnclave(Enclave.NITRO)
                .withRecursionLimit(7) // Kaprekar bound
                .spawn()) {
            cluster.streamTensorBatch(batchStream);
            System.out.println(""FourDigitCore [6174] JVM foreign memory sealed."");
        }
    }
}"
            });

            await context.SdkShowcaseSections.AddAsync(sdkSection);
        }

        // 8. Solutions Page
        if (!await context.SolutionsHeroSections.AnyAsync())
        {
            await context.SolutionsHeroSections.AddAsync(new SolutionsHeroSection
            {
                BadgeText = "01 // SOLUTIONS & ARCHITECTURAL DISCIPLINES",
                Headline = "Technology engineered around deterministic scale.",
                Subheadline = "From direct-to-metal neural kernels to post-quantum zero-trust enclaves and distributed mesh fabrics, we engineer systems that remain invariant under infinite load."
            });

            var d1 = new SolutionDiscipline { DisciplineNumber = "01", CategoryName = "01 // NEURAL RUNTIMES & ACCELERATION", Title = "Autonomous Neural Kernels", Description = "Direct-to-metal model execution bypassing conventional runtime schedulers.", CtaText = "Request Kernel Benchmark →", CtaUrl = "contact.html?intent=ai", SortOrder = 1 };
            d1.Specs.Add(new SolutionDisciplineSpec { SpecText = "FP8 Tensor Streaming", SortOrder = 1 });
            d1.Specs.Add(new SolutionDisciplineSpec { SpecText = "Sub-0.18ms Latency", SortOrder = 2 });
            d1.Applications.Add(new SolutionDisciplineApplication { ApplicationName = "Autonomous Agents", SortOrder = 1 });

            var d2 = new SolutionDiscipline { DisciplineNumber = "02", CategoryName = "02 // CLOUD & DISTRIBUTED STATE", Title = "Autonomous Multi-Region Mesh", Description = "Kubernetes-native orchestration coupled with zero-data-loss failover.", CtaText = "Deploy Cloud Fabric →", CtaUrl = "contact.html?intent=cloud", SortOrder = 2 };
            d2.Specs.Add(new SolutionDisciplineSpec { SpecText = "< 1.2ms Replication Lag", SortOrder = 1 });
            d2.Applications.Add(new SolutionDisciplineApplication { ApplicationName = "Global Microservice Mesh", SortOrder = 1 });

            var d3 = new SolutionDiscipline { DisciplineNumber = "03", CategoryName = "03 // HARDWARE-ENFORCED ZERO-TRUST", Title = "Quantum-Safe Silicon Enclaves", Description = "Real-time vulnerability containment without thread pauses.", CtaText = "Inspect Security Enclaves →", CtaUrl = "contact.html?intent=security", SortOrder = 3 };
            d3.Specs.Add(new SolutionDisciplineSpec { SpecText = "100% eBPF Isolation", SortOrder = 1 });
            d3.Applications.Add(new SolutionDisciplineApplication { ApplicationName = "Cryptographic Hardware Key Stores", SortOrder = 1 });

            await context.SolutionDisciplines.AddRangeAsync(d1, d2, d3);
        }

        // 9. About Page & Architect
        if (!await context.AboutHeroSections.AnyAsync())
        {
            await context.AboutHeroSections.AddAsync(new AboutHeroSection());
            await context.AboutStorySections.AddAsync(new AboutStorySection());
            await context.ArchitectProfiles.AddAsync(new ArchitectProfile());
        }

        // 10. Innovation & Convergence Steps
        if (!await context.ConvergenceSections.AnyAsync())
        {
            await context.InnovationHeroSections.AddAsync(new InnovationHeroSection());

            var conv = new ConvergenceSection();
            conv.Steps.Add(new ConvergenceStep { StepNumberText = "TRANSFORMATION 01", FormulaText = "5432 - 2345 = 3087", Description = "High-entropy ingress sorted by latency priorities.", SortOrder = 1 });
            conv.Steps.Add(new ConvergenceStep { StepNumberText = "TRANSFORMATION 02", FormulaText = "8730 - 0378 = 8352", Description = "Automated re-binning across edge execution threads.", SortOrder = 2 });
            conv.Steps.Add(new ConvergenceStep { StepNumberText = "TRANSFORMATION 03", FormulaText = "8532 - 2358 = 6174", Description = "Equilibrium reached. Cycles stabilize deterministically.", SortOrder = 3 });
            conv.Steps.Add(new ConvergenceStep { StepNumberText = "ATTRACTOR INVARIANT", FormulaText = "6174", SubFormulaText = "7641 - 1467 = 6174", Description = "Attractor invariant reached.", IsAttractor = true, SortOrder = 4 });
            await context.ConvergenceSections.AddAsync(conv);
        }

        // 11. Systems Academy
        if (!await context.AcademyCurriculumSections.AnyAsync())
        {
            await context.AcademyHeroSections.AddAsync(new AcademyHeroSection());

            var curr = new AcademyCurriculumSection();
            curr.Phases.Add(new AcademyPhase { PhaseNumber = "01", PhaseBadge = "01 // DISCOVER", Title = "Computer Architecture", Description = "Memory hierarchies, CPU caches, and register allocation.", SortOrder = 1 });
            curr.Phases.Add(new AcademyPhase { PhaseNumber = "02", PhaseBadge = "02 // LEARN", Title = "Kernel Internals", Description = "eBPF behavioral filters, page tables, and context switching.", SortOrder = 2 });
            curr.Phases.Add(new AcademyPhase { PhaseNumber = "03", PhaseBadge = "03 // BUILD", Title = "Lock-Free Structures", Description = "Atomic primitives, CAS loops, and ring-buffer streaming.", SortOrder = 3 });
            curr.Phases.Add(new AcademyPhase { PhaseNumber = "04", PhaseBadge = "04 // TEST", Title = "Fault Injection", Description = "Jepsen testing, chaos engineering, and network partitions.", SortOrder = 4 });
            curr.Phases.Add(new AcademyPhase { PhaseNumber = "05", PhaseBadge = "05 // DEMO", Title = "Sovereign Cluster", Description = "Deploy multi-node invariant clusters live on bare-metal.", SortOrder = 5 });
            await context.AcademyCurriculumSections.AddAsync(curr);

            var track = new AcademyTrack();
            track.Cohorts.Add(new AcademyCohort
            {
                Name = "Fellowship Cohort 2026-Q1",
                ApplicationDeadline = DateTime.UtcNow.AddDays(30),
                StartDate = DateTime.UtcNow.AddDays(45),
                EndDate = DateTime.UtcNow.AddDays(135),
                MaxSeats = 30,
                Status = CohortStatus.Enrolling
            });
            await context.AcademyTracks.AddAsync(track);
        }

        // 12. Careers & Job Postings
        if (!await context.JobDepartments.AnyAsync())
        {
            await context.CareersHeroSections.AddAsync(new CareersHeroSection());

            var depKernel = new JobDepartment { Name = "Kernel Infrastructure", DisplayOrder = 1 };
            depKernel.JobPostings.Add(new JobPosting
            {
                Title = "Staff Rust / eBPF Kernel Engineer",
                Slug = "staff-rust-ebpf-kernel-engineer",
                Description = "Distributed concurrency, lock-free memory allocation, Ring 0 tracing.",
                Requirements = "Strong Rust and C systems engineering experience; Linux kernel and eBPF internals.",
                TechStackTags = "[\"Rust\", \"eBPF\", \"Ring 0\", \"Linux\"]",
                LocationType = LocationType.Remote,
                EmploymentType = EmploymentType.FullTime,
                ExperienceLevel = ExperienceLevel.Staff,
                Status = JobStatus.Published
            });

            var depCrypto = new JobDepartment { Name = "Post-Quantum Cryptography", DisplayOrder = 2 };
            depCrypto.JobPostings.Add(new JobPosting
            {
                Title = "Cryptographic Systems Architect",
                Slug = "cryptographic-systems-architect",
                Description = "Kyber lattice implementations, hardware security module firmware, FIPS certification.",
                Requirements = "Post-quantum lattice cryptography, HSM firmware, FIPS 140-3.",
                TechStackTags = "[\"Kyber\", \"NIST PQ\", \"HSM\", \"C\"]",
                LocationType = LocationType.Remote,
                EmploymentType = EmploymentType.FullTime,
                ExperienceLevel = ExperienceLevel.Principal,
                Status = JobStatus.Published
            });

            var depNeural = new JobDepartment { Name = "Neural Acceleration", DisplayOrder = 3 };
            depNeural.JobPostings.Add(new JobPosting
            {
                Title = "Lead FP8 / Tensor Compiler Engineer",
                Slug = "lead-fp8-tensor-compiler-engineer",
                Description = "Direct-to-metal model execution, CUDA/Triton kernels, distributed DAG schedulers.",
                Requirements = "CUDA, Triton, MLIR, LLVM compiler infrastructure.",
                TechStackTags = "[\"CUDA\", \"Triton\", \"FP8\", \"C++\"]",
                LocationType = LocationType.Remote,
                EmploymentType = EmploymentType.FullTime,
                ExperienceLevel = ExperienceLevel.Senior,
                Status = JobStatus.Published
            });

            await context.JobDepartments.AddRangeAsync(depKernel, depCrypto, depNeural);
        }

        // 13. Contact Page Settings & Intent / Scale Options
        if (!await context.ContactPageSettings.AnyAsync())
        {
            await context.ContactPageSettings.AddAsync(new ContactPageSettings());

            await context.InquiryIntentOptions.AddRangeAsync(
                new InquiryIntentOption { IntentKey = "cluster", DisplayName = "Deploy Cluster", SortOrder = 1 },
                new InquiryIntentOption { IntentKey = "enclave", DisplayName = "Hardware Enclave", SortOrder = 2 },
                new InquiryIntentOption { IntentKey = "academy", DisplayName = "Academy Partnership", SortOrder = 3 },
                new InquiryIntentOption { IntentKey = "careers", DisplayName = "Join Engineering", SortOrder = 4 }
            );

            await context.InquiryScaleOptions.AddRangeAsync(
                new InquiryScaleOption { ScaleKey = "cluster", DisplayText = "Cluster (< 100k TPS / Sub-millisecond)", SortOrder = 1 },
                new InquiryScaleOption { ScaleKey = "enterprise", DisplayText = "Enterprise Mesh (100k - 1M TPS)", SortOrder = 2 },
                new InquiryScaleOption { ScaleKey = "sovereign", DisplayText = "Hyper-Scale Sovereign (> 1M TPS)", SortOrder = 3 },
                new InquiryScaleOption { ScaleKey = "academic", DisplayText = "Academic / Research Grant", SortOrder = 4 }
            );
        }

        // 14. Telemetry Nodes
        if (!await context.TelemetryNodes.AnyAsync())
        {
            var node = new TelemetryNode { NodeIdentifier = "NODE-US-EAST-01", Region = "US-EAST", EnclaveType = EnclaveType.AmdSevSnp, Status = ClusterStatus.Online };
            node.Snapshots.Add(new TelemetrySnapshot { LatencyMs = 0.14, TpsThroughput = 148290, JitterVariancePct = 0.0, ConvergenceState = "6174_CONVERGED" });
            await context.TelemetryNodes.AddAsync(node);
        }

        await context.SaveChangesAsync();
    }
}
