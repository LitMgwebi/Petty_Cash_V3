namespace Backend
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            //IdentityBuilder builder = services.AddIdentityCore<User>(q => q.User.RequireUniqueEmail = true);

            //builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            //builder.AddEntityFrameworkStores<PettyCashContext>().AddDefaultTokenProviders();
        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration config)
        {
            //var settings = config.GetSection("Jwt");
            //var key = Environment.GetEnvironmentVariable("JWTKEY");

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = false,
            //            ValidateLifetime = true,
            //            ValidateAudience = false,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = settings.GetSection("Issuer").Value,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
            //        };
            //    });
        }
    }
}