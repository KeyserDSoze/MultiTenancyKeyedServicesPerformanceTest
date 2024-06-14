namespace MultiTenancyKeyedServicesTest.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTestMiddlewares(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseRuntimeServiceProvider();
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseRouting();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(x => x.MapControllers());
            return applicationBuilder;
        }
    }
}
