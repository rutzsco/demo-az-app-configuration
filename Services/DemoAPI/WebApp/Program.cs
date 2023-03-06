using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using WebApp.Models.Settings;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Retrieve the connection string
string connectionString = builder.Configuration.GetConnectionString("AppConfig");

// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
           // Load all keys that start with `TestApp:` and have no label
           .Select("WebApp:*", LabelFilter.Null)
           // Configure to reload configuration if the registered sentinel key is modified
           .ConfigureRefresh(refreshOptions => refreshOptions.Register("WebApp:Settings:Sentinel", refreshAll: true)
             .SetCacheExpiration(TimeSpan.FromSeconds(15)));

    // Load all feature flags with no label
    options.UseFeatureFlags();
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAzureAppConfiguration();
// Bind configuration "WebApp:Settings" section to the Settings object
builder.Services.Configure<StyleSettings>(builder.Configuration.GetSection("WebApp:Settings"));

// Add feature management to the container of services.
builder.Services.AddFeatureManagement();

// Add feature management, targeting filter, and ITargetingContextAccessor to service collection
builder.Services.AddFeatureManagement()
    .AddFeatureFilter<TargetingFilter>()
    .AddFeatureFilter<PercentageFilter>();

builder.Services.AddSingleton<ITargetingContextAccessor, TestTargetingContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseAzureAppConfiguration();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();

app.Run();
