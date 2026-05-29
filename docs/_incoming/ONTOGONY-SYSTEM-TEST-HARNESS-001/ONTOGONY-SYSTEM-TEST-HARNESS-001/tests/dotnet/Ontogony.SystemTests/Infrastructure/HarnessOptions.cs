namespace Ontogony.SystemTests.Infrastructure;

public sealed record HarnessOptions
{
    public string PlatformBaseUrl { get; init; } = Env("ONTOGONY_PLATFORM_BASE_URL", "http://localhost:5080");
    public string KanonBaseUrl { get; init; } = Env("KANON_BASE_URL", "http://localhost:5081");
    public string ConexusBaseUrl { get; init; } = Env("CONEXUS_BASE_URL", "http://localhost:5082");
    public string AllagmaBaseUrl { get; init; } = Env("ALLAGMA_BASE_URL", "http://localhost:5083");
    public string MetaboleBaseUrl { get; init; } = Env("METABOLE_BASE_URL", "http://localhost:5084");
    public string AisthesisBaseUrl { get; init; } = Env("AISTHESIS_BASE_URL", "http://localhost:5085");
    public string FrontendBaseUrl { get; init; } = Env("ONTOGONY_FRONTEND_BASE_URL", "http://localhost:5173");

    public string AllagmaServiceToken { get; init; } = Env("ALLAGMA_SERVICE_TOKEN", "allagma-dev-service-token-change-in-production");
    public string KanonServiceToken { get; init; } = Env("KANON_SERVICE_TOKEN", "kanon-dev-service-token-change-in-production");
    public string ConexusProjectApiKey { get; init; } = Env("CONEXUS_PROJECT_API_KEY", "cx-dev-key-change-me");
    public string ConexusAdminApiKey { get; init; } = Env("CONEXUS_ADMIN_API_KEY", "cx-conexus-admin-dev");
    public string MetaboleServiceToken { get; init; } = Env("METABOLE_SERVICE_TOKEN", "dev-metabole-service-token");
    public string AisthesisServiceToken { get; init; } = Env("AISTHESIS_SERVICE_TOKEN", "dev-aisthesis-service-token");
    public string EvidenceDir { get; init; } = Env("ONTOGONY_EVIDENCE_DIR", "./evidence/local");

    public bool RunRestartTests { get; init; } = EnvBool("RUN_RESTART_TESTS");
    public bool RunExternalProviderTests { get; init; } = EnvBool("RUN_EXTERNAL_PROVIDER_TESTS");
    public bool RunDestructiveTests { get; init; } = EnvBool("RUN_DESTRUCTIVE_TESTS");

    public static HarnessOptions FromEnvironment() => new();

    private static string Env(string key, string fallback) =>
        string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(key)) ? fallback : Environment.GetEnvironmentVariable(key)!;

    private static bool EnvBool(string key) =>
        string.Equals(Environment.GetEnvironmentVariable(key), "true", StringComparison.OrdinalIgnoreCase);
}
