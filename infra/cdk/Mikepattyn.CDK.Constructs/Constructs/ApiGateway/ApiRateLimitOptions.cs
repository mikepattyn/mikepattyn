namespace Mikepattyn.CDK.Constructs;

public sealed record ApiRateLimitOptions(
    double ThrottlingRateLimit,
    double ThrottlingBurstLimit,
    double WafRateLimit,
    double WafEvaluationWindowSeconds
)
{
    public static ApiRateLimitOptions ForEnvironment(DeploymentEnvironment environment)
    {
        if (environment.IsProduction)
        {
            return new ApiRateLimitOptions(
                ThrottlingRateLimit: 100,
                ThrottlingBurstLimit: 200,
                WafRateLimit: 2_000,
                WafEvaluationWindowSeconds: 300
            );
        }

        if (environment.IsStaging)
        {
            return new ApiRateLimitOptions(
                ThrottlingRateLimit: 200,
                ThrottlingBurstLimit: 400,
                WafRateLimit: 5_000,
                WafEvaluationWindowSeconds: 300
            );
        }

        return new ApiRateLimitOptions(
            ThrottlingRateLimit: 300,
            ThrottlingBurstLimit: 600,
            WafRateLimit: 10_000,
            WafEvaluationWindowSeconds: 300
        );
    }
}
