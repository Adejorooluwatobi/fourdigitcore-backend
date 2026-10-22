namespace FourDigitCore.Domain.Enums;

public enum ClusterStatus
{
    Online = 0,
    Degraded = 1,
    Maintenance = 2,
    Offline = 3
}

public enum EnclaveType
{
    AmdSevSnp = 0,
    AwsNitro = 1,
    IntelSgx = 2,
    PhotonicN3e = 3
}

public enum SeverityLevel
{
    Info = 0,
    Warning = 1,
    Critical = 2
}
