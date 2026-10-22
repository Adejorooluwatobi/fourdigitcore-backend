namespace FourDigitCore.Domain.Enums;

public enum InquiryStatus
{
    New = 0,
    InReview = 1,
    Engaged = 2,
    DiscoveryScheduled = 3,
    ClosedWon = 4,
    ClosedLost = 5,
    Spam = 6
}

public enum InquiryPriority
{
    Low = 0,
    Normal = 1,
    High = 2,
    Urgent = 3
}
