namespace FourDigitCore.Domain.Enums;

public enum JobStatus
{
    Draft = 0,
    Published = 1,
    Paused = 2,
    Closed = 3
}

public enum LocationType
{
    Remote = 0,
    Hybrid = 1,
    OnSite = 2
}

public enum EmploymentType
{
    FullTime = 0,
    Contract = 1,
    Fellowship = 2,
    PartTime = 3
}

public enum ExperienceLevel
{
    Junior = 0,
    Mid = 1,
    Senior = 2,
    Staff = 3,
    Principal = 4
}

public enum ApplicationStage
{
    Applied = 0,
    Screening = 1,
    TechnicalRound1 = 2,
    SystemsDesignRound2 = 3,
    FounderInterview = 4,
    OfferExtended = 5,
    Hired = 6,
    Rejected = 7,
    Withdrawn = 8
}
